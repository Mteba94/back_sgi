﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApi_SGI_T.Models;

#nullable disable

namespace WebApi_SGI_T.Models.Migrations
{
    [DbContext(typeof(SgiSacramentosContext))]
    [Migration("20240828042052_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("Modern_Spanish_CI_AS")
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebApi_SGI_T.Models.TblPersona", b =>
                {
                    b.Property<int>("PeIdpersona")
                        .HasColumnType("int")
                        .HasColumnName("pe_idpersona");

                    b.Property<DateTime?>("PeCreateDate")
                        .HasColumnType("datetime")
                        .HasColumnName("pe_create_date");

                    b.Property<int?>("PeCreateUser")
                        .HasColumnType("int")
                        .HasColumnName("pe_create_user");

                    b.Property<DateTime?>("PeDeleteDate")
                        .HasColumnType("datetime")
                        .HasColumnName("pe_delete_date");

                    b.Property<int?>("PeDeleteUser")
                        .HasColumnType("int")
                        .HasColumnName("pe_delete_user");

                    b.Property<string>("PeDireccion")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("pe_direccion");

                    b.Property<byte>("PeEstado")
                        .HasColumnType("tinyint")
                        .HasColumnName("pe_estado");

                    b.Property<DateTime>("PeFechaNacimiento")
                        .HasColumnType("date")
                        .HasColumnName("pe_fechaNacimiento");

                    b.Property<byte>("PeIdTipoDocumento")
                        .HasColumnType("tinyint")
                        .HasColumnName("pe_idTipoDocumento");

                    b.Property<string>("PeNombre")
                        .IsRequired()
                        .HasMaxLength(64)
                        .IsUnicode(false)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("pe_nombre");

                    b.Property<string>("PeNumeroDocumento")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("pe_numeroDocumento");

                    b.Property<byte?>("PeSexoId")
                        .HasColumnType("tinyint");

                    b.Property<DateTime?>("PeUpdateDate")
                        .HasColumnType("datetime")
                        .HasColumnName("pe_update_date");

                    b.Property<int?>("PeUpdateUser")
                        .HasColumnType("int")
                        .HasColumnName("pe_update_user");

                    b.HasKey("PeIdpersona")
                        .HasName("PK__tbl_pers__AE24A86E49FF0980");

                    b.HasIndex("PeIdTipoDocumento");

                    b.HasIndex("PeSexoId");

                    b.ToTable("tbl_personas", (string)null);
                });

            modelBuilder.Entity("WebApi_SGI_T.Models.TblRol", b =>
                {
                    b.Property<int>("RoIdRol")
                        .HasColumnType("int")
                        .HasColumnName("ro_idRol");

                    b.Property<string>("RoDescripcion")
                        .IsRequired()
                        .HasMaxLength(64)
                        .IsUnicode(false)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("ro_descripcion");

                    b.Property<byte>("RoEstado")
                        .HasColumnType("tinyint")
                        .HasColumnName("ro_estado");

                    b.Property<string>("RoNombre")
                        .IsRequired()
                        .HasMaxLength(64)
                        .IsUnicode(false)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("ro_nombre");

                    b.HasKey("RoIdRol")
                        .HasName("PK__tbl_rol__DEC9EC9EAD3E0078");

                    b.ToTable("tbl_rol", (string)null);
                });

            modelBuilder.Entity("WebApi_SGI_T.Models.TblSacramento", b =>
                {
                    b.Property<int>("ScIdSacramento")
                        .HasColumnType("int")
                        .HasColumnName("sc_idSacramento");

                    b.Property<DateTime>("ScCreateDate")
                        .HasColumnType("datetime")
                        .HasColumnName("sc_create_date");

                    b.Property<int>("ScCreateUser")
                        .HasColumnType("int")
                        .HasColumnName("sc_create_user");

                    b.Property<DateTime?>("ScDeleteDate")
                        .HasColumnType("datetime")
                        .HasColumnName("sc_delete_date");

                    b.Property<int?>("ScDeleteUser")
                        .HasColumnType("int")
                        .HasColumnName("sc_delete_user");

                    b.Property<DateTime>("ScFechaSacramento")
                        .HasColumnType("date")
                        .HasColumnName("sc_fechaSacramento");

                    b.Property<int>("ScIdTipoSacramento")
                        .HasColumnType("int")
                        .HasColumnName("sc_idTipoSacramento");

                    b.Property<int>("ScIdpersona")
                        .HasColumnType("int")
                        .HasColumnName("sc_idpersona");

                    b.Property<string>("ScMadre")
                        .HasMaxLength(64)
                        .IsUnicode(false)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("sc_madre");

                    b.Property<string>("ScMadrina")
                        .HasMaxLength(64)
                        .IsUnicode(false)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("sc_madrina");

                    b.Property<string>("ScNumeroPartida")
                        .HasMaxLength(64)
                        .IsUnicode(false)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("sc_numeroPartida");

                    b.Property<string>("ScObservaciones")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("sc_observaciones");

                    b.Property<string>("ScPadre")
                        .HasMaxLength(64)
                        .IsUnicode(false)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("sc_padre");

                    b.Property<string>("ScPadrino")
                        .HasMaxLength(64)
                        .IsUnicode(false)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("sc_padrino");

                    b.Property<string>("ScParroco")
                        .HasMaxLength(64)
                        .IsUnicode(false)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("sc_parroco");

                    b.Property<DateTime?>("ScUpdateDate")
                        .HasColumnType("datetime")
                        .HasColumnName("sc_update_date");

                    b.Property<int?>("ScUpdateUser")
                        .HasColumnType("int")
                        .HasColumnName("sc_update_user");

                    b.HasKey("ScIdSacramento")
                        .HasName("PK__tbl_sacr__DF1C6ED661087010");

                    b.HasIndex("ScIdTipoSacramento")
                        .IsUnique();

                    b.HasIndex("ScIdpersona");

                    b.ToTable("tbl_sacramentos", (string)null);
                });

            modelBuilder.Entity("WebApi_SGI_T.Models.TblSexo", b =>
                {
                    b.Property<byte>("SexoId")
                        .HasColumnType("tinyint")
                        .HasColumnName("se_idSexo");

                    b.Property<string>("SexoNombre")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("se_nombre");

                    b.HasKey("SexoId")
                        .HasName("PK__tbl_sexo__5E35066F3EC7CDA4");

                    b.ToTable("tbl_sexo", (string)null);
                });

            modelBuilder.Entity("WebApi_SGI_T.Models.TblTipoDocumento", b =>
                {
                    b.Property<byte>("TdIdTipoDocumento")
                        .HasColumnType("tinyint")
                        .HasColumnName("td_idTipoDocumento");

                    b.Property<string>("TdAbreviacion")
                        .IsRequired()
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnType("varchar(15)")
                        .HasColumnName("td_abreviacion");

                    b.Property<byte>("TdEstado")
                        .HasColumnType("tinyint")
                        .HasColumnName("td_estado");

                    b.Property<string>("TdNombre")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("td_nombre");

                    b.HasKey("TdIdTipoDocumento")
                        .HasName("PK__tbl_Tipo__76D773EF0F77BDFB");

                    b.ToTable("tbl_Tipo_Documentos", (string)null);
                });

            modelBuilder.Entity("WebApi_SGI_T.Models.TblTipoSacramento", b =>
                {
                    b.Property<int>("TsIdTipoSacramento")
                        .HasColumnType("int")
                        .HasColumnName("ts_idTipoSacramento");

                    b.Property<DateTime?>("TsCreateDate")
                        .HasColumnType("datetime")
                        .HasColumnName("ts_create_date");

                    b.Property<int?>("TsCreateUser")
                        .HasColumnType("int")
                        .HasColumnName("ts_create_user");

                    b.Property<DateTime?>("TsDeleteDate")
                        .HasColumnType("datetime")
                        .HasColumnName("ts_delete_date");

                    b.Property<int?>("TsDeleteUser")
                        .HasColumnType("int")
                        .HasColumnName("ts_delete_user");

                    b.Property<string>("TsDescripcion")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("ts_descripcion");

                    b.Property<int>("TsEstado")
                        .HasColumnType("int")
                        .HasColumnName("ts_estado");

                    b.Property<string>("TsNombre")
                        .IsRequired()
                        .HasMaxLength(64)
                        .IsUnicode(false)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("ts_nombre");

                    b.Property<string>("TsRequerimiento")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("ts_requerimiento");

                    b.Property<DateTime?>("TsUpdateDate")
                        .HasColumnType("datetime")
                        .HasColumnName("ts_update_date");

                    b.Property<int?>("TsUpdateUser")
                        .HasColumnType("int")
                        .HasColumnName("ts_update_user");

                    b.HasKey("TsIdTipoSacramento")
                        .HasName("PK__tbl_tipo__3CD075E48F30F21E");

                    b.ToTable("tbl_tipo_sacramentos", (string)null);
                });

            modelBuilder.Entity("WebApi_SGI_T.Models.TblUserRol", b =>
                {
                    b.Property<int>("UrIdUserRol")
                        .HasColumnType("int")
                        .HasColumnName("ur_idUserRol");

                    b.Property<byte>("UrEstado")
                        .HasColumnType("tinyint")
                        .HasColumnName("ur_estado");

                    b.Property<int>("UrIdRol")
                        .HasColumnType("int")
                        .HasColumnName("ur_idRol");

                    b.Property<int>("UrIdUsuario")
                        .HasColumnType("int")
                        .HasColumnName("ur_idUsuario");

                    b.HasKey("UrIdUserRol")
                        .HasName("PK__tbl_User__BDFDB1E35882934B");

                    b.HasIndex("UrIdRol");

                    b.HasIndex("UrIdUsuario");

                    b.ToTable("tbl_User_rol", (string)null);
                });

            modelBuilder.Entity("WebApi_SGI_T.Models.TblUsuario", b =>
                {
                    b.Property<int>("UsIdusuario")
                        .HasColumnType("int")
                        .HasColumnName("us_idusuario");

                    b.Property<DateTime>("UsCreateDate")
                        .HasColumnType("datetime")
                        .HasColumnName("us_create_date");

                    b.Property<int>("UsCreateUser")
                        .HasColumnType("int")
                        .HasColumnName("us_create_user");

                    b.Property<DateTime?>("UsDeleteDate")
                        .HasColumnType("datetime")
                        .HasColumnName("us_delete_date");

                    b.Property<int?>("UsDeleteUser")
                        .HasColumnType("int")
                        .HasColumnName("us_delete_user");

                    b.Property<string>("UsDireccion")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("us_direccion");

                    b.Property<byte>("UsEstado")
                        .HasColumnType("tinyint")
                        .HasColumnName("us_estado");

                    b.Property<DateTime>("UsFechaNacimiento")
                        .HasColumnType("date")
                        .HasColumnName("us_fechaNacimiento");

                    b.Property<byte>("UsIdTipoDocumento")
                        .HasColumnType("tinyint")
                        .HasColumnName("us_idTipoDocumento");

                    b.Property<string>("UsImage")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("us_image");

                    b.Property<string>("UsNombre")
                        .IsRequired()
                        .HasMaxLength(64)
                        .IsUnicode(false)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("us_nombre");

                    b.Property<string>("UsNumerodocumento")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("us_numerodocumento");

                    b.Property<string>("UsPass")
                        .IsRequired()
                        .HasMaxLength(64)
                        .IsUnicode(false)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("us_pass");

                    b.Property<byte?>("UsSexoId")
                        .HasColumnType("tinyint");

                    b.Property<DateTime?>("UsUpdateDate")
                        .HasColumnType("datetime")
                        .HasColumnName("us_update_date");

                    b.Property<int?>("UsUpdateUser")
                        .HasColumnType("int")
                        .HasColumnName("us_update_user");

                    b.Property<string>("UsUserName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .IsUnicode(false)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("us_userName");

                    b.HasKey("UsIdusuario")
                        .HasName("PK__tbl_usua__3BB39D3051877A6E");

                    b.HasIndex("UsIdTipoDocumento");

                    b.HasIndex("UsSexoId");

                    b.ToTable("tbl_usuarios", (string)null);
                });

            modelBuilder.Entity("WebApi_SGI_T.Models.TblPersona", b =>
                {
                    b.HasOne("WebApi_SGI_T.Models.TblTipoDocumento", "PeIdTipoDocumentoNavigation")
                        .WithMany("TblPersonas")
                        .HasForeignKey("PeIdTipoDocumento")
                        .IsRequired()
                        .HasConstraintName("FK__tbl_perso__pe_id__2F10007B");

                    b.HasOne("WebApi_SGI_T.Models.TblSexo", "PeSexoNavigation")
                        .WithMany("TblPersona")
                        .HasForeignKey("PeSexoId")
                        .HasConstraintName("FK_tbl_personas_pe_sexo");

                    b.Navigation("PeIdTipoDocumentoNavigation");

                    b.Navigation("PeSexoNavigation");
                });

            modelBuilder.Entity("WebApi_SGI_T.Models.TblSacramento", b =>
                {
                    b.HasOne("WebApi_SGI_T.Models.TblTipoSacramento", "ScIdSacramentoNavigation")
                        .WithOne("TblSacramento")
                        .HasForeignKey("WebApi_SGI_T.Models.TblSacramento", "ScIdTipoSacramento")
                        .IsRequired()
                        .HasConstraintName("FK__tbl_sacra__sc_id__36B12243");

                    b.HasOne("WebApi_SGI_T.Models.TblPersona", "ScIdpersonaNavigation")
                        .WithMany("TblSacramentos")
                        .HasForeignKey("ScIdpersona")
                        .IsRequired()
                        .HasConstraintName("FK__tbl_sacra__sc_id__35BCFE0A");

                    b.Navigation("ScIdSacramentoNavigation");

                    b.Navigation("ScIdpersonaNavigation");
                });

            modelBuilder.Entity("WebApi_SGI_T.Models.TblUserRol", b =>
                {
                    b.HasOne("WebApi_SGI_T.Models.TblRol", "UrIdRolNavigation")
                        .WithMany("TblUserRols")
                        .HasForeignKey("UrIdRol")
                        .IsRequired()
                        .HasConstraintName("FK__tbl_User___ur_id__4D94879B");

                    b.HasOne("WebApi_SGI_T.Models.TblUsuario", "UrIdUsuarioNavigation")
                        .WithMany("TblUserRols")
                        .HasForeignKey("UrIdUsuario")
                        .IsRequired()
                        .HasConstraintName("FK__tbl_User___ur_id__4E88ABD4");

                    b.Navigation("UrIdRolNavigation");

                    b.Navigation("UrIdUsuarioNavigation");
                });

            modelBuilder.Entity("WebApi_SGI_T.Models.TblUsuario", b =>
                {
                    b.HasOne("WebApi_SGI_T.Models.TblTipoDocumento", "UsIdTipoDocumentoNavigation")
                        .WithMany("TblUsuarios")
                        .HasForeignKey("UsIdTipoDocumento")
                        .IsRequired()
                        .HasConstraintName("FK__tbl_usuar__us_id__3F466844");

                    b.HasOne("WebApi_SGI_T.Models.TblSexo", "UsSexoNavigation")
                        .WithMany("TblUsuario")
                        .HasForeignKey("UsSexoId")
                        .HasConstraintName("FK_tbl_usuarios_us_sexo");

                    b.Navigation("UsIdTipoDocumentoNavigation");

                    b.Navigation("UsSexoNavigation");
                });

            modelBuilder.Entity("WebApi_SGI_T.Models.TblPersona", b =>
                {
                    b.Navigation("TblSacramentos");
                });

            modelBuilder.Entity("WebApi_SGI_T.Models.TblRol", b =>
                {
                    b.Navigation("TblUserRols");
                });

            modelBuilder.Entity("WebApi_SGI_T.Models.TblSexo", b =>
                {
                    b.Navigation("TblPersona");

                    b.Navigation("TblUsuario");
                });

            modelBuilder.Entity("WebApi_SGI_T.Models.TblTipoDocumento", b =>
                {
                    b.Navigation("TblPersonas");

                    b.Navigation("TblUsuarios");
                });

            modelBuilder.Entity("WebApi_SGI_T.Models.TblTipoSacramento", b =>
                {
                    b.Navigation("TblSacramento");
                });

            modelBuilder.Entity("WebApi_SGI_T.Models.TblUsuario", b =>
                {
                    b.Navigation("TblUserRols");
                });
#pragma warning restore 612, 618
        }
    }
}
