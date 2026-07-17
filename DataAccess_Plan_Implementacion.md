# Plan de implementación de DataAccess

## 1. Objetivo

Construir de forma incremental la capa `Servicio.RetazoMarket.DataAccess`, responsable exclusiva de mapear y persistir el modelo PostgreSQL de El Retazo Market mediante Entity Framework Core y Npgsql.

Al terminar esta etapa, el proyecto debe compilar y proporcionar a `DataManagment` entidades, repositorios y consultas paginadas suficientes para implementar el núcleo administrativo del sprint.

## 2. Alcance de esta iteración

Se implementará acceso a datos para:

- Roles.
- Usuarios.
- Clientes.
- Favoritos.
- Proveedores.
- Categorías de materiales.
- Materiales.
- Relaciones proveedor-material.
- Líneas de productos.
- Productos.
- Relaciones producto-material.
- Personalizaciones.
- Imágenes.
- Movimientos de materiales.
- Movimientos de productos.
- Métodos de pago.
- Pedidos.
- Detalles de pedidos.

Quedan fuera de esta iteración:

- Envíos.
- Facturación electrónica.
- Integración con Deuna.
- Integración con tarjetas de crédito o débito.
- Triggers de auditoría.
- Mapeo y administración de la tabla `AUDITORIA` desde el backend; su consulta se realizará directamente en la base de datos.
- Marketplace público.
- Endpoints y reglas de negocio, porque pertenecen a capas posteriores.

## 3. Reglas técnicas generales

1. `DataAccess` solo contendrá responsabilidades de persistencia; no aplicará reglas funcionales del negocio.
2. Las entidades representarán fielmente las tablas existentes, sin reutilizarlas como DTOs o modelos de API.
3. Los nombres de tablas y columnas se configurarán explícitamente para PostgreSQL.
4. Las claves simples se configurarán con `ValueGeneratedOnAdd()` y las secuencias creadas en `retazo_market_sequences_v1.sql`.
5. Las claves compuestas se configurarán con `HasKey(...)` y no usarán secuencias.
6. Las relaciones respetarán las claves foráneas y el comportamiento `Restrict` del DDL.
7. Las lecturas usarán `AsNoTracking()` por defecto.
8. Los métodos destinados a actualización obtendrán entidades con tracking.
9. Todas las operaciones de entrada/salida serán asíncronas y aceptarán `CancellationToken`.
10. Los repositorios no llamarán a `SaveChangesAsync()`; el guardado se coordinará posteriormente desde `UnitOfWork` en `DataManagment`.
11. No se expondrá eliminación física, excepto para `FAVORITOS`.
12. La eliminación lógica se representará mediante los estados existentes `ACT` e `INA` en las entidades que los soportan.
13. Los movimientos y pedidos serán registros históricos: no tendrán operaciones de eliminación.
14. Imágenes y métodos de pago permitirán creación y actualización, pero no eliminación.
15. Los valores `JSONB` se mapearán explícitamente como `jsonb`.
16. Los valores monetarios se mapearán con precisión `decimal(10,2)`.
17. Las fechas se tratarán como UTC. Como el DDL actual usa `timestamp without time zone`, se configurará una conversión consistente para almacenar el valor UTC y restaurarlo con `DateTimeKind.Utc` al leerlo.
18. La conversión a `America/Guayaquil` no pertenece a esta capa y se realizará al presentar la información.

### Convención de fechas UTC

- En C# se utilizará `DateTime` para las columnas temporales del DDL actual.
- Toda fecha recibida por `DataAccess` deberá representar un instante UTC.
- Debido a que PostgreSQL usa actualmente `timestamp without time zone`, antes de persistir se conservará el valor UTC con `DateTimeKind.Unspecified`, que es el tipo esperado por Npgsql para esa columna.
- Al materializar una fecha desde PostgreSQL, se restaurará explícitamente `DateTimeKind.Utc`.
- La conversión se implementará mediante una configuración reutilizable de EF Core cuando se cree la primera entidad con fechas.
- Las propiedades opcionales, como `PEDIDOS.FECHA_PAGO`, conservarán su nulabilidad.
- No se aplicará la zona `America/Guayaquil` dentro de entidades, repositorios, queries ni configuraciones de `DataAccess`.
- Ningún repositorio utilizará `DateTime.Now`; la generación de fechas corresponderá a las capas superiores y utilizará UTC.

