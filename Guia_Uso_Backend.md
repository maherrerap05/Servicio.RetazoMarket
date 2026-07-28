# Guía de uso del backend de Retazo Market

## 1. Propósito

Esta guía describe los endpoints disponibles en la API de Retazo Market y las reglas que debe considerar el frontend al consumirlos. La información se basa en los controladores, DTO, validadores y servicios implementados, además de los casos funcionales ejecutados en `Matriz_Pruebas_Api_Resumida.xlsx`.

La versión actual de la API es la **V1** y todas las rutas parten de:

```text
https://localhost:{puerto}/api/v1
```

El puerto depende del perfil con el que se ejecute `Servicio.RetazoMarket.Api`.

## 2. Convenciones generales

### 2.1. Encabezados

Para solicitudes con JSON:

```http
Content-Type: application/json
Accept: application/json
```

Para endpoints protegidos:

```http
Authorization: Bearer <TOKEN_JWT>
```

No se debe enviar el encabezado `Authorization` a los endpoints públicos.

### 2.2. Respuesta exitosa

Toda respuesta exitosa utiliza este sobre:

```json
{
  "success": true,
  "message": "Operación exitosa",
  "data": {}
}
```

`data` puede ser un objeto, una colección, un resultado paginado o un valor booleano.

### 2.3. Respuesta de error

```json
{
  "success": false,
  "message": "Descripción general del error.",
  "errors": [
    "Detalle de validación."
  ],
  "traceId": "identificador-de-la-solicitud"
}
```

El frontend debe mostrar `message` como mensaje principal y, cuando `errors` contenga elementos, presentar también sus detalles. `traceId` sirve para rastrear un error en los registros del backend.

Estados HTTP habituales:

| Estado | Significado |
|---|---|
| `200` | Consulta, actualización, eliminación lógica o acción completada |
| `201` | Recurso creado |
| `400` | Datos inválidos o regla de negocio incumplida |
| `401` | Token ausente, inválido o vencido |
| `403` | Usuario autenticado sin permisos o acceso a un recurso ajeno |
| `404` | Recurso inexistente o no visible para el actor |
| `409` | Conflicto de unicidad |
| `500` | Error interno no controlado |

### 2.4. Paginación

Los endpoints `buscar` y los listados paginados reciben normalmente:

```text
page_number=1&page_size=10
```

La respuesta paginada tiene esta estructura:

```json
{
  "items": [],
  "pageNumber": 1,
  "pageSize": 10,
  "totalRecords": 0,
  "totalPages": 0,
  "hasPreviousPage": false,
  "hasNextPage": false
}
```

El frontend debe tratar los filtros vacíos como parámetros omitidos. No debe enviar cadenas como `"string"`, `"null"` o `"undefined"`.

### 2.5. Nombres y tipos

- Los cuerpos JSON utilizan `snake_case`.
- Los estados de catálogos y entidades suelen ser `ACT` o `INA`.
- Los indicadores de un carácter utilizan `"S"` o `"N"`.
- `entrega_fisica = "S"` significa retiro en tienda.
- `entrega_fisica = "N"` significa entrega coordinada externamente.
- Las fechas se intercambian en UTC con formato ISO 8601.
- Los importes admiten decimales.
- Las cantidades de materiales admiten decimales; las cantidades de productos son enteras.

## 3. Autenticación y autorización

### 3.1. Roles

| Política | Roles admitidos | Uso |
|---|---|---|
| Pública | Sin token | Login, autorregistro y Marketplace |
| Cliente | `CLIENTE` | Perfil, favoritos y pedidos propios |
| Administrador | `ADMINISTRADOR`, `SUPERADMINISTRADOR` | Todos los módulos internos excepto usuarios y roles |
| Superadministrador | `SUPERADMINISTRADOR` | Todo lo permitido al administrador, más usuarios y roles |

El rol `ADMINISTRADOR` puede utilizar todos los endpoints de catálogo y los demás módulos internos. Únicamente la gestión de roles y usuarios permanece reservada para `SUPERADMINISTRADOR`.

### 3.2. Login

**POST** `/auth/login` — público

```json
{
  "userName": "correo-o-usuario",
  "password": "contraseña"
}
```

