namespace SmartDoc.WebApp.Models;

public sealed record SentimentAnalysisResult
{
    public bool Success { get; set; } = false;
    public SentimentData? Data { get; set; }
}

public sealed record SentimentData(string Sentiment, string Resume);
