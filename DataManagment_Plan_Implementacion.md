# Plan de implementación de DataManagment

## 1. Objetivo

Construir de forma incremental la capa `Servicio.RetazoMarket.DataManagment`, responsable de orquestar la persistencia disponible en `DataAccess` y exponer a `Business` contratos y modelos desacoplados de Entity Framework Core y del esquema físico de PostgreSQL.

Esta capa deberá:

- Coordinar repositorios y queries de `DataAccess`.
- Centralizar `SaveChangesAsync()` y las transacciones.
- Convertir entidades de persistencia en modelos propios de datos.
- Exponer servicios de datos orientados a las necesidades de `Business`.
- Mantener aislados de `Business` el `DbContext`, los `DbSet`, las entidades y los tipos propios de EF Core.
- Permitir que varias modificaciones relacionadas se confirmen como una única unidad atómica.

Flujo arquitectónico:

```text
Api
 ↓
Business
 ↓
DataManagment
 ↓
DataAccess
 ↓
PostgreSQL
```

## 2. Alcance

Se implementarán servicios de datos para:

- Roles.
- Usuarios.
- Clientes.
- Categorías de materiales.
- Líneas.
- Métodos de pago.
- Proveedores.
- Materiales.
- Relaciones proveedor-material.
- Productos.
- Relaciones producto-material.
- Personalizaciones.
- Imágenes.
- Favoritos.
- Movimientos de materiales.
- Movimientos de productos.
- Pedidos.
- Detalles de pedidos.
- Pago simulado y actualización atómica de existencias.

Quedan fuera de esta iteración:

- Auditoría desde el backend.
- Envíos.
- Facturación electrónica.
- Integración con Deuna.
- Integración con tarjetas de crédito o débito.
- Controllers, respuestas HTTP, JWT, CORS y Swagger.
- Validaciones funcionales profundas, cálculos comerciales y autorización por roles, que pertenecen a `Business`.

## 3. Responsabilidades y límites

### DataManagment sí debe

- Consumir únicamente componentes públicos de `DataAccess`.
- Agrupar repositorios y queries mediante `IUnitOfWork`.
- Ejecutar altas, lecturas, actualizaciones y desactivaciones solicitadas por `Business`.
- Mapear `Entity` ↔ `DataModel`.
- Mapear `PagedResult<T>` → `DataPagedResult<T>`.
- Ejecutar `SaveChangesAsync()` una sola vez por unidad lógica.
- Abrir, confirmar y revertir transacciones.
- Mantener atomicidad en cambios que afecten varias tablas.
- Revalidar dentro de una transacción las condiciones técnicas sensibles a concurrencia.
- Propagar `CancellationToken` en todas las operaciones asíncronas.

### DataManagment no debe

- Referenciar `Business` ni `Api`.
- Exponer entidades de `DataAccess` a `Business`.
- Exponer `RetazoMarketDbContext`, `DbSet` o `IDbContextTransaction` en sus interfaces públicas.
- Emitir JWT o interpretar el contexto HTTP.
- Definir mensajes HTTP ni códigos de estado.
- Calcular IVA, márgenes, precios o descuentos como regla comercial.
- Decidir permisos de usuario.
- Crear endpoints.
- Administrar auditoría, envíos o facturas.

### Excepción técnica controlada

`UnitOfWork` será el único componente de esta capa que podrá conocer internamente `RetazoMarketDbContext`, porque debe centralizar `SaveChangesAsync()` y las transacciones. Ningún DataService ni contrato expuesto a `Business` accederá directamente al contexto.

## 4. Estructura prevista

```text
Servicio.RetazoMarket.DataManagment/
├── Interfaces/
│   ├── IUnitOfWork.cs
│   └── I[Modulo]DataService.cs
├── Mappers/
│   └── [Modulo]DataMapper.cs
├── Models/
│   ├── Common/
│   │   └── DataPagedResult.cs
│   └── [Modulo]/
│       ├── [Modulo]DataModel.cs
│       └── [Modulo]FiltroDataModel.cs
├── Services/
│   ├── UnitOfWork.cs
│   └── [Modulo]DataService.cs
└── Servicio.RetazoMarket.DataManagment.csproj
```

Se conservará intencionalmente el nombre actual `DataManagment` en carpetas, namespaces y proyecto.

## 5. Convenciones

