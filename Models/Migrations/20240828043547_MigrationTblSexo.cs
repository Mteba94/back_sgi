using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi_SGI_T.Models.Migrations
{
    /// <inheritdoc />
    public partial class MigrationTblSexo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "se_abreviacion",
                table: "tbl_sexo",
                type: "varchar(10)",
                unicode: false,
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte>(
                name: "se_estado",
                table: "tbl_sexo",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "se_abreviacion",
                table: "tbl_sexo");

            migrationBuilder.DropColumn(
                name: "se_estado",
                table: "tbl_sexo");
        }
    }
}
