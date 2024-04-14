using SmartDoc.BL.Services.DocumentClassifier;

namespace SmartDoc.Api.Models;

public sealed record FileErrorResponse(DocumentType DocumentType, string ErrorMessage);
