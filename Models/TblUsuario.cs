using System;
using System.Collections.Generic;

namespace WebApi_SGI_T.Models;

public partial class TblUsuario
{
    public int UsIdusuario { get; set; }
    public string UsUserName { get; set; } = null!;
    public string UsPass { get; set; } = null!;
    public string? UsImage { get; set; }

    public string UsNombre { get; set; } = null!;

    public DateTime UsFechaNacimiento { get; set; }

    public byte UsIdTipoDocumento { get; set; }

    public string UsNumerodocumento { get; set; } = null!;

    public byte? UsSexoId { get; set; }

    public string? UsDireccion { get; set; }

    public byte UsEstado { get; set; }

    public int UsCreateUser { get; set; }

    public DateTime UsCreateDate { get; set; }

    public int? UsUpdateUser { get; set; }

    public DateTime? UsUpdateDate { get; set; }

    public int? UsDeleteUser { get; set; }

    public DateTime? UsDeleteDate { get; set; }

    public virtual ICollection<TblUserRol> TblUserRols { get; set; } = new List<TblUserRol>();

    public virtual TblTipoDocumento UsIdTipoDocumentoNavigation { get; set; } = null!;

    public virtual TblSexo? UsSexoNavigation { get; set; }
}
