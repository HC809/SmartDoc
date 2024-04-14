using SmartDoc.BL.Services.DocumentClassifier;
using SmartDoc.BL.Services.InvoiceAnalyze;

namespace SmartDoc.Api.Models;

public sealed record InvoiceAnalysisResponse(DocumentType DocumentType, InvoiceData Data);
