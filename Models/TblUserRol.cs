using System;
using System.Collections.Generic;

namespace WebApi_SGI_T.Models;

public partial class TblUserRol
{
    public int UrIdUserRol { get; set; }

    public int UrIdRol { get; set; }

    public int UrIdUsuario { get; set; }

    public byte UrEstado { get; set; }

    public virtual TblRol UrIdRolNavigation { get; set; } = null!;

    public virtual TblUsuario UrIdUsuarioNavigation { get; set; } = null!;
}
