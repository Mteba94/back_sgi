using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace WebApi_SGI_T.Models;

public partial class SgiSacramentosContext : DbContext
{
    public SgiSacramentosContext()
    {
    }

    public SgiSacramentosContext(DbContextOptions<SgiSacramentosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblPersona> TblPersonas { get; set; }

    public virtual DbSet<TblRol> TblRols { get; set; }

    public virtual DbSet<TblSacramento> TblSacramentos { get; set; }

    public virtual DbSet<TblTipoDocumento> TblTipoDocumentos { get; set; }

    public virtual DbSet<TblTipoSacramento> TblTipoSacramentos { get; set; }

    public virtual DbSet<TblUserRol> TblUserRols { get; set; }

    public virtual DbSet<TblUsuario> TblUsuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<TblPersona>(entity =>
        {
            entity.HasKey(e => e.PeIdpersona).HasName("PK__tbl_pers__AE24A86E49FF0980");

            entity.ToTable("tbl_personas");

            entity.Property(e => e.PeIdpersona)
                .ValueGeneratedNever()
                .HasColumnName("pe_idpersona");
            entity.Property(e => e.PeCreateDate)
                .HasColumnType("datetime")
                .HasColumnName("pe_create_date");
            entity.Property(e => e.PeCreateUser).HasColumnName("pe_create_user");
            entity.Property(e => e.PeDeleteDate)
                .HasColumnType("datetime")
                .HasColumnName("pe_delete_date");
            entity.Property(e => e.PeDeleteUser).HasColumnName("pe_delete_user");
            entity.Property(e => e.PeDireccion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("pe_direccion");
            entity.Property(e => e.PeEstado).HasColumnName("pe_estado");
            entity.Property(e => e.PeFechaNacimiento)
                .HasColumnType("date")
                .HasColumnName("pe_fechaNacimiento");
            entity.Property(e => e.PeIdTipoDocumento).HasColumnName("pe_idTipoDocumento");
            entity.Property(e => e.PeNombre)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("pe_nombre");
            entity.Property(e => e.PeNumeroDocumento)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("pe_numeroDocumento");
            entity.Property(e => e.PeUpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("pe_update_date");
            entity.Property(e => e.PeUpdateUser).HasColumnName("pe_update_user");

            entity.HasOne(d => d.PeIdTipoDocumentoNavigation).WithMany(p => p.TblPersonas)
                .HasForeignKey(d => d.PeIdTipoDocumento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tbl_perso__pe_id__2F10007B");
        });

        modelBuilder.Entity<TblRol>(entity =>
        {
            entity.HasKey(e => e.RoIdRol).HasName("PK__tbl_rol__DEC9EC9EAD3E0078");

            entity.ToTable("tbl_rol");

            entity.Property(e => e.RoIdRol)
                .ValueGeneratedNever()
                .HasColumnName("ro_idRol");
            entity.Property(e => e.RoDescripcion)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("ro_descripcion");
            entity.Property(e => e.RoEstado).HasColumnName("ro_estado");
            entity.Property(e => e.RoNombre)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("ro_nombre");
        });

        modelBuilder.Entity<TblSacramento>(entity =>
        {
            entity.HasKey(e => e.ScIdSacramento).HasName("PK__tbl_sacr__DF1C6ED661087010");

            entity.ToTable("tbl_sacramentos");

            entity.Property(e => e.ScIdSacramento)
                .ValueGeneratedNever()
                .HasColumnName("sc_idSacramento");
            entity.Property(e => e.ScCreateDate)
                .HasColumnType("datetime")
                .HasColumnName("sc_create_date");
            entity.Property(e => e.ScCreateUser).HasColumnName("sc_create_user");
            entity.Property(e => e.ScDeleteDate)
                .HasColumnType("datetime")
                .HasColumnName("sc_delete_date");
            entity.Property(e => e.ScDeleteUser).HasColumnName("sc_delete_user");
            entity.Property(e => e.ScFechaSacramento)
                .HasColumnType("date")
                .HasColumnName("sc_fechaSacramento");
            entity.Property(e => e.ScIdTipoSacramento).HasColumnName("sc_idTipoSacramento");
            entity.Property(e => e.ScIdpersona).HasColumnName("sc_idpersona");
            entity.Property(e => e.ScMadre)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("sc_madre");
            entity.Property(e => e.ScMadrina)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("sc_madrina");
            entity.Property(e => e.ScNumeroPartida)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("sc_numeroPartida");
            entity.Property(e => e.ScObservaciones)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("sc_observaciones");
            entity.Property(e => e.ScPadre)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("sc_padre");
            entity.Property(e => e.ScPadrino)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("sc_padrino");
            entity.Property(e => e.ScParroco)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("sc_parroco");
            entity.Property(e => e.ScUpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("sc_update_date");
            entity.Property(e => e.ScUpdateUser).HasColumnName("sc_update_user");

            entity.HasOne(d => d.ScIdSacramentoNavigation).WithOne(p => p.TblSacramento)
                .HasForeignKey<TblSacramento>(d => d.ScIdTipoSacramento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tbl_sacra__sc_id__36B12243");

            entity.HasOne(d => d.ScIdpersonaNavigation).WithMany(p => p.TblSacramentos)
                .HasForeignKey(d => d.ScIdpersona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tbl_sacra__sc_id__35BCFE0A");
        });

        modelBuilder.Entity<TblTipoDocumento>(entity =>
        {
            entity.HasKey(e => e.TdIdTipoDocumento).HasName("PK__tbl_Tipo__76D773EF0F77BDFB");

            entity.ToTable("tbl_Tipo_Documentos");

            entity.Property(e => e.TdIdTipoDocumento).HasColumnName("td_idTipoDocumento");
            entity.Property(e => e.TdAbreviacion)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("td_abreviacion");
            entity.Property(e => e.TdEstado).HasColumnName("td_estado");
            entity.Property(e => e.TdNombre)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("td_nombre");
        });

        modelBuilder.Entity<TblTipoSacramento>(entity =>
        {
            entity.HasKey(e => e.TsIdTipoSacramento).HasName("PK__tbl_tipo__3CD075E48F30F21E");

            entity.ToTable("tbl_tipo_sacramentos");

            entity.Property(e => e.TsIdTipoSacramento)
                .ValueGeneratedNever()
                .HasColumnName("ts_idTipoSacramento");
            entity.Property(e => e.TsCreateDate)
                .HasColumnType("datetime")
                .HasColumnName("ts_create_date");
            entity.Property(e => e.TsCreateUser).HasColumnName("ts_create_user");
            entity.Property(e => e.TsDeleteDate)
                .HasColumnType("datetime")
                .HasColumnName("ts_delete_date");
            entity.Property(e => e.TsDeleteUser).HasColumnName("ts_delete_user");
            entity.Property(e => e.TsDescripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ts_descripcion");
            entity.Property(e => e.TsEstado).HasColumnName("ts_estado");
            entity.Property(e => e.TsNombre)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("ts_nombre");
            entity.Property(e => e.TsRequerimiento)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ts_requerimiento");
            entity.Property(e => e.TsUpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("ts_update_date");
            entity.Property(e => e.TsUpdateUser).HasColumnName("ts_update_user");
        });

        modelBuilder.Entity<TblUserRol>(entity =>
        {
            entity.HasKey(e => e.UrIdUserRol).HasName("PK__tbl_User__BDFDB1E35882934B");

            entity.ToTable("tbl_User_rol");

            entity.Property(e => e.UrIdUserRol)
                .ValueGeneratedNever()
                .HasColumnName("ur_idUserRol");
            entity.Property(e => e.UrEstado).HasColumnName("ur_estado");
            entity.Property(e => e.UrIdRol).HasColumnName("ur_idRol");
            entity.Property(e => e.UrIdUsuario).HasColumnName("ur_idUsuario");

            entity.HasOne(d => d.UrIdRolNavigation).WithMany(p => p.TblUserRols)
                .HasForeignKey(d => d.UrIdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tbl_User___ur_id__4D94879B");

            entity.HasOne(d => d.UrIdUsuarioNavigation).WithMany(p => p.TblUserRols)
                .HasForeignKey(d => d.UrIdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tbl_User___ur_id__4E88ABD4");
        });

        modelBuilder.Entity<TblUsuario>(entity =>
        {
            entity.HasKey(e => e.UsIdusuario).HasName("PK__tbl_usua__3BB39D3051877A6E");

            entity.ToTable("tbl_usuarios");

            entity.Property(e => e.UsIdusuario)
                .ValueGeneratedNever()
                .HasColumnName("us_idusuario");

            entity.Property(e => e.UsUserName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("us_userName");
            entity.Property(e => e.UsPass)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("us_pass");
            entity.Property(e => e.UsImage)
                .IsRequired(false)
                .HasColumnName("us_image");

            entity.Property(e => e.UsCreateDate)
                .HasColumnType("datetime")
                .HasColumnName("us_create_date");
            entity.Property(e => e.UsCreateUser).HasColumnName("us_create_user");
            entity.Property(e => e.UsDeleteDate)
                .HasColumnType("datetime")
                .HasColumnName("us_delete_date");
            entity.Property(e => e.UsDeleteUser).HasColumnName("us_delete_user");
            entity.Property(e => e.UsDireccion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("us_direccion");
            entity.Property(e => e.UsEstado).HasColumnName("us_estado");
            entity.Property(e => e.UsFechaNacimiento)
                .HasColumnType("date")
                .HasColumnName("us_fechaNacimiento");
            entity.Property(e => e.UsIdTipoDocumento).HasColumnName("us_idTipoDocumento");
            entity.Property(e => e.UsNombre)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("us_nombre");
            entity.Property(e => e.UsNumerodocumento)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("us_numerodocumento");
            entity.Property(e => e.UsUpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("us_update_date");
            entity.Property(e => e.UsUpdateUser).HasColumnName("us_update_user");

            entity.HasOne(d => d.UsIdTipoDocumentoNavigation).WithMany(p => p.TblUsuarios)
                .HasForeignKey(d => d.UsIdTipoDocumento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tbl_usuar__us_id__3F466844");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
