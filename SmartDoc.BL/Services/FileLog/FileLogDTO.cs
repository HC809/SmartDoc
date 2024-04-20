using SmartDoc.Data.Entites.DocumentLogEntries;

namespace SmartDoc.BL.Services.Log;
public sealed record FileLogDTO(
        int Id,
        string ActionType,
        string Description,
        DateTime CreatedOn);