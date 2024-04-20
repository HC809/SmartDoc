using Microsoft.EntityFrameworkCore;
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

    public async Task<IEnumerable<T?>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
    }
}
