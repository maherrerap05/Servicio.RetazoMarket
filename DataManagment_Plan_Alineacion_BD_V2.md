# Plan de implementación: alineación de DataManagment con DataAccess V2

## 1. Objetivo

Alinear `Servicio.RetazoMarket.DataManagment` con la versión actual de DataAccess y con los cambios ya aplicados en PostgreSQL.

DataManagment continuará cumpliendo su responsabilidad de:

- Orquestar repositorios y queries de DataAccess.
- Convertir entidades en modelos de datos y viceversa.
- Coordinar persistencia y transacciones.
- Exponer operaciones de datos a Business sin incorporar reglas de negocio puras.

La capa solo mantendrá referencia hacia DataAccess. No se conectará directamente con Business ni Api.

## 2. Alcance

- Propagar cantidades decimales de materiales.
- Eliminar `stock_descuento` de los modelos y mapeadores.
- Incorporar los campos nuevos de productos.
- Propagar el filtro `tiene_descuentos`.
- Incorporar completamente el acceso de datos para descuentos.
- Agregar descuentos al `UnitOfWork`.
- Revisar personalizaciones y códigos de movimientos.
- Mantener la transacción del pago simulado sin añadir reglas nuevas.
- Recuperar la compilación de DataManagment y clasificar los impactos restantes.

Quedan fuera de esta alineación:

- Cálculo del precio base.
- Cálculo o aplicación monetaria de descuentos.
- Validaciones de negocio sobre porcentajes, cantidades mínimas o coherencia del indicador.
- Validación semántica de los JSON de personalización.
- Diseño definitivo de pagos parciales.
- Integraciones con pasarelas reales.
- Cambios en Business o Api.
- Pruebas de integración contra PostgreSQL.

## 3. Estado inicial confirmado

- [x] DataAccess compila sin errores ni advertencias.
- [x] La solución completa expone errores de tipos y referencias obsoletas en DataManagment.
- [x] `MaterialDataModel.stock_actual` todavía es `int`.
- [x] `MovimientoMaterialDataModel.cantidad` todavía es `int`.
- [x] `MaterialMovimientoResumenDataModel.stock_actual` todavía es `int`.
- [x] `ProductoMaterialDataModel.cantidad_req` todavía es `int`.
- [x] `ProductoMaterialResumenDataModel.cantidad_req` todavía es `int`.
- [x] `MaterialFiltroDataModel` todavía utiliza stocks mínimos y máximos enteros.
- [x] `IMovimientoMaterialDataService` todavía recibe `stock_resultante` como entero.
- [x] `ProductoDataModel` todavía contiene `stock_descuento`.
- [x] Los campos nuevos de producto aún no están modelados ni mapeados.
- [x] `ProductoDataService` todavía invoca la firma anterior de búsqueda.
- [x] No existen modelos, mapper ni servicio de descuentos.
- [x] `UnitOfWork` todavía no expone el repositorio ni el query de descuentos.

## 4. Orden de implementación

```text
Cantidades decimales
→ productos
→ descuentos
→ UnitOfWork
→ personalizaciones y movimientos
→ revisión transversal
→ compilación de DataManagment
→ compilación de la solución
```

## 5. Fase DM-1: cantidades decimales de materiales

### 5.1. Modelos

- [x] Cambiar `MaterialDataModel.stock_actual` de `int` a `decimal`.
- [x] Cambiar `MaterialFiltroDataModel.stockMinimo` de `int?` a `decimal?`.
- [x] Cambiar `MaterialFiltroDataModel.stockMaximo` de `int?` a `decimal?`.
- [x] Cambiar `MovimientoMaterialDataModel.cantidad` de `int` a `decimal`.
- [x] Cambiar `MaterialMovimientoResumenDataModel.stock_actual` de `int` a `decimal`.
- [x] Cambiar `ProductoMaterialDataModel.cantidad_req` de `int` a `decimal`.
- [x] Cambiar `ProductoMaterialResumenDataModel.cantidad_req` de `int` a `decimal`.
- [x] Mantener `ProveedorMaterialResumenDataModel.cantidad_min` como `int`.
- [x] Mantener stocks y cantidades de productos terminados como `int`.

Archivos principales:

- `Models/MaterialDataModel.cs`.
- `Models/MaterialFiltroDataModel.cs`.
- `Models/MovimientoMaterialDataModel.cs`.
- `Models/ProductoMaterialDataModel.cs`.
- `Models/ProductoDataModel.cs`.

### 5.2. Mapeadores

