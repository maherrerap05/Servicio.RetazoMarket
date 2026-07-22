# Plan de implementación de la capa Api

## 1. Objetivo

Construir `Servicio.RetazoMarket.Api` como la capa exterior del backend. Esta capa expondrá los casos de uso ya implementados en Business mediante una API REST versionada, segura y documentada, sin duplicar reglas de negocio ni acceder directamente a repositorios.

La implementación seguirá la guía académica revisada, adaptada a:

- PostgreSQL mediante Npgsql;
- .NET 10 en los cuatro proyectos;
- `Microsoft.AspNetCore.Authentication.JwtBearer` 8.0.25, conservado por decisión del proyecto;
- versionamiento por URL con `Asp.Versioning.Mvc`;
- Swagger/OpenAPI;
- los roles `SUPERADMINISTRADOR`, `ADMINISTRADOR` y `CLIENTE`;
- el Marketplace, inventario, descuentos, fabricación y pago simulado desarrollados;
- el nuevo `codigo_proveedor` y la búsqueda de materiales por dicho código.

No se implementarán Events ni RabbitMQ.

## 2. Límites arquitectónicos

Api podrá referenciar los proyectos necesarios para composición e infraestructura, pero los controladores solo consumirán interfaces de Business.

```text
HTTP / JSON
    ↓
Api: controllers, JWT, policies, middleware, Swagger y DI
    ↓
Business: reglas, validadores, DTOs y servicios
    ↓
DataManagment: orquestación y transacciones
    ↓
DataAccess: EF Core y PostgreSQL
```

### Api sí será responsable de

- routing, model binding y serialización JSON;
- respuestas y códigos HTTP;
- autenticación JWT y autorización por policies;
- creación de `ActorContext` desde claims confiables;
- CORS, HTTPS, versionamiento y Swagger;
- registro de dependencias;
- traducción global de excepciones;
- registro seguro de errores mediante `ILogger`.

### Api no deberá

- implementar cálculos, stock, descuentos o reglas funcionales;
- usar repositorios, `DbContext` o DataServices desde controladores;
- contener SQL;
- aceptar como confiable un `id_cliente` enviado por el frontend cuando existe en el token;
- exponer contraseñas, hashes, trazas internas o secretos;
- convertir fechas persistidas a hora de Ecuador: la API devolverá UTC y el frontend presentará `America/Guayaquil`.

## 3. Estructura objetivo

```text
Servicio.RetazoMarket.Api
│
├── Controllers
│   └── V1
│       ├── Public
│       │   └── Marketplace
│       │       └── MarketplaceProductosController.cs
│       ├── Cliente
│       │   ├── Cuenta
│       │   │   └── PerfilController.cs
│       │   ├── Favoritos
│       │   │   └── FavoritosController.cs
│       │   └── Pedidos
│       │       └── PedidosClienteController.cs
│       └── Internal
│           ├── Auth
│           │   ├── AuthController.cs
│           │   ├── RolesController.cs
│           │   └── UsuariosController.cs
│           ├── Catalogo
│           │   ├── CategoriasMaterialesController.cs
│           │   ├── LineasController.cs
│           │   ├── MetodosPagoController.cs
│           │   ├── ProveedoresController.cs
│           │   ├── ProveedoresMaterialesController.cs
│           │   ├── MaterialesController.cs
│           │   ├── ProductosController.cs
│           │   ├── ProductosMaterialesController.cs
│           │   ├── PersonalizacionesController.cs
│           │   ├── ImagenesController.cs
│           │   └── DescuentosController.cs
│           ├── Inventario
│           │   ├── MovimientosMaterialesController.cs
│           │   └── MovimientosProductosController.cs
│           └── Comercial
│               ├── ClientesController.cs
│               └── PedidosController.cs
│
├── Extensions
│   ├── ApiVersioningExtensions.cs
│   ├── AuthenticationExtensions.cs
│   ├── ClaimsPrincipalExtensions.cs
│   ├── CorsExtensions.cs
│   ├── ServiceCollectionExtensions.cs
│   └── SwaggerExtensions.cs
│
├── Middleware
│   └── ExceptionHandlingMiddleware.cs
│
├── Models
│   ├── Common
│   │   ├── ApiErrorResponse.cs
│   │   └── ApiResponse.cs
│   └── Settings
│       └── JwtSettings.cs
│
├── Properties
│   └── launchSettings.json
├── appsettings.json
├── appsettings.Development.json
├── Servicio.RetazoMarket.Api.csproj
├── Servicio.RetazoMarket.Api.http
└── Program.cs
```

