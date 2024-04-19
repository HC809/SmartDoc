using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartDoc.Data.Entites.DocumentLogEntries;

namespace SmartDoc.DataAccess.Configurations;
internal sealed class FileLogEntryConfiguration : IEntityTypeConfiguration<FileLogEntry>
{
    public void Configure(EntityTypeBuilder<FileLogEntry> builder)
    {
        builder.ToTable("FileLogEntries");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ActionType)
            .IsRequired()
            .HasMaxLength(15)
            .HasConversion(
                value => value.ToString(),
                v => (FileActionType)Enum.Parse(typeof(FileActionType), v)
            );

        builder.Property(x => x.Description)
           .IsRequired()
           .HasMaxLength(500)
           .HasConversion(
                description => description.Value,
                value => new Description(value)
            );

        builder.Property(x => x.Timestamp)
            .IsRequired();
    }
}