Representación conceptual de la conversión:

```text
Escritura: DateTime UTC → timestamp sin zona que conserva el valor UTC
Lectura:   timestamp sin zona → DateTime marcado como UTC
Salida:    DateTime UTC → America/Guayaquil en una capa superior
```

### Convención de nombres

- Namespace raíz: `Servicio.RetazoMarket.DataAccess`.
- Entidades: nombre singular en PascalCase seguido de `Entity`, por ejemplo `ClienteEntity`.
- Configuraciones: nombre de la entidad sin el sufijo `Entity`, seguido de `Configuration`, por ejemplo `ClienteConfiguration`.
- Interfaces de repositorio: prefijo `I`, nombre singular y sufijo `Repository`, por ejemplo `IClienteRepository`.
- Implementaciones de repositorio: nombre singular y sufijo `Repository`, por ejemplo `ClienteRepository`.
- Consultas especializadas: nombre singular y sufijo `QueryRepository`, por ejemplo `ClienteQueryRepository`.
- Contexto EF Core: `RetazoMarketDbContext`.
- `DbSet`: nombres plurales en español, por ejemplo `Clientes`, `Productos` y `MovimientosProductos`.
- Propiedades escalares de entidades: `snake_case` en minúsculas, alineadas con el formato de referencia aprobado y con las columnas del DDL, por ejemplo `id_cliente` y `fecha_registro`.
- Propiedades de navegación: PascalCase y nombres descriptivos, por ejemplo `Rol`, `Cliente` y `Usuarios`.
- Tablas, columnas, claves, índices, secuencias y tipos PostgreSQL: se declararán explícitamente en las configuraciones con sus nombres físicos del DDL.
- Métodos asíncronos: sufijo `Async` y parámetro opcional `CancellationToken cancellationToken = default`.
- Métodos de lectura puntual: `ObtenerPorIdAsync`.
- Métodos con tracking: `ObtenerParaActualizarAsync`.
- Métodos de creación técnica: `AgregarAsync`.
- Métodos de actualización técnica: `Actualizar`.
- Consultas paginadas: `BuscarAsync` y retorno `PagedResult<T>`.
- Los nombres actuales de los proyectos, incluido `DataManagment`, se conservarán sin correcciones ortográficas.

## 4. Estructura prevista

```text
Servicio.RetazoMarket.DataAccess/
├── Common/
│   └── PagedResult.cs
├── Configurations/
├── Context/
│   └── RetazoMarketDbContext.cs
├── Entities/
├── Queries/
├── Repositories/
│   ├── Interfaces/
│   └── implementaciones
└── Servicio.RetazoMarket.DataAccess.csproj
```

Las interfaces de consultas especializadas podrán colocarse en `Queries/Interfaces` si los archivos de referencia proporcionados posteriormente siguen ese criterio.

## 5. Orden de implementación

### Seguimiento general

- [x] Fase 0. Preparación y convenciones comunes — completada.
- [x] Fase 1. Catálogos y seguridad base — completada.
- [x] Fase 2. Catálogos de materiales y productos — completada.
- [x] Fase 3. Proveedores y materiales — completada.
- [x] Fase 4. Catálogo de productos — completada.
- [x] Fase 5. Favoritos de clientes — completada.
- [x] Fase 6. Inventario y movimientos — completada.
- [x] Fase 7. Pedidos y detalle — completada.
- [x] Fase 8. Auditoría fuera del backend — completada por decisión de alcance.
- [x] Fase 9. DbContext completo y revisión transversal — completada para el alcance actual.

Una fase solo se marcará como completada cuando todas sus tareas y su validación estén terminadas.

### Fase 0. Preparación y convenciones comunes

