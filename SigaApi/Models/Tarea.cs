using System;
using System.Collections.Generic;

namespace SigaApi.Models;

public partial class Tarea
{
    public Guid IdTarea { get; set; }

    public Guid IdCampana { get; set; }

    public Guid IdPotrero { get; set; }

    public Guid IdTipoTarea { get; set; }

    public Guid IdUsuarioCreador { get; set; }

    public Guid? IdUsuarioEjecutor { get; set; }

    public bool EsContratista { get; set; }

    public DateOnly FechaProgramada { get; set; }

    public DateTime? FechaHoraRealizacion { get; set; }

    public decimal CostoManoObraServicio { get; set; }

    public decimal CostoTotalCalculado { get; set; }

    public string EstadoTarea { get; set; } = null!;

    public Guid? IdRegistroSustituto { get; set; }

    public virtual ICollection<EventoTarea> EventoTareas { get; set; } = new List<EventoTarea>();

    public virtual ICollection<Fotografium> Fotografia { get; set; } = new List<Fotografium>();

    public virtual Campana IdCampanaNavigation { get; set; } = null!;

    public virtual Potrero IdPotreroNavigation { get; set; } = null!;

    public virtual Tarea? IdRegistroSustitutoNavigation { get; set; }

    public virtual Usuario IdUsuarioCreadorNavigation { get; set; } = null!;

    public virtual Usuario? IdUsuarioEjecutorNavigation { get; set; }

    public virtual ICollection<Tarea> InverseIdRegistroSustitutoNavigation { get; set; } = new List<Tarea>();

    public virtual ICollection<TareaInsumo> TareaInsumos { get; set; } = new List<TareaInsumo>();

    public virtual ICollection<TareaMaquinarium> TareaMaquinaria { get; set; } = new List<TareaMaquinarium>();
}