La organización física se define primero por audiencia y después por módulo funcional. La carpeta no concede permisos: cada controlador y acción conservará sus atributos `[AllowAnonymous]`, `[Authorize]` o policies. No se creará una carpeta `Events` porque no forma parte del alcance.

## 4. Fase A-0: preparación y alineaciones previas

- [x] Confirmar que DataAccess, DataManagment y Business compilan.
- [x] Agregar a Api referencias de proyecto hacia Business, DataManagment y DataAccess.
- [x] Confirmar que Api permanece como único proyecto ejecutable.
- [x] Revisar compatibilidad de los paquetes actuales antes de cambiar versiones.
- [x] Mantener JwtBearer 8.0.25 según la decisión confirmada.
- [x] Confirmar que Npgsql se obtiene desde DataAccess y no instalar SQL Server.
- [x] No copiar configuraciones ni nombres del microservicio de ejemplo.

### 4.1. Prerrequisito para claims

`ActorContext` exige un `id_usuario` positivo. Actualmente `LoginResponse` no expone este identificador.

- [x] Agregar `IdUsuario` a `LoginResponse`.
- [x] Mapear `UsuarioDataModel.id_usuario` en `AuthService`.
- [x] Confirmar que el login nunca devuelve hash ni contraseña.
- [x] Compilar Business antes de continuar.

> Fase A-0 completada el 21 de julio de 2026. La restauración del grafo de paquetes terminó sin advertencias; DataAccess, DataManagment y Business compilan con cero errores y cero advertencias. Api mantiene `Program.cs` vacío hasta la Fase A-7.

## 5. Fase A-1: estructura base de Api

- [x] Crear `Controllers/V1/Public`.
- [x] Crear `Controllers/V1/Public/Marketplace`.
- [x] Crear `Controllers/V1/Cliente`.
- [x] Crear `Controllers/V1/Cliente/Cuenta`.
- [x] Crear `Controllers/V1/Cliente/Favoritos`.
- [x] Crear `Controllers/V1/Cliente/Pedidos`.
- [x] Crear `Controllers/V1/Internal`.
- [x] Crear `Controllers/V1/Internal/Auth`.
- [x] Crear `Controllers/V1/Internal/Catalogo`.
- [x] Crear `Controllers/V1/Internal/Inventario`.
- [x] Crear `Controllers/V1/Internal/Comercial`.
- [x] Crear `Extensions`.
- [x] Crear `Middleware`.
- [x] Crear `Models/Common`.
- [x] Crear `Models/Settings`.
- [x] Conservar `Properties` y ajustar `launchSettings.json` al final.
- [x] Confirmar que no se agregaron carpetas de eventos o mensajería.

> Estructura base creada el 21 de julio de 2026. Las carpetas se poblarán en sus fases correspondientes; no se agregaron placeholders, Events ni componentes de RabbitMQ.

## 6. Fase A-2: modelos comunes y configuración

### 6.1. Respuestas HTTP

- [x] Crear `ApiResponse<T>` con `success`, `message` y `data`.
- [x] Crear `ApiErrorResponse` con `success`, `message`, `errors` y `trace_id`.
- [x] Proveer métodos de fábrica para respuestas exitosas y fallidas.
- [x] Evitar incluir detalles internos o stack traces.

### 6.2. JWT settings

- [x] Crear `JwtSettings` con `SecretKey`, `Issuer`, `Audience` y `ExpirationMinutes`.
- [x] Activar la validación de la sección al iniciar la aplicación mediante `ValidateDataAnnotations()` y `ValidateOnStart()` durante la configuración de servicios.
- [x] Exigir una clave suficientemente extensa mediante `MinLength(32)`.
- [x] No guardar la clave real de producción en el repositorio.

