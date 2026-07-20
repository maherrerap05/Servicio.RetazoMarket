# Plan de implementación: alineación de DataAccess con la base de datos V2

## 1. Objetivo

Alinear `Servicio.RetazoMarket.DataAccess` con los cambios estructurales aplicados en PostgreSQL después de confirmar las reglas de negocio sobre materiales, descuentos, costos, personalizaciones y movimientos.

Este plan modifica únicamente DataAccess. La corrección de modelos, mapeadores, interfaces y servicios de DataManagment se realizará después de que DataAccess compile correctamente.

## 2. Alcance confirmado

- Cantidades de materiales con precisión decimal de tres posiciones.
- Nueva tabla `DESCUENTOS`, asociada directamente con `PRODUCTOS`.
- Nuevo indicador `PRODUCTOS.TIENE_DESCUENTOS`.
- Nuevos porcentajes editables de gastos fijos y operativos.
- Margen de ganancia editable entre `0` inclusive y `100` exclusivo.
- Eliminación de `PRODUCTOS.STOCK_DESCUENTO`.
- `PERSONALIZACION.VALORES_JSON` como objeto JSONB enriquecido.
- Tipos de personalización `LISTA`, `MATERIAL`, `TEXTO`, `MEDIDA` y `COLOR`.
- Tipos de movimiento `ING`, `EGR`, `AJP` y `AJN`.
- Cantidades de movimientos siempre mayores que cero.

Quedan fuera de este cambio:

- Tabla de producción.
- Tablas definitivas de pagos parciales.
- Pasarelas reales de pago.
- Imágenes de referencia para personalizaciones.
- Implementación de reglas de descuento y cálculo de precios en Business.
- Pruebas de integración completas contra PostgreSQL.

## 3. Estado inicial detectado

- [x] La estructura final fue comprobada directamente en PostgreSQL.
- [x] `MaterialEntity.stock_actual` todavía usa `int`.
- [x] `MovimientoMaterialEntity.cantidad` todavía usa `int`.
- [x] `ProductoMaterialEntity.cantidad_req` todavía usa `int`.
- [x] `ProductoEntity` todavía contiene `stock_descuento`.
- [x] `ProductoEntity` todavía no contiene los tres campos nuevos.
- [x] DataAccess todavía no contiene la entidad ni el acceso a datos de `DESCUENTOS`.
- [x] `ProductoConfiguration` conserva los constraints anteriores de stock y margen.
- [x] `PersonalizacionConfiguration` solo admite `LISTA` y `MATERIAL`.
- [x] Las configuraciones de movimientos todavía no declaran los nuevos checks.

## 4. Orden de implementación

La alineación se realizará en este orden:

```text
Entidades existentes
→ nueva entidad Descuento
→ configuraciones de EF Core
→ DbContext y relaciones
→ repositorio de descuentos
→ consultas de descuentos
→ consultas afectadas por tipos decimales
→ revisión global de referencias obsoletas
→ compilación de DataAccess
```

## 5. Fase DA-1: cantidades decimales

### 5.1. Entidades

- [x] Cambiar `MaterialEntity.stock_actual` de `int` a `decimal`.
- [x] Cambiar `MovimientoMaterialEntity.cantidad` de `int` a `decimal`.
- [x] Cambiar `ProductoMaterialEntity.cantidad_req` de `int` a `decimal`.
- [x] Mantener `MovimientoProductoEntity.cantidad` como `int`, porque los productos terminados se contabilizan por unidades completas.
- [x] Mantener `ProductoEntity.stock_actual` como `int` por la misma razón.

Archivos:

- `Entities/MaterialEntity.cs`.
- `Entities/MovimientoMaterialEntity.cs`.
- `Entities/ProductoMaterialEntity.cs`.

### 5.2. Configuraciones

