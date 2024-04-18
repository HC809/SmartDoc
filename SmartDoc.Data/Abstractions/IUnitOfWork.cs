namespace SmartDoc.Data.Abstractions;
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}
