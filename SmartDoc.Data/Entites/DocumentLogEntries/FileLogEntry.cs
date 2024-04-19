using SmartDoc.Data.Abstractions;

namespace SmartDoc.Data.Entites.DocumentLogEntries;
public sealed class FileLogEntry : Entity
{
    public FileLogEntry(Guid id, FileActionType actionType, Description description, DateTime timeStamp) : base(id)
    {
        ActionType = actionType;
        Description = description;
        Timestamp = timeStamp;
    }

    public FileActionType ActionType { get; private set; }
    public Description Description { get; private set; }
    public DateTime Timestamp { get; private set; }

    public static FileLogEntry Register(FileActionType actionType, Description description)
    {
        var documentLog = new FileLogEntry(
            Guid.NewGuid(),
            actionType,
            description,
            DateTime.Now);

        return documentLog;
    }
}
