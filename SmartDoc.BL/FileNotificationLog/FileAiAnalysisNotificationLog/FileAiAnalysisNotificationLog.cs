using SmartDoc.BL.Abstractions;

namespace SmartDoc.BL.FileNotificationLog.FileLoadNotificationLog;
public sealed record FileAiAnalysisNotificationLog(
    string AIName,
    string FileName,
    long FileSize
    ) : IDomainEvent;