La respuesta contiene:

```json
{
  "idUsuario": 1,
  "userName": "usuario",
  "correo": "usuario@dominio.com",
  "activo": true,
  "idCliente": null,
  "roles": ["ADMINISTRADOR"],
  "token": "<JWT>",
  "expirationUtc": "2026-01-01T00:00:00Z"
}
```

Consideraciones para el frontend:

- Guardar el token de forma segura y enviarlo con el prefijo `Bearer`.
- Cerrar la sesión cuando llegue `401`.
- Usar `expirationUtc` para anticipar el vencimiento.
- `idCliente` es `null` para usuarios internos.
- Un usuario `CLIENTE` incluye el claim `id_cliente`; el backend usa ese claim para impedir acceso horizontal a datos de otros clientes.

### 3.3. Autorregistro

**POST** `/auth/registro` — público

```json
{
  "nombre": "Jorge",
  "apellidos": "Jara",
  "correo": "jorge@gmail.com",
  "telefono": "0999999999",
  "direccion": "Dirección del cliente",
  "contrasena": "clave-segura"
}
```

Crea de forma coordinada un cliente y un usuario con rol `CLIENTE`. El correo debe ser único globalmente. Después del registro el frontend debe dirigir al login; el endpoint no inicia sesión automáticamente.

## 4. Marketplace público

### 4.1. Buscar productos

**GET** `/marketplace/productos` — público

Filtros:

| Parámetro | Tipo | Descripción |
|---|---|---|
| `nombre` | string | Coincidencia por nombre |
| `id_linea` | integer | Línea del producto |
| `es_personalizable` | `S`/`N` | Filtra personalizables |
| `tiene_descuentos` | `S`/`N` | Filtra productos con descuentos |
| `con_stock` | boolean | `true` exige existencias |
| `precio_minimo` | decimal | Precio inferior |
| `precio_maximo` | decimal | Precio superior |
| `page_number` | integer | Página, inicia en 1 |
| `page_size` | integer | Tamaño de página |

Ejemplo:

```http
GET /api/v1/marketplace/productos?id_linea=9&con_stock=true&page_number=1&page_size=12
```

Reglas:

- Solo expone productos `ACT`.
- Un producto activo con stock cero sigue visible y su `disponibilidad` indica que está agotado.
- No expone costos, margen, gastos, receta ni movimientos.
- Devuelve `precio_base`, colores, disponibilidad e imágenes públicas.

### 4.2. Detalle público

**GET** `/marketplace/productos/{id}` — público

Devuelve `ProductoMarketplaceResponse`. Si el producto no existe o no está publicable responde `404`.

El frontend no debe depender de campos internos como `costo_mat_prim`, `costo_mano_obra`, porcentajes o receta, pues deliberadamente no pertenecen a este contrato.

## 5. Área del cliente

Todos los endpoints de esta sección requieren un token con rol `CLIENTE`.

### 5.1. Perfil

| Método | Ruta | Función |
|---|---|---|
| `GET` | `/cliente/perfil` | Obtiene el perfil asociado al JWT |
| `PUT` | `/cliente/perfil` | Actualiza el perfil asociado al JWT |

Cuerpo de actualización:

```json
{
  "id_cliente": 12,
  "nombre": "Jorge",
  "apellidos": "Jara",
  "correo": "jorge@gmail.com",
  "telefono": "0999999999",
  "direccion": "Nueva dirección",
  "origen": "MKT",
  "cli_estado": "ACT"
}
```

El controlador ignora el `id_cliente`, `origen` y `cli_estado` manipulados por el navegador: toma el cliente desde el JWT y conserva los valores protegidos. El frontend puede conservar el cuerpo completo por compatibilidad con el DTO, pero nunca debe utilizarlo para intentar cambiar de propietario.

### 5.2. Favoritos

| Método | Ruta | Función |
|---|---|---|
| `GET` | `/cliente/favoritos` | Lista todos los favoritos del cliente |
| `GET` | `/cliente/favoritos/buscar` | Lista paginada con filtros |
| `GET` | `/cliente/favoritos/{idProducto}` | Comprueba/obtiene un favorito |
| `POST` | `/cliente/favoritos` | Agrega un producto |
| `DELETE` | `/cliente/favoritos/{idProducto}` | Quita un producto |

