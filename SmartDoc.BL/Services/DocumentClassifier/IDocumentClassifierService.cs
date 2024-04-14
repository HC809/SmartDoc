namespace SmartDoc.BL.Services.DocumentClassifier;
public interface IDocumentClassifierService
{
    Task<DocumentType> GetDocumentType(Stream fileStream);
}
