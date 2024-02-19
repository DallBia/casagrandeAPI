datusing Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicaAPI.Migrations
{
    /// <inheritdoc />
    public partial class ajusteFinanceiro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "refAgenda",
                table: "Financeiros",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "refAgenda",
                table: "Financeiros",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