Cuerpo para agregar:

```json
{
  "id_cliente": 0,
  "id_producto": 17
}
```

`id_cliente` es sobrescrito con el claim. Un producto se puede agregar aunque no tenga stock: favoritos expresa interés, no reserva existencias. Agregar dos veces la misma combinación produce conflicto.

Filtros de búsqueda:

```text
nombre_producto, id_linea, estado_producto, page_number, page_size
```

### 5.3. Pedidos del cliente

| Método | Ruta | Función |
|---|---|---|
| `GET` | `/cliente/pedidos` | Busca únicamente pedidos propios |
| `GET` | `/cliente/pedidos/{id}` | Obtiene un pedido propio |
| `POST` | `/cliente/pedidos` | Crea un pedido para el cliente autenticado |
| `PUT` | `/cliente/pedidos/{id}` | Edita un pedido propio pendiente |
| `POST` | `/cliente/pedidos/{id}/pagar` | Ejecuta el pago simulado de un pedido propio |

Filtros:

```text
estado, fecha_desde_utc, fecha_hasta_utc, id_linea, id_metodo,
page_number, page_size
```

El `id_cliente` enviado en filtros o cuerpos se sobrescribe con el claim. Un cliente no puede consultar, editar ni pagar pedidos ajenos.

## 6. Pedidos y pago

Los endpoints internos usan la base `/internal/pedidos` y admiten `ADMINISTRADOR` o `SUPERADMINISTRADOR`.

### 6.1. Endpoints internos

| Método | Ruta | Función |
|---|---|---|
| `GET` | `/internal/pedidos` | Lista completa |
| `GET` | `/internal/pedidos/{id}` | Detalle por ID |
| `GET` | `/internal/pedidos/buscar` | Búsqueda paginada |
| `POST` | `/internal/pedidos` | Crea un pedido |
| `PUT` | `/internal/pedidos/{id}` | Actualiza un pedido pendiente |
| `POST` | `/internal/pedidos/{id}/pagar` | Confirma el pago simulado |

### 6.2. Cuerpo para crear

```json
{
  "id_metodo": 9,
  "id_cliente": 12,
  "entrega_fisica": "N",
  "detalles": [
    {
      "id_producto": 18,
      "cantidad": 2,
      "personalizaciones": []
    }
  ]
}
```

El cuerpo de actualización agrega `id_pedido`, aunque el controlador utiliza el ID de la ruta:

```json
{
  "id_pedido": 12,
  "id_metodo": 9,
  "id_cliente": 12,
  "entrega_fisica": "N",
  "detalles": []
}
```

Reglas importantes:

- El cliente, método y productos deben existir y estar activos.
- Cada producto aparece una sola vez por pedido.
- La cantidad debe ser positiva y no superar el stock.
- Al crear, el pedido queda `PEN`.
- Solo un pedido `PEN` se puede editar.
- Crear el pedido **no descuenta stock**.
- Pagar cambia el estado a `REA`, registra `fecha_pago`, descuenta stock y crea movimientos `EGR`.
- Un segundo pago se rechaza y no descuenta stock nuevamente.
- IVA se conserva en `0`; por ello `total = subtotal`.
- Los descuentos se calculan por detalle y se guarda el porcentaje/monto histórico.
- Se aplica únicamente el tramo activo más alto cuya cantidad mínima se alcanzó; los porcentajes no se acumulan.

### 6.3. Personalizaciones seleccionadas

Ejemplo:

```json
{
  "id_producto": 17,
  "cantidad": 3,
  "personalizaciones": [
    {
      "id_opcion": 8,
      "valor": "ALG"
    },
    {
      "id_opcion": 9,
      "valor": 2.0
    }
  ]
}
```

Todas las configuraciones cuyo `valores_json.requerido` sea `true` deben enviarse.

Formato de `valor`:

| Tipo | Valor enviado |
|---|---|
| `LISTA` / `COLOR` | Código, por ejemplo `"ALG"` |
| `MEDIDA` | Número dentro del rango, por ejemplo `2.0` |
| `TEXTO` | Cadena que cumpla longitud y patrón |
| `MATERIAL` | ID numérico de un material permitido |

