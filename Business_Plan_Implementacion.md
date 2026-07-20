# Plan de implementación de la capa Business

## 1. Objetivo

Construir `Servicio.RetazoMarket.Business` como la capa que interpreta los requerimientos, valida entradas, aplica reglas de negocio, coordina los servicios de DataManagment y devuelve DTOs preparados para Api.

El diseño sigue la guía académica de implementación del microservicio y la arquitectura acordada:

```text
Api
→ Business
→ DataManagment
→ DataAccess
→ PostgreSQL
```

Business:

- Sí valida datos y reglas funcionales.
- Sí controla los casos de uso.
- Sí transforma DTOs y DataModels.
- Sí produce excepciones funcionales.
- Sí aplica seguridad funcional y decisiones comerciales.
- No utiliza `DbContext`, entidades, EF Core ni SQL.
- No referencia DataAccess ni Api.
- No devuelve DataModels a los controladores.

## 2. Fuentes y precedencia

La implementación debe consultar, en este orden:

1. Decisiones y aclaraciones más recientes de la dueña y del equipo.
2. `Reglas_Negocio_Business.md`, considerando las actualizaciones de este plan.
3. `Decisiones_Diseno_Compartidas.md`.
4. Modelo vigente expuesto por DataManagment.
5. Documento de requerimientos.
6. Guía académica `implementacion del microservicio.pdf` para el patrón arquitectónico.

Cuando una regla antigua contradiga una decisión posterior, prevalece la decisión posterior. En particular:

- Los descuentos ya no usan `stock_descuento`; usan la tabla `DESCUENTOS` por producto y tramos.
- IVA se conserva técnicamente, pero su tasa actual es `0`.
- Gastos fijos y operativos son porcentajes editables del producto.
- Los materiales, recetas y movimientos de materiales admiten cantidades decimales.
- Los ajustes usan `AJP` o `AJN`, con cantidades siempre positivas.
- No se implementan envíos, cancelación, facturación real ni pasarelas reales.

## 3. Alcance funcional actual

### Núcleo administrativo

- Roles.
- Usuarios y autenticación base.
- Clientes.
- Proveedores.
- Categorías de materiales.
- Materiales.
- Asociaciones proveedor-material.
- Líneas.
- Productos y recetas de materiales.
- Personalizaciones.
- Imágenes.
- Descuentos.
- Movimientos de materiales y productos.
- Métodos de pago.
- Pedidos y pago simulado.

### Marketplace necesario

- Catálogo público de productos activos.
- Favoritos.
- Registro de cliente y usuario asociado.
- Creación e historial de pedidos del cliente autenticado.

### Fuera de alcance

- CRUD o consulta de auditoría desde la aplicación.
- Envíos.
- Cancelación de pedidos.
- Deuna.
- Tarjetas de crédito o débito.
- Facturación electrónica real.
- Pagos parciales 50/50 para personalizados.
- DTF, dibujos, diseños y archivos de referencia.
- JWT y atributos HTTP; se configuran en Api.

## 4. Estructura esperada

```text
Servicio.RetazoMarket.Business
├── DTOs
│   ├── Auth
│   ├── Rol
│   ├── Usuario
│   ├── Cliente
│   ├── Proveedor
│   ├── CategoriaMaterial
│   ├── Material
│   ├── ProveedorMaterial
│   ├── Linea
│   ├── Producto
│   ├── ProductoMaterial
│   ├── Personalizacion
│   ├── Imagen
│   ├── Descuento
│   ├── Favorito
│   ├── MovimientoMaterial
│   ├── MovimientoProducto
│   ├── MetodoPago
│   └── Pedido
├── Exceptions
│   ├── BusinessException.cs
│   ├── NotFoundException.cs
│   ├── UnauthorizedBusinessException.cs
│   └── ValidationException.cs
├── Interfaces
├── Mappers
├── Services
├── Validators
└── Servicio.RetazoMarket.Business.csproj
```

Cada módulo seguirá, según corresponda:

```text
Crear{Modulo}Request
Actualizar{Modulo}Request
{Modulo}FiltroRequest
{Modulo}Response
{Modulo}Validator
{Modulo}BusinessMapper
I{Modulo}Service
{Modulo}Service
```

