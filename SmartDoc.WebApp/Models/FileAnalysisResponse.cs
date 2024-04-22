using System.Text.Json;

namespace SmartDoc.WebApp.Models;

public sealed record FileAnalysisResponse(
    string DocumentType,
    dynamic Data
    );