- [x] Establecer el flujo de revisión del archivo de referencia antes de crear cada tipo de componente. Su aplicación continuará durante toda la implementación.
- [x] Confirmar los paquetes de EF Core y Npgsql.
- [x] Crear la estructura base de carpetas.
- [x] Crear `PagedResult<T>` usando el archivo de referencia aprobado.
- [x] Definir la convención común para fechas UTC.
- [x] Definir la convención de nombres de entidades, configuraciones, repositorios y consultas.

**Validación:**

- [x] Compilar `Servicio.RetazoMarket.DataAccess` después de crear la estructura base: 0 errores y 0 advertencias.
- [x] Compilar `Servicio.RetazoMarket.DataAccess` después de crear `PagedResult<T>`: 0 errores y 0 advertencias.
- [x] Realizar la compilación final de la Fase 0: 0 errores y 0 advertencias.

### Fase 1. Catálogos y seguridad base

Orden:

- [x] Crear `RolEntity`.
- [x] Crear `RolConfiguration`.
- [x] Crear `ClienteEntity`.
- [x] Crear `ClienteConfiguration`.
- [x] Crear `UsuarioEntity`.
- [x] Crear `UsuarioConfiguration`.
- [x] Crear interfaces de repositorios para roles, clientes y usuarios.
- [x] Crear `RetazoMarketDbContext` incremental con roles, clientes y usuarios.
- [x] Crear implementaciones de repositorios para roles, clientes y usuarios.
- [x] Crear consultas paginadas y filtrables.

Motivo: `USUARIO` depende de `ROL` y opcionalmente de `CLIENTE`. Estas entidades también serán necesarias para autenticación, autorización y administración.

Persistencia prevista:

- Repositorios de creación, lectura y actualización para roles.
- Repositorios CRUD técnico con desactivación lógica para usuarios y clientes.
- Obtención con tracking para actualizaciones y desactivaciones.
- Consultas paginadas y filtrables.
- Búsquedas por correo para garantizar unicidad desde la capa de negocio.
- Consulta de usuario con su rol y cliente asociado para autenticación futura.

**Validación:**

- [x] Compilar después de crear `RolEntity`, `ClienteEntity` y `UsuarioEntity`: 0 errores y 0 advertencias.
- [x] Compilar y verificar configuraciones, claves, relaciones, secuencias, restricciones de longitud y campos opcionales: 0 errores y 0 advertencias.
- [x] Compilar después de crear las interfaces de repositorios: 0 errores y 0 advertencias.
- [x] Compilar después de crear el contexto, los repositorios y los queries: 0 errores y 0 advertencias.

### Fase 2. Catálogos de materiales y productos

Orden:

- [x] Crear `CategoriaMaterialEntity` (`CAT_MAT`).
- [x] Crear `CategoriaMaterialConfiguration`.
- [x] Crear `LineaEntity` (`LINEAS`).
- [x] Crear `LineaConfiguration`.
- [x] Crear `MetodoPagoEntity` (`METODO_PAGO`).
- [x] Crear `MetodoPagoConfiguration`.
- [x] Ampliar `RetazoMarketDbContext` con los catálogos de la Fase 2.
- [x] Crear interfaces e implementaciones de repositorios para los tres catálogos.
- [x] Confirmar que estos catálogos no requieren `QueryRepository`; sus consultas simples quedan encapsuladas en los repositorios.

Motivo: son entidades independientes requeridas por materiales, productos y pedidos.

Operaciones previstas:

- Categorías y líneas: crear, obtener, listar, filtrar, paginar, actualizar y desactivar.
- Métodos de pago: crear, obtener, listar, filtrar, paginar y actualizar; sin eliminación.

**Validación:**

- [x] Compilar después de crear las entidades de la Fase 2: 0 errores y 0 advertencias.
- [x] Compilar y comprobar configuraciones, secuencias, nulabilidad y mapeo de estados `ACT/INA`: 0 errores y 0 advertencias.
- [x] Compilar después de crear interfaces y repositorios: 0 errores y 0 advertencias.

### Fase 3. Proveedores y materiales

Orden:

- [x] Crear `ProveedorEntity` y su configuración.
- [x] Crear `MaterialEntity` y su configuración.
- [x] Crear `ProveedorMaterialEntity` (`PRV_X_MAT`) y su configuración de clave compuesta.
- [x] Ampliar `CategoriaMaterialEntity` con su navegación de materiales.
- [x] Ampliar `RetazoMarketDbContext` con proveedores, materiales y sus asociaciones.
- [x] Crear interfaces e implementaciones de repositorios.
- [x] Crear consultas paginadas y filtros.

Motivo: el material depende de su categoría y la asociación proveedor-material depende de ambas entidades.

Consultas previstas:

- Proveedores por nombre, correo, estado y material suministrado.
- Materiales por nombre, categoría, estado y disponibilidad de stock.
- Condiciones de compra por proveedor y material.
- Relaciones proveedor-material con origen, precio, cantidad mínima y días de entrega.

**Validación:**

- [x] Compilar y verificar relaciones muchos-a-muchos explícitas, clave compuesta, checks y secuencias: 0 errores y 0 advertencias.

### Fase 4. Catálogo de productos

Orden:

- [x] Crear `ProductoEntity` y su configuración.
- [x] Incorporar `PRODUCTOS.COLORES` como `JSONB` y preparar el script incremental para la base existente.
- [x] Crear `ProductoMaterialEntity` (`PRO_X_MAT`) y su configuración.
- [x] Crear `PersonalizacionEntity` y su configuración `JSONB`.
- [x] Crear `ImagenEntity` y su configuración.
- [x] Ampliar las navegaciones de `LineaEntity` y `MaterialEntity`.
- [x] Ampliar `RetazoMarketDbContext` con el catálogo de productos.
- [x] Crear interfaces e implementaciones de repositorios.
- [x] Crear consultas paginadas y filtros del catálogo administrativo.

Motivo: el producto depende de `LINEAS`; sus materiales, personalizaciones e imágenes dependen del producto.

Consultas previstas:

- Productos por nombre, línea, estado, personalización y disponibilidad.
- Producto por ID con línea, materiales, opciones de personalización e imágenes.
- Materiales requeridos por producto.
- Imágenes ordenadas y determinación de imagen principal.

Operaciones especiales:

- Imágenes: crear y actualizar, sin eliminación.
- Productos: eliminación lógica mediante `PROD_ESTADO`.
- Relaciones producto-material: se gestionarán como parte de la composición del producto y no como CRUD administrativo independiente.

**Validación:**

- [x] Compilar y verificar navegación, claves compuestas, precisión monetaria, checks y columnas `JSONB`: 0 errores y 0 advertencias.

### Fase 5. Favoritos de clientes

Orden:

- [x] Crear `FavoritoEntity` y su configuración de clave compuesta.
- [x] Ampliar las navegaciones de `ClienteEntity` y `ProductoEntity`.
- [x] Ampliar `RetazoMarketDbContext` con favoritos.
- [x] Crear el repositorio para agregar, comprobar existencia, listar y eliminar físicamente una relación.
- [x] Crear la consulta paginada de productos favoritos de un cliente sin filtrar por existencias.

Motivo: requiere que clientes y productos ya estén implementados.

**Validación:**

- [x] Compilar y comprobar que `FAVORITOS` sea la única operación de eliminación física de esta iteración: 0 errores y 0 advertencias.

### Fase 6. Inventario y movimientos

Orden:

- [x] Crear `MovimientoMaterialEntity` y su configuración.
- [x] Crear `MovimientoProductoEntity` y su configuración.
- [x] Ampliar las navegaciones de materiales y productos.
- [x] Ampliar `RetazoMarketDbContext` con movimientos de inventario.
- [x] Crear repositorios de creación y consulta.
- [x] Crear consultas paginadas y filtros.

Operaciones permitidas:

- Crear movimientos de ingreso, egreso o ajuste.
- Obtener un movimiento por ID.
- Listar, filtrar y paginar.
- No actualizar movimientos históricos.
- No eliminar movimientos.

Filtros previstos:

- Material o producto.
- Tipo de movimiento.
- Rango de fechas UTC.
- Motivo.

Nota: el cambio de stock y la creación del movimiento se orquestarán como una sola transacción desde capas superiores. `DataAccess` proporcionará las operaciones técnicas necesarias, sin decidir cuándo corresponde ejecutarlas.