- Namespace raíz: `Servicio.RetazoMarket.DataManagment`.
- Modelos principales: `[Entidad]DataModel`.
- Modelos de filtros: `[Entidad]FiltroDataModel`.
- Interfaces: `I[Entidad]DataService`.
- Servicios: `[Entidad]DataService`.
- Mapeadores: `[Entidad]DataMapper`.
- Propiedades de DataModels: PascalCase, para evitar trasladar el formato físico `snake_case` de las entidades.
- Métodos asíncronos: sufijo `Async` y `CancellationToken cancellationToken = default`.
- Fechas: se conservarán en UTC; la conversión a `America/Guayaquil` seguirá fuera de esta capa.
- Montos: `decimal`, conservando la precisión de los datos persistidos.
- Estados codificados: `string`, sin reemplazarlos todavía por enums para mantener compatibilidad entre capas.
- JSONB: se expondrá como `JsonElement` clonado en los DataModels, evitando propagar la propiedad y ciclo de vida de `JsonDocument` de las entidades.
- Colecciones públicas: `IReadOnlyCollection<T>` o `IReadOnlyList<T>`.
- Los mapeadores no ejecutarán consultas, guardados ni reglas de negocio.
- Los DataServices devolverán `null` o `bool` cuando la ausencia sea un resultado técnico esperado; `Business` decidirá qué excepción funcional corresponde.

## 6. Seguimiento general

- [x] Fase 0. Preparación, estructura y componentes comunes — completada.
- [x] Fase 1. Unit of Work y transacciones — completada.
- [x] Fase 2. Seguridad y clientes — completada.
- [x] Fase 3. Catálogos base — completada.
- [x] Fase 4. Proveedores y materiales — completada.
- [x] Fase 5. Catálogo de productos — completada.
- [x] Fase 6. Favoritos — completada.
- [x] Fase 7. Inventario y movimientos — completada.
- [x] Fase 8. Pedidos y pago simulado — completada; prueba de integración PostgreSQL diferida a la Fase 9.
- [x] Fase 9. Revisión transversal y finalización — completada; integración PostgreSQL diferida.

Una fase se marcará como completada cuando se hayan terminado sus modelos, filtros, mapeadores, contratos, servicios y validación técnica aplicable.

## 7. Orden de implementación

### Fase 0. Preparación, estructura y componentes comunes

- [x] Crear las carpetas `Interfaces`, `Mappers`, `Models`, `Models/Common` y `Services`.
- [x] Confirmar que el proyecto solo referencia `Servicio.RetazoMarket.DataAccess`.
- [x] Crear `DataPagedResult<T>` con elementos, página, tamaño, total de registros, total de páginas y navegación anterior/siguiente.
- [x] Crear `DataPagedResultMapper` para convertir `PagedResult<TEntity>` en `DataPagedResult<TModel>`.
- [x] Confirmar las convenciones para UTC, JSONB, colecciones y nombres.
- [x] Compilar `Servicio.RetazoMarket.DataManagment`: 0 errores y 0 advertencias.

### Fase 1. Unit of Work y transacciones

#### IUnitOfWork

- [x] Exponer los 18 repositorios de DataAccess.
- [x] Exponer los 10 query repositories de DataAccess.
- [x] Exponer `SaveChangesAsync()`.
- [x] Exponer métodos abstractos para iniciar, confirmar y revertir transacciones sin devolver tipos de EF Core.
- [x] Permitir seleccionar `IsolationLevel` mediante `System.Data` cuando una operación lo requiera.
- [x] Implementar `IAsyncDisposable` para liberar transacciones y contexto de forma segura.

#### UnitOfWork

- [x] Recibir `RetazoMarketDbContext` por inyección.
- [x] Inicializar los 18 repositorios y 10 queries siguiendo el patrón aprobado.
- [x] Centralizar `SaveChangesAsync()`.
- [x] Mantener internamente una única transacción activa.
- [x] Implementar `BeginTransactionAsync`, `CommitTransactionAsync` y `RollbackTransactionAsync`.
- [x] Impedir comenzar una segunda transacción mientras exista otra activa.
- [x] Liberar la transacción activa mediante `Dispose()` y `DisposeAsync()`.
- [x] Compilar y comprobar que ninguna interfaz pública exponga `DbContext` o `IDbContextTransaction`: 0 errores y 0 advertencias.

### Fase 2. Seguridad y clientes

Orden: roles → clientes → usuarios.

#### Roles

