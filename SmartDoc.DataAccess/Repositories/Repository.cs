using SmartDoc.Data.Abstractions;

namespace SmartDoc.DataAccess.Repositories;
internal abstract class Repository<T> where T : Entity
{
    protected readonly ApplicationDbContext DbContext;

    protected Repository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public void Add(T entity) => DbContext.Add(entity);
}
