using MediatR;
using SmartDoc.BL.FileNotificationLog.FileLoadNotificationLog;

namespace SmartDoc.BL.DocumentLog.DocumentUploadLog;
internal class FileLoadNotificationLogHandler : INotificationHandler<FileLoadNotificationLog>
{
    public Task Handle(FileLoadNotificationLog notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Notification Success! File loaded: {notification.FileName}, Size: {notification.FileSize}");
        return Task.CompletedTask;
    }
}