No todos los módulos necesitan las cuatro clases DTO. Por ejemplo, favoritos utilizan agregar/quitar; movimientos solo se crean y consultan; imágenes y métodos de pago no se eliminan.

## 5. Fase B-0: preparación y estructura

- [x] Confirmar que Business referencia únicamente a DataManagment.
- [x] Confirmar que no tenga referencias a DataAccess ni Api.
- [x] Crear carpetas `DTOs`, `Exceptions`, `Interfaces`, `Mappers`, `Services` y `Validators`.
- [x] Crear las subcarpetas DTO enumeradas en la sección 4.
- [x] Mantener `net10.0`, nullable e implicit usings según el proyecto actual.
- [x] No agregar EF Core ni Npgsql a Business.
- [x] Compilar el proyecto vacío antes de agregar componentes.

## 6. Fase B-1: componentes comunes

### 6.1. Excepciones

- [x] Crear `BusinessException` como excepción base funcional.
- [x] Crear `ValidationException` con colección de errores.
- [x] Crear `NotFoundException`.
- [x] Crear `UnauthorizedBusinessException`.
- [x] No incluir códigos HTTP en las excepciones; Api hará la traducción.

### 6.2. Paginación

- [x] Reutilizar `DataPagedResult<T>` de DataManagment, siguiendo el proyecto de referencia.
- [x] No crear una clase adicional de paginación en Business.
- [x] Transformar únicamente los elementos internos de `DataModel` a DTO de respuesta en cada servicio Business.
- [x] Validar `PageNumber >= 1` y un rango seguro de `PageSize` antes de consultar.

### 6.3. Convenciones compartidas sin carpeta adicional

- [x] Acordar que los estados `ACT`, `INA`, `PEN` y `REA` se definirán dentro de los validators o servicios que los utilicen.
- [x] Acordar que los indicadores `S` y `N` se definirán dentro de los validators o servicios que los utilicen.
- [x] Acordar que los movimientos `ING`, `EGR`, `AJP` y `AJN` se definirán en los validators de movimientos.
- [x] Acordar que los tipos `LISTA`, `MATERIAL`, `TEXTO`, `MEDIDA` y `COLOR` se definirán en el validator de personalizaciones.
- [x] Acordar mantener `TasaIva = 0m` dentro del servicio de cálculo de pedidos, sin eliminar campos de IVA.
- [x] Acordar mantener `CantidadMinimaSabanaPersonalizada = 3` dentro del servicio o validator correspondiente.
- [x] Acordar usar `America/Guayaquil` únicamente al preparar respuestas que requieran hora local; la persistencia seguirá en UTC.
- [x] Acordar implementar el redondeo monetario como método privado reutilizable del servicio de cálculo correspondiente.
- [x] Acordar implementar la normalización mediante métodos privados de validators o mappers según el módulo.
- [x] No crear una carpeta genérica `Common`, `Helpers` o `Configuration` en esta fase.
- [x] No duplicar porcentajes de gastos en constantes; pertenecen a cada producto.

## 7. Fase B-2: patrón base de módulo

Implementar primero un módulo pequeño —recomendado `Linea`— para validar el patrón completo.

- [x] Crear DTOs de creación, actualización, filtro y respuesta de Línea.
- [x] Crear `LineaValidator`.
- [x] Crear `LineaBusinessMapper`.
- [x] Crear `ILineaService`.
- [x] Crear `LineaService` usando únicamente `ILineaDataService`.
- [x] Validar nombre obligatorio, longitud, estado, duplicados y paginación.
- [x] Normalizar entradas antes de consultar o persistir.
- [x] Mantener respuestas anulables cuando un registro no existe, de acuerdo con el patrón del proyecto de referencia.
- [x] Aplicar eliminación lógica con estado `INA` mediante DataManagment.
- [x] Compilar Business y usar este módulo como plantilla para los demás CRUD.

## 8. Fase B-3: catálogos base

### 8.1. Roles

