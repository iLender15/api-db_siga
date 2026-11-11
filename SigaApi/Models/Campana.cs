using System;
using System.Collections.Generic;

namespace SigaApi.Models;

public partial class Campana
{
    public Guid IdCampana { get; set; }

    public Guid IdPotrero { get; set; }

    public Guid IdCultivo { get; set; }

    public string NombreCampana { get; set; } = null!;

    public DateOnly FechaInicioPreparacion { get; set; }

    public DateOnly? FechaFinCierre { get; set; }

    public string EstadoCampana { get; set; } = null!;

    public Guid? IdRegistroSustituto { get; set; }

    public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();

    public virtual Potrero IdPotreroNavigation { get; set; } = null!;

    public virtual Campana? IdRegistroSustitutoNavigation { get; set; }

    public virtual ICollection<Campana> InverseIdRegistroSustitutoNavigation { get; set; } = new List<Campana>();

    public virtual ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
}
