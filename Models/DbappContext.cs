using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace App.Models;

public partial class DbappContext : DbContext
{
    public DbappContext()
    {
    }

    public DbappContext(DbContextOptions<DbappContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__Clientes__71ABD0A73571DB87");

            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.Codigo).HasMaxLength(50);
            entity.Property(e => e.Correo).HasMaxLength(255);
            entity.Property(e => e.Direccion).HasMaxLength(255);
            entity.Property(e => e.FechaAlta).HasColumnType("date");
            entity.Property(e => e.Nombre).HasMaxLength(255);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.ServicioId).HasName("PK__Servicio__D5AEEC223C8B98B9");

            entity.Property(e => e.ServicioId).HasColumnName("ServicioID");
            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.TipoCable).HasMaxLength(50);
            entity.Property(e => e.TipoServicio).HasMaxLength(50);
            entity.Property(e => e.Ubicacion).HasMaxLength(255);

            entity.HasOne(d => d.Cliente).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.ClienteId)
                .HasConstraintName("FK__Servicios__Clien__398D8EEE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
