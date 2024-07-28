using System;
using System.Collections.Generic;

namespace WebApi_SGI_T.Models;

public partial class TblPersona
{
    public int PeIdpersona { get; set; }

    public string PeNombre { get; set; } = null!;

    public DateTime PeFechaNacimiento { get; set; }

    public byte PeIdTipoDocumento { get; set; }

    public string PeNumeroDocumento { get; set; } = null!;

    public string? PeDireccion { get; set; }

    public byte PeEstado { get; set; }

    public int? PeCreateUser { get; set; }

    public DateTime? PeCreateDate { get; set; }

    public int? PeUpdateUser { get; set; }

    public DateTime? PeUpdateDate { get; set; }

    public int? PeDeleteUser { get; set; }

    public DateTime? PeDeleteDate { get; set; }

    public virtual TblTipoDocumento PeIdTipoDocumentoNavigation { get; set; } = null!;

    public virtual ICollection<TblSacramento> TblSacramentos { get; set; } = new List<TblSacramento>();
}