Para sábanas personalizadas se requieren mínimo tres unidades. El backend guarda una copia histórica normalizada de la selección en JSONB.

## 7. Usuarios y roles

Solo `SUPERADMINISTRADOR`.

### 7.1. Roles — `/internal/roles`

| Método | Ruta | Función |
|---|---|---|
| `GET` | `/internal/roles` | Lista roles |
| `GET` | `/internal/roles/{id}` | Obtiene por ID |
| `GET` | `/internal/roles/nombre/{nombre}` | Obtiene por nombre |
| `GET` | `/internal/roles/buscar` | Filtra por `nombre_rol` y pagina |
| `POST` | `/internal/roles` | Crea |
| `PUT` | `/internal/roles/{id}` | Actualiza |

Crear:

```json
{
  "nombre_rol": "CLIENTE"
}
```

Actualizar incluye `id_rol`, pero prevalece el ID de la ruta.

### 7.2. Usuarios — `/internal/usuarios`

| Método | Ruta | Función |
|---|---|---|
| `GET` | `/internal/usuarios` | Lista usuarios |
| `GET` | `/internal/usuarios/{id}` | Obtiene por ID |
| `GET` | `/internal/usuarios/correo/{correo}` | Obtiene por correo |
| `GET` | `/internal/usuarios/buscar` | Busca y pagina |
| `POST` | `/internal/usuarios` | Crea usuario |
| `PUT` | `/internal/usuarios/{id}` | Actualiza datos/rol/estado |
| `PUT` | `/internal/usuarios/{id}/contrasena` | Cambia contraseña |
| `DELETE` | `/internal/usuarios/{id}` | Eliminación lógica |

Crear:

```json
{
  "id_cliente": null,
  "id_rol": 1,
  "nombre": "Usuario interno",
  "correo": "usuario@retazomarket.com",
  "contrasena": "clave",
  "usr_estado": "ACT"
}
```

Cambiar contraseña:

```json
{
  "id_usuario": 1,
  "contrasena_actual": "clave-anterior",
  "contrasena_nueva": "clave-nueva"
}
```

Filtros: `nombre`, `correo`, `estado`, `id_rol`, `id_cliente`, `page_number`, `page_size`.

## 8. Catálogos internos

Todos los catálogos de esta sección admiten `ADMINISTRADOR` y `SUPERADMINISTRADOR`. La exclusividad del superadministrador se limita a los módulos de roles y usuarios.

### 8.1. Categorías de materiales — `/internal/categorias-materiales`

| Método | Ruta |
|---|---|
| `GET` | `/internal/categorias-materiales` |
| `GET` | `/internal/categorias-materiales/{id}` |
| `GET` | `/internal/categorias-materiales/nombre/{nombre}` |
| `POST` | `/internal/categorias-materiales` |
| `PUT` | `/internal/categorias-materiales/{id}` |
| `DELETE` | `/internal/categorias-materiales/{id}` |

```json
{
  "cat_nombre": "Telas",
  "cat_estado": "ACT"
}
```

### 8.2. Líneas — `/internal/lineas`

| Método | Ruta |
|---|---|
| `GET` | `/internal/lineas` |
| `GET` | `/internal/lineas/{id}` |
| `GET` | `/internal/lineas/nombre/{nombre}` |
| `GET` | `/internal/lineas/buscar` |
| `POST` | `/internal/lineas` |
| `PUT` | `/internal/lineas/{id}` |
| `DELETE` | `/internal/lineas/{id}` |

```json
{
  "lin_nombre": "Cosméticos",
  "lin_estado": "ACT"
}
```

Filtros: `lin_nombre`, `lin_estado`, `page_number`, `page_size`.

### 8.3. Métodos de pago — `/internal/metodos-pago`

| Método | Ruta |
|---|---|
| `GET` | `/internal/metodos-pago` |
| `GET` | `/internal/metodos-pago/{id}` |
| `GET` | `/internal/metodos-pago/nombre/{nombre}` |
| `GET` | `/internal/metodos-pago/codigo-sri/{codigoSri}` |
| `POST` | `/internal/metodos-pago` |
| `PUT` | `/internal/metodos-pago/{id}` |

