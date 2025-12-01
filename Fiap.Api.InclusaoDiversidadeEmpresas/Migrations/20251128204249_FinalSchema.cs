using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InclusaoDiversidadeEmpresas.Migrations
{
    /// <inheritdoc />
    public partial class FinalSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_colaboradores",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NomeColaborador = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    GeneroColaborador = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    EtniaColaborador = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    TemDisabilidade = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    Departamento = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    Senha = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    TreinamentoCompleto = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_colaboradores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_treinamentos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Titulo = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Data = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_treinamentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_participacao_treinamento",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ColaboradorId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    TreinamentoId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    Completo = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    DataDeConclusao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_participacao_treinamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_participacao_treinamento_tbl_colaboradores_ColaboradorId",
                        column: x => x.ColaboradorId,
                        principalTable: "tbl_colaboradores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_participacao_treinamento_tbl_treinamentos_TreinamentoId",
                        column: x => x.TreinamentoId,
                        principalTable: "tbl_treinamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_colaboradores_Email",
                table: "tbl_colaboradores",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_participacao_treinamento_ColaboradorId_TreinamentoId",
                table: "tbl_participacao_treinamento",
                columns: new[] { "ColaboradorId", "TreinamentoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_participacao_treinamento_TreinamentoId",
                table: "tbl_participacao_treinamento",
                column: "TreinamentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_participacao_treinamento");

            migrationBuilder.DropTable(
                name: "tbl_colaboradores");

            migrationBuilder.DropTable(
                name: "tbl_treinamentos");
        }
    }
}
