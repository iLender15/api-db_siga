using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace SigaApi.Models;

public partial class Fotografium
{
    public Guid IdFotografia { get; set; }

    public Guid? IdEvento { get; set; }

    public Guid? IdTarea { get; set; }

    public string UrlAlmacenamiento { get; set; } = null!;

    public string HashIntegridadSha256 { get; set; } = null!;

    public Guid IdUsuarioAutor { get; set; }

    public DateTime FechaHoraCaptura { get; set; }

    public Point UbicacionGpsCaptura { get; set; } = null!;

    public string EstadoRegistro { get; set; } = null!;

    public virtual Evento? IdEventoNavigation { get; set; }

    public virtual Tarea? IdTareaNavigation { get; set; }

    public virtual Usuario IdUsuarioAutorNavigation { get; set; } = null!;
}