**Validación:**

- [x] Compilar y comprobar relaciones con materiales/productos, secuencias y configuración UTC: 0 errores y 0 advertencias.

### Fase 7. Pedidos y detalle

Orden:

- [x] Crear `PedidoEntity` y su configuración.
- [x] Incorporar `PEDIDOS.ENTREGA_FISICA` como `CHAR(1)`, con valores permitidos `S/N` y valor predeterminado `N`.
- [x] Crear `ProductoPedidoEntity` (`PRO_X_PED`) y su configuración de clave compuesta y `JSONB`.
- [x] Ampliar las navegaciones de clientes, métodos de pago y productos.
- [x] Ampliar `RetazoMarketDbContext` con pedidos y detalles.
- [x] Crear repositorios para creación, lectura y actualización controlada.
- [x] Preparar la obtención con tracking del pedido pendiente y sus productos para el pago simulado.
- [x] Crear consultas paginadas y filtros administrativos.

Consultas previstas:

- Pedidos por cliente.
- Estado.
- Rango de fechas UTC.
- Línea de producto.
- Método de pago.
- Pedido por ID con cliente, método de pago y detalles de productos.

Restricciones de persistencia:

- No se eliminarán pedidos.
- No se modelará cancelación.
- La clave compuesta impedirá repetir el mismo producto dentro del mismo pedido.
- `FECHA_PAGO` será opcional mientras el pedido esté pendiente.
- Los importes históricos del detalle no se recalcularán automáticamente al cambiar el catálogo.

Preparación para el pago simulado:

- Obtener pedido pendiente con tracking.
- Obtener productos involucrados con tracking.
- Actualizar stock.
- Agregar movimientos de egreso.
- Registrar `FECHA_PAGO` en UTC.
- Cambiar el estado a `REA`.

La atomicidad y la regla completa del pago se implementarán en `DataManagment` y `Business`; esta fase solo dejará disponibles los componentes de persistencia.

**Validación:**

- [x] Compilar y verificar relaciones, clave compuesta, `JSONB`, fechas UTC, nulabilidad, checks y precisión de cálculos almacenados: 0 errores y 0 advertencias.

### Fase 8. Auditoría fuera del backend

Orden:

- [x] Confirmar que no se creará `AuditoriaEntity`.
- [x] Confirmar que no se creará `AuditoriaConfiguration`.
- [x] Confirmar que auditoría no se incorporará a `RetazoMarketDbContext`.
- [x] Restringir la consulta de auditoría al acceso directo sobre PostgreSQL.

No se crearán:

- Endpoints de auditoría.
- CRUD administrativo de auditoría.
- Repositorios o queries de auditoría.
- Servicios de DataManagment o Business para auditoría.
- Triggers en esta iteración.

**Validación:**

- [x] Comprobar que DataAccess no expone componentes de acceso a `AUDITORIA`.

### Fase 9. DbContext completo y revisión transversal — completada

- [x] Incorporar los 18 `DbSet` del alcance.
- [x] Aplicar explícitamente las 18 configuraciones según el patrón de referencia aprobado.
- [x] Revisar todas las relaciones y comportamientos de eliminación: 18 relaciones físicas configuradas con `DeleteBehavior.Restrict`.
- [x] Revisar secuencias y generación de IDs: 14 claves simples con secuencia y 4 claves compuestas sin secuencia.
- [x] Verificar que DataAccess no tenga dependencias hacia `DataManagment`, `Business` o `Api`.
- [x] Compilar `Servicio.RetazoMarket.DataAccess`: 0 errores y 0 advertencias.
- [x] Diferir la compilación completa de la solución hasta implementar `Servicio.RetazoMarket.Api/Program.cs`. DataAccess, DataManagment y Business compilan; la API permanece vacía y actualmente produce `CS5001`.
- [x] Diferir la prueba de conexión y consultas mínimas sobre PostgreSQL hasta configurar la conexión desde la API.

## 6. Contratos mínimos por tipo de entidad

### Entidades con estado administrativo

Aplicable a roles solo según reglas futuras y, actualmente, a clientes, usuarios, proveedores, categorías, materiales, líneas y productos según sus campos disponibles.

