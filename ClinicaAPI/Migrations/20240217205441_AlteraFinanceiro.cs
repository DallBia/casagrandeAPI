using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AlteraFinanceiro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nome",
                table: "Financeiros");

            migrationBuilder.AddColumn<double>(
                name: "saldo",
                table: "Financeiros",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "saldo",
                table: "Financeiros");

            migrationBuilder.AddColumn<string>(
                name: "nome",
                table: "Financeiros",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
