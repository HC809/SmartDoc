using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartDoc.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RenameTimeStamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "FileLogEntries",
                newName: "CreatedOn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "FileLogEntries",
                newName: "Timestamp");
        }
    }
}
