using SmartDoc.Data.Abstractions;

namespace SmartDoc.Data.Entites.DocumentLogEntries;
public sealed class FileLogEntry : Entity
{
    public FileLogEntry(int id, FileActionType actionType, Description description, DateTime createdOn) : base(id)
    {
        ActionType = actionType;
        Description = description;
        CreatedOn = createdOn;
    }

    public FileActionType ActionType { get; private set; }
    public Description Description { get; private set; }
    public DateTime CreatedOn { get; private set; }

    public static FileLogEntry Register(FileActionType actionType, Description description, DateTime createdOn)
    {
        var documentLog = new FileLogEntry(0, actionType, description, createdOn);

        return documentLog;
    }
}
