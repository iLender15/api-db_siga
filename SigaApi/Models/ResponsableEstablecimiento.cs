using System;
using System.Collections.Generic;

namespace SigaApi.Models;

public partial class ResponsableEstablecimiento
{
    public Guid IdEstablecimiento { get; set; }

    public Guid IdUsuario { get; set; }

    public string? RolAsignado { get; set; }

    public DateOnly FechaDesde { get; set; }

    public DateOnly? FechaHasta { get; set; }

    public virtual Establecimiento IdEstablecimientoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