```json
{
  "met_nombre": "EFECTIVO",
  "codigo_sri": "01"
}
```

### 8.4. Personalizaciones — `/internal/personalizaciones`

| Método | Ruta |
|---|---|
| `GET` | `/internal/personalizaciones/{id}` |
| `GET` | `/internal/personalizaciones/producto/{idProducto}` |
| `POST` | `/internal/personalizaciones` |
| `PUT` | `/internal/personalizaciones/{id}` |

Cuerpo base:

```json
{
  "id_producto": 17,
  "nombre_atr": "Tipo de tela",
  "tipo_valor": "LISTA",
  "valores_json": {
    "version": 1,
    "requerido": true,
    "opciones": [
      {
        "codigo": "ALG",
        "nombre": "Algodón"
      }
    ]
  },
  "costo_adicional": 0
}
```

`valores_json` es un objeto JSON real, aunque alguna interfaz de Swagger o generador de formularios pudiera describirlo como string.

Estructuras admitidas:

```json
{
  "version": 1,
  "requerido": true,
  "opciones": [
    {
      "codigo": "ROJO",
      "nombre": "Rojo",
      "costo_adicional": 1.5
    }
  ]
}
```

Para `LISTA` y `COLOR`.

```json
{
  "version": 1,
  "requerido": true,
  "minimo": 1.25,
  "maximo": 3.0,
  "unidad": "metros"
}
```

Para `MEDIDA`.

```json
{
  "version": 1,
  "requerido": false,
  "longitud_minima": 1,
  "longitud_maxima": 30,
  "patron": "^[A-Za-z ]+$"
}
```

Para `TEXTO`.

```json
{
  "version": 1,
  "requerido": true,
  "opciones": [
    {
      "id_material": 11,
      "costo_adicional": 2.0
    }
  ]
}
```

Para `MATERIAL`; el backend valida el material y normaliza su nombre.

## 9. Proveedores y materiales

Política `Administrador`: admite administrador y superadministrador.

### 9.1. Proveedores — `/internal/proveedores`

| Método | Ruta |
|---|---|
| `GET` | `/internal/proveedores` |
| `GET` | `/internal/proveedores/{id}` |
| `GET` | `/internal/proveedores/codigo/{codigo}` |
| `GET` | `/internal/proveedores/correo/{correo}` |
| `GET` | `/internal/proveedores/buscar` |
| `POST` | `/internal/proveedores` |
| `PUT` | `/internal/proveedores/{id}` |
| `DELETE` | `/internal/proveedores/{id}` |

```json
{
  "codigo_proveedor": "PROV-001",
  "prov_nombre": "Proveedor Textil",
  "prov_telefono": "0999999999",
  "prov_correo": "ventas@proveedor.com",
  "prov_direccion": "Dirección",
  "prov_estado": "ACT"
}
```

Filtros: `codigo_proveedor`, `nombre`, `correo`, `estado`, `id_material`, `page_number`, `page_size`.

El código de proveedor es único y se incluye en la información de las relaciones con materiales para facilitar búsquedas del frontend.

### 9.2. Materiales — `/internal/materiales`

| Método | Ruta |
|---|---|
| `GET` | `/internal/materiales` |
| `GET` | `/internal/materiales/{id}` |
| `GET` | `/internal/materiales/nombre/{nombre}` |
| `GET` | `/internal/materiales/buscar` |
| `POST` | `/internal/materiales` |
| `PUT` | `/internal/materiales/{id}` |
| `DELETE` | `/internal/materiales/{id}` |

Crear:

```json
{
  "id_categoria": 1,
  "mat_nombre": "Tela de algodón",
  "unidad_medida": "METRO",
  "stock_actual": 100.5,
  "mat_estado": "ACT"
}
```

Actualizar no recibe `stock_actual`; las existencias se alteran mediante movimientos.

Filtros: `nombre`, `id_categoria`, `estado`, `stock_minimo`, `stock_maximo`, `codigo_proveedor`, `page_number`, `page_size`.

### 9.3. Proveedor-material — `/internal/proveedores-materiales`