- [x] Configurar `MaterialEntity.stock_actual` con `HasPrecision(12, 3)`.
- [x] Configurar `MovimientoMaterialEntity.cantidad` con `HasPrecision(12, 3)`.
- [x] Configurar `ProductoMaterialEntity.cantidad_req` con `HasPrecision(12, 3)`.
- [x] Conservar `ck_materiales_stock` con `stock_actual >= 0`.
- [x] Conservar `ck_mov_materiales_cantidad` con `cantidad > 0`.
- [x] Conservar `ck_pro_x_mat_cantidad` con `cantidad_req > 0`.

Archivos:

- `Configurations/MaterialConfiguration.cs`.
- `Configurations/MovimientoMaterialConfiguration.cs`.
- `Configurations/ProductoMaterialConfiguration.cs`.

### 5.3. Consultas afectadas

- [x] Cambiar `stockMinimo` y `stockMaximo` de `int?` a `decimal?` en `MaterialQueryRepository.BuscarAsync`.
- [x] Revisar que ninguna interfaz o repositorio DataAccess exponga cantidades de materiales como `int`.
- [x] Confirmar que las expresiones LINQ comparen valores `decimal` sin conversiones implícitas innecesarias.

Archivo principal:

- `Queries/MaterialQueryRepository.cs`.

## 6. Fase DA-2: cambios de PRODUCTOS

### 6.1. Entidad

- [x] Eliminar `ProductoEntity.stock_descuento`.
- [x] Agregar `string tiene_descuentos`.
- [x] Agregar `decimal porcentaje_gastos_fijos`.
- [x] Agregar `decimal porcentaje_gastos_operativos`.
- [x] Agregar la navegación `ICollection<DescuentoEntity> Descuentos` inicializada como lista vacía.
- [x] Mantener `porcentaje_margen_ganancia` como `decimal`.
- [x] Mantener `colores` como `JsonDocument` y columna `jsonb`.

Archivo:

- `Entities/ProductoEntity.cs`.

### 6.2. Configuración

- [x] Eliminar el mapeo de `stock_descuento`.
- [x] Cambiar `ck_productos_stock` a `stock_actual >= 0`.
- [x] Cambiar `ck_productos_margen` a margen mayor o igual que `0` y menor que `100`.
- [x] Agregar `ck_productos_tiene_descuentos` con valores `S` y `N`.
- [x] Agregar `ck_productos_gastos` con ambos porcentajes entre `0` y `100`, inclusive.
- [x] Mapear `tiene_descuentos` como `CHAR(1)`, obligatorio y con valor predeterminado `N`.
- [x] Mapear `porcentaje_gastos_fijos` como `NUMERIC(5,2)`, obligatorio y con valor predeterminado `0`.
- [x] Mapear `porcentaje_gastos_operativos` como `NUMERIC(5,2)`, obligatorio y con valor predeterminado `0`.
- [x] Conservar `porcentaje_margen_ganancia` como `NUMERIC(10,2)` para coincidir con la columna existente.

Archivo:

- `Configurations/ProductoConfiguration.cs`.

### 6.3. Repositorio y consultas de productos

- [x] Incluir `Descuentos` cuando `ProductoRepository.ObtenerPorIdAsync` necesite devolver el agregado completo.
- [x] Cargar únicamente reglas relevantes mediante el repositorio especializado cuando no sea necesario incluir toda la colección. Su implementación se realizará en las fases DA-4 y DA-5.
- [x] Evaluar un filtro administrativo opcional por `tiene_descuentos` en `ProductoQueryRepository.BuscarAsync`. Se aprueba e incorpora como `string?`.
- [x] No filtrar productos agotados en operaciones de favoritos.
- [x] No aplicar cálculos de descuentos ni precios dentro de DataAccess.

Archivos:

- `Repositories/ProductoRepository.cs`.
- `Queries/ProductoQueryRepository.cs`.

## 7. Fase DA-3: nueva entidad DESCUENTOS

### 7.1. Entidad

