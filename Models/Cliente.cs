using System;
using System.Collections.Generic;

namespace App.Models;

public partial class Cliente
{
    public int ClienteId { get; set; }

    public string? Nombre { get; set; }

    public string? Codigo { get; set; }

    public DateTime? FechaAlta { get; set; }

    public string? Direccion { get; set; }

    public string? Correo { get; set; }

    public string? Telefono { get; set; }

    public bool? EsClienteCable { get; set; }

    public bool? EsClienteInternet { get; set; }

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
