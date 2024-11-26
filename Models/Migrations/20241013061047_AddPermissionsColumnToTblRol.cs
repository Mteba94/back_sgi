using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi_SGI_T.Models.Migrations
{
    /// <inheritdoc />
    public partial class AddPermissionsColumnToTblRol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Permissions",
                table: "tbl_rol",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Permissions",
                table: "tbl_rol");
        }
    }
}