- [x] Crear `Entities/DescuentoEntity.cs`. Adelantado como dependencia de `ProductoEntity` en la Fase 6.1.
- [x] Agregar `int id_descuento`.
- [x] Agregar `int id_producto`.
- [x] Agregar `int cantidad_minima`.
- [x] Agregar `decimal porcentaje`.
- [x] Agregar `string estado`.
- [x] Agregar navegación obligatoria `ProductoEntity Producto`.
- [x] No agregar propiedades inexistentes de auditoría o eliminación física.

### 7.2. Configuración de EF Core

- [x] Crear `Configurations/DescuentoConfiguration.cs`.
- [x] Mapear la tabla `descuentos` en el esquema `public`.
- [x] Configurar `id_descuento` como PK generada con `seq_descuentos_id`.
- [x] Configurar FK obligatoria hacia `productos.id_producto` con `DeleteBehavior.Restrict`.
- [x] Configurar `cantidad_minima` como entero obligatorio.
- [x] Configurar `porcentaje` como `NUMERIC(5,2)` obligatorio.
- [x] Configurar `estado` como `CHAR(3)`, obligatorio y con valor predeterminado `ACT`.
- [x] Mapear `ck_descuentos_cantidad`: `cantidad_minima > 0`.
- [x] Mapear `ck_descuentos_porcentaje`: porcentaje mayor que `0` y menor o igual que `100`.
- [x] Mapear `ck_descuentos_estado`: `ACT` o `INA`.
- [x] Configurar el índice único `(id_producto, cantidad_minima)` con el nombre real de la base.
- [x] Configurar el índice no único `ix_descuentos_producto`.
- [x] Confirmar los nombres finales de PK, FK, checks e índices contra PostgreSQL.

### 7.3. DbContext

- [x] Agregar `DbSet<DescuentoEntity> Descuentos`.
- [x] Aplicar `DescuentoConfiguration` en `OnModelCreating`.
- [x] Ubicarlo junto con el catálogo de productos.

Archivo:

- `Context/RetazoMarketDbContext.cs`.

## 8. Fase DA-4: repositorio de descuentos

### 8.1. Interfaz

- [x] Crear `Repositories/Interfaces/IDescuentoRepository.cs`.
- [x] Definir `ObtenerTodosAsync`.
- [x] Definir `ObtenerPorIdAsync`.
- [x] Definir `ObtenerParaActualizarAsync`.
- [x] Definir `ObtenerPorProductoAsync`.
- [x] Definir `AgregarAsync`.
- [x] Definir `Actualizar`.
- [x] Definir validación de existencia por `(id_producto, cantidad_minima)`.
- [x] Permitir excluir un `id_descuento` en la validación de duplicados para actualizaciones.
- [x] No agregar eliminación física.

### 8.2. Implementación

- [x] Crear `Repositories/DescuentoRepository.cs`.
- [x] Usar `AsNoTracking` en consultas de lectura.
- [x] Usar una consulta con seguimiento para actualizar o inactivar.
- [x] Ordenar reglas de un producto por `cantidad_minima` ascendente para administración.
- [x] Implementar eliminación lógica cambiando `estado` mediante las capas superiores; el repositorio solo debe exponer actualización.
- [x] Usar `CancellationToken` en todos los métodos asíncronos.

## 9. Fase DA-5: consultas especializadas de descuentos

- [x] Crear `Queries/DescuentoQueryRepository.cs`.
- [x] Implementar búsqueda administrativa paginada por producto, cantidad mínima, porcentaje y estado.
- [x] Implementar consulta de reglas activas por producto.
- [x] Implementar selección de regla aplicable usando:

```text
ID_PRODUCTO coincidente
AND ESTADO = 'ACT'
AND CANTIDAD_MINIMA <= cantidad comprada
ORDER BY CANTIDAD_MINIMA DESC
LIMIT 1
```