- [x] Crear DTOs, validator, mapper, interfaz y servicio de Rol.
- [x] Validar nombre único y uno de los nombres de rol admitidos por la base de datos.
- [x] No permitir eliminación física.
- [ ] Reservar gestión de roles al Superadministrador mediante contexto funcional recibido desde Api.
- [x] No codificar permisos HTTP dentro del servicio.

### 8.2. Categorías de materiales

- [x] Crear DTOs, validator, mapper, interfaz y servicio.
- [x] Validar nombre único, campos obligatorios y estado.
- [x] Aplicar eliminación lógica.

### 8.3. Métodos de pago

- [x] Crear DTOs de creación, actualización y respuesta.
- [x] Validar nombre y código SRI únicos.
- [x] Permitir crear y actualizar.
- [x] No exponer eliminación por ahora.
- [x] No implementar pasarela ni transacciones reales.

## 9. Fase B-4: clientes, usuarios y autenticación

### 9.1. Clientes

- [x] Crear `CrearClienteRequest`, `ActualizarClienteRequest`, `ClienteFiltroRequest` y `ClienteResponse`.
- [x] Crear validator, mapper, interfaz y servicio.
- [x] Validar nombre, correo, teléfono, dirección y todos los campos obligatorios del esquema.
- [x] Normalizar correo antes de comprobar duplicados.
- [x] Distinguir origen Marketplace y registro administrativo.
- [x] Aplicar eliminación lógica sin eliminar pedidos.
- [ ] Permitir que un cliente solo edite o desactive su perfil mediante validación del actor.

### 9.2. Usuarios

- [x] Crear DTOs de creación interna, actualización, filtro y respuesta.
- [x] No incluir hash o contraseña en las respuestas.
- [x] Validar correo único dentro de usuarios.
- [x] Validar rol existente para usuarios internos; `ROL` no posee campo de estado.
- [x] Validar asociación con cliente para usuarios Marketplace.
- [x] Permitir que cliente y usuario vinculados compartan su propio correo.
- [x] Rechazar que personas distintas reutilicen el mismo correo.
- [x] Aplicar eliminación lógica mediante `USR_ESTADO = 'INA'`.

### 9.3. Contraseñas y login

- [x] Crear DTOs `LoginRequest`, `LoginResponse` y los necesarios para registrar/cambiar contraseña.
- [x] Crear `IPasswordHashService` sin exponer infraestructura a otras capas.
- [x] Implementar hash seguro; nunca comparar ni almacenar texto plano.
- [x] Validar usuario existente y activo.
- [x] Validar cliente asociado activo cuando corresponda.
- [x] Devolver identidad y rol necesarios para que Api genere JWT.
- [x] No generar JWT dentro de Business.
- [x] No definir todavía bloqueo, recuperación o expiración exacta sin política confirmada.

### 9.4. Autorregistro Marketplace

- [x] Diseñar un caso de uso que cree coordinadamente Cliente y Usuario.
- [x] Asignar origen `MKT` y el rol oficial de cliente una vez confirmado su nombre/ID.
- [x] Solicitar dirección y todos los campos obligatorios.
- [x] Validar correo global según la regla de vinculación cliente-usuario.
- [x] Exigir una operación atómica mediante `IAutorregistroMarketplaceDataService` y transacción de DataManagment.

## 10. Fase B-5: proveedores y materiales

### 10.1. Proveedores

- [x] Crear DTOs, filtro, response, validator, mapper, interfaz y servicio.
- [x] Validar nombre, teléfono, correo y dirección.
- [x] Validar correo único normalizado.
- [x] Aplicar eliminación lógica.

### 10.2. Materiales

- [x] Crear DTOs con `decimal` para stock.
- [x] Validar categoría existente y activa.
- [x] Validar nombre único.
- [x] Validar unidad de medida y cantidades con máximo tres decimales.
- [x] No permitir editar stock como parte del CRUD ordinario.
- [x] Reservar los cambios posteriores de stock exclusivamente para movimientos.

### 10.3. Proveedor-material

- [x] Crear DTOs, response, validator, mapper, interfaz y servicio.
- [x] Validar existencia y estado de proveedor y material.
- [x] Rechazar la combinación duplicada.
- [x] Validar precio no negativo, cantidad mínima positiva y días de entrega válidos.
- [x] Validar los códigos de origen `NAC` e `IMP` definidos por el constraint actual.
- [x] No agregar eliminación física o lógica no soportada por el modelo actual.

