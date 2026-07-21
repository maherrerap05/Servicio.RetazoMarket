# Servicios que Api deberá registrar

Este inventario prepara la configuración de inyección de dependencias. No introduce referencias desde Business hacia Api.

## Business

Todos se registrarán con alcance `Scoped`:

```csharp
services.AddScoped<IAuthService, AuthService>();
services.AddScoped<IAutorizacionService, AutorizacionService>();
services.AddScoped<IAutorregistroMarketplaceService, AutorregistroMarketplaceService>();
services.AddScoped<ICategoriaMaterialService, CategoriaMaterialService>();
services.AddScoped<IClienteService, ClienteService>();
services.AddScoped<IDescuentoService, DescuentoService>();
services.AddScoped<IFavoritoService, FavoritoService>();
services.AddScoped<IImagenService, ImagenService>();
services.AddScoped<ILineaService, LineaService>();
services.AddScoped<IMaterialService, MaterialService>();
services.AddScoped<IMetodoPagoService, MetodoPagoService>();
services.AddScoped<IMovimientoMaterialService, MovimientoMaterialService>();
services.AddScoped<IMovimientoProductoService, MovimientoProductoService>();
services.AddScoped<IPasswordHashService, PasswordHashService>();
services.AddScoped<IPedidoService, PedidoService>();
services.AddScoped<IPersonalizacionService, PersonalizacionService>();
services.AddScoped<IProductoMarketplaceService, ProductoMarketplaceService>();
services.AddScoped<IProductoMaterialService, ProductoMaterialService>();
services.AddScoped<IProductoService, ProductoService>();
services.AddScoped<IProveedorMaterialService, ProveedorMaterialService>();
services.AddScoped<IProveedorService, ProveedorService>();
services.AddScoped<IRolService, RolService>();
services.AddScoped<IUsuarioService, UsuarioService>();
```

## DataManagment

Todos se registrarán con alcance `Scoped`:

```csharp
services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped<IAutorregistroMarketplaceDataService, AutorregistroMarketplaceDataService>();
services.AddScoped<ICategoriaMaterialDataService, CategoriaMaterialDataService>();
services.AddScoped<IClienteDataService, ClienteDataService>();
services.AddScoped<IDescuentoAdministracionDataService, DescuentoAdministracionDataService>();
services.AddScoped<IDescuentoDataService, DescuentoDataService>();
services.AddScoped<IFavoritoDataService, FavoritoDataService>();
services.AddScoped<IImagenAdministracionDataService, ImagenAdministracionDataService>();
services.AddScoped<IImagenDataService, ImagenDataService>();
services.AddScoped<ILineaDataService, LineaDataService>();
services.AddScoped<IMaterialDataService, MaterialDataService>();
services.AddScoped<IMetodoPagoDataService, MetodoPagoDataService>();
services.AddScoped<IMovimientoMaterialDataService, MovimientoMaterialDataService>();
services.AddScoped<IMovimientoProductoDataService, MovimientoProductoDataService>();
services.AddScoped<IPedidoAdministracionDataService, PedidoAdministracionDataService>();
services.AddScoped<IPedidoDataService, PedidoDataService>();
services.AddScoped<IPersonalizacionDataService, PersonalizacionDataService>();
services.AddScoped<IProductoDataService, ProductoDataService>();
services.AddScoped<IProductoFabricacionDataService, ProductoFabricacionDataService>();
services.AddScoped<IProductoMaterialDataService, ProductoMaterialDataService>();
services.AddScoped<IProductoPedidoDataService, ProductoPedidoDataService>();
services.AddScoped<IProveedorDataService, ProveedorDataService>();
services.AddScoped<IProveedorMaterialDataService, ProveedorMaterialDataService>();
services.AddScoped<IRolDataService, RolDataService>();
services.AddScoped<IUsuarioDataService, UsuarioDataService>();
```

## Infraestructura de datos

- Registrar `RetazoMarketDbContext` con `AddDbContext` y el proveedor Npgsql.
- Obtener la cadena de conexión desde configuración; no colocar credenciales en código.
- No registrar manualmente repositorios creados internamente por `UnitOfWork`.
- Mantener un mismo alcance para `DbContext`, `UnitOfWork`, DataServices y servicios Business.

## Responsabilidades adicionales de Api

- Construir `ActorContext` desde los claims autenticados.
- Configurar JWT, policies, CORS, versionado y Swagger.
- Traducir excepciones Business mediante middleware.
- Exponer a los controladores exclusivamente interfaces Business.
