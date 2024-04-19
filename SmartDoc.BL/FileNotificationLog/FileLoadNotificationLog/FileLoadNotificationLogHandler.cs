using MediatR;
using SmartDoc.BL.FileNotificationLog.FileLoadNotificationLog;
using SmartDoc.Data.Abstractions;
using SmartDoc.Data.Entites.DocumentLogEntries;

namespace SmartDoc.BL.DocumentLog.DocumentUploadLog;
internal class FileLoadNotificationLogHandler : INotificationHandler<FileLoadNotificationLog>
{
    private readonly IFileLogEntryRepository _documentLogEntryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public FileLoadNotificationLogHandler(IFileLogEntryRepository documentLogEntryRepository, IUnitOfWork unitOfWork)
    {
        _documentLogEntryRepository = documentLogEntryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(FileLoadNotificationLog notification, CancellationToken cancellationToken)
    {
        var logDescription = new Description($"Nombre: {notification.FileName}, Tamaño: {notification.FileSize}");
        var logEntry = FileLogEntry.Register(FileActionType.FileUpload, logDescription);

        _documentLogEntryRepository.Add(logEntry);
        await _unitOfWork.SaveChangesAsync();
    }
}