## 11. Fase B-6: inventario y movimientos

### 11.1. Movimientos de materiales

- [x] Crear request de creación, filtro y response.
- [x] Usar `decimal` para cantidad y stock resultante.
- [x] Validar cantidad positiva y máximo tres decimales.
- [x] Validar tipo `ING`, `EGR`, `AJP` o `AJN`.
- [x] Exigir motivo, especialmente para ajustes.
- [x] Calcular el stock resultante en Business.
- [x] Sumar para `ING` y `AJP`; restar para `EGR` y `AJN`.
- [x] Rechazar resultados negativos.
- [x] Llamar a la operación transaccional de DataManagment.
- [x] No incluir usuario responsable.

### 11.2. Movimientos de productos

- [x] Crear request de creación, filtro y response.
- [x] Mantener cantidades enteras positivas.
- [x] Aplicar la misma convención de tipos y signo.
- [x] Rechazar stock negativo.
- [x] Mantener movimiento y stock en una transacción.
- [x] Tratar movimientos como históricos e inmutables.

## 12. Fase B-7: productos, costos y recetas

### 12.1. DTOs y validación de producto

- [x] Crear requests de creación/actualización, filtro y response.
- [x] Incluir colores JSON, costos, gastos, margen, personalizable, stock, descuento y estado.
- [x] Validar línea existente y activa.
- [x] Validar nombre único.
- [x] Validar peso positivo y costos no negativos.
- [x] Validar gastos entre `0` y `100`.
- [x] Validar margen entre `0` inclusive y `100` exclusivo.
- [x] Validar indicadores `S/N` y estados `ACT/INA`.
- [x] Validar que colores sea una estructura JSON aceptada.

### 12.2. Cálculo de precio

- [x] Ignorar cualquier `precio_base` calculado enviado por el cliente.
- [x] Calcular `costo_base = costo_mat_prim + costo_mano_obra`.
- [x] Calcular gastos fijos y operativos como porcentajes de `costo_base`.
- [x] Calcular `costo_total` sumando ambos gastos.
- [x] Calcular `precio_base = costo_total / (1 - margen / 100)`.
- [x] Redondear el resultado con la política monetaria común.
- [x] Recalcular al crear o cambiar componentes de costo.

### 12.3. Recetas producto-material

- [x] Crear DTOs, validator, mapper, interfaz y servicio.
- [x] Usar cantidades `decimal(12,3)` positivas.
- [x] Validar existencia de producto y material.
- [x] Rechazar combinaciones duplicadas.
- [x] Validar coherencia de materiales personalizables.

### 12.4. Registro de productos ya fabricados

La decisión vigente indica que el producto se registra cuando físicamente ya existe y la cantidad fabricada debe ser mayor que cero. Deben contemplarse producto nuevo e incremento de uno existente.

- [x] Diseñar `RegistrarProductoFabricadoRequest` con cantidad entera positiva.
- [x] Calcular consumo: `cantidad_req × cantidad_fabricada`.
- [x] Validar todos los materiales antes de modificar cualquier stock.
- [x] Rechazar la operación completa si falta un material.
- [x] Crear movimientos `EGR` de materiales.
- [x] Incrementar stock del producto.
- [x] Crear movimiento `ING` del producto.
- [x] Confirmar todos los cambios en una sola transacción.
- [x] Diferenciar creación de producto nuevo e incremento de producto existente.
- [x] No crear tabla `PRODUCCION` en este sprint.

### 12.5. Puerta técnica de fabricación

- [x] Confirmar que DataManagment exponga una única operación atómica para producto, varios materiales y movimientos.
- [x] Ampliar DataManagment mediante `IProductoFabricacionDataService` antes de ejecutar la fabricación desde Business.
- [x] No simular atomicidad llamando secuencialmente a servicios que confirmen transacciones independientes.

## 13. Fase B-8: descuentos

### 13.1. Administración

