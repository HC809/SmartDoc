using MediatR;
using SmartDoc.BL.FileNotificationLog.FileLoadNotificationLog;
using SmartDoc.Data.Abstractions;
using SmartDoc.Data.Entites.DocumentLogEntries;

namespace SmartDoc.BL.DocumentLog.DocumentUploadLog;
internal class FileAiAnalysisNotificationLogHandler : INotificationHandler<FileAiAnalysisNotificationLog>
{
    private readonly IFileLogEntryRepository _fileLogEntryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public FileAiAnalysisNotificationLogHandler(IFileLogEntryRepository fileLogEntryRepository, IUnitOfWork unitOfWork)
    {
        _fileLogEntryRepository = fileLogEntryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(FileAiAnalysisNotificationLog notification, CancellationToken cancellationToken)
    {
        var logDescription = $"AI: {notification.AIName}, File: {notification.FileName}, Size: {notification.FileSize}";
        var logEntry = FileLogEntry.Register(FileActionType.AIAnalysis, logDescription, DateTime.Now);

        try
        {
            _fileLogEntryRepository.Add(logEntry);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
