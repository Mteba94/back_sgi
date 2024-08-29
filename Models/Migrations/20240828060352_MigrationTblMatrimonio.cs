using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi_SGI_T.Models.Migrations
{
    /// <inheritdoc />
    public partial class MigrationTblMatrimonio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_matrimonios",
                columns: table => new
                {
                    ma_idMatrimonio = table.Column<int>(type: "int", nullable: false),
                    ma_esposo = table.Column<int>(type: "int", nullable: false),
                    ma_esposa = table.Column<int>(type: "int", nullable: false),
                    MatrimonioEstado = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tbl_matri__D3A3E3A3A3A3A3A3", x => x.ma_idMatrimonio);
                    table.ForeignKey(
                        name: "FK__tbl_matri__ma_es__3A81B327",
                        column: x => x.ma_esposo,
                        principalTable: "tbl_personas",
                        principalColumn: "pe_idpersona");
                    table.ForeignKey(
                        name: "FK__tbl_matri__ma_es__3B75D760",
                        column: x => x.ma_esposa,
                        principalTable: "tbl_personas",
                        principalColumn: "pe_idpersona");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_matrimonios_ma_esposa",
                table: "tbl_matrimonios",
                column: "ma_esposa");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_matrimonios_ma_esposo",
                table: "tbl_matrimonios",
                column: "ma_esposo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_matrimonios");
        }
    }
}
