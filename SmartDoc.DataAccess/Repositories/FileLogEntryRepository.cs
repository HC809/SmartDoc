using SmartDoc.Data.Entites.DocumentLogEntries;

namespace SmartDoc.DataAccess.Repositories;
public sealed class FileLogEntryRepository : Repository<FileLogEntry>, IFileLogEntryRepository
{
    public FileLogEntryRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