| Método | Ruta |
|---|---|
| `GET` | `/internal/proveedores-materiales/material/{idMaterial}/proveedor/{idProveedor}` |
| `GET` | `/internal/proveedores-materiales/proveedor/{id}` |
| `GET` | `/internal/proveedores-materiales/material/{id}` |
| `POST` | `/internal/proveedores-materiales` |
| `PUT` | `/internal/proveedores-materiales/material/{idMaterial}/proveedor/{idProveedor}` |

```json
{
  "id_material": 11,
  "id_proveedor": 2,
  "origen": "NACIONAL",
  "precio_compra": 4.75,
  "cantidad_min": 10,
  "dias_entrega": 3
}
```

La relación se identifica por la clave compuesta material + proveedor.

## 10. Productos

Política `Administrador`.

### 10.1. Endpoints — `/internal/productos`

| Método | Ruta | Función |
|---|---|---|
| `GET` | `/internal/productos` | Lista todos |
| `GET` | `/internal/productos/{id}` | Obtiene detalle interno |
| `GET` | `/internal/productos/nombre/{nombre}` | Busca por nombre |
| `GET` | `/internal/productos/buscar` | Búsqueda paginada |
| `POST` | `/internal/productos` | Crea producto |
| `PUT` | `/internal/productos/{id}` | Actualiza producto |
| `DELETE` | `/internal/productos/{id}` | Eliminación lógica |
| `POST` | `/internal/productos/fabricacion` | Registra fabricación transaccional |

Filtros: `nombre`, `id_linea`, `estado`, `es_personalizable`, `tiene_descuentos`, `con_stock`, `precio_minimo`, `precio_maximo`, `page_number`, `page_size`.

### 10.2. Crear producto

```json
{
  "id_linea": 9,
  "prod_nombre": "Labial artesanal",
  "prod_peso": 0.05,
  "prod_descripcion": "Descripción",
  "colores": ["ROJO", "ROSA", "VINO"],
  "costo_mat_prim": 3.5,
  "costo_mano_obra": 2,
  "porcentaje_margen_ganancia": 30,
  "porcentaje_gastos_fijos": 5,
  "porcentaje_gastos_operativos": 2,
  "stock_actual": 10,
  "es_personalizable": "N",
  "tiene_descuentos": "N",
  "prod_estado": "ACT"
}
```

`colores` es JSONB y se debe enviar como arreglo JSON, no como texto serializado.

El backend calcula `precio_base`:

```text
costoBase = costo_mat_prim + costo_mano_obra
totalCostos = costoBase
            + costoBase * porcentaje_gastos_fijos / 100
            + costoBase * porcentaje_gastos_operativos / 100
precio_base = totalCostos / (1 - porcentaje_margen_ganancia / 100)
```

El precio se redondea a dos decimales. El frontend no debe calcularlo como valor definitivo ni enviarlo.

### 10.3. Receta producto-material

Base: `/internal/productos-materiales`

| Método | Ruta |
|---|---|
| `GET` | `/producto/{idProducto}/material/{idMaterial}` |
| `GET` | `/producto/{id}` |
| `POST` | raíz |
| `PUT` | `/producto/{idProducto}/material/{idMaterial}` |

Crear:

```json
{
  "id_producto": 17,
  "id_material": 11,
  "cantidad_req": 1.125,
  "es_personalizable": "N"
}
```

`cantidad_req` admite decimales y debe ser mayor que cero. La combinación producto-material no se puede duplicar.

### 10.4. Fabricación

Producto existente:

```json
{
  "id_producto": 17,
  "producto_nuevo": null,
  "receta": [],
  "cantidad_fabricada": 1,
  "motivo": "Fabricación"
}
```

El backend carga la receta almacenada, verifica materiales, descuenta sus existencias, aumenta el stock del producto y registra movimientos `EGR`/`ING` en una única transacción.

Producto nuevo:

