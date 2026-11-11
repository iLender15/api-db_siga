using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace SigaApi.Models;

public partial class Evento
{
    public Guid IdEvento { get; set; }

    public Guid IdCampana { get; set; }

    public Guid IdPotrero { get; set; }

    public Guid IdTipoEvento { get; set; }

    public string? SubtipoAgente { get; set; }

    public DateTime FechaHoraOcurrencia { get; set; }

    public DateTime FechaHoraSincronizacion { get; set; }

    public Point UbicacionGps { get; set; } = null!;

    public string? Descripcion { get; set; }

    public Guid IdUsuarioRegistro { get; set; }

    public string EstadoResolucion { get; set; } = null!;

    public string EstadoRegistro { get; set; } = null!;

    public Guid? IdRegistroSustituto { get; set; }

    public virtual ICollection<EventoTarea> EventoTareas { get; set; } = new List<EventoTarea>();

    public virtual ICollection<Fotografium> Fotografia { get; set; } = new List<Fotografium>();

    public virtual Campana IdCampanaNavigation { get; set; } = null!;

    public virtual Potrero IdPotreroNavigation { get; set; } = null!;

    public virtual Evento? IdRegistroSustitutoNavigation { get; set; }

    public virtual Usuario IdUsuarioRegistroNavigation { get; set; } = null!;

    public virtual ICollection<Evento> InverseIdRegistroSustitutoNavigation { get; set; } = new List<Evento>();
}
