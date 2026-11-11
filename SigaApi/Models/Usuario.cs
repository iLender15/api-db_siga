using System;
using System.Collections.Generic;

namespace SigaApi.Models;

public partial class Usuario
{
    public Guid IdUsuario { get; set; }

    public Guid? IdEmpresa { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Telefono { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public bool EsExterno { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<AuditoriaLog> AuditoriaLogs { get; set; } = new List<AuditoriaLog>();

    public virtual ICollection<EventoTarea> EventoTareas { get; set; } = new List<EventoTarea>();

    public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();

    public virtual ICollection<Fotografium> Fotografia { get; set; } = new List<Fotografium>();

    public virtual Empresa? IdEmpresaNavigation { get; set; }

    public virtual ICollection<ResponsableEstablecimiento> ResponsableEstablecimientos { get; set; } = new List<ResponsableEstablecimiento>();

    public virtual ICollection<Tarea> TareaIdUsuarioCreadorNavigations { get; set; } = new List<Tarea>();

    public virtual ICollection<Tarea> TareaIdUsuarioEjecutorNavigations { get; set; } = new List<Tarea>();
}