### 6.3. appsettings

- [x] Agregar `ConnectionStrings:RetazoMarketDb`.
- [x] Agregar `JwtSettings`.
- [x] Agregar `Cors:AllowedOrigins`.
- [x] Mantener niveles de logging apropiados.
- [x] Configurar temporalmente la conexión local en `appsettings.json`, por decisión expresa del proyecto; migrarla a `appsettings.Development.json`, variables de entorno o user-secrets antes de publicar.
- [x] No incluir credenciales reales de producción en `appsettings.json`; los valores actuales son exclusivamente locales.

> Nota de desarrollo: la contraseña local fue incorporada por solicitud expresa para esta etapa. Debe retirarse del archivo versionado antes de desplegar o compartir un entorno no local.

## 7. Fase A-3: middleware global de excepciones

- [x] Crear `ExceptionHandlingMiddleware`.
- [x] Traducir `ValidationException` a `400 Bad Request` con su colección de errores.
- [x] Traducir `NotFoundException` a `404 Not Found`.
- [x] Traducir `UnauthorizedBusinessException` a `401 Unauthorized` cuando no exista identidad autenticada.
- [x] Traducir `UnauthorizedBusinessException` a `403 Forbidden` cuando el actor ya esté autenticado.
- [x] Traducir `BusinessException` a `400 Bad Request`.
- [x] Respetar `OperationCanceledException` cuando la solicitud haya sido cancelada por el cliente.
- [x] Traducir errores inesperados a `500 Internal Server Error` sin filtrar datos internos.
- [x] Registrar excepciones inesperadas con `ILogger` y `TraceIdentifier`.
- [x] Escribir JSON con el mismo `JsonSerializerOptions` utilizado por MVC.
- [x] Evitar bloques `try/catch` repetidos en controladores mediante el middleware global.

## 8. Fase A-4: versionamiento, CORS y Swagger

### 8.1. Versionamiento

- [x] Crear `ApiVersioningExtensions`.
- [x] Definir versión predeterminada `1.0`.
- [x] Usar rutas `api/v{version:apiVersion}/...` en los controladores.
- [x] Reportar versiones soportadas.
- [x] Sustituir la versión en la URL mediante el explorador de versiones.

### 8.2. CORS

- [x] Crear `CorsExtensions`.
- [x] Leer orígenes permitidos desde configuración.
- [x] Permitir únicamente headers y métodos necesarios.
- [x] No combinar `AllowAnyOrigin` con credenciales.
- [x] Rechazar al iniciar una configuración vacía en ambientes que requieran frontend.

### 8.3. Swagger

- [x] Crear `SwaggerExtensions`.
- [x] Documentar la API como Retazo Market API v1.
- [x] Integrar el explorador de versiones.
- [x] Configurar esquema Bearer JWT.
- [x] Mostrar el botón `Authorize`.
- [x] No incluir XML comments mientras su generación no esté habilitada en el proyecto.
- [x] Documentar respuestas exitosas y errores comunes con `ProducesResponseType` en los controladores; los administrativos compactos declaran los errores comunes a nivel de clase.

## 9. Fase A-5: autenticación, claims y autorización

### 9.1. JWT Bearer

- [x] Crear `AuthenticationExtensions`.
- [x] Configurar esquema predeterminado JwtBearer.
- [x] Validar firma, issuer, audience y expiración.
- [x] Usar `ClockSkew = TimeSpan.Zero`.
- [x] Requerir HTTPS metadata.
- [x] Configurar correctamente `NameClaimType` y `RoleClaimType` con claims JWT sin mapeo implícito.

### 9.2. Claims emitidos

El token deberá contener únicamente datos necesarios:

- [x] `sub`: `id_usuario`.
- [x] `name`: nombre del usuario.
- [x] `email`: correo normalizado.
- [x] `role`: un claim por rol.
- [x] `id_cliente`: solo cuando exista un cliente asociado.
- [x] `jti`: identificador único del token.
- [x] No incluir contraseña, hash, dirección, teléfono ni información comercial.

### 9.3. ActorContext