- [x] Crear DTOs de creación, actualización, filtro y respuesta.
- [x] Crear validator, mapper, interfaz y servicio.
- [x] Validar producto existente.
- [x] Validar cantidad mínima mayor que cero.
- [x] Validar porcentaje mayor que cero y menor o igual que cien.
- [x] Rechazar dos tramos con igual cantidad mínima para un producto.
- [x] Aplicar eliminación lógica mediante `INA`.

### 13.2. Coherencia del producto

- [x] Al activar la primera regla, establecer `tiene_descuentos = 'S'`.
- [x] Al inactivar la última regla activa, establecer `tiene_descuentos = 'N'`.
- [x] Al reactivar una regla, volver a establecer `S`.
- [x] Confirmar que regla e indicador cambien atómicamente.
- [x] Ampliar DataManagment con `IDescuentoAdministracionDataService` para ejecutar la operación compuesta.

### 13.3. Selección del tramo

- [x] Seleccionar la regla activa con mayor `cantidad_minima <= cantidad comprada`.
- [x] No sumar porcentajes de distintos tramos.
- [x] No exigir stock para consultar una regla.
- [x] Si no existe tramo aplicable, utilizar porcentaje y monto cero.

## 14. Fase B-9: personalizaciones e imágenes

### 14.1. Personalizaciones

- [x] Crear DTOs, validator, mapper, interfaz y servicio.
- [x] Validar que el producto esté marcado como personalizable.
- [x] Validar tipos `LISTA`, `MATERIAL`, `TEXTO`, `MEDIDA` y `COLOR`.
- [x] Validar que `valores_json` sea un objeto.
- [x] Validar el esquema enriquecido por tipo: versión, requerido, opciones, límites y costos.
- [x] Para `LISTA` y `COLOR`, validar opciones permitidas.
- [x] Para `MATERIAL`, validar material permitido.
- [x] Para `TEXTO`, validar longitud y contenido configurados.
- [x] Para `MEDIDA`, validar rango y unidad configurados.
- [x] No incluir archivos, DTF, dibujos ni diseños.

### 14.2. Imágenes

- [x] Crear DTOs de creación, actualización y respuesta.
- [x] Validar producto existente, URL, indicador principal y orden.
- [x] Garantizar máximo una imagen principal por producto.
- [x] Rechazar una segunda imagen principal mediante una operación transaccional de DataManagment.
- [x] No exponer eliminación.

## 15. Fase B-10: favoritos y Marketplace

### 15.1. Favoritos

- [x] Crear request de agregar, filtro y response.
- [x] Validar cliente y producto existentes.
- [x] Rechazar combinación duplicada.
- [x] Permitir agregar aunque el stock sea cero.
- [x] Quitar favorito mediante eliminación física de la relación.
- [x] Validar mediante `idClienteActor` que el cliente autenticado opere solo sobre sus favoritos.

### 15.2. Catálogo público

- [x] Crear response específico del Marketplace sin campos administrativos sensibles.
- [x] Mostrar únicamente productos `ACT`.
- [x] Distinguir activo agotado de producto inactivo.
- [x] Permitir filtros de línea, precio, personalización, stock y descuentos.
- [x] Mantener consulta pública sin autorización; Api decidirá el endpoint anónimo.

## 16. Fase B-11: pedidos y cálculo comercial

### 16.1. DTOs

- [ ] Crear `CrearPedidoRequest`, `ActualizarPedidoRequest`, `PedidoFiltroRequest` y `PedidoResponse`.
- [ ] Crear DTO de detalle solicitado sin aceptar cálculos confiables del cliente.
- [ ] Crear DTO de personalización seleccionada.
- [ ] Crear DTO de resultado de pago simulado.
- [ ] Mantener `entrega_fisica` como `S/N`.

### 16.2. Creación y edición

- [ ] Validar cliente y método de pago válidos.
- [ ] Exigir al menos un detalle.
- [ ] Exigir cantidades enteras positivas.
- [ ] Rechazar productos repetidos, incluso con personalizaciones diferentes.
- [ ] Validar productos activos y stock suficiente al crear.
- [ ] No reservar ni descontar stock al crear pedido pendiente.
- [ ] Crear pedido con estado `PEN` y fecha UTC.
- [ ] Permitir edición únicamente mientras esté `PEN`.
- [ ] No implementar cancelación ni eliminación.

