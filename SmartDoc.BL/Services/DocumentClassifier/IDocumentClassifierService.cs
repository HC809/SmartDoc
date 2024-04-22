namespace SmartDoc.BL.Services.DocumentClassifier;
public interface IDocumentClassifierService
{
    Task<DocumentClassifierResponse> GetDocumentType(Stream fileStream);
}