- [x] Crear `ClaimsPrincipalExtensions.ToActorContext()`.
- [x] Leer `id_usuario`, `id_cliente`, correo y roles desde claims.
- [x] Marcar `autenticado` según `Identity.IsAuthenticated`.
- [x] Rechazar claims numéricos inválidos en operaciones protegidas.
- [x] Nunca construir el actor usando IDs enviados libremente en el body.

### 9.4. Policies

- [x] Crear policy `SuperAdministrador` para el rol `SUPERADMINISTRADOR`.
- [x] Crear policy `Administrador` para `SUPERADMINISTRADOR` o `ADMINISTRADOR`.
- [x] Crear policy `Cliente` para `CLIENTE`.
- [x] Mantener `[Authorize]`, policies y códigos HTTP exclusivamente en Api.
- [x] Mantener disponible `IAutorizacionService` para validaciones funcionales de rol o propiedad adicionales.
- [x] No confiar en que el frontend oculte botones; la autorización se aplicará mediante policies y reglas de propiedad.

## 10. Fase A-6: inyección de dependencias

- [x] Crear `ServiceCollectionExtensions`.
- [x] Registrar `RetazoMarketDbContext` con `UseNpgsql` y `RetazoMarketDb`.
- [x] Registrar `IUnitOfWork` como `Scoped`.
- [x] Registrar todos los DataServices como `Scoped`.
- [x] Registrar todos los servicios Business como `Scoped`.
- [x] Usar `Servicios_Registro_Api.md` como lista de control.
- [x] No registrar repositorios individuales porque `UnitOfWork` los construye con el mismo contexto.
- [x] No registrar servicios duplicados con ciclos de vida incompatibles.
- [x] Confirmar que DbContext, UnitOfWork, DataServices y Business compartan el mismo scope por request.
- [x] Validar la construcción del contenedor al iniciar en Development al configurar el host en `Program.cs`.

## 11. Fase A-7: Program.cs y pipeline HTTP

- [x] Configurar controllers y opciones JSON.
- [x] Registrar versionamiento, CORS, JWT, policies, Swagger y servicios de aplicación.
- [x] Validar configuración y construcción del contenedor al inicio en Development.
- [x] Registrar `ExceptionHandlingMiddleware` antes de los componentes que puedan lanzar excepciones relevantes.
- [x] Habilitar Swagger solo en Development salvo decisión posterior.
- [x] Usar `UseHttpsRedirection`.
- [x] Aplicar CORS antes de autenticación y autorización.
- [x] Ejecutar `UseAuthentication` antes de `UseAuthorization`.
- [x] Mapear controladores.
- [x] No ejecutar migraciones, `EnsureCreated` ni borrados automáticos al iniciar.
- [x] No agregar `public partial class Program` todavía; se incorporará únicamente si las futuras pruebas de integración lo requieren.
- [x] Compilar la solución y eliminar el error actual de `Program/Main` vacío.

Pipeline previsto:

```text
Exception middleware
→ HTTPS redirection
→ CORS
→ Authentication
→ Authorization
→ Controllers
```

## 12. Fase A-8: endpoints públicos y autenticación

### 12.1. AuthController

Ruta base: `api/v1/auth`.
Ubicación: `Controllers/V1/Internal/Auth/AuthController.cs`. Aunque se organiza dentro del módulo técnico `Auth`, las acciones de login y registro seguirán siendo públicas mediante `[AllowAnonymous]`.

- [x] `POST /login`: validar credenciales mediante `IAuthService` y emitir JWT.
- [x] `POST /registro`: ejecutar el autorregistro Marketplace mediante `IAutorregistroMarketplaceService`.
- [x] Marcar ambos endpoints con `[AllowAnonymous]`.
- [x] Devolver `200 OK` para login exitoso.
- [x] Devolver `201 Created` para registro exitoso.
- [x] No generar ni validar contraseñas directamente en el controlador.
- [x] No devolver tokens cuando el login falle.

### 12.2. MarketplaceProductosController

Ruta base: `api/v1/marketplace/productos`.
Ubicación: `Controllers/V1/Public/Marketplace/MarketplaceProductosController.cs`.

