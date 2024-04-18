using SmartDoc.Data.Abstractions;

namespace SmartDoc.Data.Entites.DocumentLogEntries;
public sealed class DocumentLogEntry : Entity
{
    public DocumentLogEntry(Guid id, DocumentActionType actionType, Description description, DateTime timeStamp) : base(id)
    {
        ActionType = actionType;
        Description = description;
        Timestamp = timeStamp;
    }

    public DocumentActionType ActionType { get; private set; }
    public Description Description { get; private set; }
    public DateTime Timestamp { get; private set; }

    public static DocumentLogEntry Register(DocumentActionType actionType, Description description)
    {
        var documentLog = new DocumentLogEntry(
            Guid.NewGuid(),
            actionType,
            description,
            DateTime.Now);

        return documentLog;
    }
}