```json
{
  "id_producto": null,
  "producto_nuevo": {
    "id_linea": 9,
    "prod_nombre": "Producto nuevo",
    "prod_peso": 1,
    "prod_descripcion": "Descripción",
    "colores": ["AZUL"],
    "costo_mat_prim": 10,
    "costo_mano_obra": 5,
    "porcentaje_margen_ganancia": 30,
    "porcentaje_gastos_fijos": 0,
    "porcentaje_gastos_operativos": 0,
    "stock_actual": 0,
    "es_personalizable": "N",
    "tiene_descuentos": "N",
    "prod_estado": "ACT"
  },
  "receta": [
    {
      "id_producto": 0,
      "id_material": 11,
      "cantidad_req": 1.5,
      "es_personalizable": "N"
    }
  ],
  "cantidad_fabricada": 2,
  "motivo": "Registro y fabricación inicial"
}
```

Solo se usa uno de los modos: producto existente o producto nuevo. `cantidad_fabricada` debe ser mayor que cero. Si falta material se revierte toda la operación.

## 11. Imágenes

Base: `/internal/imagenes`. Política `Administrador`.

| Método | Ruta |
|---|---|
| `GET` | `/internal/imagenes/{id}` |
| `GET` | `/internal/imagenes/producto/{id}` |
| `POST` | `/internal/imagenes` |
| `PUT` | `/internal/imagenes/{id}` |

```json
{
  "id_producto": 17,
  "url": "https://cdn.ejemplo.com/producto.jpg",
  "es_principal": "N",
  "orden": 2
}
```

La URL debe ser absoluta, usar HTTP o HTTPS y admitir como máximo 500 caracteres. Una URL relativa como `/imagenes/foto.jpg` se rechaza. `orden` controla la presentación y `es_principal` identifica la imagen destacada.

## 12. Descuentos

Base: `/internal/descuentos`. Política `Administrador`.

| Método | Ruta |
|---|---|
| `GET` | `/internal/descuentos` |
| `GET` | `/internal/descuentos/{id}` |
| `GET` | `/internal/descuentos/producto/{id}` |
| `GET` | `/internal/descuentos/buscar` |
| `POST` | `/internal/descuentos` |
| `PUT` | `/internal/descuentos/{id}` |
| `POST` | `/internal/descuentos/{id}/inactivar` |
| `GET` | `/internal/descuentos/producto/{idProducto}/calcular` |

Crear tramo:

```json
{
  "id_producto": 17,
  "cantidad_minima": 3,
  "porcentaje": 10,
  "estado": "ACT"
}
```

La combinación producto + cantidad mínima es única. Al crear un descuento activo, el producto queda marcado con `tiene_descuentos = "S"`.

Calcular:

```http
GET /api/v1/internal/descuentos/producto/17/calcular?cantidad=6&subtotal=100
```

- `cantidad` selecciona el tramo.
- `subtotal` es `precio unitario × cantidad` antes del descuento.
- El endpoint es solo de consulta y no modifica datos.
- Si existen `3 → 10%` y `5 → 20%`, una cantidad 6 usa únicamente 20%.

Filtros: `id_producto`, `cantidad_minima`, `porcentaje`, `estado`, `page_number`, `page_size`.

## 13. Inventario y movimientos

Política `Administrador`.

### 13.1. Movimientos de materiales

Base: `/internal/inventario/movimientos-materiales`

| Método | Ruta |
|---|---|
| `GET` | `/{id}` |
| `GET` | `/material/{id}` |
| `GET` | `/buscar` |
| `POST` | raíz |

```json
{
  "id_material": 11,
  "tipo_movimiento": "ING",
  "cantidad": 5.5,
  "motivo_mov": "Ingreso de compra",
  "metodo_pago": null
}
```

Tipos: `ING`, `EGR`, `AJP`, `AJN`. Las cantidades admiten decimales. Un egreso superior al stock se rechaza y no deja stock negativo.

### 13.2. Movimientos de productos

Base: `/internal/inventario/movimientos-productos`

| Método | Ruta |
|---|---|
| `GET` | `/{id}` |
| `GET` | `/producto/{id}` |
| `GET` | `/buscar` |
| `POST` | raíz |

```json
{
  "id_producto": 17,
  "tipo_movimiento": "ING",
  "cantidad": 5,
  "motivo_mov": "Ingreso de producto"
}
```

Las cantidades son enteras. Un egreso superior al stock se rechaza.

Filtros de ambos módulos:

