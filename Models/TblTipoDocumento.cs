using System;
using System.Collections.Generic;

namespace WebApi_SGI_T.Models;

public partial class TblTipoDocumento
{
    public byte TdIdTipoDocumento { get; set; }

    public string TdNombre { get; set; } = null!;

    public string TdAbreviacion { get; set; } = null!;

    public byte TdEstado { get; set; }

    public virtual ICollection<TblPersona> TblPersonas { get; set; } = new List<TblPersona>();

    public virtual ICollection<TblUsuario> TblUsuarios { get; set; } = new List<TblUsuario>();
}