using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi_SGI_T.Models.Migrations
{
    /// <inheritdoc />
    public partial class HistConstancias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_constancias",
                columns: table => new
                {
                    ct_idConstancia = table.Column<int>(type: "int", nullable: false),
                    ct_SacramentoId = table.Column<int>(type: "int", nullable: false),
                    ct_Correlativo = table.Column<int>(type: "int", nullable: false),
                    ct_UsuarioId = table.Column<int>(type: "int", nullable: false),
                    ct_Fecha = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tbl_cons__D3A3E3A3A3A3A3A4", x => x.ct_idConstancia);
                    table.ForeignKey(
                        name: "FK__tbl_const__ct_Sa__3A81B327",
                        column: x => x.ct_SacramentoId,
                        principalTable: "tbl_sacramentos",
                        principalColumn: "sc_idSacramento");
                    table.ForeignKey(
                        name: "FK_tbl_const_ct_Us_3A81BR328",
                        column: x => x.ct_UsuarioId,
                        principalTable: "tbl_usuarios",
                        principalColumn: "us_idusuario");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_constancias_ct_SacramentoId",
                table: "tbl_constancias",
                column: "ct_SacramentoId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_constancias_ct_UsuarioId",
                table: "tbl_constancias",
                column: "ct_UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_constancias");
        }
    }
}
