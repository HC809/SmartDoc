namespace SmartDoc.BL.Services.DocumentClassifier;
public sealed record DocumentClassifierResponse(
    DocumentType DocumentType,
    string? Document = null);
