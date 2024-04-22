namespace SmartDoc.BL.Services.SentimentAnalysis;
public interface ISentimentAnalysisService
{
    Task<SentimentAnalysisResponse> GetSentimentAnalysisResponse(string document);
}