- [x] Crear `RolDataModel`; por decisión funcional no se requiere `RolFiltroDataModel`.
- [x] Crear `RolDataMapper`.
- [x] Crear `IRolDataService`.
- [x] Crear `RolDataService` para listar, obtener, buscar, crear y actualizar.
- [x] No implementar eliminación de roles porque `ROL` no tiene estado de eliminación.

#### Clientes

- [x] Crear `ClienteDataModel` y `ClienteFiltroDataModel`.
- [x] Crear modelos resumidos para relaciones cuando sean necesarios, evitando ciclos.
- [x] Crear `ClienteDataMapper`.
- [x] Crear `IClienteDataService`.
- [x] Crear `ClienteDataService` para listar, obtener, buscar, crear, actualizar y desactivar mediante `CLI_ESTADO = 'INA'`.
- [x] Permitir reactivar clientes mediante actualización de estado.
- [x] Exponer consultas técnicas de existencia y búsqueda por correo.

#### Usuarios

- [x] Crear `UsuarioDataModel` y `UsuarioFiltroDataModel`.
- [x] Incluir datos resumidos de rol y cliente sin exponer entidades.
- [x] Crear `UsuarioDataMapper`.
- [x] Crear `IUsuarioDataService`.
- [x] Crear `UsuarioDataService` para listar, obtener, buscar, crear, actualizar y desactivar mediante `USR_ESTADO = 'INA'`.
- [x] Exponer búsqueda por correo con rol y cliente para autenticación futura.
- [x] No verificar contraseñas ni generar JWT en esta capa.

#### Validación de fase

- [x] Comprobar que ningún DataModel contiene `Entity`.
- [x] Compilar con 0 errores y 0 advertencias.

### Fase 3. Catálogos base

Módulos: categorías de materiales, líneas y métodos de pago.

#### Categorías de materiales

- [x] Crear `CategoriaMaterialDataModel`.
- [x] Crear `CategoriaMaterialDataMapper`.
- [x] Crear `ICategoriaMaterialDataService` y `CategoriaMaterialDataService`.
- [x] Implementar listar, obtener, crear, actualizar, desactivar y reactivar.

#### Líneas

- [x] Crear `LineaDataModel`.
- [x] Crear `LineaDataMapper`.
- [x] Crear `ILineaDataService` y `LineaDataService`.
- [x] Implementar listar, obtener, crear, actualizar, desactivar y reactivar.

#### Métodos de pago

- [x] Crear `MetodoPagoDataModel`.
- [x] Crear `MetodoPagoDataMapper`.
- [x] Crear `IMetodoPagoDataService` y `MetodoPagoDataService`.
- [x] Implementar listar, obtener, crear y actualizar.
- [x] No implementar eliminación de métodos de pago.

#### Validación de fase

- [x] Confirmar que no se crean filtros paginados innecesarios para estos catálogos simples.
- [x] Compilar con 0 errores y 0 advertencias.

### Fase 4. Proveedores y materiales

Orden: proveedores → materiales → asociación proveedor-material.

#### Proveedores

- [x] Crear `ProveedorDataModel` y `ProveedorFiltroDataModel`.
- [x] Crear `ProveedorDataMapper`.
- [x] Crear `IProveedorDataService` y `ProveedorDataService`.
- [x] Implementar listar, obtener, buscar, crear, actualizar, desactivar y reactivar.
- [x] Exponer consultas técnicas de existencia y búsqueda por correo.

#### Materiales

- [x] Crear `MaterialDataModel` y `MaterialFiltroDataModel`.
- [x] Incluir información resumida de categoría y proveedores.
- [x] Crear `MaterialDataMapper`.
- [x] Crear `IMaterialDataService` y `MaterialDataService`.
- [x] Implementar listar, obtener, buscar, crear, actualizar, desactivar y reactivar.
- [x] Mantener el stock como dato persistente; las reglas de modificación se coordinarán con movimientos.

#### Asociación proveedor-material

- [x] Crear `ProveedorMaterialDataModel`.
- [x] Crear `ProveedorMaterialDataMapper`.
- [x] Crear `IProveedorMaterialDataService` y `ProveedorMaterialDataService`.
- [x] Implementar obtener, listar por proveedor/material, crear y actualizar condiciones de compra.
- [x] No implementar eliminación física ni lógica para `PRV_X_MAT`.

#### Validación de fase

