using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace SigaApi.Models;

public partial class Establecimiento
{
    public Guid IdEstablecimiento { get; set; }

    public Guid IdEmpresa { get; set; }

    public string Nombre { get; set; } = null!;

    public string? UbicacionDescripcion { get; set; }

    public Point? CoordenadasCentroide { get; set; }

    public double? SuperficieTotalHa { get; set; }

    public string Estado { get; set; } = null!;

    public Guid? IdRegistroSustituto { get; set; }

    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    public virtual Establecimiento? IdRegistroSustitutoNavigation { get; set; }

    public virtual ICollection<Establecimiento> InverseIdRegistroSustitutoNavigation { get; set; } = new List<Establecimiento>();

    public virtual ICollection<Potrero> Potreros { get; set; } = new List<Potrero>();

    public virtual ICollection<ResponsableEstablecimiento> ResponsableEstablecimientos { get; set; } = new List<ResponsableEstablecimiento>();
}
