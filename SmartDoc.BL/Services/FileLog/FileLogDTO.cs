using SmartDoc.Data.Entites.DocumentLogEntries;

namespace SmartDoc.BL.Services.Log;
public sealed record FileLogDTO(
        int Id,
        FileActionType ActionType,
        string Description,
        DateTime CreatedOn);