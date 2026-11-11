using System;
using System.Collections.Generic;

namespace SigaApi.Models;

public partial class TareaMaquinarium
{
    public Guid IdTareaMaquinaria { get; set; }

    public Guid IdTarea { get; set; }

    public Guid IdMaquinaria { get; set; }

    public double HorasHectareasTrabajadas { get; set; }

    public decimal CostoAsociadoMaquinaria { get; set; }

    public virtual Tarea IdTareaNavigation { get; set; } = null!;
}
