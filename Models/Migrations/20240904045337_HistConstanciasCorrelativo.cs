using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi_SGI_T.Models.Migrations
{
    /// <inheritdoc />
    public partial class HistConstanciasCorrelativo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ct_FormatoCorrelativo",
                table: "tbl_constancias",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_constancias_ct_FormatoCorrelativo",
                table: "tbl_constancias",
                column: "ct_FormatoCorrelativo",
                unique: true,
                filter: "[ct_FormatoCorrelativo] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tbl_constancias_ct_FormatoCorrelativo",
                table: "tbl_constancias");

            migrationBuilder.DropColumn(
                name: "ct_FormatoCorrelativo",
                table: "tbl_constancias");
        }
    }
}