- [x] Verificar el mapeo decimal bidireccional en `MaterialDataMapper`.
- [x] Verificar el mapeo decimal bidireccional en `MovimientoMaterialDataMapper`.
- [x] Verificar el mapeo decimal bidireccional en `ProductoMaterialDataMapper`.
- [x] Ajustar el resumen de materiales dentro de `ProductoDataMapper`.
- [x] No introducir conversiones explícitas a `int` ni redondeos.

### 5.3. Interfaces y servicios

- [x] Cambiar `stock_resultante` a `decimal` en `IMovimientoMaterialDataService.CrearConActualizacionStockAsync`.
- [x] Cambiar `stock_resultante` a `decimal` en `MovimientoMaterialDataService`.
- [x] Propagar directamente los filtros decimales hacia `MaterialQueryRepository.BuscarAsync`.
- [x] Mantener el movimiento y la actualización de stock dentro de la transacción existente.
- [x] No decidir el signo del movimiento ni calcular el stock en DataManagment; esos valores serán entregados por Business.

Archivos principales:

- `Interfaces/IMovimientoMaterialDataService.cs`.
- `Services/MovimientoMaterialDataService.cs`.
- `Services/MaterialDataService.cs`.

## 6. Fase DM-2: modelo y mapeo de productos

### 6.1. ProductoDataModel

- [x] Eliminar `stock_descuento`.
- [x] Agregar `string tiene_descuentos`.
- [x] Agregar `decimal porcentaje_gastos_fijos`.
- [x] Agregar `decimal porcentaje_gastos_operativos`.
- [x] Mantener `porcentaje_margen_ganancia` como `decimal`.
- [x] Mantener `stock_actual` como `int`.
- [x] Mantener `colores` como `JsonElement`.
- [x] Agregar una colección de resumen de descuentos para exponer las reglas cargadas en el agregado.

### 6.2. ProductoFiltroDataModel

- [x] Agregar `string? tiene_descuentos`.
- [x] Mantener los filtros de precio como `decimal?`.
- [x] Mantener `conStock` como `bool?`.

### 6.3. ProductoDataMapper

- [x] Eliminar todos los usos de `stock_descuento`.
- [x] Mapear `tiene_descuentos` en ambas direcciones.
- [x] Mapear `porcentaje_gastos_fijos` en ambas direcciones.
- [x] Mapear `porcentaje_gastos_operativos` en ambas direcciones.
- [x] Mapear correctamente `cantidad_req` como decimal en los resúmenes.
- [x] Mapear la colección de descuentos cuando se encuentre cargada.
- [x] Mantener la clonación segura de `JsonDocument` a `JsonElement`.

### 6.4. ProductoDataService

- [x] Propagar `filtro.tiene_descuentos` a la nueva firma de `ProductoQueryRepository.BuscarAsync`.
- [x] Mantener CRUD lógico de productos sin calcular precios ni descuentos.
- [x] No modificar automáticamente `tiene_descuentos` desde este servicio genérico.
- [x] Reservar la coherencia entre el indicador y las reglas activas para Business.

## 7. Fase DM-3: modelos de descuentos

### 7.1. DescuentoDataModel

- [x] Crear `Models/DescuentoDataModel.cs`.
- [x] Agregar `id_descuento`, `id_producto`, `cantidad_minima`, `porcentaje` y `estado`.
- [x] Agregar un resumen opcional del producto para consultas administrativas.
- [x] No agregar auditoría ni eliminación física inexistentes en la tabla.

### 7.2. DescuentoFiltroDataModel

- [x] Crear `Models/DescuentoFiltroDataModel.cs`.
- [x] Agregar filtros opcionales por producto, cantidad mínima, porcentaje y estado.
- [x] Agregar `PageNumber` con valor predeterminado `1`.
- [x] Agregar `PageSize` con valor predeterminado `10`.

### 7.3. Resumen dentro de productos

- [x] Crear `DescuentoResumenDataModel` porque `ProductoDataModel` expone las reglas incluidas por DataAccess. Adelantado en la Fase DM-2.
- [x] Incluir únicamente identificador, cantidad mínima, porcentaje y estado.
- [x] Evitar referencias circulares entre modelos completos de producto y descuento.

## 8. Fase DM-4: mapper de descuentos

- [x] Crear `Mappers/DescuentoDataMapper.cs`.
- [x] Implementar `ToDataModel(DescuentoEntity)`.
- [x] Implementar `ToEntity(DescuentoDataModel)`.
- [x] Implementar `ApplyToEntity` para actualizaciones controladas.
- [x] Mapear el resumen opcional del producto sin generar ciclos.
- [x] No calcular montos, precios finales ni porcentajes acumulados.

