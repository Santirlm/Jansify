using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jansify_Santiago_Ramirez.Migrations
{
    /// <inheritdoc />
    public partial class FixAno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Año",
                table: "Albums",
                newName: "Ano");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ano",
                table: "Albums",
                newName: "Año");
        }
    }
}
