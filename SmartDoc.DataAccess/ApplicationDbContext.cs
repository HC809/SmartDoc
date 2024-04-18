using Microsoft.EntityFrameworkCore;
using SmartDoc.Data.Abstractions;

namespace SmartDoc.DataAccess;
internal class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }
}
