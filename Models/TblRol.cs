using System;
using System.Collections.Generic;

namespace WebApi_SGI_T.Models;

public partial class TblRol
{
    public int RoIdRol { get; set; }

    public string RoNombre { get; set; } = null!;

    public string RoDescripcion { get; set; } = null!;

    public byte RoEstado { get; set; }

    public virtual ICollection<TblUserRol> TblUserRols { get; set; } = new List<TblUserRol>();
}
