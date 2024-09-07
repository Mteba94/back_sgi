using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi_SGI_T.Models.Migrations
{
    /// <inheritdoc />
    public partial class RelMatrimonio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScIdMatrimonio",
                table: "tbl_sacramentos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sacramentos_ScIdMatrimonio",
                table: "tbl_sacramentos",
                column: "ScIdMatrimonio");

            migrationBuilder.AddForeignKey(
                name: "FK__tbl_sacra__sc_id__35BCFE0B",
                table: "tbl_sacramentos",
                column: "ScIdMatrimonio",
                principalTable: "tbl_matrimonios",
                principalColumn: "ma_idMatrimonio");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__tbl_sacra__sc_id__35BCFE0B",
                table: "tbl_sacramentos");

            migrationBuilder.DropIndex(
                name: "IX_tbl_sacramentos_ScIdMatrimonio",
                table: "tbl_sacramentos");

            migrationBuilder.DropColumn(
                name: "ScIdMatrimonio",
                table: "tbl_sacramentos");
        }
    }
}
