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
