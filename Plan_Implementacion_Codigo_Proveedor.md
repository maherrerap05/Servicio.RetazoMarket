# Plan de implementación: código de proveedor

## 1. Objetivo

Agregar `codigo_proveedor` a `PROVEEDORES` y propagarlo por DataAccess, DataManagment y Business. El cambio permitirá:

- identificar y consultar un proveedor por su código;
- filtrar proveedores por código;
- buscar materiales asociados a un código de proveedor;
- mostrar el código junto con la información del proveedor vinculada a un material.

El código será obligatorio, único, no reutilizable mientras el registro permanezca en la tabla y se almacenará normalizado en mayúsculas, sin espacios externos.

## 2. Decisiones confirmadas

- [x] La tabla `PROVEEDORES` existe y actualmente no contiene registros.
- [x] El campo se llamará `codigo_proveedor`.
- [x] El tipo será `VARCHAR(200)`.
- [x] El campo será obligatorio.
- [x] El código será único globalmente, incluso para proveedores inactivos.
- [x] Business normalizará el valor con `Trim().ToUpperInvariant()`.
- [x] El código no se duplicará en `MATERIALES` ni en `PRV_X_MAT`.

## 3. Fase 1: modificación de PostgreSQL

### 3.1. Aplicar columna y restricciones

- [x] Ejecutar la siguiente sentencia en la base local:

```sql
BEGIN;

ALTER TABLE public.proveedores
    ADD COLUMN codigo_proveedor VARCHAR(200) NOT NULL;

ALTER TABLE public.proveedores
    ADD CONSTRAINT uq_proveedores_codigo_proveedor
        UNIQUE (codigo_proveedor);

ALTER TABLE public.proveedores
    ADD CONSTRAINT ck_proveedores_codigo_proveedor
        CHECK (
            char_length(btrim(codigo_proveedor)) > 0
            AND codigo_proveedor = upper(btrim(codigo_proveedor))
        );

COMMIT;
```

La tabla está vacía, por lo que no se requiere una actualización previa ni un valor temporal para registros existentes.

### 3.2. Verificar la estructura

- [x] Confirmar tipo, nulabilidad y longitud:

```sql
SELECT
    column_name,
    data_type,
    character_maximum_length,
    is_nullable
FROM information_schema.columns
WHERE table_schema = 'public'
  AND table_name = 'proveedores'
  AND column_name = 'codigo_proveedor';
```

- [x] Confirmar las restricciones:

```sql
SELECT
    conname AS nombre_constraint,
    pg_get_constraintdef(oid) AS definicion
FROM pg_constraint
WHERE conrelid = 'public.proveedores'::regclass
  AND conname IN (
      'uq_proveedores_codigo_proveedor',
      'ck_proveedores_codigo_proveedor'
  )
ORDER BY conname;
```

### 3.3. Prueba reversible

- [x] Ejecutar una prueba dentro de una transacción y revertirla:

```sql
BEGIN;

INSERT INTO public.proveedores
(
    codigo_proveedor,
    prov_nombre,
    prov_telefono,
    prov_correo,
    prov_direccion,
    prov_estado
)
VALUES
(
    'PRV-TEST-001',
    'Proveedor de prueba',
    '0999999999',
    'proveedor.prueba@example.com',
    'Dirección de prueba',
    'ACT'
);

SELECT id_proveedor, codigo_proveedor, prov_nombre
FROM public.proveedores
WHERE codigo_proveedor = 'PRV-TEST-001';

ROLLBACK;
```

- [x] Confirmar que no quedaron datos de prueba.

### 3.4. Reversión del cambio, solo si fuera necesaria

```sql
BEGIN;

ALTER TABLE public.proveedores
    DROP CONSTRAINT IF EXISTS ck_proveedores_codigo_proveedor;

ALTER TABLE public.proveedores
    DROP CONSTRAINT IF EXISTS uq_proveedores_codigo_proveedor;

ALTER TABLE public.proveedores
    DROP COLUMN IF EXISTS codigo_proveedor;

COMMIT;
```

## 4. Fase 2: alinear DataAccess

### 4.1. Entidad y configuración

- [x] Agregar `string codigo_proveedor` a `ProveedorEntity`.
- [x] Mapear la columna como requerida y con longitud máxima de 200 en `ProveedorConfiguration`.
- [x] Incorporar el check `ck_proveedores_codigo_proveedor` en la configuración.
- [x] Configurar el índice único `uq_proveedores_codigo_proveedor`.

Configuración esperada:

```csharp
builder.Property(p => p.codigo_proveedor)
    .HasColumnName("codigo_proveedor")
    .IsRequired()
    .HasMaxLength(200);

builder.HasIndex(p => p.codigo_proveedor)
    .IsUnique()
    .HasDatabaseName("uq_proveedores_codigo_proveedor");
```

### 4.2. Repositorio de proveedores

- [x] Agregar `ObtenerPorCodigoAsync` a `IProveedorRepository` y `ProveedorRepository`.
- [x] Agregar `ExistePorCodigoAsync` a `IProveedorRepository` y `ProveedorRepository`.
- [x] Mantener las consultas con `AsNoTracking()` cuando no haya actualización.
- [x] No excluir proveedores inactivos al validar duplicados, para impedir la reutilización del código.

### 4.3. Filtros y consultas

- [x] Agregar `codigoProveedor` a `ProveedorQueryRepository.BuscarAsync`.
- [x] Aplicar coincidencia parcial con `EF.Functions.ILike` en el filtro paginado de proveedores.
- [x] Agregar `codigoProveedor` a `MaterialQueryRepository.BuscarAsync`.
- [x] Filtrar materiales mediante la relación existente:

```csharp
query = query.Where(m => m.Proveedores.Any(pm =>
    pm.Proveedor.codigo_proveedor == codigoProveedor));
```

- [x] Incluir la relación proveedor-material necesaria para evitar consultas adicionales al mostrar proveedores de un material.
- [x] No agregar `codigo_proveedor` a `MaterialEntity` ni `ProveedorMaterialEntity`.

### 4.4. Verificación de DataAccess

- [x] Confirmar que el modelo EF coincide con PostgreSQL.
- [x] Compilar DataAccess con cero errores y sin advertencias nuevas relevantes.

## 5. Fase 3: alinear DataManagment

### 5.1. Modelos

- [x] Agregar `codigo_proveedor` a `ProveedorDataModel`.
- [x] Agregar `codigo_proveedor` a `ProveedorFiltroDataModel`.
- [x] Agregar `codigo_proveedor` a `MaterialFiltroDataModel`.
- [x] Agregar `codigo_proveedor` a `ProveedorMaterialDataModel` como dato descriptivo de respuesta, sin tratarlo como columna de `PRV_X_MAT`.

### 5.2. Mappers

- [x] Mapear `codigo_proveedor` entre `ProveedorEntity` y `ProveedorDataModel`.
- [x] Exponer el código desde la navegación `Proveedor` en `ProveedorMaterialDataMapper`.
- [x] No modificar claves primarias ni foráneas existentes.

### 5.3. Servicios e interfaces

- [x] Agregar `ObtenerPorCodigoAsync` a `IProveedorDataService` y `ProveedorDataService`.
- [x] Agregar `ExistePorCodigoAsync` a `IProveedorDataService` y `ProveedorDataService`.
- [x] Propagar el filtro por código en la búsqueda paginada de proveedores.
- [x] Propagar el filtro por código de proveedor en la búsqueda paginada de materiales.
- [x] Mantener las operaciones de guardado mediante `IUnitOfWork`.

### 5.4. Verificación de DataManagment

- [x] Compilar DataAccess y DataManagment.
- [x] Confirmar cero errores y sin advertencias nuevas relevantes.

## 6. Fase 4: alinear Business

### 6.1. DTOs de proveedor

- [x] Agregar `codigo_proveedor` a `CrearProveedorRequest`.
- [x] Agregar `codigo_proveedor` a `ActualizarProveedorRequest`.
- [x] Agregar `codigo_proveedor` a `ProveedorFiltroRequest`.
- [x] Agregar `codigo_proveedor` a `ProveedorResponse`.
- [x] El cliente no proporcionará `id_proveedor`; continuará generado por la base de datos.

### 6.2. DTOs de materiales y asociaciones

- [x] Agregar `codigo_proveedor` a `MaterialFiltroRequest`.
- [x] Agregar `codigo_proveedor` a `ProveedorMaterialResponse` para mostrarlo junto al proveedor asociado.
- [x] Mantener `MaterialResponse` sin un único código de proveedor, porque un material puede tener varios proveedores.

### 6.3. Validación y normalización

- [x] Hacer obligatorio `codigo_proveedor` al crear y actualizar proveedores.
- [x] Rechazar valores vacíos o mayores a 200 caracteres.
- [x] Normalizar con `Trim().ToUpperInvariant()` antes de consultar o persistir.
- [x] Validar duplicados al crear.
- [x] Validar duplicados al actualizar, excluyendo el mismo `id_proveedor`.
- [x] Validar paginación y longitud del código cuando se use como filtro.

### 6.4. Mapper, interfaz y servicio

- [x] Mapear el código en `ProveedorBusinessMapper` para creación, actualización y respuesta.
- [x] Mapear el código en `ProveedorMaterialBusinessMapper.ToResponse`.
- [x] Agregar `ObtenerPorCodigoAsync` a `IProveedorService` y `ProveedorService`.
- [x] Propagar el código normalizado en filtros de proveedores y materiales.
- [x] Lanzar `ValidationException` ante duplicados o formato inválido.

### 6.5. Verificación de Business

- [x] Compilar DataAccess, DataManagment y Business.
- [x] Confirmar cero errores y sin advertencias nuevas relevantes.

## 7. Fase 5: preparación para Api

- [x] Planificar `GET /api/v1/proveedores/codigo/{codigo}`.
- [x] Incluir `codigo_proveedor` en los contratos de creación, actualización, consulta y listado de proveedores.
- [x] Permitir `codigo_proveedor` como query parameter del listado paginado de proveedores.
- [x] Permitir `codigo_proveedor` como query parameter del listado paginado de materiales.
- [x] Mostrar el código en las respuestas de relaciones proveedor-material.
- [x] Mantener autorización administrativa conforme a `Matriz_Permisos_Business.md`.

Esta fase solo prepara los contratos. Los controladores se implementarán durante la construcción de Api.

> Fase cerrada como preparación contractual. Los endpoints, query parameters y policies se materializarán en los controladores durante la implementación de Api.

## 8. Criterios de finalización

- [x] PostgreSQL contiene `codigo_proveedor VARCHAR(200) NOT NULL`.
- [x] PostgreSQL impide códigos vacíos, sin normalizar o duplicados.
- [x] DataAccess representa la columna y permite consultas por código.
- [x] DataManagment propaga el campo sin duplicarlo en entidades incorrectas.
- [x] Business valida, normaliza y expone el código.
- [x] Se pueden filtrar proveedores por código.
- [x] Se pueden filtrar materiales por código de proveedor.
- [x] Las respuestas proveedor-material muestran el código del proveedor.
- [x] Las tres capas implementadas compilan sin errores.
- [x] No se alteró el comportamiento de eliminación lógica.
- [x] No se duplicó `codigo_proveedor` en `MATERIALES` o `PRV_X_MAT`.
