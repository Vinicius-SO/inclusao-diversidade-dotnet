using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InclusaoDiversidadeEmpresas.Migrations
{
    /// <inheritdoc />
    public partial class TabelaRelatorios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_relatorios_diversidade",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DataGerada = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    TotalColaborador = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ContagemDeMulheres = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ContagemDePessoasNegras = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ContagemDePessoasLgbt = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ContagemDePessoasComDesabilidade = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_relatorios_diversidade", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_relatorios_diversidade");
        }
    }
}