- [x] Mapear paginación de proveedores y materiales sin exponer `PagedResult<TEntity>`.
- [x] Evitar ciclos entre proveedores y materiales mediante modelos resumidos.
- [x] Compilar con 0 errores y 0 advertencias.

### Fase 5. Catálogo de productos

Orden: productos → materiales asociados → personalizaciones → imágenes.

#### Productos

- [x] Crear `ProductoDataModel` y `ProductoFiltroDataModel`.
- [x] Incluir `Colores` como `JsonElement`.
- [x] Incluir modelos resumidos de línea, materiales, personalizaciones e imágenes.
- [x] Crear `ProductoDataMapper`.
- [x] Crear `IProductoDataService` y `ProductoDataService`.
- [x] Implementar listar, obtener detalle, buscar, crear, actualizar, desactivar y reactivar.
- [x] No recalcular costos, IVA ni margen en esta capa.

#### Relación producto-material

- [x] Crear `ProductoMaterialDataModel` y su mapper.
- [x] Crear `IProductoMaterialDataService` y su implementación.
- [x] Implementar obtener, listar por producto, crear y actualizar.
- [x] No implementar eliminación de `PRO_X_MAT`.

#### Personalizaciones

- [x] Crear `PersonalizacionDataModel` con `ValoresJson` como `JsonElement`.
- [x] Crear `PersonalizacionDataMapper`.
- [x] Crear `IPersonalizacionDataService` y su implementación.
- [x] Implementar obtener, listar por producto, crear y actualizar.
- [x] No implementar eliminación.

#### Imágenes

- [x] Crear `ImagenDataModel` y `ImagenDataMapper`.
- [x] Crear `IImagenDataService` y su implementación.
- [x] Implementar obtener, listar ordenadamente por producto, crear y actualizar.
- [x] No implementar eliminación.

#### Validación de fase

- [x] Clonar correctamente los valores JSON para evitar referencias a documentos desechados.
- [x] Evitar ciclos de navegación en modelos y mappers.
- [x] Mapear la búsqueda paginada de productos.
- [x] Compilar con 0 errores y 0 advertencias.

### Fase 6. Favoritos

- [x] Crear `FavoritoDataModel` y `FavoritoFiltroDataModel`.
- [x] Incluir un resumen del producto, línea e imágenes necesarias.
- [x] Crear `FavoritoDataMapper`.
- [x] Crear `IFavoritoDataService` y `FavoritoDataService`.
- [x] Implementar agregar, comprobar existencia, obtener y listar por cliente.
- [x] Implementar eliminación física exclusivamente para favoritos.
- [x] No validar ni filtrar favoritos por stock del producto.
- [x] Mapear la consulta paginada sin exponer entidades.
- [x] Compilar con 0 errores y 0 advertencias.

### Fase 7. Inventario y movimientos

#### Movimientos de materiales

- [x] Crear `MovimientoMaterialDataModel` y `MovimientoMaterialFiltroDataModel`.
- [x] Crear `MovimientoMaterialDataMapper`.
- [x] Crear `IMovimientoMaterialDataService` y su implementación.
- [x] Implementar crear, obtener, listar por material, filtrar y paginar.
- [x] No implementar actualización ni eliminación.

#### Movimientos de productos

- [x] Crear `MovimientoProductoDataModel` y `MovimientoProductoFiltroDataModel`.
- [x] Crear `MovimientoProductoDataMapper`.
- [x] Crear `IMovimientoProductoDataService` y su implementación.
- [x] Implementar crear, obtener, listar por producto, filtrar y paginar.
- [x] No implementar actualización ni eliminación.

#### Operaciones atómicas de inventario

- [x] Preparar métodos internos para actualizar stock y registrar el movimiento en una sola unidad de trabajo.
- [x] No decidir en esta capa si un movimiento solicitado es comercialmente válido.
- [x] Garantizar que un fallo al guardar revierta tanto el stock como el movimiento.
- [x] Compilar con 0 errores y 0 advertencias.

### Fase 8. Pedidos y pago simulado

#### Pedidos y detalles

