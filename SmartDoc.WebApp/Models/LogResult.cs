namespace SmartDoc.WebApp.Models;

public sealed record LogResult(
        int Id,
        string ActionType,
        string Description,
        DateTime CreatedOn);