## 9. Fase DM-5: contrato y servicio de descuentos

### 9.1. IDescuentoDataService

- [x] Crear `Interfaces/IDescuentoDataService.cs`.
- [x] Definir consulta por ID.
- [x] Definir listado completo.
- [x] Definir listado por producto.
- [x] Definir búsqueda administrativa paginada.
- [x] Definir consulta de reglas activas por producto.
- [x] Definir consulta del tramo aplicable por producto y cantidad comprada.
- [x] Definir creación.
- [x] Definir actualización.
- [x] Definir inactivación lógica mediante `estado = 'INA'`.
- [x] Definir validación de existencia por producto y cantidad mínima, con ID excluido opcional.

### 9.2. DescuentoDataService

- [x] Crear `Services/DescuentoDataService.cs`.
- [x] Orquestar `DescuentoRepository` para CRUD.
- [x] Orquestar `DescuentoQueryRepository` para filtros y selección de tramo.
- [x] Convertir resultados con `DescuentoDataMapper`.
- [x] Convertir paginación mediante `DataPagedResultMapper`.
- [x] Ejecutar `SaveChangesAsync` después de comandos.
- [x] Implementar la eliminación lógica únicamente cambiando el estado.
- [x] Devolver `null` o `false` cuando el registro no exista, siguiendo el patrón actual.
- [x] No validar todavía la coherencia de `ProductoEntity.tiene_descuentos`.
- [x] No aplicar el porcentaje al precio dentro de esta capa.

## 10. Fase DM-6: UnitOfWork

### 10.1. IUnitOfWork

- [x] Agregar `IDescuentoRepository DescuentoRepository`.
- [x] Agregar `DescuentoQueryRepository DescuentoQueryRepository`.
- [x] Mantener la interfaz dependiente únicamente de DataAccess.

### 10.2. UnitOfWork

- [x] Crear `DescuentoRepository` con el contexto compartido.
- [x] Crear `DescuentoQueryRepository` con el mismo contexto.
- [x] Exponer ambas propiedades.
- [x] Mantener una única transacción compartida para todas las operaciones.
- [x] Conservar `Dispose` y `DisposeAsync` sin cambios funcionales.

## 11. Fase DM-7: personalizaciones y movimientos

### 11.1. Personalizaciones

- [x] Confirmar que `PersonalizacionDataModel.tipo_valor` admita como texto los cinco códigos.
- [x] Mantener `valores_json` como `JsonElement`.
- [x] Mantener el mapper basado en `JsonDataMapper`.
- [x] No validar la forma interna del objeto JSON en DataManagment.
- [x] No agregar imágenes, archivos, DTF, dibujos ni diseños.

### 11.2. Movimientos

- [x] Confirmar que los modelos y filtros no restrinjan los códigos `ING`, `EGR`, `AJP` y `AJN`.
- [x] Confirmar que no existan referencias vigentes a `ENT`, `SAL` o `AJU`.
- [x] Mantener cantidades de productos como `int`.
- [x] Mantener cantidades de materiales como `decimal`.
- [x] Mantener cantidades siempre positivas; el tipo indicará el sentido.
- [x] Mantener la atomicidad de actualización de stock mediante `UnitOfWork`.

## 12. Fase DM-8: pago simulado y pedidos

- [x] Confirmar que el pago simulado siga usando cantidades enteras de productos terminados.
- [x] Confirmar que los egresos de productos continúen registrándose con `EGR`.
- [x] Confirmar que el flujo conserve una sola transacción para validar, descontar, mover y cambiar el pedido a `REA`.
- [x] Confirmar que los cambios decimales de materiales no alteren `PagoSimuladoDataResult`.
- [x] No incorporar pagos parciales ni reglas del 50/50 en esta fase.
- [x] No aplicar descuentos dentro de `PedidoDataService`; esa composición pertenecerá a Business.

## 13. Fase DM-9: revisión transversal

- [x] Eliminar todas las referencias a `stock_descuento` de DataManagment.
- [x] Buscar cantidades de materiales todavía declaradas como `int`.
- [x] Confirmar que stocks de productos, pedidos, favoritos y movimientos de productos permanezcan como `int`.
- [x] Confirmar que `tiene_descuentos` se propague desde entidad hasta modelo y filtro.
- [x] Confirmar que los dos porcentajes de gastos se propaguen bidireccionalmente.
- [x] Confirmar que descuentos estén registrados en interfaz, implementación y servicios.
- [x] Confirmar que no exista cálculo de precios o descuentos en DataManagment.
- [x] Confirmar que no se agreguen referencias hacia Business o Api.
- [x] Confirmar que no se realicen consultas directas al `DbContext` fuera de `UnitOfWork`; los servicios usan repositorios y queries expuestos.