- [x] `GET /{id}` para detalle público del producto.
- [x] `GET` para búsqueda paginada pública.
- [x] Exponer productos agotados cuando corresponda, sin confundirlos con productos inactivos.
- [x] No exigir stock para agregar posteriormente un producto a favoritos.
- [x] No exponer costos internos, recetas ni inventario administrativo.

## 13. Fase A-9: endpoints del cliente autenticado

Todos exigirán policy `Cliente`. El `id_cliente` operativo se obtendrá del JWT.

### 13.1. PerfilController

Ubicación: `Controllers/V1/Cliente/Cuenta/PerfilController.cs`.

- [x] Consultar el perfil propio.
- [x] Actualizar únicamente el perfil propio.
- [x] No permitir que el body cambie la propiedad hacia otro cliente.
- [x] Aplicar `ExigirClientePropietario` cuando corresponda.

### 13.2. FavoritosController

Ubicación: `Controllers/V1/Cliente/Favoritos/FavoritosController.cs`.

- [x] Listar favoritos propios.
- [x] Buscar favoritos propios con paginación.
- [x] Consultar un favorito propio.
- [x] Agregar un favorito propio aunque el producto no tenga stock.
- [x] Quitar un favorito mediante la eliminación física expresamente permitida.
- [x] Pasar a `IFavoritoService` el `id_cliente` obtenido del token como actor.

### 13.3. PedidosClienteController

Ubicación: `Controllers/V1/Cliente/Pedidos/PedidosClienteController.cs`.

- [x] Listar o buscar únicamente pedidos propios.
- [x] Consultar únicamente un pedido propio.
- [x] Crear un pedido para el cliente del token.
- [x] Actualizar únicamente un pedido propio pendiente; Business conserva la validación del estado `PEN`.
- [x] Ejecutar pago simulado únicamente sobre un pedido propio permitido.
- [x] Validar propiedad con `IAutorizacionService` antes de devolver o modificar recursos.
- [x] No exponer cancelación de pedidos.
- [x] No exponer pagos reales, DeUna, tarjetas ni facturación real.

## 14. Fase A-10: seguridad y configuración técnica interna

Estos controladores usarán policy `SuperAdministrador`:

- El módulo `Controllers/V1/Internal/Auth` contendrá login, autorregistro, roles y usuarios.
- El módulo `Controllers/V1/Internal/Catalogo` contendrá los catálogos y configuraciones técnicas.

### 14.1. RolesController

- [x] Listar, buscar y consultar roles.
- [x] Crear y actualizar roles.
- [x] No inventar eliminación si Business no la expone.

### 14.2. UsuariosController

- [x] Listar, buscar y consultar usuarios internos.
- [x] Crear y actualizar usuarios internos.
- [x] Cambiar contraseña mediante el caso de uso Business.
- [x] Ejecutar eliminación lógica.
- [x] Nunca devolver contraseña ni hash.

### 14.3. Catálogos técnicos

- [x] Implementar `CategoriasMaterialesController`.
- [x] Implementar `LineasController`.
- [x] Implementar `MetodosPagoController`.
- [x] Implementar configuración administrativa de `PersonalizacionesController`.
- [x] Respetar que métodos de pago y personalizaciones no exponen eliminación.
- [x] Mantener la gestión técnica fuera del rol `ADMINISTRADOR` mediante policy `SuperAdministrador`.

## 15. Fase A-11: gestión administrativa

Estos controladores usarán policy `Administrador`, que también admite `SUPERADMINISTRADOR`.

- `Controllers/V1/Internal/Catalogo`: proveedores, materiales, productos, recetas, imágenes y descuentos.
- `Controllers/V1/Internal/Inventario`: movimientos de materiales y productos.
- `Controllers/V1/Internal/Comercial`: clientes y pedidos administrativos.

### 15.1. ClientesController

- [x] Listar, buscar y consultar clientes.
- [x] Crear y actualizar clientes administrativamente.
- [x] Ejecutar eliminación lógica.

### 15.2. ProveedoresController

