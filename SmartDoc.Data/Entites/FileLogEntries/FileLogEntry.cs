using SmartDoc.Data.Abstractions;

namespace SmartDoc.Data.Entites.DocumentLogEntries;
public sealed class FileLogEntry : Entity
{
    public FileLogEntry(int id, FileActionType actionType, string description, DateTime createdOn) : base(id)
    {
        ActionType = actionType;
        Description = description;
        CreatedOn = createdOn;
    }

    public FileActionType ActionType { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedOn { get; private set; }

    /// <summary>
    /// This C# function creates and returns a new FileLogEntry object with the provided action type,
    /// description, and creation date.
    /// </summary>
    /// <param name="FileActionType">FileActionType is an enum that represents the type of action being
    /// performed on a file, such as Create, Update, Delete, etc.</param>
    /// <param name="description">A brief description of the file action that was performed.</param>
    /// <param name="DateTime">The `DateTime` parameter in the `Register` method represents the
    /// timestamp when the file action was created or occurred. It is used to record the date and time
    /// of the action in the file log entry.</param>
    /// <returns>
    /// An instance of the `FileLogEntry` class is being returned.
    /// </returns>
    public static FileLogEntry Register(FileActionType actionType, string description, DateTime createdOn)
    {
        var documentLog = new FileLogEntry(0, actionType, description, createdOn);

        return documentLog;
    }
}