### 16.3. Personalización seleccionada

- [ ] Rechazar personalización si el producto no es personalizable.
- [ ] Validar cada valor seleccionado contra la configuración vigente.
- [ ] Exigir mínimo tres unidades para sábanas personalizadas.
- [ ] Copiar el JSON validado al detalle para conservar el histórico.
- [ ] Calcular costos adicionales según las opciones seleccionadas.
- [ ] No incluir todavía imágenes de referencia o archivos.

### 16.4. Precio, descuento e IVA

- [ ] Tomar el precio base vigente desde Business/DataManagment, no desde el cliente.
- [ ] Sumar costos de personalización según la política confirmada.
- [ ] Seleccionar el tramo de descuento aplicable por producto.
- [ ] Aplicar el descuento sobre la base comercial aprobada.
- [ ] Persistir `precio_unitario`, `porcentaje_descuento`, `monto_descuento` y `subtotal_item`.
- [ ] Cumplir `subtotal_item = precio_unitario × cantidad - monto_descuento`.
- [ ] Calcular subtotal del pedido desde los detalles.
- [ ] Mantener IVA en `0` durante este sprint.
- [ ] Cumplir `total = subtotal + iva`.
- [ ] Redondear por detalle y total con la política común.
- [ ] No recalcular pedidos `REA` cuando cambien productos o reglas.

### 16.5. Entrega física

- [ ] Validar `S` como retiro en tienda.
- [ ] Validar `N` como entrega coordinada externamente.
- [ ] No crear registros de envío.
- [ ] No agregar costos de entrega sin una decisión posterior.

### 16.6. Pago simulado

- [ ] Validar el identificador y obtener el pedido.
- [ ] Rechazar pedido inexistente, no pendiente o sin detalles.
- [ ] Delegar la transacción de stock y movimientos a `IPedidoDataService.PagarAsync`.
- [ ] Traducir cada estado del resultado a respuesta o excepción funcional.
- [ ] Exponer stock insuficiente como conflicto comprensible.
- [ ] Preservar fecha de pago UTC y estado `REA`.
- [ ] Rechazar un segundo intento sin descontar nuevamente.

## 17. Fase B-12: autorización funcional

- [ ] Definir un DTO/contexto de actor independiente de `HttpContext`.
- [ ] Permitir que Api construya ese contexto desde claims.
- [ ] Aplicar Superadministrador para usuarios internos, roles y configuración técnica.
- [ ] Aplicar Administrador al resto de gestión autorizada, excluyendo roles y configuración técnica.
- [ ] Aplicar propiedad del recurso para perfil, favoritos, pedidos e historial del cliente.
- [ ] No confiar únicamente en ocultar botones en frontend.
- [ ] Completar una matriz de permisos antes de cerrar esta fase.
- [ ] Mantener `[Authorize]`, policies y códigos HTTP en Api.

## 18. Fase B-13: revisión transversal

- [ ] Confirmar que Business solo referencia DataManagment.
- [ ] Confirmar que ningún DTO exponga password hash, entidades o DataModels.
- [ ] Confirmar que todos los comandos validen antes de persistir.
- [ ] Confirmar que códigos y correos se normalicen uniformemente.
- [ ] Confirmar que todas las fechas persistidas sean UTC.
- [ ] Confirmar que identificadores de creación no sean aceptados del cliente.
- [ ] Confirmar que la eliminación sea lógica salvo favoritos.
- [ ] Confirmar que imágenes y métodos de pago no expongan eliminación.
- [ ] Confirmar que pedidos, movimientos, asociaciones y personalizaciones no se eliminen.
- [ ] Confirmar que no exista administración de auditoría.
- [ ] Confirmar que IVA sea cero pero sus campos permanezcan.
- [ ] Confirmar que no se hayan implementado funcionalidades diferidas.

## 19. Fase B-14: pruebas unitarias