- [x] Crear `PedidoDataModel` y `PedidoFiltroDataModel`.
- [x] Incorporar `EntregaFisica` con valores persistidos `S/N`.
- [x] Crear `ProductoPedidoDataModel` con `PersonalizacionSeleccionada` como `JsonElement`.
- [x] Crear modelos resumidos de cliente, método de pago y producto.
- [x] Crear `PedidoDataMapper` y `ProductoPedidoDataMapper`.
- [x] Crear `IPedidoDataService` y `IProductoPedidoDataService`.
- [x] Crear sus implementaciones.
- [x] Implementar listar, obtener detalle, buscar, crear y actualizar pedidos pendientes.
- [x] No implementar eliminación ni cancelación de pedidos.
- [x] Conservar precios, descuentos y personalización históricos del detalle.

#### Pago simulado atómico

- [x] Definir un contrato de resultado técnico para el pago simulado sin excepciones HTTP.
- [x] Iniciar una transacción con aislamiento adecuado para evitar carreras de stock.
- [x] Recargar con tracking el pedido `PEN`, sus detalles y productos dentro de la transacción.
- [x] Revalidar estado pendiente y existencias dentro de la transacción.
- [x] Descontar stock de productos.
- [x] Crear un movimiento de egreso por cada producto.
- [x] Registrar `FechaPago` UTC.
- [x] Cambiar el pedido a `REA`.
- [x] Ejecutar un único `SaveChangesAsync()` y confirmar la transacción.
- [x] Revertir la transacción completa ante cualquier error.
- [x] Tratar conflictos de serialización/concurrencia como resultado técnico que `Business` pueda interpretar.
- [x] No generar factura, envío ni transacción Deuna.

#### Validación de fase

- [x] Mapear `PERSONALIZACION_SELEC` sin perder su estructura JSON.
- [x] Confirmar que no se repite el mismo producto en el pedido.
- [x] Comprobar estructuralmente que un fallo revierte stock y movimientos; prueba contra PostgreSQL diferida a la Fase 9.
- [x] Compilar con 0 errores y 0 advertencias.

### Fase 9. Revisión transversal y finalización

- [x] Confirmar que `DataManagment` solo referencia `DataAccess`.
- [x] Confirmar que ningún contrato público expone Entity Framework Core.
- [x] Confirmar que ningún DataModel contiene entidades de persistencia.
- [x] Confirmar que todos los JSONB se mapean mediante copias seguras.
- [x] Confirmar que todas las fechas permanecen en UTC.
- [x] Confirmar que todos los métodos asíncronos reciben `CancellationToken` (121 contratos revisados).
- [x] Confirmar que cada alta o actualización llama a `SaveChangesAsync()` exactamente una vez por unidad lógica.
- [x] Confirmar que movimientos y pedidos no tienen eliminación.
- [x] Confirmar que favoritos es la única eliminación física.
- [x] Confirmar que auditoría, envíos y facturación no están expuestos.
- [x] Compilar `DataAccess` y `DataManagment` con 0 errores y 0 advertencias.
- [x] Pruebas de integración contra PostgreSQL diferidas hasta que las cuatro capas estén configuradas.
- [x] Documentar cualquier comprobación diferida.

#### Comprobación diferida

La validación real de persistencia, restricciones, concurrencia y rollback contra PostgreSQL se ejecutará cuando `DataAccess`, `DataManagment`, `Business` y `Api` estén configuradas. En este cierre se verificaron estáticamente las transacciones y se compiló cada una de las dos capas implementadas, pero no se abrió una conexión a la base de datos.

## 8. Componentes previstos

### Modelos comunes

- `DataPagedResult<T>`.
- Modelos resumidos de relaciones para evitar grafos circulares.
- Resultado técnico del pago simulado.

### DataServices principales

- `IRolDataService` / `RolDataService`.
- `IClienteDataService` / `ClienteDataService`.
- `IUsuarioDataService` / `UsuarioDataService`.
- `ICategoriaMaterialDataService` / `CategoriaMaterialDataService`.
- `ILineaDataService` / `LineaDataService`.
- `IMetodoPagoDataService` / `MetodoPagoDataService`.
- `IProveedorDataService` / `ProveedorDataService`.
- `IMaterialDataService` / `MaterialDataService`.
- `IProveedorMaterialDataService` / `ProveedorMaterialDataService`.
- `IProductoDataService` / `ProductoDataService`.
- `IProductoMaterialDataService` / `ProductoMaterialDataService`.
- `IPersonalizacionDataService` / `PersonalizacionDataService`.
- `IImagenDataService` / `ImagenDataService`.
- `IFavoritoDataService` / `FavoritoDataService`.
- `IMovimientoMaterialDataService` / `MovimientoMaterialDataService`.
- `IMovimientoProductoDataService` / `MovimientoProductoDataService`.
- `IPedidoDataService` / `PedidoDataService`.
- `IProductoPedidoDataService` / `ProductoPedidoDataService`.

