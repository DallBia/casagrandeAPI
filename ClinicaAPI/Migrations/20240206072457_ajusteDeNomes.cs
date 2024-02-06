using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicaAPI.Migrations
{
    /// <inheritdoc />
    public partial class ajusteDeNomes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Registro",
                table: "Formacaos",
                newName: "registro");

            migrationBuilder.RenameColumn(
                name: "NomeFormacao",
                table: "Formacaos",
                newName: "nomeFormacao");

            migrationBuilder.RenameColumn(
                name: "Nivel",
                table: "Formacaos",
                newName: "nivel");

            migrationBuilder.RenameColumn(
                name: "Instituicao",
                table: "Formacaos",
                newName: "instituicao");

            migrationBuilder.RenameColumn(
                name: "IdFuncionario",
                table: "Formacaos",
                newName: "idFuncionario");

            migrationBuilder.RenameColumn(
                name: "DtConclusao",
                table: "Formacaos",
                newName: "dtConclusao");

            migrationBuilder.RenameColumn(
                name: "AreasRelacionadas",
                table: "Formacaos",
                newName: "areasRelacionadas");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Formacaos",
                newName: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "registro",
                table: "Formacaos",
                newName: "Registro");

            migrationBuilder.RenameColumn(
                name: "nomeFormacao",
                table: "Formacaos",
                newName: "NomeFormacao");

            migrationBuilder.RenameColumn(
                name: "nivel",
                table: "Formacaos",
                newName: "Nivel");

            migrationBuilder.RenameColumn(
                name: "instituicao",
                table: "Formacaos",
                newName: "Instituicao");

            migrationBuilder.RenameColumn(
                name: "idFuncionario",
                table: "Formacaos",
                newName: "IdFuncionario");

            migrationBuilder.RenameColumn(
                name: "dtConclusao",
                table: "Formacaos",
                newName: "DtConclusao");

            migrationBuilder.RenameColumn(
                name: "areasRelacionadas",
                table: "Formacaos",
                newName: "AreasRelacionadas");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Formacaos",
                newName: "Id");
        }
    }
}