- [x] Listar, buscar y consultar proveedores.
- [x] Consultar por correo.
- [x] Implementar `GET /codigo/{codigo}`.
- [x] Crear y actualizar incluyendo `codigo_proveedor`.
- [x] Ejecutar eliminación lógica.
- [x] Permitir filtrar proveedores por `codigo_proveedor`.

### 15.3. ProveedoresMaterialesController

- [x] Consultar la relación por clave compuesta.
- [x] Listar relaciones por proveedor o material.
- [x] Crear y actualizar la relación.
- [x] Mostrar `codigo_proveedor` en la respuesta.
- [x] No exponer eliminación de la asociación.

### 15.4. MaterialesController

- [x] Listar, buscar y consultar materiales.
- [x] Consultar por nombre.
- [x] Crear y actualizar materiales.
- [x] Ejecutar eliminación lógica.
- [x] Permitir filtrar materiales por `codigo_proveedor`.
- [x] Mantener cantidades decimales con hasta tres posiciones donde corresponda.

### 15.5. Inventario

- [x] Implementar `MovimientosMaterialesController` con consulta, filtro paginado y creación.
- [x] Implementar `MovimientosProductosController` con consulta, filtro paginado y creación.
- [x] Admitir ingreso, egreso y ajuste según Business.
- [x] No exponer actualización ni eliminación de movimientos.
- [x] No aceptar un usuario responsable porque no forma parte del modelo acordado.

### 15.6. Productos

- [x] Implementar `ProductosController` con listado, filtros, detalle, creación, actualización y eliminación lógica.
- [x] Exponer `colores` como JSON.
- [x] Exponer costos, margen y gastos únicamente en rutas administrativas.
- [x] Implementar el endpoint de registro de fabricación para producto nuevo o existente.
- [x] Mantener la fabricación atómica delegándola a Business/DataManagment.
- [x] No crear una tabla o endpoint de `PRODUCCION`.

### 15.7. Recetas, imágenes y descuentos

- [x] Implementar `ProductosMaterialesController` sin eliminación.
- [x] Implementar `ImagenesController` solo con creación y actualización, además de consultas disponibles.
- [x] Implementar `DescuentosController` con consultas, creación, actualización, inactivación y cálculo aplicable.
- [x] No permitir administrar `tiene_descuentos` fuera del flujo de reglas de descuento; Business conserva esa coherencia.

### 15.8. PedidosController

- [x] Listar, filtrar y consultar pedidos administrativamente.
- [x] Crear y actualizar pedidos pendientes según contratos Business.
- [x] Exponer `entrega_fisica` con valores `S` o `N`.
- [x] Exponer `personalizacion_selec` como JSON histórico.
- [x] Exponer la operación `POST /{id}/pagar` para pago simulado.
- [x] No exponer eliminación ni cancelación.
- [x] No modelar envíos en este sprint.

## 16. Fase A-12: convenciones de controladores

- [x] Usar `[ApiController]`, `[ApiVersion("1.0")]` y rutas versionadas.
- [x] Inyectar únicamente interfaces Business y, cuando aplique, `IAutorizacionService`.
- [x] Aceptar `CancellationToken` en todas las acciones asíncronas.
- [x] Sobrescribir IDs del DTO de actualización con el ID de la ruta.
- [x] Devolver `404` cuando Business retorne `null` o `false` por recurso inexistente.
- [x] Usar `201 Created` y `CreatedAtAction` al crear recursos con endpoint de detalle; el autorregistro usa 201 simple porque no posee detalle público equivalente.
- [x] Usar `200 OK` para consultas, actualizaciones y operaciones con respuesta.
- [x] No usar `204 No Content` porque las operaciones actuales requieren el cuerpo estandarizado.
- [x] Envolver respuestas exitosas con `ApiResponse<T>`.
- [x] Documentar códigos con `ProducesResponseType`, a nivel de acción o mediante errores comunes a nivel de controlador.
- [x] No capturar excepciones Business dentro de cada controlador.
- [x] No acceder a DataServices, DataAccess, EF Core ni PostgreSQL desde controladores. `DataPagedResult<T>` permanece únicamente como contrato retornado por las interfaces Business existentes.
- [x] No confiar en IDs de actor enviados por body o query string.

## 17. Fase A-13: archivo HTTP y documentación manual

