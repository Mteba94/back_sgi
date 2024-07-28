using System;
using System.Collections.Generic;

namespace WebApi_SGI_T.Models;

public partial class TblSacramento
{
    public int ScIdSacramento { get; set; }

    public int ScIdTipoSacramento { get; set; }

    public int ScIdpersona { get; set; }

    public DateTime ScFechaSacramento { get; set; }

    public string? ScNumeroPartida { get; set; }

    public string? ScPadre { get; set; }

    public string? ScMadre { get; set; }

    public string? ScPadrino { get; set; }

    public string? ScMadrina { get; set; }

    public string? ScParroco { get; set; }

    public string? ScObservaciones { get; set; }

    public int ScCreateUser { get; set; }

    public DateTime ScCreateDate { get; set; }

    public int? ScUpdateUser { get; set; }

    public DateTime? ScUpdateDate { get; set; }

    public int? ScDeleteUser { get; set; }

    public DateTime? ScDeleteDate { get; set; }

    public virtual TblTipoSacramento ScIdSacramentoNavigation { get; set; } = null!;

    public virtual TblPersona ScIdpersonaNavigation { get; set; } = null!;
}
