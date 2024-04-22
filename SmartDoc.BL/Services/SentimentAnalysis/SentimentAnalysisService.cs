using Azure;
using Azure.AI.TextAnalytics;
using Microsoft.Extensions.Options;

namespace SmartDoc.BL.Services.SentimentAnalysis;
internal sealed class SentimentAnalysisService : ISentimentAnalysisService
{
    private readonly SentimentAnalysisSettings _settings;

    public SentimentAnalysisService(IOptions<SentimentAnalysisSettings> options)
    {
        _settings = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// This C# function analyzes the sentiment of a given document and provides a summarized version of
    /// the text.
    /// </summary>
    /// <param name="document">The `document` parameter in the `GetSentimentAnalysisResponse` method is
    /// the text that you want to analyze for sentiment and generate a summary from. This text could be
    /// any piece of content such as a paragraph, article, review, or any other form of textual data.
    /// The method uses Azure</param>
    /// <returns>
    /// The method `GetSentimentAnalysisResponse` returns a `Task` that will eventually yield a
    /// `SentimentAnalysisResponse` object. This object contains the sentiment analysis result of the
    /// input document as well as a summarized sentence based on the document content. If the sentiment
    /// analysis is successful, the sentiment and the summary sentence are returned. If the sentiment
    /// analysis fails, a default response with sentiment "N/D
    /// </returns>
    public async Task<SentimentAnalysisResponse> GetSentimentAnalysisResponse(string document)
    {
        AzureKeyCredential credential = new AzureKeyCredential(_settings.LanguageApiKey);
        TextAnalyticsClient client = new TextAnalyticsClient(new Uri(_settings.LanguageEndpoint), credential);

        DocumentSentiment docSentiment = await client.AnalyzeSentimentAsync(document);

        if (docSentiment is not null)
        {
            var summarizeAction = new AbstractiveSummarizeAction()
            {
                ModelVersion = "latest",
                DisableServiceLogs = true,
                SentenceCount = 3
            };

            TextAnalyticsActions actions = new TextAnalyticsActions()
            {
                AbstractiveSummarizeActions = new List<AbstractiveSummarizeAction>() { summarizeAction },
            };

            AnalyzeActionsOperation operation = await client.StartAnalyzeActionsAsync(new List<string> { document }, actions, "es");
            await operation.WaitForCompletionAsync();

            string summarySentence = string.Empty;
            await foreach (AnalyzeActionsResult documentsInPage in operation.Value)
            {
                var summaryResults = documentsInPage.AbstractiveSummarizeResults;
                summarySentence = GetSummarySentence(summaryResults);
            }

            return new SentimentAnalysisResponse(docSentiment.Sentiment.ToString(), summarySentence);
        }

        return new SentimentAnalysisResponse("N/D", "No se pudo obtener el sentimiento del texto.");
    }

    /// <summary>
    /// This C# function takes a collection of abstractive summarization results, extracts the summary
    /// sentences from each document result, and returns them as a single string joined by spaces.
    /// </summary>
    /// <param name="summariesActions">A collection of AbstractiveSummarizeActionResult objects, each
    /// containing a list of document results which in turn contain summaries.</param>
    /// <returns>
    /// The `GetSummarySentence` method returns a single string that concatenates all the summary
    /// sentences extracted from the `summariesActions` input parameter.
    /// </returns>
    private string GetSummarySentence(IReadOnlyCollection<AbstractiveSummarizeActionResult> summariesActions)
    {
        List<string> summarySentences = new List<string>();

        foreach (var summaryActionResults in summariesActions)
        {
            foreach (var documentResults in summaryActionResults.DocumentsResults)
            {
                foreach (var summary in documentResults.Summaries)
                {
                    summarySentences.Add(summary.Text);
                }
            }
        }

        return string.Join(" ", summarySentences);
    }
}
