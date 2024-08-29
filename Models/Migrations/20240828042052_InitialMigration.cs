using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi_SGI_T.Models.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_rol",
                columns: table => new
                {
                    ro_idRol = table.Column<int>(type: "int", nullable: false),
                    ro_nombre = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    ro_descripcion = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    ro_estado = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tbl_rol__DEC9EC9EAD3E0078", x => x.ro_idRol);
                });

            migrationBuilder.CreateTable(
                name: "tbl_sexo",
                columns: table => new
                {
                    se_idSexo = table.Column<byte>(type: "tinyint", nullable: false),
                    se_nombre = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tbl_sexo__5E35066F3EC7CDA4", x => x.se_idSexo);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Tipo_Documentos",
                columns: table => new
                {
                    td_idTipoDocumento = table.Column<byte>(type: "tinyint", nullable: false),
                    td_nombre = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    td_abreviacion = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    td_estado = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tbl_Tipo__76D773EF0F77BDFB", x => x.td_idTipoDocumento);
                });

            migrationBuilder.CreateTable(
                name: "tbl_tipo_sacramentos",
                columns: table => new
                {
                    ts_idTipoSacramento = table.Column<int>(type: "int", nullable: false),
                    ts_nombre = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    ts_descripcion = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    ts_requerimiento = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ts_estado = table.Column<int>(type: "int", nullable: false),
                    ts_create_user = table.Column<int>(type: "int", nullable: true),
                    ts_create_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    ts_update_user = table.Column<int>(type: "int", nullable: true),
                    ts_update_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    ts_delete_user = table.Column<int>(type: "int", nullable: true),
                    ts_delete_date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tbl_tipo__3CD075E48F30F21E", x => x.ts_idTipoSacramento);
                });

            migrationBuilder.CreateTable(
                name: "tbl_personas",
                columns: table => new
                {
                    pe_idpersona = table.Column<int>(type: "int", nullable: false),
                    pe_nombre = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    pe_fechaNacimiento = table.Column<DateTime>(type: "date", nullable: false),
                    pe_idTipoDocumento = table.Column<byte>(type: "tinyint", nullable: false),
                    pe_numeroDocumento = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    PeSexoId = table.Column<byte>(type: "tinyint", nullable: true),
                    pe_direccion = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    pe_estado = table.Column<byte>(type: "tinyint", nullable: false),
                    pe_create_user = table.Column<int>(type: "int", nullable: true),
                    pe_create_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    pe_update_user = table.Column<int>(type: "int", nullable: true),
                    pe_update_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    pe_delete_user = table.Column<int>(type: "int", nullable: true),
                    pe_delete_date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tbl_pers__AE24A86E49FF0980", x => x.pe_idpersona);
                    table.ForeignKey(
                        name: "FK__tbl_perso__pe_id__2F10007B",
                        column: x => x.pe_idTipoDocumento,
                        principalTable: "tbl_Tipo_Documentos",
                        principalColumn: "td_idTipoDocumento");
                    table.ForeignKey(
                        name: "FK_tbl_personas_pe_sexo",
                        column: x => x.PeSexoId,
                        principalTable: "tbl_sexo",
                        principalColumn: "se_idSexo");
                });

            migrationBuilder.CreateTable(
                name: "tbl_usuarios",
                columns: table => new
                {
                    us_idusuario = table.Column<int>(type: "int", nullable: false),
                    us_userName = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    us_pass = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    us_image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    us_nombre = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    us_fechaNacimiento = table.Column<DateTime>(type: "date", nullable: false),
                    us_idTipoDocumento = table.Column<byte>(type: "tinyint", nullable: false),
                    us_numerodocumento = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    UsSexoId = table.Column<byte>(type: "tinyint", nullable: true),
                    us_direccion = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    us_estado = table.Column<byte>(type: "tinyint", nullable: false),
                    us_create_user = table.Column<int>(type: "int", nullable: false),
                    us_create_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    us_update_user = table.Column<int>(type: "int", nullable: true),
                    us_update_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    us_delete_user = table.Column<int>(type: "int", nullable: true),
                    us_delete_date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tbl_usua__3BB39D3051877A6E", x => x.us_idusuario);
                    table.ForeignKey(
                        name: "FK__tbl_usuar__us_id__3F466844",
                        column: x => x.us_idTipoDocumento,
                        principalTable: "tbl_Tipo_Documentos",
                        principalColumn: "td_idTipoDocumento");
                    table.ForeignKey(
                        name: "FK_tbl_usuarios_us_sexo",
                        column: x => x.UsSexoId,
                        principalTable: "tbl_sexo",
                        principalColumn: "se_idSexo");
                });

            migrationBuilder.CreateTable(
                name: "tbl_sacramentos",
                columns: table => new
                {
                    sc_idSacramento = table.Column<int>(type: "int", nullable: false),
                    sc_idTipoSacramento = table.Column<int>(type: "int", nullable: false),
                    sc_idpersona = table.Column<int>(type: "int", nullable: false),
                    sc_fechaSacramento = table.Column<DateTime>(type: "date", nullable: false),
                    sc_numeroPartida = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true),
                    sc_padre = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true),
                    sc_madre = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true),
                    sc_padrino = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true),
                    sc_madrina = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true),
                    sc_parroco = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true),
                    sc_observaciones = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    sc_create_user = table.Column<int>(type: "int", nullable: false),
                    sc_create_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    sc_update_user = table.Column<int>(type: "int", nullable: true),
                    sc_update_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    sc_delete_user = table.Column<int>(type: "int", nullable: true),
                    sc_delete_date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tbl_sacr__DF1C6ED661087010", x => x.sc_idSacramento);
                    table.ForeignKey(
                        name: "FK__tbl_sacra__sc_id__35BCFE0A",
                        column: x => x.sc_idpersona,
                        principalTable: "tbl_personas",
                        principalColumn: "pe_idpersona");
                    table.ForeignKey(
                        name: "FK__tbl_sacra__sc_id__36B12243",
                        column: x => x.sc_idTipoSacramento,
                        principalTable: "tbl_tipo_sacramentos",
                        principalColumn: "ts_idTipoSacramento");
                });

            migrationBuilder.CreateTable(
                name: "tbl_User_rol",
                columns: table => new
                {
                    ur_idUserRol = table.Column<int>(type: "int", nullable: false),
                    ur_idRol = table.Column<int>(type: "int", nullable: false),
                    ur_idUsuario = table.Column<int>(type: "int", nullable: false),
                    ur_estado = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tbl_User__BDFDB1E35882934B", x => x.ur_idUserRol);
                    table.ForeignKey(
                        name: "FK__tbl_User___ur_id__4D94879B",
                        column: x => x.ur_idRol,
                        principalTable: "tbl_rol",
                        principalColumn: "ro_idRol");
                    table.ForeignKey(
                        name: "FK__tbl_User___ur_id__4E88ABD4",
                        column: x => x.ur_idUsuario,
                        principalTable: "tbl_usuarios",
                        principalColumn: "us_idusuario");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_personas_pe_idTipoDocumento",
                table: "tbl_personas",
                column: "pe_idTipoDocumento");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_personas_PeSexoId",
                table: "tbl_personas",
                column: "PeSexoId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sacramentos_sc_idpersona",
                table: "tbl_sacramentos",
                column: "sc_idpersona");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sacramentos_sc_idTipoSacramento",
                table: "tbl_sacramentos",
                column: "sc_idTipoSacramento",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_User_rol_ur_idRol",
                table: "tbl_User_rol",
                column: "ur_idRol");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_User_rol_ur_idUsuario",
                table: "tbl_User_rol",
                column: "ur_idUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_usuarios_us_idTipoDocumento",
                table: "tbl_usuarios",
                column: "us_idTipoDocumento");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_usuarios_UsSexoId",
                table: "tbl_usuarios",
                column: "UsSexoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_sacramentos");

            migrationBuilder.DropTable(
                name: "tbl_User_rol");

            migrationBuilder.DropTable(
                name: "tbl_personas");

            migrationBuilder.DropTable(
                name: "tbl_tipo_sacramentos");

            migrationBuilder.DropTable(
                name: "tbl_rol");

            migrationBuilder.DropTable(
                name: "tbl_usuarios");

            migrationBuilder.DropTable(
                name: "tbl_Tipo_Documentos");

            migrationBuilder.DropTable(
                name: "tbl_sexo");
        }
    }
}
