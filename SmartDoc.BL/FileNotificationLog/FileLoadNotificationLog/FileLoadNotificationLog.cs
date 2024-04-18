using SmartDoc.BL.Abstractions;

namespace SmartDoc.BL.FileNotificationLog.FileLoadNotificationLog;
public sealed record FileLoadNotificationLog(
    string FileName,
    long FileSize
    ) : IDomainEvent;
