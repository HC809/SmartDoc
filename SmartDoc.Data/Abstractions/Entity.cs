namespace SmartDoc.Data.Abstractions;
public abstract class Entity
{
    protected Entity(int id) => Id = id;
    protected Entity() { }

    public int Id { get; init; }
}
