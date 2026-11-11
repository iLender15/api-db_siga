using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SigaApi.Models;

namespace SigaApi.Data;

public partial class SigaDbContext : DbContext
{
    public SigaDbContext()
    {
    }

    public SigaDbContext(DbContextOptions<SigaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditoriaLog> AuditoriaLogs { get; set; }

    public virtual DbSet<Campana> Campanas { get; set; }

    public virtual DbSet<Empresa> Empresas { get; set; }

    public virtual DbSet<Establecimiento> Establecimientos { get; set; }

    public virtual DbSet<Evento> Eventos { get; set; }

    public virtual DbSet<EventoTarea> EventoTareas { get; set; }

    public virtual DbSet<Fotografium> Fotografia { get; set; }

    public virtual DbSet<Potrero> Potreros { get; set; }

    public virtual DbSet<ResponsableEstablecimiento> ResponsableEstablecimientos { get; set; }

    public virtual DbSet<Tarea> Tareas { get; set; }

    public virtual DbSet<TareaInsumo> TareaInsumos { get; set; }

    public virtual DbSet<TareaMaquinarium> TareaMaquinaria { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=siga_db;Username=postgres;Password=beckolinha2008", x => x.UseNetTopologySuite());

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entradas = ChangeTracker.Entries<IEntidadAuditable>();

        foreach (var entrada in entradas)
        {
            if (entrada.State == EntityState.Deleted)
            {
                entrada.State = EntityState.Modified;
                entrada.Entity.Estado = "Inactivo";
                
                // Inserción a la tabla AuditoriaLog
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("postgis");

        modelBuilder.Entity<AuditoriaLog>(entity =>
        {
            entity.HasKey(e => e.IdAuditoria).HasName("auditoria_log_pkey");

            entity.ToTable("auditoria_log");

            entity.Property(e => e.IdAuditoria)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id_auditoria");
            entity.Property(e => e.Accion)
                .HasMaxLength(50)
                .HasColumnName("accion");
            entity.Property(e => e.DatosAnterioresJson)
                .HasColumnType("jsonb")
                .HasColumnName("datos_anteriores_json");
            entity.Property(e => e.DatosNuevosJson)
                .HasColumnType("jsonb")
                .HasColumnName("datos_nuevos_json");
            entity.Property(e => e.EntidadAfectada)
                .HasMaxLength(100)
                .HasColumnName("entidad_afectada");
            entity.Property(e => e.FechaHoraServidor)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_hora_servidor");
            entity.Property(e => e.IdRegistroAfectado).HasColumnName("id_registro_afectado");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.IpDispositivoOrigen)
                .HasMaxLength(50)
                .HasColumnName("ip_dispositivo_origen");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.AuditoriaLogs)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("auditoria_log_id_usuario_fkey");
        });

        modelBuilder.Entity<Campana>(entity =>
        {
            entity.HasKey(e => e.IdCampana).HasName("campana_pkey");

            entity.ToTable("campana");

            entity.HasIndex(e => e.IdPotrero, "idx_campana_potrero");

            entity.Property(e => e.IdCampana)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id_campana");
            entity.Property(e => e.EstadoCampana)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Planificada'::character varying")
                .HasColumnName("estado_campana");
            entity.Property(e => e.FechaFinCierre).HasColumnName("fecha_fin_cierre");
            entity.Property(e => e.FechaInicioPreparacion).HasColumnName("fecha_inicio_preparacion");
            entity.Property(e => e.IdCultivo).HasColumnName("id_cultivo");
            entity.Property(e => e.IdPotrero).HasColumnName("id_potrero");
            entity.Property(e => e.IdRegistroSustituto).HasColumnName("id_registro_sustituto");
            entity.Property(e => e.NombreCampana)
                .HasMaxLength(255)
                .HasColumnName("nombre_campana");

            entity.HasOne(d => d.IdPotreroNavigation).WithMany(p => p.Campanas)
                .HasForeignKey(d => d.IdPotrero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("campana_id_potrero_fkey");

            entity.HasOne(d => d.IdRegistroSustitutoNavigation).WithMany(p => p.InverseIdRegistroSustitutoNavigation)
                .HasForeignKey(d => d.IdRegistroSustituto)
                .HasConstraintName("campana_id_registro_sustituto_fkey");
        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.IdEmpresa).HasName("empresa_pkey");

            entity.ToTable("empresa");

            entity.Property(e => e.IdEmpresa)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id_empresa");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Activo'::character varying")
                .HasColumnName("estado");
            entity.Property(e => e.FechaAlta)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_alta");
            entity.Property(e => e.NombreComercial)
                .HasMaxLength(255)
                .HasColumnName("nombre_comercial");
            entity.Property(e => e.RazonSocial)
                .HasMaxLength(255)
                .HasColumnName("razon_social");
            entity.Property(e => e.RutIdentificacion)
                .HasMaxLength(50)
                .HasColumnName("rut_identificacion");
        });

        modelBuilder.Entity<Establecimiento>(entity =>
        {
            entity.HasKey(e => e.IdEstablecimiento).HasName("establecimiento_pkey");

            entity.ToTable("establecimiento");

            entity.HasIndex(e => e.CoordenadasCentroide, "idx_establecimiento_coordenadas").HasMethod("gist");

            entity.Property(e => e.IdEstablecimiento)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id_establecimiento");
            entity.Property(e => e.CoordenadasCentroide)
                .HasColumnType("geography(Point,4326)")
                .HasColumnName("coordenadas_centroide");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Activo'::character varying")
                .HasColumnName("estado");
            entity.Property(e => e.IdEmpresa).HasColumnName("id_empresa");
            entity.Property(e => e.IdRegistroSustituto).HasColumnName("id_registro_sustituto");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .HasColumnName("nombre");
            entity.Property(e => e.SuperficieTotalHa).HasColumnName("superficie_total_ha");
            entity.Property(e => e.UbicacionDescripcion).HasColumnName("ubicacion_descripcion");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Establecimientos)
                .HasForeignKey(d => d.IdEmpresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("establecimiento_id_empresa_fkey");

            entity.HasOne(d => d.IdRegistroSustitutoNavigation).WithMany(p => p.InverseIdRegistroSustitutoNavigation)
                .HasForeignKey(d => d.IdRegistroSustituto)
                .HasConstraintName("establecimiento_id_registro_sustituto_fkey");
        });

        modelBuilder.Entity<Evento>(entity =>
        {
            entity.HasKey(e => e.IdEvento).HasName("evento_pkey");

            entity.ToTable("evento");

            entity.HasIndex(e => e.IdCampana, "idx_evento_campana");

            entity.HasIndex(e => e.UbicacionGps, "idx_evento_ubicacion").HasMethod("gist");

            entity.Property(e => e.IdEvento)
                .ValueGeneratedNever()
                .HasColumnName("id_evento");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.EstadoRegistro)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Activo'::character varying")
                .HasColumnName("estado_registro");
            entity.Property(e => e.EstadoResolucion)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Pendiente'::character varying")
                .HasColumnName("estado_resolucion");
            entity.Property(e => e.FechaHoraOcurrencia)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_hora_ocurrencia");
            entity.Property(e => e.FechaHoraSincronizacion)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_hora_sincronizacion");
            entity.Property(e => e.IdCampana).HasColumnName("id_campana");
            entity.Property(e => e.IdPotrero).HasColumnName("id_potrero");
            entity.Property(e => e.IdRegistroSustituto).HasColumnName("id_registro_sustituto");
            entity.Property(e => e.IdTipoEvento).HasColumnName("id_tipo_evento");
            entity.Property(e => e.IdUsuarioRegistro).HasColumnName("id_usuario_registro");
            entity.Property(e => e.SubtipoAgente)
                .HasMaxLength(255)
                .HasColumnName("subtipo_agente");
            entity.Property(e => e.UbicacionGps)
                .HasColumnType("geography(Point,4326)")
                .HasColumnName("ubicacion_gps");

            entity.HasOne(d => d.IdCampanaNavigation).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.IdCampana)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("evento_id_campana_fkey");

            entity.HasOne(d => d.IdPotreroNavigation).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.IdPotrero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("evento_id_potrero_fkey");

            entity.HasOne(d => d.IdRegistroSustitutoNavigation).WithMany(p => p.InverseIdRegistroSustitutoNavigation)
                .HasForeignKey(d => d.IdRegistroSustituto)
                .HasConstraintName("evento_id_registro_sustituto_fkey");

            entity.HasOne(d => d.IdUsuarioRegistroNavigation).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.IdUsuarioRegistro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("evento_id_usuario_registro_fkey");
        });

        modelBuilder.Entity<EventoTarea>(entity =>
        {
            entity.HasKey(e => e.IdEventoTarea).HasName("evento_tarea_pkey");

            entity.ToTable("evento_tarea");

            entity.Property(e => e.IdEventoTarea)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id_evento_tarea");
            entity.Property(e => e.FechaHoraComprobacion)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_hora_comprobacion");
            entity.Property(e => e.IdEvento).HasColumnName("id_evento");
            entity.Property(e => e.IdTarea).HasColumnName("id_tarea");
            entity.Property(e => e.IdUsuarioEvaluador).HasColumnName("id_usuario_evaluador");
            entity.Property(e => e.ObservacionesResultado).HasColumnName("observaciones_resultado");
            entity.Property(e => e.SolucionoProblema)
                .HasMaxLength(50)
                .HasColumnName("soluciono_problema");
            entity.Property(e => e.TiempoTranscurridoHoras).HasColumnName("tiempo_transcurrido_horas");

            entity.HasOne(d => d.IdEventoNavigation).WithMany(p => p.EventoTareas)
                .HasForeignKey(d => d.IdEvento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("evento_tarea_id_evento_fkey");

            entity.HasOne(d => d.IdTareaNavigation).WithMany(p => p.EventoTareas)
                .HasForeignKey(d => d.IdTarea)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("evento_tarea_id_tarea_fkey");

            entity.HasOne(d => d.IdUsuarioEvaluadorNavigation).WithMany(p => p.EventoTareas)
                .HasForeignKey(d => d.IdUsuarioEvaluador)
                .HasConstraintName("evento_tarea_id_usuario_evaluador_fkey");
        });

        modelBuilder.Entity<Fotografium>(entity =>
        {
            entity.HasKey(e => e.IdFotografia).HasName("fotografia_pkey");

            entity.ToTable("fotografia");

            entity.HasIndex(e => e.UbicacionGpsCaptura, "idx_fotografia_ubicacion").HasMethod("gist");

            entity.Property(e => e.IdFotografia)
                .ValueGeneratedNever()
                .HasColumnName("id_fotografia");
            entity.Property(e => e.EstadoRegistro)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Activo'::character varying")
                .HasColumnName("estado_registro");
            entity.Property(e => e.FechaHoraCaptura)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_hora_captura");
            entity.Property(e => e.HashIntegridadSha256)
                .HasMaxLength(256)
                .HasColumnName("hash_integridad_sha256");
            entity.Property(e => e.IdEvento).HasColumnName("id_evento");
            entity.Property(e => e.IdTarea).HasColumnName("id_tarea");
            entity.Property(e => e.IdUsuarioAutor).HasColumnName("id_usuario_autor");
            entity.Property(e => e.UbicacionGpsCaptura)
                .HasColumnType("geography(Point,4326)")
                .HasColumnName("ubicacion_gps_captura");
            entity.Property(e => e.UrlAlmacenamiento).HasColumnName("url_almacenamiento");

            entity.HasOne(d => d.IdEventoNavigation).WithMany(p => p.Fotografia)
                .HasForeignKey(d => d.IdEvento)
                .HasConstraintName("fotografia_id_evento_fkey");

            entity.HasOne(d => d.IdTareaNavigation).WithMany(p => p.Fotografia)
                .HasForeignKey(d => d.IdTarea)
                .HasConstraintName("fotografia_id_tarea_fkey");

            entity.HasOne(d => d.IdUsuarioAutorNavigation).WithMany(p => p.Fotografia)
                .HasForeignKey(d => d.IdUsuarioAutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fotografia_id_usuario_autor_fkey");
        });

        modelBuilder.Entity<Potrero>(entity =>
        {
            entity.HasKey(e => e.IdPotrero).HasName("potrero_pkey");

            entity.ToTable("potrero");

            entity.HasIndex(e => e.PoligonoLimites, "idx_potrero_poligono").HasMethod("gist");

            entity.Property(e => e.IdPotrero)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id_potrero");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Activo'::character varying")
                .HasColumnName("estado");
            entity.Property(e => e.IdEstablecimiento).HasColumnName("id_establecimiento");
            entity.Property(e => e.IdRegistroSustituto).HasColumnName("id_registro_sustituto");
            entity.Property(e => e.NombreCodigo)
                .HasMaxLength(100)
                .HasColumnName("nombre_codigo");
            entity.Property(e => e.PoligonoLimites)
                .HasColumnType("geography(Polygon,4326)")
                .HasColumnName("poligono_limites");
            entity.Property(e => e.SuperficieHa).HasColumnName("superficie_ha");

            entity.HasOne(d => d.IdEstablecimientoNavigation).WithMany(p => p.Potreros)
                .HasForeignKey(d => d.IdEstablecimiento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("potrero_id_establecimiento_fkey");

            entity.HasOne(d => d.IdRegistroSustitutoNavigation).WithMany(p => p.InverseIdRegistroSustitutoNavigation)
                .HasForeignKey(d => d.IdRegistroSustituto)
                .HasConstraintName("potrero_id_registro_sustituto_fkey");
        });

        modelBuilder.Entity<ResponsableEstablecimiento>(entity =>
        {
            entity.HasKey(e => new { e.IdEstablecimiento, e.IdUsuario }).HasName("responsable_establecimiento_pkey");

            entity.ToTable("responsable_establecimiento");

            entity.Property(e => e.IdEstablecimiento).HasColumnName("id_establecimiento");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.FechaDesde).HasColumnName("fecha_desde");
            entity.Property(e => e.FechaHasta).HasColumnName("fecha_hasta");
            entity.Property(e => e.RolAsignado)
                .HasMaxLength(100)
                .HasColumnName("rol_asignado");

            entity.HasOne(d => d.IdEstablecimientoNavigation).WithMany(p => p.ResponsableEstablecimientos)
                .HasForeignKey(d => d.IdEstablecimiento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("responsable_establecimiento_id_establecimiento_fkey");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ResponsableEstablecimientos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("responsable_establecimiento_id_usuario_fkey");
        });

        modelBuilder.Entity<Tarea>(entity =>
        {
            entity.HasKey(e => e.IdTarea).HasName("tarea_pkey");

            entity.ToTable("tarea");

            entity.HasIndex(e => e.IdCampana, "idx_tarea_campana");

            entity.Property(e => e.IdTarea)
                .ValueGeneratedNever()
                .HasColumnName("id_tarea");
            entity.Property(e => e.CostoManoObraServicio)
                .HasPrecision(12, 2)
                .HasColumnName("costo_mano_obra_servicio");
            entity.Property(e => e.CostoTotalCalculado)
                .HasPrecision(12, 2)
                .HasColumnName("costo_total_calculado");
            entity.Property(e => e.EsContratista).HasColumnName("es_contratista");
            entity.Property(e => e.EstadoTarea)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Pendiente'::character varying")
                .HasColumnName("estado_tarea");
            entity.Property(e => e.FechaHoraRealizacion)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_hora_realizacion");
            entity.Property(e => e.FechaProgramada).HasColumnName("fecha_programada");
            entity.Property(e => e.IdCampana).HasColumnName("id_campana");
            entity.Property(e => e.IdPotrero).HasColumnName("id_potrero");
            entity.Property(e => e.IdRegistroSustituto).HasColumnName("id_registro_sustituto");
            entity.Property(e => e.IdTipoTarea).HasColumnName("id_tipo_tarea");
            entity.Property(e => e.IdUsuarioCreador).HasColumnName("id_usuario_creador");
            entity.Property(e => e.IdUsuarioEjecutor).HasColumnName("id_usuario_ejecutor");

            entity.HasOne(d => d.IdCampanaNavigation).WithMany(p => p.Tareas)
                .HasForeignKey(d => d.IdCampana)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tarea_id_campana_fkey");

            entity.HasOne(d => d.IdPotreroNavigation).WithMany(p => p.Tareas)
                .HasForeignKey(d => d.IdPotrero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tarea_id_potrero_fkey");

            entity.HasOne(d => d.IdRegistroSustitutoNavigation).WithMany(p => p.InverseIdRegistroSustitutoNavigation)
                .HasForeignKey(d => d.IdRegistroSustituto)
                .HasConstraintName("tarea_id_registro_sustituto_fkey");

            entity.HasOne(d => d.IdUsuarioCreadorNavigation).WithMany(p => p.TareaIdUsuarioCreadorNavigations)
                .HasForeignKey(d => d.IdUsuarioCreador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tarea_id_usuario_creador_fkey");

            entity.HasOne(d => d.IdUsuarioEjecutorNavigation).WithMany(p => p.TareaIdUsuarioEjecutorNavigations)
                .HasForeignKey(d => d.IdUsuarioEjecutor)
                .HasConstraintName("tarea_id_usuario_ejecutor_fkey");
        });

        modelBuilder.Entity<TareaInsumo>(entity =>
        {
            entity.HasKey(e => e.IdTareaInsumo).HasName("tarea_insumo_pkey");

            entity.ToTable("tarea_insumo");

            entity.Property(e => e.IdTareaInsumo)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id_tarea_insumo");
            entity.Property(e => e.CantidadUtilizada).HasColumnName("cantidad_utilizada");
            entity.Property(e => e.CostoSubtotalInsumo)
                .HasPrecision(12, 2)
                .HasColumnName("costo_subtotal_insumo");
            entity.Property(e => e.CostoUnitarioMomento)
                .HasPrecision(12, 2)
                .HasColumnName("costo_unitario_momento");
            entity.Property(e => e.DosisPorHa).HasColumnName("dosis_por_ha");
            entity.Property(e => e.IdInsumo).HasColumnName("id_insumo");
            entity.Property(e => e.IdTarea).HasColumnName("id_tarea");
            entity.Property(e => e.UnidadMedida)
                .HasMaxLength(50)
                .HasColumnName("unidad_medida");

            entity.HasOne(d => d.IdTareaNavigation).WithMany(p => p.TareaInsumos)
                .HasForeignKey(d => d.IdTarea)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tarea_insumo_id_tarea_fkey");
        });

        modelBuilder.Entity<TareaMaquinarium>(entity =>
        {
            entity.HasKey(e => e.IdTareaMaquinaria).HasName("tarea_maquinaria_pkey");

            entity.ToTable("tarea_maquinaria");

            entity.Property(e => e.IdTareaMaquinaria)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id_tarea_maquinaria");
            entity.Property(e => e.CostoAsociadoMaquinaria)
                .HasPrecision(12, 2)
                .HasColumnName("costo_asociado_maquinaria");
            entity.Property(e => e.HorasHectareasTrabajadas).HasColumnName("horas_hectareas_trabajadas");
            entity.Property(e => e.IdMaquinaria).HasColumnName("id_maquinaria");
            entity.Property(e => e.IdTarea).HasColumnName("id_tarea");

            entity.HasOne(d => d.IdTareaNavigation).WithMany(p => p.TareaMaquinaria)
                .HasForeignKey(d => d.IdTarea)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tarea_maquinaria_id_tarea_fkey");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("usuario_pkey");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.Email, "usuario_email_key").IsUnique();

            entity.Property(e => e.IdUsuario)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id_usuario");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.EsExterno).HasColumnName("es_externo");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Activo'::character varying")
                .HasColumnName("estado");
            entity.Property(e => e.IdEmpresa).HasColumnName("id_empresa");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(255)
                .HasColumnName("nombre_completo");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .HasColumnName("rol");
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .HasColumnName("telefono");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdEmpresa)
                .HasConstraintName("usuario_id_empresa_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
