using SmartDoc.BL.Services.DocumentClassifier;
using SmartDoc.BL.Services.InvoiceAnalyze;

namespace SmartDoc.Api.Models;

public sealed record FileAnalysisResponse(DocumentType DocumentType, dynamic Data);