- [x] Devolver la entidad o `null`; el cálculo monetario seguirá perteneciendo a Business.
- [x] No exigir stock para consultar o seleccionar una regla de descuento.
- [x] No sumar porcentajes de diferentes tramos.
- [x] Incluir paginación mediante `PagedResult<DescuentoEntity>` para administración.

## 10. Fase DA-6: personalizaciones

- [x] Ampliar `ck_personalizacion_tipo_valor` a `LISTA`, `MATERIAL`, `TEXTO`, `MEDIDA` y `COLOR`.
- [x] Agregar `ck_personalizacion_valores_json_objeto` con `jsonb_typeof(valores_json) = 'object'`.
- [x] Mantener `valores_json` como `JsonDocument`, obligatorio y `jsonb`.
- [x] Mantener `tipo_valor` con longitud máxima `10`.
- [x] No modelar imágenes, archivos, DTF, dibujos ni diseños en esta fase.
- [x] No validar la estructura interna específica de cada JSON dentro de DataAccess.

Archivo:

- `Configurations/PersonalizacionConfiguration.cs`.

## 11. Fase DA-7: movimientos

### 11.1. Movimiento de materiales

- [x] Agregar a la configuración `ck_mov_materiales_tipo` para `ING`, `EGR`, `AJP` y `AJN`.
- [x] Agregar o conservar `ck_mov_materiales_cantidad` con `cantidad > 0`.
- [x] Mantener `cantidad` como `decimal` con precisión `(12,3)`.
- [x] Mantener `tipo_movimiento` como `CHAR(3)`.

### 11.2. Movimiento de productos

- [x] Agregar `ck_mov_productos_tipo` para `ING`, `EGR`, `AJP` y `AJN`.
- [x] Agregar `ck_mov_productos_cantidad` con `cantidad > 0`.
- [x] Mantener `cantidad` como `int`.
- [x] Mantener `tipo_movimiento` como `CHAR(3)`.

### 11.3. Consultas

- [x] Confirmar que los filtros acepten los cuatro códigos nuevos.
- [x] No almacenar cantidades negativas para representar egresos o ajustes negativos.
- [x] No modificar stock automáticamente desde repositorios o queries.
- [x] Reservar la atomicidad entre stock y movimientos para los servicios y transacciones de capas superiores.

Archivos:

- `Configurations/MovimientoMaterialConfiguration.cs`.
- `Configurations/MovimientoProductoConfiguration.cs`.
- `Queries/MovimientoMaterialQueryRepository.cs`.
- `Queries/MovimientoProductoQueryRepository.cs`.

## 12. Fase DA-8: revisión transversal

- [x] Buscar todas las referencias a `stock_descuento` y eliminar las pertenecientes a DataAccess.
- [x] Buscar parámetros o propiedades `int` relacionados con stock, movimientos o recetas de materiales y convertir únicamente los tres campos acordados.
- [x] Buscar constraints antiguos que todavía fijen el margen en `50`.
- [x] Buscar constraints que todavía limiten personalización a dos tipos.
- [x] Buscar lógica que interprete `AJU`, `ENT` o `SAL` como códigos vigentes.
- [x] Confirmar que no se introduzca lógica de negocio en repositorios o configuraciones.
- [x] Confirmar que DataAccess no adquiera referencias hacia DataManagment, Business o Api.
- [x] Confirmar que no se generen migraciones de EF Core, porque la base ya fue modificada manualmente.

Comandos de revisión sugeridos:

```powershell
rg -n "stock_descuento|porcentaje_margen_ganancia = 50|LISTA.*MATERIAL|\bAJU\b|\bENT\b|\bSAL\b" Servicio.RetazoMarket.DataAccess
rg -n "int.*stock_actual|int.*cantidad_req|int.*cantidad" Servicio.RetazoMarket.DataAccess
```

## 13. Fase DA-9: compilación y validación técnica

