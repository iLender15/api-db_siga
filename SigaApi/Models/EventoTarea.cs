using System;
using System.Collections.Generic;

namespace SigaApi.Models;

public partial class EventoTarea
{
    public Guid IdEventoTarea { get; set; }

    public Guid IdEvento { get; set; }

    public Guid IdTarea { get; set; }

    public string? SolucionoProblema { get; set; }

    public DateTime? FechaHoraComprobacion { get; set; }

    public double? TiempoTranscurridoHoras { get; set; }

    public Guid? IdUsuarioEvaluador { get; set; }

    public string? ObservacionesResultado { get; set; }

    public virtual Evento IdEventoNavigation { get; set; } = null!;

    public virtual Tarea IdTareaNavigation { get; set; } = null!;

    public virtual Usuario? IdUsuarioEvaluadorNavigation { get; set; }
}
