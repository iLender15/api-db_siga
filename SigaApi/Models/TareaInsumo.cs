using System;
using System.Collections.Generic;

namespace SigaApi.Models;

public partial class TareaInsumo
{
    public Guid IdTareaInsumo { get; set; }

    public Guid IdTarea { get; set; }

    public Guid IdInsumo { get; set; }

    public double CantidadUtilizada { get; set; }

    public string UnidadMedida { get; set; } = null!;

    public double DosisPorHa { get; set; }

    public decimal CostoUnitarioMomento { get; set; }

    public decimal CostoSubtotalInsumo { get; set; }

    public virtual Tarea IdTareaNavigation { get; set; } = null!;
}
