using SmartDoc.BL.FileNotificationLog.FileLoadNotificationLog;

namespace SmartDoc.BL.Services.FileNotificationLogFactory;
internal class FileNotificationLogFactory : IFileNotificationLogFactory
{
    public FileAiAnalysisNotificationLog Create(string AIName, string fileName, long fileSize)
    {
        return new FileAiAnalysisNotificationLog(AIName, fileName, fileSize);
    }
}
