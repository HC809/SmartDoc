using SmartDoc.BL.FileNotificationLog.FileLoadNotificationLog;

namespace SmartDoc.BL.Services.FileNotificationLogFactory;
public interface IFileNotificationLogFactory
{
    FileAiAnalysisNotificationLog Create(string IAName, string fileName, long fileSize);
}
