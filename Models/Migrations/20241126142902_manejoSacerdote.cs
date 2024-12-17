using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi_SGI_T.Models.Migrations
{
    /// <inheritdoc />
    public partial class manejoSacerdote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sc_parroco",
                table: "tbl_sacramentos");

            migrationBuilder.AddColumn<int>(
                name: "sc_parrocoId",
                table: "tbl_sacramentos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tbl_categoria_sacerdote",
                columns: table => new
                {
                    cs_idCategoria = table.Column<int>(type: "int", nullable: false),
                    cs_nombre = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true),
                    cs_descripcion = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tbl_cate__D3A3E3A3A3A3A3A6", x => x.cs_idCategoria);
                });

            migrationBuilder.CreateTable(
                name: "tbl_sacerdotes",
                columns: table => new
                {
                    sa_idSacerdote = table.Column<int>(type: "int", nullable: false),
                    sa_nombre = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true),
                    sa_categoria = table.Column<int>(type: "int", nullable: false),
                    sa_firma = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ScEstado = table.Column<int>(type: "int", nullable: false),
                    sa_create_user = table.Column<int>(type: "int", nullable: false),
                    sa_create_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    sa_update_user = table.Column<int>(type: "int", nullable: true),
                    sa_update_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    sa_delete_user = table.Column<int>(type: "int", nullable: true),
                    sa_delete_date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tbl_sace__D3A3E3A3A3A3A3A5", x => x.sa_idSacerdote);
                    table.ForeignKey(
                        name: "FK_TblSacerdote_TblCategoriaSacerdote",
                        column: x => x.sa_categoria,
                        principalTable: "tbl_categoria_sacerdote",
                        principalColumn: "cs_idCategoria");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sacramentos_sc_parrocoId",
                table: "tbl_sacramentos",
                column: "sc_parrocoId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sacerdotes_sa_categoria",
                table: "tbl_sacerdotes",
                column: "sa_categoria");

            migrationBuilder.AddForeignKey(
                name: "FK_TblSacramento_TblSacerdote",
                table: "tbl_sacramentos",
                column: "sc_parrocoId",
                principalTable: "tbl_sacerdotes",
                principalColumn: "sa_idSacerdote");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblSacramento_TblSacerdote",
                table: "tbl_sacramentos");

            migrationBuilder.DropTable(
                name: "tbl_sacerdotes");

            migrationBuilder.DropTable(
                name: "tbl_categoria_sacerdote");

            migrationBuilder.DropIndex(
                name: "IX_tbl_sacramentos_sc_parrocoId",
                table: "tbl_sacramentos");

            migrationBuilder.DropColumn(
                name: "sc_parrocoId",
                table: "tbl_sacramentos");

            migrationBuilder.AddColumn<string>(
                name: "sc_parroco",
                table: "tbl_sacramentos",
                type: "varchar(64)",
                unicode: false,
                maxLength: 64,
                nullable: true);
        }
    }
}
