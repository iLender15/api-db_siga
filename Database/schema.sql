-- Habilitar la extensión PostGIS (Obligatorio para datos espaciales)
CREATE EXTENSION IF NOT EXISTS postgis;

-- 1. Catálogos y Entidades Base
CREATE TABLE Empresa (
    id_empresa UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    razon_social VARCHAR(255) NOT NULL,
    nombre_comercial VARCHAR(255),
    rut_identificacion VARCHAR(50),
    estado VARCHAR(50) NOT NULL DEFAULT 'Activo',
    fecha_alta TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE TABLE Usuario (
    id_usuario UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    id_empresa UUID NULL REFERENCES Empresa(id_empresa),
    nombre_completo VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    telefono VARCHAR(50),
    password_hash VARCHAR(255) NOT NULL,
    rol VARCHAR(50) NOT NULL,
    es_externo BOOLEAN NOT NULL DEFAULT FALSE,
    estado VARCHAR(50) NOT NULL DEFAULT 'Activo'
);

-- 2. Estructura Organizativa y Geográfica
CREATE TABLE Establecimiento (
    id_establecimiento UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    id_empresa UUID NOT NULL REFERENCES Empresa(id_empresa),
    nombre VARCHAR(255) NOT NULL,
    ubicacion_descripcion TEXT,
    coordenadas_centroide GEOGRAPHY(Point, 4326), -- Punto GPS central
    superficie_total_ha DOUBLE PRECISION,
    estado VARCHAR(50) NOT NULL DEFAULT 'Activo',
    id_registro_sustituto UUID NULL REFERENCES Establecimiento(id_establecimiento)
);

CREATE TABLE Responsable_Establecimiento (
    id_establecimiento UUID NOT NULL REFERENCES Establecimiento(id_establecimiento),
    id_usuario UUID NOT NULL REFERENCES Usuario(id_usuario),
    rol_asignado VARCHAR(100),
    fecha_desde DATE NOT NULL,
    fecha_hasta DATE NULL,
    PRIMARY KEY (id_establecimiento, id_usuario)
);

CREATE TABLE Potrero (
    id_potrero UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    id_establecimiento UUID NOT NULL REFERENCES Establecimiento(id_establecimiento),
    nombre_codigo VARCHAR(100) NOT NULL,
    poligono_limites GEOGRAPHY(Polygon, 4326), -- Polígono con coordenadas WGS 84
    superficie_ha DOUBLE PRECISION NOT NULL,
    estado VARCHAR(50) NOT NULL DEFAULT 'Activo',
    id_registro_sustituto UUID NULL REFERENCES Potrero(id_potrero)
);

-- 3. Campañas y Operaciones (Con inmutabilidad)
CREATE TABLE Campana (
    id_campana UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    id_potrero UUID NOT NULL REFERENCES Potrero(id_potrero),
    id_cultivo UUID NOT NULL,
    nombre_campana VARCHAR(255) NOT NULL,
    fecha_inicio_preparacion DATE NOT NULL,
    fecha_fin_cierre DATE NULL,
    estado_campana VARCHAR(50) NOT NULL DEFAULT 'Planificada',
    id_registro_sustituto UUID NULL REFERENCES Campana(id_campana)
);

CREATE TABLE Evento (
    id_evento UUID PRIMARY KEY, -- Generado en el cliente móvil (UUID v4)
    id_campana UUID NOT NULL REFERENCES Campana(id_campana),
    id_potrero UUID NOT NULL REFERENCES Potrero(id_potrero),
    id_tipo_evento UUID NOT NULL,
    subtipo_agente VARCHAR(255),
    fecha_hora_ocurrencia TIMESTAMP NOT NULL,
    fecha_hora_sincronizacion TIMESTAMP NOT NULL DEFAULT NOW(),
    ubicacion_gps GEOGRAPHY(Point, 4326) NOT NULL,
    descripcion TEXT,
    id_usuario_registro UUID NOT NULL REFERENCES Usuario(id_usuario),
    estado_resolucion VARCHAR(50) NOT NULL DEFAULT 'Pendiente',
    estado_registro VARCHAR(50) NOT NULL DEFAULT 'Activo',
    id_registro_sustituto UUID NULL REFERENCES Evento(id_evento)
);

CREATE TABLE Tarea (
    id_tarea UUID PRIMARY KEY, -- Generado en el cliente móvil (UUID v4)
    id_campana UUID NOT NULL REFERENCES Campana(id_campana),
    id_potrero UUID NOT NULL REFERENCES Potrero(id_potrero),
    id_tipo_tarea UUID NOT NULL,
    id_usuario_creador UUID NOT NULL REFERENCES Usuario(id_usuario),
    id_usuario_ejecutor UUID NULL REFERENCES Usuario(id_usuario),
    es_contratista BOOLEAN NOT NULL DEFAULT FALSE,
    fecha_programada DATE NOT NULL,
    fecha_hora_realizacion TIMESTAMP NULL,
    costo_mano_obra_servicio NUMERIC(12,2) NOT NULL DEFAULT 0,
    costo_total_calculado NUMERIC(12,2) NOT NULL DEFAULT 0,
    estado_tarea VARCHAR(50) NOT NULL DEFAULT 'Pendiente',
    id_registro_sustituto UUID NULL REFERENCES Tarea(id_tarea)
);

-- 4. Relaciones, Metadatos y Evidencia
CREATE TABLE Evento_Tarea (
    id_evento_tarea UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    id_evento UUID NOT NULL REFERENCES Evento(id_evento),
    id_tarea UUID NOT NULL REFERENCES Tarea(id_tarea),
    soluciono_problema VARCHAR(50) NULL,
    fecha_hora_comprobacion TIMESTAMP NULL,
    tiempo_transcurrido_horas DOUBLE PRECISION NULL,
    id_usuario_evaluador UUID NULL REFERENCES Usuario(id_usuario),
    observaciones_resultado TEXT NULL
);

CREATE TABLE Fotografia (
    id_fotografia UUID PRIMARY KEY, -- Generado en el cliente móvil (UUID v4)
    id_evento UUID NULL REFERENCES Evento(id_evento),
    id_tarea UUID NULL REFERENCES Tarea(id_tarea),
    url_almacenamiento TEXT NOT NULL,
    hash_integridad_sha256 VARCHAR(256) NOT NULL,
    id_usuario_autor UUID NOT NULL REFERENCES Usuario(id_usuario),
    fecha_hora_captura TIMESTAMP NOT NULL,
    ubicacion_gps_captura GEOGRAPHY(Point, 4326) NOT NULL,
    estado_registro VARCHAR(50) NOT NULL DEFAULT 'Activo'
);

CREATE TABLE Tarea_Insumo (
    id_tarea_insumo UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    id_tarea UUID NOT NULL REFERENCES Tarea(id_tarea),
    id_insumo UUID NOT NULL,
    cantidad_utilizada DOUBLE PRECISION NOT NULL,
    unidad_medida VARCHAR(50) NOT NULL,
    dosis_por_ha DOUBLE PRECISION NOT NULL,
    costo_unitario_momento NUMERIC(12,2) NOT NULL,
    costo_subtotal_insumo NUMERIC(12,2) NOT NULL
);

CREATE TABLE Tarea_Maquinaria (
    id_tarea_maquinaria UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    id_tarea UUID NOT NULL REFERENCES Tarea(id_tarea),
    id_maquinaria UUID NOT NULL,
    horas_hectareas_trabajadas DOUBLE PRECISION NOT NULL,
    costo_asociado_maquinaria NUMERIC(12,2) NOT NULL
);

-- 5. Log de Auditoría Estricto
CREATE TABLE Auditoria_Log (
    id_auditoria UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    id_usuario UUID NOT NULL REFERENCES Usuario(id_usuario),
    fecha_hora_servidor TIMESTAMP NOT NULL DEFAULT NOW(),
    entidad_afectada VARCHAR(100) NOT NULL,
    id_registro_afectado UUID NOT NULL,
    accion VARCHAR(50) NOT NULL, -- 'INSERT', 'UPDATE_LOGICO', 'DELETE_LOGICO'
    datos_anteriores_json JSONB NULL,  -- JSONB nativo de Postgres
    datos_nuevos_json JSONB NOT NULL,  -- JSONB nativo de Postgres
    ip_dispositivo_origen VARCHAR(50) NULL
);
CREATE INDEX idx_establecimiento_coordenadas ON Establecimiento USING GIST (coordenadas_centroide);
CREATE INDEX idx_potrero_poligono ON Potrero USING GIST (poligono_limites);
CREATE INDEX idx_evento_ubicacion ON Evento USING GIST (ubicacion_gps);
CREATE INDEX idx_fotografia_ubicacion ON Fotografia USING GIST (ubicacion_gps_captura);

CREATE INDEX idx_campana_potrero ON Campana(id_potrero);
CREATE INDEX idx_evento_campana ON Evento(id_campana);
CREATE INDEX idx_tarea_campana ON Tarea(id_campana);