- [ ] Crear proyecto de pruebas para Business si todavía no existe.
- [ ] Probar validators con entradas válidas, vacías, límites y códigos inválidos.
- [ ] Probar duplicados y registros inexistentes con servicios de datos simulados.
- [ ] Probar cálculos monetarios y redondeos.
- [ ] Probar selección de tramos `3 → 5 %`, `6 → 10 %` y ausencia de tramo.
- [ ] Probar que los tramos no se acumulen.
- [ ] Probar movimientos de ingreso, egreso, ajustes y stock insuficiente.
- [ ] Probar consumo decimal de materiales al fabricar.
- [ ] Probar rechazo total de fabricación cuando falta un material.
- [ ] Probar personalizaciones válidas e inválidas por tipo.
- [ ] Probar mínimo de sábanas personalizadas.
- [ ] Probar pedido duplicado por producto.
- [ ] Probar totales con IVA cero.
- [ ] Probar pago exitoso, stock insuficiente, pedido no pendiente y concurrencia.
- [ ] Probar favoritos de productos agotados.
- [ ] Probar autorización funcional por rol y propiedad.

## 20. Fase B-15: compilación e integración interna

- [ ] Compilar DataAccess.
- [ ] Compilar DataManagment.
- [ ] Compilar Business.
- [ ] Ejecutar pruebas unitarias de Business.
- [ ] Compilar la solución e identificar únicamente pendientes de Api.
- [ ] Confirmar cero advertencias nuevas relevantes.
- [ ] No conectarse todavía a PostgreSQL si Api y DI aún no están configurados.
- [ ] Preparar la lista de servicios Business/DataManagment que Api deberá registrar.

Comandos previstos:

```powershell
dotnet build .\Servicio.RetazoMarket.DataAccess\Servicio.RetazoMarket.DataAccess.csproj
dotnet build .\Servicio.RetazoMarket.DataManagment\Servicio.RetazoMarket.DataManagment.csproj
dotnet build .\Servicio.RetazoMarket.Business\Servicio.RetazoMarket.Business.csproj
dotnet build .\Servicio.RetazoMarket.slnx
```

## 21. Orden recomendado de desarrollo

```text
B-0  Estructura
B-1  Excepciones y componentes comunes
B-2  Línea como patrón base
B-3  Roles, categorías y métodos de pago
B-4  Clientes, usuarios y autenticación
B-5  Proveedores y materiales
B-6  Movimientos
B-7  Productos, precios, recetas y fabricación
B-8  Descuentos
B-9  Personalizaciones e imágenes
B-10 Favoritos y catálogo público
B-11 Pedidos y pago simulado
B-12 Autorización funcional
B-13 Revisión transversal
B-14 Pruebas unitarias
B-15 Compilación final
```

Cada módulo debe compilar antes de iniciar el siguiente. No se implementará un caso de uso que requiera atomicidad hasta que DataManagment exponga la operación transaccional correspondiente.

## 22. Criterios de finalización

- [ ] Existe la estructura completa de Business.
- [ ] Todos los módulos del alcance tienen DTOs propios.
- [ ] Todos los comandos tienen validadores.
- [ ] Todos los módulos tienen mappers DTO ⇄ DataModel.
- [ ] Api podrá depender únicamente de interfaces Business.
- [ ] Ningún controlador futuro necesitará DataManagment directamente.
- [ ] Las reglas confirmadas están implementadas y probadas.
- [ ] Las reglas no confirmadas permanecen documentadas y no fueron inventadas.
- [ ] Los precios, descuentos, IVA y redondeos son deterministas.
- [ ] Las operaciones de stock son atómicas.
- [ ] La seguridad funcional está separada de JWT y HTTP.
- [ ] Business no referencia DataAccess, EF Core, PostgreSQL ni Api.
- [ ] Business compila sin errores ni advertencias nuevas relevantes.
- [ ] Las pruebas unitarias críticas son exitosas.
- [ ] No se implementó administración de auditoría.
- [ ] No se implementaron envíos, cancelación, facturación ni pasarelas reales.

## 23. Próximo paso

Después de completar Business:

```text
Business estable y probado
→ configurar Api y Program.cs
→ registrar dependencias
→ configurar autenticación, JWT, CORS, versionado y Swagger
→ crear middleware de excepciones
→ exponer controladores versionados
→ ejecutar pruebas integrales contra PostgreSQL
```