- [x] Actualizar `Servicio.RetazoMarket.Api.http`.
- [x] Incluir ejemplos de login y autorregistro.
- [x] Incluir variable para token Bearer.
- [x] Incluir ejemplos de Marketplace público.
- [x] Incluir ejemplos de proveedor por código.
- [x] Incluir ejemplo de materiales filtrados por código de proveedor.
- [x] Incluir ejemplos de favoritos y pedidos propios.
- [x] Incluir ejemplo de pago simulado.
- [x] No incluir secretos reales ni contraseñas de producción; los datos incluidos usan dominios `.test` y claves ficticias locales.

## 18. Fase A-14: compilación y pruebas sin PostgreSQL

- [x] Compilar DataAccess.
- [x] Compilar DataManagment.
- [x] Compilar Business.
- [x] Compilar Api.
- [x] Compilar `Servicio.RetazoMarket.slnx`.
- [x] Confirmar cero errores y cero advertencias nuevas relevantes.
- [x] Ejecutar la aplicación con una configuración sintácticamente válida sin realizar operaciones destructivas.
- [x] Confirmar que Swagger contiene todos los grupos y endpoints v1: se detectaron 80 rutas versionadas.
- [x] Confirmar que el contenedor DI puede construir los controladores mediante `AddControllersAsServices()` y validación al iniciar en Development.
- [x] Confirmar que endpoints protegidos sin token devuelven `401` con el contrato común.
- [x] Confirmar que roles insuficientes producen `403`.
- [x] Confirmar que errores Business usan el contrato de error común con `errors` y `traceId`.

## 19. Fase A-15: integración con PostgreSQL

Esta fase se ejecutará cuando la configuración completa de Api esté disponible.

- [x] Configurar la cadena local mediante user-secrets o variables de entorno.
- [x] Probar apertura de conexión sin modificar el esquema.
- [x] No ejecutar `EnsureCreated`, migraciones automáticas ni recreación de tablas.
- [x] Preparar datos mínimos: roles, usuario administrativo y catálogos requeridos.
- [x] Crear contraseñas mediante `IPasswordHashService`; no insertar texto plano.
- [x] Probar login y emisión de JWT.
- [x] Probar cada policy con SUPERADMINISTRADOR, ADMINISTRADOR y CLIENTE.
- [x] Probar CRUDs y eliminación lógica.
- [x] Probar favoritos de productos sin stock.
- [x] Probar filtros de proveedor y material por `codigo_proveedor`.
- [x] Probar fabricación con consumo decimal y rollback por material insuficiente.
- [x] Probar descuentos por tramo sin acumulación.
- [x] Probar personalizaciones y mínimo de sábanas.
- [x] Probar pedido con `entrega_fisica` `S` y `N`.
- [x] Probar pago simulado, stock insuficiente y concurrencia.
- [x] Confirmar fechas UTC y presentación delegada al frontend.
- [x] Limpiar únicamente los datos de prueba autorizados.

Resultado de la ejecución local (21 de julio de 2026): la matriz de integración finalizó correctamente contra `Retazo_Market_DB`. Se validaron respuestas `200/201`, rechazos `400/403`, pago concurrente con una sola confirmación y posterior limpieza de todos los registros temporales. La API entrega fechas UTC; su transformación a hora local no se ejecuta en backend porque permanece expresamente delegada al frontend.

## 20. Fase A-16: revisión de seguridad

- [x] Confirmar que JWT no contiene datos sensibles.
- [x] Confirmar que secretos no están versionados.
- [x] Confirmar validación estricta de issuer, audience, firma y expiración.
- [x] Confirmar orden correcto de authentication y authorization.
- [x] Confirmar que rutas internas tienen policy explícita.
- [x] Confirmar propiedad del cliente en perfil, favoritos y pedidos.
- [x] Confirmar que errores `500` no muestran stack trace.
- [x] Confirmar que CORS no permite orígenes indiscriminados.
- [x] Confirmar que Swagger no queda expuesto accidentalmente en producción.
- [x] Confirmar que no hay controladores que dependan directamente de DataManagment o DataAccess.
- [x] Confirmar que no se registran contraseñas, tokens o connection strings en logs.

