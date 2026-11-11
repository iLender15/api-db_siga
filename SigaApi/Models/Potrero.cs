using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace SigaApi.Models;

public partial class Potrero
{
    public Guid IdPotrero { get; set; }

    public Guid IdEstablecimiento { get; set; }

    public string NombreCodigo { get; set; } = null!;

    public Polygon? PoligonoLimites { get; set; }

    public double SuperficieHa { get; set; }

    public string Estado { get; set; } = null!;

    public Guid? IdRegistroSustituto { get; set; }

    public virtual ICollection<Campana> Campanas { get; set; } = new List<Campana>();

    public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();

    public virtual Establecimiento IdEstablecimientoNavigation { get; set; } = null!;

    public virtual Potrero? IdRegistroSustitutoNavigation { get; set; }

    public virtual ICollection<Potrero> InverseIdRegistroSustitutoNavigation { get; set; } = new List<Potrero>();

    public virtual ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
}
