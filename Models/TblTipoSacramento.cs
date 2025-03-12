using System;
using System.Collections.Generic;

namespace WebApi_SGI_T.Models;

public partial class TblTipoSacramento
{
    public int TsIdTipoSacramento { get; set; }

    public string TsNombre { get; set; } = null!;

    public string TsDescripcion { get; set; } = null!;

    public string? TsRequerimiento { get; set; }

    public int TsEstado { get; set; }

    public int? TsCreateUser { get; set; }

    public DateTime? TsCreateDate { get; set; }

    public int? TsUpdateUser { get; set; }

    public DateTime? TsUpdateDate { get; set; }

    public int? TsDeleteUser { get; set; }

    public DateTime? TsDeleteDate { get; set; }

    public virtual TblSacramento? TblSacramento { get; set; }
}