```text
id_material/id_producto, tipo_movimiento, motivo,
fecha_desde_utc, fecha_hasta_utc, page_number, page_size
```

## 14. Clientes internos

Base: `/internal/clientes`. Política `Administrador`.

| Método | Ruta |
|---|---|
| `GET` | `/internal/clientes` |
| `GET` | `/internal/clientes/{id}` |
| `GET` | `/internal/clientes/correo/{correo}` |
| `GET` | `/internal/clientes/buscar` |
| `POST` | `/internal/clientes` |
| `PUT` | `/internal/clientes/{id}` |
| `DELETE` | `/internal/clientes/{id}` |

Crear:

```json
{
  "nombre": "Cliente",
  "apellidos": "Ejemplo",
  "correo": "cliente@ejemplo.com",
  "telefono": "0999999999",
  "direccion": "Dirección",
  "origen": "MKT",
  "cli_estado": "ACT"
}
```

Filtros: `nombre`, `apellidos`, `correo`, `telefono`, `origen`, `estado`, `page_number`, `page_size`.

El correo es único globalmente. `DELETE` realiza eliminación lógica.

## 15. Recomendaciones para el frontend

1. Centralizar la URL base y el número de versión en la configuración del entorno.
2. Crear un interceptor HTTP que agregue el JWT solo a endpoints protegidos.
3. Ante `401`, limpiar la sesión y redirigir al login.
4. Ante `403`, mostrar que la acción no está permitida; no tratarlo como error de formulario.
5. Mapear `ApiResponse<T>` y `ApiErrorResponse` en un servicio HTTP común.
6. No enviar propiedades con valores ficticios generados por Swagger.
7. No calcular precios, descuentos, IVA o totales como autoridad final; mostrar siempre los valores respondidos por el backend.
8. No reducir existencias al crear pedidos. El stock se confirma y descuenta al pagar.
9. Después de movimientos, fabricación o pago, volver a consultar stock.
10. Consultar las personalizaciones del producto antes de construir el formulario y enviar todas las requeridas.
11. Tratar `colores` y `valores_json` como JSON real.
12. Mostrar productos activos agotados en Marketplace con una etiqueta “Agotado”, sin ocultarlos.
13. Permitir agregar agotados a favoritos.
14. Deshabilitar la edición y el pago repetido cuando el pedido ya está `REA`, aunque el backend también lo valide.
15. Usar el resultado paginado para habilitar o deshabilitar los controles Anterior/Siguiente.
16. Codificar correctamente valores incluidos en rutas, especialmente correos, nombres y códigos.

## 16. Flujo recomendado de pantallas

### Marketplace

1. Consultar `GET /marketplace/productos`.
2. Abrir `GET /marketplace/productos/{id}`.
3. Si el usuario desea favoritos o comprar, solicitar login.
4. Para un producto personalizable, cargar sus configuraciones mediante el endpoint interno solo en interfaces administrativas. Para el futuro frontend público conviene exponer dichas configuraciones mediante un contrato público si el detalle Marketplace aún no las incluye.
5. Crear el pedido con el cliente obtenido del token.
6. Confirmar el pago mediante `POST /cliente/pedidos/{id}/pagar`.

### Administración

1. Iniciar sesión y construir menú según los roles devueltos.
2. Cargar catálogos antes de formularios dependientes.
3. Crear materiales y proveedores; asociarlos mediante proveedor-material.
4. Crear producto y receta.
5. Registrar fabricación para aumentar existencias consumiendo materiales.
6. Configurar imágenes, personalizaciones y descuentos.
7. Administrar pedidos; el pago simulado es la acción que descuenta el stock.

## 17. Observaciones del alcance actual

- El pago es simulado; no existe integración real con una pasarela.
- El IVA está conservado en el modelo, pero actualmente su tasa es `0`.
- No se implementó una tabla de producción independiente: la fabricación se registra mediante producto, receta y movimientos.
- Las imágenes de referencia dentro de personalizaciones, DTF, dibujos y diseños quedaron fuera del alcance actual.
- La API usa eliminación lógica en los módulos que exponen `DELETE`.
- La tabla de auditoría no se administra mediante endpoints; se consulta directamente en base de datos cuando corresponda.
