using System;
using System.Collections.Generic;

namespace SigaApi.Models;

public partial class AuditoriaLog
{
    public Guid IdAuditoria { get; set; }

    public Guid IdUsuario { get; set; }

    public DateTime FechaHoraServidor { get; set; }

    public string EntidadAfectada { get; set; } = null!;

    public Guid IdRegistroAfectado { get; set; }

    public string Accion { get; set; } = null!;

    public string? DatosAnterioresJson { get; set; }

    public string DatosNuevosJson { get; set; } = null!;

    public string? IpDispositivoOrigen { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
