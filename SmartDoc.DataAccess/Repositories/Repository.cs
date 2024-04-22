using Microsoft.EntityFrameworkCore;
using SmartDoc.Data.Abstractions;

namespace SmartDoc.DataAccess.Repositories;
public abstract class Repository<T> where T : Entity
{
    protected readonly ApplicationDbContext DbContext;

    protected Repository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    /// <summary>
    /// The Add method adds an entity to the DbContext.
    /// </summary>
    /// <param name="T">In the provided code snippet, `T` is a generic type parameter. It allows the
    /// method `Add` to accept any type of entity as an argument. The actual type of `T` will be
    /// determined when the method is called.</param>
    public void Add(T entity) => DbContext.Add(entity);

    /// <summary>
    /// This asynchronous method retrieves all entities of type T from the database using Entity
    /// Framework Core and returns them as a list.
    /// </summary>
    /// <param name="CancellationToken">The CancellationToken parameter in the method signature allows
    /// you to pass a token that can be used to request cancellation of the asynchronous operation. This
    /// token can be used to notify the operation that it should be canceled, allowing for graceful
    /// termination of the operation if needed.</param>
    /// <returns>
    /// An asynchronous task that returns an IEnumerable of type T.
    /// </returns>
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
    }
}