## 9. Decisiones por tipo de operación

### Desactivación lógica

Se realizará actualizando estados existentes:

- `CLIENTE.CLI_ESTADO`.
- `USUARIO.USR_ESTADO`.
- `PROVEEDORES.PROV_ESTADO`.
- `CAT_MAT.CAT_ESTADO`.
- `MATERIALES.MAT_ESTADO`.
- `LINEAS.LIN_ESTADO`.
- `PRODUCTOS.PROD_ESTADO`.

### Sin eliminación

- Roles.
- Métodos de pago.
- Relaciones proveedor-material.
- Relaciones producto-material.
- Personalizaciones.
- Imágenes.
- Movimientos.
- Pedidos y detalles.

### Eliminación física permitida

- Únicamente `FAVORITOS`.

## 10. Estrategia de mapeo

Cada módulo deberá implementar:

```text
Entity
  ↓ ToDataModel
DataModel

DataModel
  ↓ ToEntity o ApplyToEntity
Entity
```

Reglas:

- `ToDataModel` no devolverá propiedades de navegación como entidades.
- `ToEntity` se utilizará principalmente para altas.
- Para actualizaciones se preferirá `ApplyToEntity(model, entity)`, preservando tracking, claves y relaciones no modificadas.
- Las colecciones se mapearán solo cuando hayan sido cargadas por el repositorio correspondiente.
- Los objetos relacionados usarán modelos resumidos para evitar referencias circulares.
- Los mapeadores JSON clonarán `JsonElement` o reconstruirán `JsonDocument` de forma segura.
- Los filtros se traducirán a parámetros del query repository sin incorporar reglas comerciales.

## 11. Estrategia de paginación

DataManagment no expondrá `DataAccess.Common.PagedResult<T>`.

Conversión requerida:

```text
PagedResult<TEntity>
  ↓ mapper
DataPagedResult<TDataModel>
```

`DataPagedResult<T>` contendrá:

- `Items`.
- `PageNumber`.
- `PageSize`.
- `TotalRecords`.
- `TotalPages`.
- `HasPreviousPage`.
- `HasNextPage`.

La normalización funcional de límites de página se realizará en `Business`; DataManagment propagará los valores recibidos.

## 12. Criterios de finalización

DataManagment se considerará terminado cuando:

- [x] La estructura base esté creada.
- [x] `IUnitOfWork` y `UnitOfWork` centralicen los 18 repositorios, 10 queries, guardado y transacciones.
- [x] Existan DataModels y mappers para todos los módulos del alcance.
- [x] Existan contratos e implementaciones de DataServices para todos los módulos del alcance.
- [x] Ninguna entidad de DataAccess se exponga a Business.
- [x] Ningún contrato público exponga tipos de EF Core.
- [x] La paginación esté desacoplada de DataAccess.
- [x] Los JSONB de colores y personalizaciones se conserven correctamente.
- [x] Las fechas se mantengan en UTC.
- [x] Las desactivaciones utilicen estados `INA` y no eliminaciones físicas.
- [x] Favoritos sea la única eliminación física.
- [x] Movimientos sean inmutables.
- [x] Pedidos no puedan eliminarse ni cancelarse.
- [x] El pago simulado sea atómico y resistente a concurrencia.
- [x] Auditoría, envíos y facturación permanezcan fuera del alcance.
- [x] `DataAccess` y `DataManagment` compilen con 0 errores y 0 advertencias.
- [x] Las pruebas que requieran PostgreSQL estén documentadas como diferidas hasta configurar las cuatro capas.

## 13. Método de trabajo

1. Se implementará una fase a la vez y en el orden definido.
2. Antes de crear cada tipo de archivo se revisará el ejemplo proporcionado por el usuario cuando exista.
3. Los ejemplos se usarán como formato, pero los campos y operaciones se adaptarán al modelo real de Retazo Market.
4. Después de cada grupo se compilará `Servicio.RetazoMarket.DataManagment`.
5. El checklist se actualizará únicamente con tareas realmente terminadas o verificaciones explícitamente diferidas.
6. No se avanzará a `Business` hasta cerrar los criterios de finalización de esta capa.