Comandos sugeridos:

```powershell
rg -n "stock_descuento|\b(AJU|ENT|SAL)\b" Servicio.RetazoMarket.DataManagment
rg -n "int.*(stock_actual|cantidad_req)|int cantidad" Servicio.RetazoMarket.DataManagment
rg -n "Servicio\.RetazoMarket\.(Business|Api)" Servicio.RetazoMarket.DataManagment
```

## 14. Fase DM-10: compilación

- [x] Compilar DataAccess como prerrequisito.
- [x] Compilar `Servicio.RetazoMarket.DataManagment.csproj`.
- [x] Corregir todos los errores de tipos, firmas y miembros obsoletos dentro de DataManagment.
- [x] Confirmar cero advertencias nuevas relevantes.
- [x] Compilar la solución completa para identificar impactos posteriores.
- [x] Clasificar separadamente cualquier error perteneciente a Business o Api.
- [x] No corregir capas exteriores como parte de esta implementación.
- [x] No ejecutar pruebas de integración contra PostgreSQL todavía.

Comandos previstos:

```powershell
dotnet build .\Servicio.RetazoMarket.DataAccess\Servicio.RetazoMarket.DataAccess.csproj
dotnet build .\Servicio.RetazoMarket.DataManagment\Servicio.RetazoMarket.DataManagment.csproj
dotnet build .\Servicio.RetazoMarket.slnx
```

### 14.1. Resultado de compilación

- DataAccess: compilación correcta, cero advertencias y cero errores.
- DataManagment: compilación correcta, cero advertencias y cero errores.
- Business: compila actualmente como biblioteca.
- Api: pendiente porque todavía no contiene el punto de entrada `Main`.
- Solución completa: un único error `CS5001` perteneciente exclusivamente a Api.
- No se realizó ninguna prueba ni conexión contra PostgreSQL.

## 15. Impactos previstos para Business

Estas tareas se documentan, pero no se implementarán en DataManagment:

- [x] Identificar que Business debe adoptar cantidades decimales para materiales.
- [x] Identificar que Business debe validar los cuatro tipos de movimientos.
- [x] Identificar que Business debe validar los cinco tipos de personalización y el contenido de cada JSON.
- [x] Identificar que Business debe calcular costo base, gastos, margen y precio final.
- [x] Identificar que Business debe mantener coherencia entre `tiene_descuentos` y reglas activas.
- [x] Identificar que Business debe validar tramos sin duplicados desde la perspectiva de negocio.
- [x] Identificar que Business debe seleccionar el tramo aplicable y calcular el descuento monetario.
- [x] Identificar que Business debe conservar en `PRO_X_PED` el porcentaje y monto realmente aplicados.
- [x] Identificar que Business debe validar la cantidad mínima de tres unidades para sábanas personalizadas.
- [x] Identificar que Business debe validar materiales suficientes al registrar o incrementar productos terminados.

Estas casillas confirman la identificación del trabajo futuro; las reglas no se implementan dentro de DataManagment.

## 16. Criterios de finalización

- [x] Los modelos reflejan todos los tipos y campos actuales de DataAccess.
- [x] Las cantidades decimales llegan sin pérdida desde entidad hasta modelo y servicio.
- [x] `stock_descuento` ya no existe en DataManagment.
- [x] Los campos nuevos de productos están mapeados en ambas direcciones.
- [x] El filtro `tiene_descuentos` llega correctamente a DataAccess.
- [x] Existen modelos, mapper, interfaz y servicio de descuentos.
- [x] El repositorio y query de descuentos están disponibles mediante `IUnitOfWork`.
- [x] La inactivación de descuentos es lógica y no física.
- [x] La consulta del tramo aplicable no calcula dinero ni acumula porcentajes.
- [x] Personalizaciones y movimientos admiten los códigos actuales.
- [x] El pago simulado conserva su transacción y no fue alterado indebidamente.
- [x] DataManagment solo depende de DataAccess.
- [x] DataManagment compila sin errores ni advertencias nuevas relevantes.
- [x] Los impactos pendientes para Business quedan identificados.
- [x] No se realizaron pruebas de integración contra PostgreSQL.

## 17. Próximo paso

```text
DataManagment alineado y compilado
→ revisar el documento de reglas confirmadas
→ crear plan de Business
→ implementar validadores, DTOs, mappers y servicios de negocio
→ compilar capas internas
→ configurar Api
→ ejecutar pruebas integrales contra PostgreSQL
```
