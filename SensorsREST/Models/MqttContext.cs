using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SensorsREST.Models;

public partial class MqttContext : DbContext
{
    public MqttContext()
    {
    }

    public MqttContext(DbContextOptions<MqttContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SensorDatum> SensorData { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Port=54321;Database=mqtt;Username=admin;Password=admin");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SensorDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sensor_data_pkey");

            entity.ToTable("sensor_data");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Timestamp)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("timestamp");
            entity.Property(e => e.Topic)
                .HasMaxLength(50)
                .HasColumnName("topic");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
