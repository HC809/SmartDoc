using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartDoc.Data.Entites.DocumentLogEntries;

namespace SmartDoc.DataAccess.Configurations;
internal sealed class FileLogEntryConfiguration : IEntityTypeConfiguration<FileLogEntry>
{
    /// <summary>
    /// The Configure function sets up the configuration for the FileLogEntry entity in C# using
    /// EntityTypeBuilder.
    /// </summary>
    /// <param name="builder">The `builder` parameter in the `Configure` method is an
    /// `EntityTypeBuilder<FileLogEntry>` object. It is used to configure the entity mapping for the
    /// `FileLogEntry` class in Entity Framework Core. The `EntityTypeBuilder` class provides methods to
    /// define various aspects of the entity such as</param>
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
           .HasMaxLength(500);

        builder.Property(x => x.CreatedOn)
            .IsRequired()
            .HasColumnType("smalldatetime");
    }
}