- Obtener por ID.
- Obtener para actualizar.
- Crear.
- Actualizar.
- Listar y paginar.
- Consultar activos/inactivos.
- Desactivar mediante actualización del estado desde capas superiores.

### Entidades sin eliminación

Aplicable a imágenes y métodos de pago:

- Obtener por ID.
- Crear.
- Actualizar.
- Listar, filtrar y paginar.
- Sin método de eliminación.

### Entidades históricas

Aplicable a movimientos y pedidos:

- Crear cuando corresponda.
- Obtener por ID.
- Listar, filtrar y paginar.
- Actualizaciones únicamente cuando formen parte de un flujo permitido, como confirmar el pago de un pedido.
- Sin eliminación.

### Relaciones compuestas

- No tendrán secuencia.
- Sus claves serán las claves foráneas combinadas.
- Se administrarán desde el agregado o servicio principal correspondiente.
- `FAVORITOS` permitirá eliminación física como excepción expresa.

## 7. Estrategia de consultas

Se separarán dos responsabilidades:

- `Repositories`: altas, obtención puntual con o sin tracking y actualización técnica.
- `Queries`: filtros combinados, proyecciones, relaciones, ordenamiento y paginación.

Cada consulta paginada deberá:

1. Normalizar página y tamaño en capas superiores.
2. Construir filtros sobre `IQueryable`.
3. Ejecutar `CountAsync()` para mantener compatibilidad con `PagedResult<T>.TotalRecords`.
4. Aplicar un orden estable antes de `Skip()` y `Take()`.
5. Usar `AsNoTracking()`.
6. Devolver `PagedResult<T>`.

## 8. Criterios de finalización de DataAccess

La capa se considerará terminada cuando:

- [x] Todas las entidades del alcance están representadas: 18 entidades y 18 `DbSet`.
- [x] Cada entidad tiene configuración explícita y coherente con el DDL: 18 configuraciones aplicadas. La comprobación contra una instancia real de PostgreSQL queda diferida hasta configurar la conexión desde la API.
- [x] Las 14 claves simples incluidas en esta iteración usan sus secuencias. Las secuencias de `AUDITORIA`, `ENVIO` y `FACTURAS` permanecen disponibles fuera de este alcance.
- [x] Las cuatro relaciones con clave compuesta están configuradas correctamente: `FAVORITOS`, `PRV_X_MAT`, `PRO_X_MAT` y `PRO_X_PED`.
- [x] Existen 18 interfaces, 18 repositorios y 10 queries para el alcance administrativo acordado.
- [x] No existen operaciones de eliminación no autorizadas; la única llamada física a `Remove` pertenece a `FavoritoRepository`.
- [x] Los movimientos son consultables y paginables, pero inmutables; sus contratos no exponen actualización ni eliminación.
- [x] Los pedidos están preparados para la transacción de pago simulado mediante obtención con tracking del pedido pendiente, detalles y productos. La ejecución transaccional completa se comprobará en `DataManagment`.
- [x] Auditoría permanece sin mapear y sin exposición desde el backend.
- [x] Envíos y facturación permanecen fuera de esta iteración; no existen entidades, configuraciones ni `DbSet` para esos módulos.
- [x] `Servicio.RetazoMarket.DataAccess` compila correctamente: 0 errores y 0 advertencias.
- [x] Las referencias unidireccionales se conservan y DataAccess no referencia capas superiores. La compilación integral queda diferida hasta implementar `Api/Program.cs`, actualmente vacío y causante de `CS5001`.

## 9. Método de trabajo acordado

1. El usuario proporcionará un archivo de referencia antes de crear cada tipo de archivo o módulo.
2. Se adaptará el patrón del archivo de referencia al modelo real de Retazo Market.
3. Se implementará una fase o grupo dependiente a la vez.
4. Después de cada grupo se compilará `DataAccess`.
5. No se avanzará a `DataManagment` hasta completar y validar esta capa.
6. Las diferencias entre los ejemplos de referencia y el DDL se resolverán a favor del DDL y de las aclaraciones funcionales vigentes.
