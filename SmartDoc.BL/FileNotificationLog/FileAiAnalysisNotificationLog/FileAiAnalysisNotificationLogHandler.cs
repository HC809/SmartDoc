using MediatR;
using SmartDoc.BL.FileNotificationLog.FileLoadNotificationLog;
using SmartDoc.Data.Abstractions;
using SmartDoc.Data.Entites.DocumentLogEntries;

namespace SmartDoc.BL.DocumentLog.DocumentUploadLog;
internal class FileAiAnalysisNotificationLogHandler : INotificationHandler<FileAiAnalysisNotificationLog>
{
    private readonly IFileLogEntryRepository _documentLogEntryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public FileAiAnalysisNotificationLogHandler(IFileLogEntryRepository documentLogEntryRepository, IUnitOfWork unitOfWork)
    {
        _documentLogEntryRepository = documentLogEntryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(FileAiAnalysisNotificationLog notification, CancellationToken cancellationToken)
    {
        var logDescription = new Description($"AI: {notification.AIName}, File: {notification.FileName}, Size: {notification.FileSize}");
        var logEntry = FileLogEntry.Register(FileActionType.AIAnalysis, logDescription, DateTime.Now);

        try
        {
            _documentLogEntryRepository.Add(logEntry);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
