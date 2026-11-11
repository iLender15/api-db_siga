using System;
using System.Collections.Generic;

namespace SigaApi.Models;

public partial class Empresa
{
    public Guid IdEmpresa { get; set; }

    public string RazonSocial { get; set; } = null!;

    public string? NombreComercial { get; set; }

    public string? RutIdentificacion { get; set; }

    public string Estado { get; set; } = null!;

    public DateTime FechaAlta { get; set; }

    public virtual ICollection<Establecimiento> Establecimientos { get; set; } = new List<Establecimiento>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
