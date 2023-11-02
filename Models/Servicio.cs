using System;
using System.Collections.Generic;

namespace App.Models;

public partial class Servicio
{
    public int ServicioId { get; set; }

    public int? ClienteId { get; set; }

    public string? TipoServicio { get; set; }

    public int? Velocidad { get; set; }

    public string? TipoCable { get; set; }

    public string? Ubicacion { get; set; }

    public virtual Cliente? Cliente { get; set; }
}