> Revisión completada el 21 de julio de 2026. La conexión local y la clave JWT se retiraron de `appsettings.json` y quedaron configuradas mediante User Secrets; producción deberá suministrarlas mediante variables de entorno o un almacén de secretos. Los controladores dependen exclusivamente de interfaces Business. La única referencia de tipos compartidos hacia DataManagment es `DataPagedResult<T>`, conservada por la decisión arquitectónica adoptada en Business; ningún controlador consume DataServices, repositorios ni `DbContext`. Swagger devolvió `404` bajo `Production`, mientras que la API continuó operativa.

## 21. Fase A-17: criterios de finalización

- [x] Existe la estructura completa de Api.
- [x] `Program.cs` configura correctamente el pipeline.
- [x] Api compila y la solución completa compila.
- [x] Api usa PostgreSQL mediante `UseNpgsql`.
- [x] Todos los servicios requeridos están registrados con el ciclo de vida correcto.
- [x] Los controladores consumen únicamente interfaces Business.
- [x] Existe versionamiento por URL y documentación Swagger v1.
- [x] JWT, claims, policies y `ActorContext` funcionan coherentemente.
- [x] Las respuestas exitosas y fallidas tienen un contrato uniforme.
- [x] Las excepciones se traducen globalmente a códigos HTTP.
- [x] Los endpoints públicos, del cliente e internos están separados y protegidos.
- [x] Están expuestos todos los módulos incluidos en el alcance actual.
- [x] Proveedores y materiales soportan `codigo_proveedor`.
- [x] Se respetan eliminación lógica, favoritos físicos y recursos sin eliminación.
- [x] Pago simulado y fabricación conservan atomicidad.
- [x] IVA permanece en cero y sus campos siguen expuestos.
- [x] Todas las fechas persistidas y emitidas permanecen en UTC.
- [x] No existe administración de auditoría.
- [x] No se implementaron envíos, cancelación, facturación real, DeUna ni tarjetas.
- [x] Las pruebas funcionales críticas contra PostgreSQL son exitosas.
- [x] La aplicación está lista para ser consumida por el frontend del alcance actual.

> Cierre técnico completado el 21 de julio de 2026. La solución compila sin errores ni advertencias, el documento Swagger v1 expone las 80 rutas previstas en Development, los endpoints públicos responden correctamente y las rutas protegidas rechazan solicitudes sin credenciales. La matriz funcional contra PostgreSQL de la Fase A-15 confirmó los flujos críticos y dejó la base limpia.

## 22. Orden recomendado de implementación

```text
A-0  Preparación y prerrequisito de id_usuario
A-1  Estructura base
A-2  Respuestas comunes y settings
A-3  Middleware de errores
A-4  Versionamiento, CORS y Swagger
A-5  JWT, claims, ActorContext y policies
A-6  Inyección de dependencias
A-7  Program.cs y pipeline
A-8  Auth y Marketplace público
A-9  Perfil, favoritos y pedidos del cliente
A-10 Roles, usuarios y configuración técnica
A-11 Gestión administrativa completa
A-12 Convenciones y revisión de controladores
A-13 Archivo HTTP
A-14 Compilación y pruebas sin PostgreSQL
A-15 Integración contra PostgreSQL
A-16 Revisión de seguridad
A-17 Cierre de Api
```

Cada fase deberá actualizar estas casillas y compilar antes de avanzar cuando el cambio afecte código. La conexión y las pruebas contra PostgreSQL se realizarán al final, una vez que DI, autenticación y todos los controladores estén configurados.

## 23. Fuera del alcance actual

- cancelación de pedidos;
- tabla o endpoints de envíos;
- facturación electrónica real;
- integración con DeUna;
- pasarela de tarjetas de crédito o débito;
- tablas `PAGOS` y `PED_X_PAG` para anticipos de personalizados;
- imágenes de referencia para personalizaciones, DTF, dibujos o diseños;
- tabla administrativa de producción;
- administración de auditoría;
- RabbitMQ, eventos o consumidores;
- refresh tokens, salvo aprobación posterior.