- [x] Compilar únicamente `Servicio.RetazoMarket.DataAccess.csproj`.
- [x] Corregir errores de tipos provocados por el cambio de `int` a `decimal` dentro de DataAccess.
- [x] Compilar la solución para identificar anticipadamente impactos en DataManagment, sin corregir todavía esa capa dentro de esta fase.
- [x] Registrar en este plan todos los errores esperados por modelos y filtros de DataManagment que todavía usan `int` o `stock_descuento`.
- [x] Revisar el modelo y sus configuraciones de EF Core sin conectarse a PostgreSQL.
- [x] No ejecutar pruebas de integración contra la base hasta alinear todas las capas.

Comandos previstos:

```powershell
dotnet build .\Servicio.RetazoMarket.DataAccess\Servicio.RetazoMarket.DataAccess.csproj
dotnet build .\Servicio.RetazoMarket.slnx
```

### 13.1. Resultado de validación

- DataAccess: compilación correcta, sin advertencias ni errores.
- Solución completa: compilación detenida por impactos previstos en DataManagment y por el punto de entrada todavía pendiente en Api.
- No se detectaron errores adicionales dentro de DataAccess.
- No se realizó ninguna conexión ni escritura en PostgreSQL.

Impactos concretos detectados para la siguiente capa:

- `MaterialDataMapper`: `stock_actual` todavía se representa como `int`.
- `MovimientoMaterialDataMapper`: `cantidad` todavía se representa como `int`.
- `ProductoMaterialDataMapper`: `cantidad_req` todavía se representa como `int`.
- `ProductoDataMapper`: todavía utiliza `stock_descuento` y no mapea los campos nuevos.
- `ProductoDataService`: todavía invoca la firma anterior de `ProductoQueryRepository.BuscarAsync`.
- Api: el proyecto aún no contiene un punto de entrada `Main`; se resolverá al implementar esa capa.

## 14. Impactos previstos para DataManagment

Esta sección solo registra trabajo posterior; no se implementará durante DataAccess.

- [x] Identificar la conversión a `decimal` del stock de materiales, la cantidad de movimientos de materiales y la cantidad requerida de recetas.
- [x] Identificar la eliminación de `stock_descuento` de modelos, mapeadores y servicios de productos.
- [x] Identificar la incorporación de los tres campos nuevos de productos.
- [x] Identificar la creación de modelo, filtro, mapper, interfaz y servicio para descuentos.
- [x] Identificar la incorporación del repositorio y query de descuentos al `IUnitOfWork` y `UnitOfWork`.
- [x] Identificar la propagación del filtro opcional `tiene_descuentos`, aprobado e incorporado en DataAccess.
- [x] Identificar los ajustes de personalizaciones y movimientos a los códigos nuevos.

Estas casillas confirman que los impactos fueron identificados y documentados; su implementación pertenece a la siguiente fase de DataManagment.

## 15. Criterios de finalización de DataAccess

- [x] Las entidades coinciden con los tipos y columnas actuales de PostgreSQL.
- [x] `stock_descuento` ya no existe en DataAccess.
- [x] Los campos nuevos de productos están mapeados con precisión, obligatoriedad y defaults correctos.
- [x] `DescuentoEntity` está completamente mapeada y relacionada con `ProductoEntity`.
- [x] Existe acceso CRUD lógico y consultas paginadas para descuentos.
- [x] Existe una consulta eficiente para seleccionar el tramo aplicable.
- [x] Los constraints modelados por EF Core coinciden con PostgreSQL.
- [x] Los tipos decimales se conservan desde entidad hasta consulta.
- [x] DataAccess compila sin errores ni advertencias nuevas relevantes.
- [x] No se agregaron dependencias hacia capas exteriores.
- [x] Los impactos pendientes de DataManagment quedaron identificados.
- [x] No se realizaron pruebas de integración contra PostgreSQL durante esta fase.

## 16. Próximo paso

Una vez completado este documento:

```text
DataAccess alineado y compilado
→ actualizar el plan de DataManagment
→ alinear DataManagment
→ compilar la solución
→ diseñar Business con las reglas confirmadas
```
