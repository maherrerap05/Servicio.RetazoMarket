# Plan detallado de pruebas de la API Retazo Market

## 1. Propósito

Este documento define cómo construir y ejecutar la matriz manual de pruebas de `Servicio.RetazoMarket.Api`. Su contenido deberá utilizarse posteriormente para generar un archivo Excel que permita registrar evidencia, resultados exitosos, defectos y observaciones durante la validación integral de la API.

El objetivo no es comprobar únicamente que una ruta responde. Cada módulo debe validarse desde cinco perspectivas:

1. funcionamiento correcto del caso exitoso;
2. validaciones de entrada y límites;
3. autenticación y autorización por rol;
4. persistencia, relaciones y cambios de estado en PostgreSQL;
5. reglas de negocio, atomicidad y concurrencia.

La cobertura objetivo incluye las 118 operaciones HTTP publicadas actualmente en Swagger v1 y los comportamientos transversales que comparten.

## 2. Formato del Excel de referencia

El archivo de referencia contiene una hoja llamada `Matriz Casos de Prueba` y utiliza estas columnas:

| Columna | Uso |
|---|---|
| ID | Identificador único y estable del caso. |
| Nombre del Caso | Descripción breve del comportamiento probado. |
| Módulo | Área funcional: Autenticación, Productos, Pedidos, etc. |
| Tipo de Prueba | Funcional, Negativa, Validación, Seguridad, Integración, Transaccional o Concurrencia. |
| Datos de Entrada | Método, URL, rol, headers, parámetros, body JSON y precondiciones. |
| Resultado Esperado | Código HTTP, contrato JSON, cambios esperados y cambios que no deben ocurrir. |
| Resultado Obtenido | Código, respuesta y efecto observado durante la ejecución. Debe quedar vacío al generar el Excel. |
| Aprobó | Se marca `X` únicamente cuando todo el resultado esperado se cumple. |
| No Aprobó | Se marca `X` cuando existe cualquier diferencia. |
| Observaciones | Evidencia, IDs generados, defectos, capturas o explicación del resultado. |

El Excel nuevo debe conservar estas diez columnas. En `Datos de Entrada` se escribirá siempre la información con este orden:

```text
Precondiciones:
Rol/token:
Método y URL:
Headers:
Path/query params:
Body JSON:
Datos que deben conservarse para pruebas posteriores:
```

En `Resultado Esperado` se usará este orden:

```text
Código HTTP:
Contrato JSON:
Validaciones de contenido:
Cambio esperado en base de datos:
Estado que no debe modificarse:
```

`Resultado Obtenido`, `Aprobó`, `No Aprobó` y `Observaciones` deben generarse vacíos.

## 3. Ambiente y convenciones

### 3.1. Ambiente

- API ejecutada con el perfil `https` de Visual Studio.
- Swagger: `https://localhost:{puerto}/swagger`.
- API base: `https://localhost:{puerto}/api/v1`.
- PostgreSQL local: `Retazo_Market_DB`.
- No ejecutar migraciones, `EnsureCreated`, restauraciones ni borrados globales durante las pruebas.
- Registrar la fecha, versión del backend, commit probado y nombre del ejecutor en el encabezado del Excel.

### 3.2. Usuarios de control

| Actor | Correo | Rol | Cliente asociado |
|---|---|---|---|
| Dana Bahamonde | `dana@retazomarket.com` | SUPERADMINISTRADOR | No |
| Martín Herrera | `martin@retazomarket.com` | SUPERADMINISTRADOR | No |
| Jaqueline Gamboa | `jaqueline@retazomarket.com` | ADMINISTRADOR | No |
| Cliente A | Crear mediante `/auth/registro` | CLIENTE | Sí |
| Cliente B | Crear mediante `/auth/registro` | CLIENTE | Sí |

Las contraseñas reales no deben copiarse dentro del Excel final. Se utilizarán marcadores como `<CLAVE_DANA>` y `<CLAVE_CLIENTE_A>`.

### 3.3. Tokens

Conservar cuatro variables durante la ejecución:

```text
TOKEN_SUPERADMIN
TOKEN_ADMIN
TOKEN_CLIENTE_A
TOKEN_CLIENTE_B
```

Para toda petición protegida usar:

```http
Authorization: Bearer <TOKEN>
Content-Type: application/json
Accept: application/json
```

### 3.4. Datos de prueba

Todos los valores creados manualmente deben incluir el prefijo `CP-API-` o el dominio `@pruebas.retazomarket.local`. Ejemplos:

- proveedor: `CP-API-PRV-001`;
- correo: `cliente.a@pruebas.retazomarket.local`;
- categoría: `CP-API-TELAS`;
- línea: `CP-API-SABANAS`;
- producto: `CP-API-SABANA-PERSONALIZABLE`.

Los IDs generados se deben registrar en una hoja auxiliar opcional llamada `Datos de Ejecución` o en Observaciones. Nunca se deben inventar IDs para las pruebas dependientes.

## 4. Contratos HTTP que deben validarse siempre

### 4.1. Respuesta exitosa

```json
{
  "success": true,
  "message": "...",
  "data": {}
}
```

Validar que `success` sea `true`, que `message` no esté vacío y que `data` tenga el tipo esperado.

### 4.2. Respuesta fallida

```json
{
  "success": false,
  "message": "...",
  "errors": [],
  "traceId": "..."
}
```

Validar que no aparezcan stack traces, SQL, rutas físicas, cadenas de conexión, hashes ni detalles de excepciones internas.

### 4.3. Códigos esperados

| Código | Uso esperado |
|---|---|
| 200 | Consulta, actualización, baja lógica, inactivación o pago exitoso. |
| 201 | Creación exitosa. |
| 400 | DTO inválido o regla de negocio incumplida. |
| 401 | Sin token, token inválido, token expirado o credenciales incorrectas. |
| 403 | Usuario autenticado sin el rol o propiedad requerida. |
| 404 | Recurso inexistente o no accesible por su identificador. |
| 409 | Violación de unicidad controlada. |
| 500 | Solo para defecto inesperado; nunca debe filtrar información interna. |

### 4.4. Paginación

Toda respuesta paginada debe validar:

- `items` como arreglo;
- `pageNumber` igual al solicitado;
- `pageSize` igual al solicitado;
- `totalRecords` coherente;
- `totalPages` calculado correctamente;
- orden estable al repetir la misma consulta;
- `page_number = 0`, `page_size = 0` y `page_size = 101` producen `400`.

## 5. Preparación ordenada de los datos

La matriz deberá comenzar con casos de preparación y no con `INSERT` directos, salvo los tres roles y usuarios administrativos iniciales. El orden recomendado es:

1. comprobar salud de Swagger y rutas públicas;
2. iniciar sesión con los tres roles internos;
3. crear el rol CLIENTE si todavía no existe;
4. autorregistrar Cliente A y Cliente B;
5. crear categorías, líneas y método de pago;
6. crear proveedores y materiales;
7. relacionar proveedores y materiales;
8. crear productos y recetas;
9. fabricar existencias;
10. crear personalizaciones, imágenes y descuentos;
11. probar Marketplace y favoritos;
12. crear y pagar pedidos;
13. ejecutar bajas lógicas al final;
14. limpiar únicamente registros con prefijo de pruebas.

## 6. Catálogo de casos de prueba

Cada fila de las tablas siguientes debe convertirse en una fila independiente del Excel.

### 6.1. Infraestructura, contratos y seguridad transversal

| ID | Tipo | Caso y ejecución | Resultado esperado |
|---|---|---|---|
| API-GEN-001 | Funcional | Abrir `/swagger` en Development. | 200; documento v1 disponible y operaciones agrupadas. |
| API-GEN-002 | Seguridad | Ejecutar en Production y solicitar `/swagger/v1/swagger.json`. | 404; Swagger no expuesto. |
| API-GEN-003 | Funcional | `GET /marketplace/productos` sin token. | 200 con contrato exitoso paginado. |
| API-GEN-004 | Seguridad | Solicitar cualquier `/internal/*` sin token. | 401 con `ApiErrorResponse` y `traceId`. |
| API-GEN-005 | Seguridad | Solicitar `/cliente/perfil` sin token. | 401. |
| API-GEN-006 | Seguridad | Usar `Bearer token_invalido`. | 401 sin detalles criptográficos. |
| API-GEN-007 | Seguridad | Alterar un carácter de un JWT válido. | 401 por firma inválida. |
| API-GEN-008 | Seguridad | Utilizar token expirado. | 401; no existe tolerancia adicional de reloj. |
| API-GEN-009 | Seguridad | Usar token con issuer incorrecto. | 401. |
| API-GEN-010 | Seguridad | Usar token con audience incorrecta. | 401. |
| API-GEN-011 | Seguridad | ADMIN llama endpoint exclusivo SUPERADMINISTRADOR. | 403 con contrato común. |
| API-GEN-012 | Seguridad | CLIENTE llama endpoint ADMINISTRADOR. | 403. |
| API-GEN-013 | Seguridad | SUPERADMINISTRADOR llama endpoint ADMINISTRADOR. | Acceso permitido. |
| API-GEN-014 | Integración | Enviar JSON mal formado. | 400; la aplicación sigue disponible. |
| API-GEN-015 | Integración | Enviar `Content-Type` incorrecto a un POST. | 415 o rechazo estándar de ASP.NET, sin 500. |
| API-GEN-016 | Seguridad | Provocar un error inesperado controlado en ambiente de prueba. | 500 genérico, sin stack trace, SQL ni secretos. |
| API-GEN-017 | Funcional | Revisar un response con fecha. | Fecha ISO-8601 UTC; conversión local queda al frontend. |
| API-GEN-018 | Seguridad | Enviar request desde origen CORS permitido. | Preflight y operación permitidos. |
| API-GEN-019 | Seguridad | Enviar preflight desde origen no configurado. | Sin encabezado CORS de autorización para ese origen. |
| API-GEN-020 | Validación | Probar paginación 1/10, segunda página, página vacía, 0/10, 1/0 y 1/101. | Resultados coherentes; límites inválidos producen 400. |

### 6.2. Autenticación y autorregistro

| ID | Tipo | Caso y ejecución | Resultado esperado |
|---|---|---|---|
| API-AUTH-001 | Funcional | Login de Dana con correo y clave válidos. | 200; nombre, correo, rol SUPERADMINISTRADOR, JWT y expiración UTC; `IdCliente` nulo. |
| API-AUTH-002 | Funcional | Login de Martín. | Mismo contrato y rol SUPERADMINISTRADOR. |
| API-AUTH-003 | Funcional | Login de Jaqueline. | Rol ADMINISTRADOR e `IdCliente` nulo. |
| API-AUTH-004 | Negativa | Correo existente con clave incorrecta. | 401; mensaje genérico de credenciales. |
| API-AUTH-005 | Negativa | Correo inexistente. | 401 con el mismo mensaje; no revelar existencia del correo. |
| API-AUTH-006 | Validación | Correo vacío y contraseña vacía. | 400; colección con ambos errores. |
| API-AUTH-007 | Validación | `userName` mayor a 100 y clave mayor a 128 caracteres. | 400. |
| API-AUTH-008 | Seguridad | Usuario en estado INA intenta login. | 401; no se emite token. |
| API-AUTH-009 | Seguridad | CLIENTE cuyo cliente asociado está INA intenta login. | 401. |
| API-AUTH-010 | Seguridad | Decodificar payload JWT administrativo. | Solo `sub`, `name`, `email`, `role`, `jti`, issuer, audience y tiempos; sin hash, teléfono o dirección. |
| API-AUTH-011 | Funcional | Autorregistro válido de Cliente A. | 201; crea CLIENTE y USUARIO activos en una transacción y devuelve IDs coherentes. |
| API-AUTH-012 | Funcional | Login del Cliente A recién registrado. | 200; rol CLIENTE y claim/id de cliente asociado. |
| API-AUTH-013 | Validación | Autorregistro sin nombre, correo, teléfono, dirección y contraseña. | 400 con errores múltiples; no crea filas. |
| API-AUTH-014 | Validación | Teléfono con 9 dígitos, letras o 11 dígitos. | 400 en cada variante. |
| API-AUTH-015 | Validación | Correo mal formado o mayor a 100 caracteres. | 400. |
| API-AUTH-016 | Validación | Contraseña de 7 y de 129 caracteres. | 400. |
| API-AUTH-017 | Conflicto | Repetir autorregistro con el mismo correo. | 400/409 conforme al contrato implementado; no duplica cliente ni usuario. |
| API-AUTH-018 | Transaccional | Forzar fallo al crear usuario luego de validar cliente. | No queda un cliente huérfano; rollback total. |

### 6.3. Roles — policy SUPERADMINISTRADOR

Rutas: `GET/POST /internal/roles`, `GET/PUT /internal/roles/{id}`, `GET /nombre/{nombre}` y `GET /buscar`.

| ID | Tipo | Caso | Resultado esperado |
|---|---|---|---|
| API-ROL-001 | Funcional | Listar roles con SUPERADMIN. | 200; contiene los roles configurados. |
| API-ROL-002 | Funcional | Consultar rol por ID válido. | 200 y mismo ID. |
| API-ROL-003 | Funcional | Consultar por nombre exacto. | 200 y nombre normalizado. |
| API-ROL-004 | Funcional | Buscar por nombre y paginar. | Solo coincidencias; metadatos correctos. |
| API-ROL-005 | Negativa | Consultar ID o nombre inexistente. | 404. |
| API-ROL-006 | Validación | Crear con nombre vacío. | 400. |
| API-ROL-007 | Validación | Crear rol fuera de SUPERADMINISTRADOR, ADMINISTRADOR o CLIENTE. | 400. |
| API-ROL-008 | Conflicto | Crear nombre ya existente. | Rechazo; no crea duplicado. |
| API-ROL-009 | Funcional | Actualizar un rol permitido de prueba. | 200 y persistencia coherente. |
| API-ROL-010 | Negativa | Actualizar ID inexistente. | 404. |
| API-ROL-011 | Seguridad | Ejecutar cada ruta con ADMIN. | 403. |
| API-ROL-012 | Seguridad | Ejecutar cada ruta con CLIENTE. | 403. |

### 6.4. Usuarios internos — policy SUPERADMINISTRADOR

Rutas: listar, buscar, obtener por ID/correo, crear, actualizar, cambiar contraseña y eliminar lógicamente.

| ID | Tipo | Caso | Resultado esperado |
|---|---|---|---|
| API-USR-001 | Funcional | Listar y buscar por nombre/correo/rol/estado. | 200; filtros y paginación correctos. |
| API-USR-002 | Funcional | Consultar usuario por ID. | No se expone `contrasena_hash`. |
| API-USR-003 | Funcional | Consultar usuario por correo. | Correo e ID correctos. |
| API-USR-004 | Funcional | Crear ADMIN interno sin cliente. | 201; `id_cliente` nulo y contraseña almacenada como hash PBKDF2. |
| API-USR-005 | Funcional | Crear usuario CLIENTE con cliente existente. | 201 y asociación correcta. |
| API-USR-006 | Validación | Crear CLIENTE sin `id_cliente`. | Rechazo según regla Business. |
| API-USR-007 | Validación | Crear ADMIN con `id_cliente`. | Rechazo si contradice el modelo de usuarios internos. |
| API-USR-008 | Validación | Rol inexistente. | 400; no crea usuario. |
| API-USR-009 | Validación | Nombre vacío, correo inválido, estado distinto de ACT/INA. | 400 con errores. |
| API-USR-010 | Validación | Contraseña menor a 8 o mayor a 128. | 400. |
| API-USR-011 | Conflicto | Correo duplicado, incluyendo variante de mayúsculas/espacios. | No duplica el usuario. |
| API-USR-012 | Funcional | Actualizar nombre, correo, rol y estado. | 200; no modifica hash si no se solicitó. |
| API-USR-013 | Funcional | Cambiar contraseña. | 200; clave anterior deja de funcionar y nueva clave funciona. |
| API-USR-014 | Seguridad | Revisar base luego del cambio. | Nunca se persiste texto plano. |
| API-USR-015 | Funcional | Eliminación lógica. | 200; estado INA; login posterior rechazado. |
| API-USR-016 | Negativa | Operar sobre ID inexistente. | 404. |
| API-USR-017 | Seguridad | ADMIN intenta cualquier gestión de usuarios. | 403. |
| API-USR-018 | Seguridad | CLIENTE intenta cualquier gestión de usuarios. | 403. |

### 6.5. Categorías de materiales

Policy SUPERADMINISTRADOR. Probar las seis operaciones publicadas.

| ID | Tipo | Caso | Resultado esperado |
|---|---|---|---|
| API-CAT-001 | Funcional | Crear `CP-API-TELAS`, ACT. | 201 con ID. |
| API-CAT-002 | Funcional | Listar y consultar por ID/nombre. | 200; datos coincidentes. |
| API-CAT-003 | Validación | Nombre vacío y nombre de 101 caracteres. | 400. |
| API-CAT-004 | Validación | Estado diferente de ACT/INA. | 400. |
| API-CAT-005 | Conflicto | Nombre duplicado si la BD lo restringe. | Rechazo sin duplicado. |
| API-CAT-006 | Funcional | Actualizar nombre/estado. | 200 y persistencia. |
| API-CAT-007 | Negativa | Obtener/actualizar/eliminar ID inexistente. | 404. |
| API-CAT-008 | Funcional | Eliminar lógicamente categoría sin dependencias. | 200; estado INA/no disponible para operaciones activas. |
| API-CAT-009 | Integración | Intentar baja con materiales dependientes. | Respuesta controlada; no romper integridad. |
| API-CAT-010 | Seguridad | ADMIN y CLIENTE ejecutan rutas. | 403. |

### 6.6. Líneas

Policy SUPERADMINISTRADOR. Rutas: listar, buscar, ID, nombre, crear, actualizar y eliminar.

Casos `API-LIN-001` a `API-LIN-012`: creación ACT; listado; consulta por ID; consulta por nombre; búsqueda paginada; nombre vacío; nombre de 101 caracteres; estado inválido; duplicado; actualización; baja lógica y dependencia con productos; 403 con ADMIN/CLIENTE. La línea `CP-API-SABANAS` debe conservarse para probar la cantidad mínima de personalización.

### 6.7. Métodos de pago

Policy SUPERADMINISTRADOR. No existe eliminación.

Casos `API-MET-001` a `API-MET-010`: crear método válido; listar; obtener por ID; obtener por nombre; obtener por código SRI; actualizar; nombre vacío/101 caracteres; código vacío/mayor a 500; ID inexistente; confirmar que Swagger no ofrece DELETE y que ADMIN/CLIENTE reciben 403. Conservar un método ACT para pedidos.

### 6.8. Clientes administrativos

Policy ADMINISTRADOR, también accesible a SUPERADMINISTRADOR.

| ID | Tipo | Caso | Resultado esperado |
|---|---|---|---|
| API-CLI-001 | Funcional | Crear cliente FIS válido. | 201. |
| API-CLI-002 | Funcional | Listar, buscar, consultar por ID y correo. | 200 y datos coherentes. |
| API-CLI-003 | Validación | Nombre vacío/101, apellidos 101. | 400. |
| API-CLI-004 | Validación | Correo vacío, inválido o mayor a 100. | 400. |
| API-CLI-005 | Validación | Teléfono no numérico o longitud distinta de 10. | 400. |
| API-CLI-006 | Validación | Dirección vacía o mayor a 255. | 400. |
| API-CLI-007 | Validación | Origen distinto de MKT/FIS y estado distinto de ACT/INA. | 400. |
| API-CLI-008 | Conflicto | Crear correo ya registrado en CLIENTE o USUARIO. | Rechazo por unicidad global; sin filas parciales. |
| API-CLI-009 | Funcional | Actualizar todos los campos editables. | 200. |
| API-CLI-010 | Negativa | ID/correo inexistente. | 404. |
| API-CLI-011 | Funcional | Eliminar lógicamente. | Estado INA, sin borrado físico. |
| API-CLI-012 | Integración | Baja de cliente con usuario/pedidos relacionados. | Integridad conservada y comportamiento documentado. |
| API-CLI-013 | Seguridad | CLIENTE intenta rutas internas. | 403. |
| API-CLI-014 | Paginación | Combinar filtros y páginas. | Conteos y orden coherentes. |
| API-CLI-015 | Seguridad | Confirmar que respuestas no exponen credenciales del usuario asociado. | Sin hash ni contraseña. |

### 6.9. Proveedores

Policy ADMINISTRADOR.

Casos `API-PRV-001` a `API-PRV-016`: crear proveedor con `codigo_proveedor` único; listar; consultar por ID; consultar por código; consultar por correo; buscar por código/nombre/correo/estado; código vacío y de 201 caracteres; teléfono inválido; correo inválido; dirección vacía/256; estado inválido; código duplicado con 409; correo duplicado; actualización; ID inexistente; eliminación lógica; 403 con CLIENTE. Confirmar que el código se devuelve junto con el proveedor y puede utilizarse luego para localizar materiales.

### 6.10. Materiales y búsqueda por proveedor

Policy ADMINISTRADOR.

| ID | Tipo | Caso | Resultado esperado |
|---|---|---|---|
| API-MAT-001 | Funcional | Crear material con categoría válida, unidad y stock `10.125`. | 201; conserva tres decimales. |
| API-MAT-002 | Funcional | Listar y consultar por ID/nombre. | 200. |
| API-MAT-003 | Funcional | Buscar por categoría, nombre, estado y rango de stock. | Resultados correctos. |
| API-MAT-004 | Funcional | Buscar por `codigo_proveedor`. | Devuelve materiales asociados al proveedor correcto. |
| API-MAT-005 | Negativa | Código de proveedor inexistente. | 200 con página vacía, no 500. |
| API-MAT-006 | Validación | Categoría cero o inexistente. | 400 o rechazo de relación; no crea. |
| API-MAT-007 | Validación | Nombre/unidad vacíos o sobre longitud. | 400. |
| API-MAT-008 | Validación | Stock negativo. | 400. |
| API-MAT-009 | Validación | Stock con cuatro decimales. | 400. |
| API-MAT-010 | Validación | Rango con mínimo mayor al máximo. | 400. |
| API-MAT-011 | Funcional | Actualizar metadatos sin alterar stock directamente. | 200; stock se conserva según contrato. |
| API-MAT-012 | Funcional | Baja lógica sin dependencias. | 200. |
| API-MAT-013 | Integración | Baja con receta/movimientos/proveedor asociado. | Integridad conservada. |
| API-MAT-014 | Negativa | Operaciones con ID inexistente. | 404. |
| API-MAT-015 | Seguridad | CLIENTE accede a rutas. | 403. |

### 6.11. Relación proveedor-material

Policy ADMINISTRADOR. No existe DELETE.

Casos `API-PM-001` a `API-PM-012`: crear relación NAC válida; crear IMP válida; consultar clave compuesta; listar por proveedor; listar por material; actualizar precio/cantidad mínima/días; precio negativo o con tres decimales; cantidad mínima cero; días negativos; origen distinto NAC/IMP; duplicar la clave compuesta; usar proveedor/material inexistente; confirmar ausencia de DELETE y 403 con CLIENTE.

### 6.12. Productos

Policy ADMINISTRADOR para administración.

| ID | Tipo | Caso | Resultado esperado |
|---|---|---|---|
| API-PROD-001 | Funcional | Crear producto ACT con stock 0 y colores `['AZUL','BLANCO']`. | 201; precio base calculado. |
| API-PROD-002 | Funcional | Listar, consultar por ID/nombre y buscar con filtros. | 200. |
| API-PROD-003 | Validación | Crear con stock inicial distinto de cero. | 400; indicar uso de fabricación. |
| API-PROD-004 | Validación | Línea inválida/inexistente. | Rechazo sin creación. |
| API-PROD-005 | Validación | Nombre vacío/101, peso 0/negativo y descripción vacía/501. | 400. |
| API-PROD-006 | Validación | `colores` no es arreglo. | 400. |
| API-PROD-007 | Validación | Colores vacíos o duplicados sin distinguir mayúsculas. | 400. |
| API-PROD-008 | Validación | Costos negativos. | 400. |
| API-PROD-009 | Validación | Margen menor a 0, igual a 100 o mayor. | 400. |
| API-PROD-010 | Validación | Gastos fijos/operativos fuera de 0..100. | 400. |
| API-PROD-011 | Validación | Indicadores personalizable/descuento distintos de S/N. | 400. |
| API-PROD-012 | Regla | Intentar crear producto con `tiene_descuentos='S'` sin reglas. | Rechazo o normalización a N según Business; documentar. |
| API-PROD-013 | Funcional | Actualizar campos permitidos. | 200; stock no se altera indebidamente. |
| API-PROD-014 | Regla | Intentar cambiar `tiene_descuentos` manualmente. | Rechazo; solo DescuentoService controla coherencia. |
| API-PROD-015 | Negativa | ID/nombre inexistente. | 404. |
| API-PROD-016 | Funcional | Eliminación lógica. | Estado INA; no aparece en Marketplace. |
| API-PROD-017 | Integración | Producto ACT con stock 0. | Se muestra como agotado en Marketplace. |
| API-PROD-018 | Seguridad | CLIENTE intenta administración. | 403. |

### 6.13. Receta producto-material

Casos `API-REC-001` a `API-REC-012`: crear receta con cantidad decimal `1.125`; consultar por clave compuesta; listar receta; actualizar a otro valor de tres decimales; cantidad 0/negativa; cuatro decimales; indicador distinto S/N; producto inexistente; material inexistente; relación duplicada; confirmar ausencia de DELETE; 403 con CLIENTE.

### 6.14. Fabricación e inventario

| ID | Tipo | Caso | Resultado esperado |
|---|---|---|---|
| API-FAB-001 | Transaccional | Fabricar cantidad positiva de producto existente con receta. | Suma stock de producto, descuenta materiales y crea movimientos. |
| API-FAB-002 | Integración | Receta `1.125` por 2 unidades. | Descuenta exactamente `2.250`, sin pérdida decimal. |
| API-FAB-003 | Transaccional | Material insuficiente. | 400; no cambia producto, material ni movimientos. |
| API-FAB-004 | Validación | Cantidad fabricada 0 o negativa. | 400. |
| API-FAB-005 | Validación | Producto existente sin receta. | 400. |
| API-FAB-006 | Validación | Enviar simultáneamente `id_producto` y `producto_nuevo`. | 400. |
| API-FAB-007 | Validación | No enviar ninguno de los dos. | 400. |
| API-FAB-008 | Transaccional | Fabricar producto nuevo con receta válida. | Producto, receta, stocks y movimientos se confirman juntos. |
| API-FAB-009 | Transaccional | Fallar receta del producto nuevo. | No queda producto ni receta parcial. |
| API-FAB-010 | Concurrencia | Dos fabricaciones simultáneas consumen el último material. | No queda stock negativo; solo operaciones válidas se confirman. |

### 6.15. Movimientos de materiales

Rutas: obtener por ID/material, buscar y crear.

Casos `API-MM-001` a `API-MM-014`: ING decimal; EGR decimal; AJP; AJN; consulta por ID; lista por material; filtros por tipo/fecha/material y paginación; tipo inválido; cantidad cero/negativa; más de tres decimales; motivo vacío/101; egreso superior al stock; material inexistente; pago/método cuando corresponda; confirmar que un fallo no crea movimiento ni deja stock negativo.

### 6.16. Movimientos de productos

Casos `API-MP-001` a `API-MP-013`: ING, EGR, AJP, AJN; consulta por ID/producto; filtros; tipo inválido; cantidad cero/negativa; motivo vacío/101; egreso superior al stock; producto inexistente; efecto exacto en stock; no permitir stock negativo; 403 con CLIENTE.

### 6.17. Personalizaciones JSONB

Policy SUPERADMINISTRADOR; no existe DELETE.

| ID | Tipo | Caso | Resultado esperado |
|---|---|---|---|
| API-PER-001 | Funcional | Crear LISTA con `version:1`, `requerido` y opciones código/nombre. | 201; JSONB preservado. |
| API-PER-002 | Funcional | Crear COLOR con opciones y costo adicional. | 201. |
| API-PER-003 | Funcional | Crear MATERIAL con IDs permitidos existentes. | 201. |
| API-PER-004 | Funcional | Crear TEXTO con longitudes y patrón. | 201. |
| API-PER-005 | Funcional | Crear MEDIDA con mínimo, máximo y unidad. | 201. |
| API-PER-006 | Validación | Tipo fuera de LISTA/MATERIAL/TEXTO/MEDIDA/COLOR. | 400. |
| API-PER-007 | Validación | `valores_json` no objeto, sin versión, versión distinta de 1 o requerido no booleano. | 400. |
| API-PER-008 | Validación | LISTA/COLOR sin opciones o con códigos repetidos. | 400. |
| API-PER-009 | Validación | MATERIAL con ID inválido/repetido/inexistente. | 400. |
| API-PER-010 | Validación | TEXTO con mínimos/máximos inconsistentes. | 400. |
| API-PER-011 | Validación | MEDIDA con rango invertido o unidad vacía. | 400. |
| API-PER-012 | Validación | Costos negativos o más de dos decimales. | 400. |
| API-PER-013 | Funcional | Consultar por ID y producto. | Tipo sin espacios de relleno y JSON correcto. |
| API-PER-014 | Funcional | Actualizar configuración. | 200. |
| API-PER-015 | Negativa | ID/producto inexistente. | 404 o lista vacía según ruta. |
| API-PER-016 | Seguridad | ADMIN/CLIENTE intentan modificar. | 403. |
| API-PER-017 | Contrato | Confirmar ausencia de DELETE. | Operación no publicada/405. |

### 6.18. Imágenes

Policy ADMINISTRADOR; no existe eliminación.

Casos `API-IMG-001` a `API-IMG-012`: crear imagen HTTP; crear HTTPS; consultar por ID; listar por producto en orden; actualizar URL/principal/orden; URL relativa; esquema no HTTP; URL de más de 500; indicador distinto S/N; orden negativo; producto inexistente; verificar coherencia de una sola imagen principal según regla actual; confirmar ausencia de DELETE y 403 con CLIENTE.

### 6.19. Descuentos por tramos

| ID | Tipo | Caso | Resultado esperado |
|---|---|---|---|
| API-DES-001 | Funcional | Crear tramo mínimo 3, 10 %. | 201 y producto pasa a `tiene_descuentos='S'`. |
| API-DES-002 | Funcional | Crear tramo mínimo 5, 20 %. | 201. |
| API-DES-003 | Funcional | Listar, ID, por producto y buscar. | 200. |
| API-DES-004 | Regla | Calcular cantidad 2, subtotal 100. | Sin descuento; total 100. |
| API-DES-005 | Regla | Calcular cantidad 3. | Aplica 10 %. |
| API-DES-006 | Regla | Calcular cantidad 6. | Aplica únicamente 20 %, no 30 %. |
| API-DES-007 | Regla | Comprobar redondeo monetario a dos decimales. | Resultado consistente. |
| API-DES-008 | Conflicto | Duplicar producto + cantidad mínima. | 409; no duplica tramo. |
| API-DES-009 | Validación | Cantidad mínima 0/negativa. | 400. |
| API-DES-010 | Validación | Porcentaje 0, negativo, >100 o más de dos decimales. | 400. |
| API-DES-011 | Validación | Producto inexistente/INA. | Rechazo controlado. |
| API-DES-012 | Funcional | Actualizar tramo. | 200 y cálculo usa nuevo valor. |
| API-DES-013 | Funcional | Inactivar un tramo. | Ya no se aplica. |
| API-DES-014 | Coherencia | Inactivar último tramo activo. | Producto vuelve a `tiene_descuentos='N'`. |
| API-DES-015 | Negativa | ID inexistente. | 404. |
| API-DES-016 | Seguridad | CLIENTE intenta administración. | 403. |

### 6.20. Marketplace público

| ID | Tipo | Caso | Resultado esperado |
|---|---|---|---|
| API-MKT-001 | Funcional | Buscar sin filtros ni token. | Solo productos ACT; paginación válida. |
| API-MKT-002 | Funcional | Producto ACT con stock 0. | Se incluye como agotado. |
| API-MKT-003 | Funcional | Producto INA. | No se incluye. |
| API-MKT-004 | Funcional | Filtros por línea, nombre, personalizable, descuento y rango de precio. | Coincidencias correctas. |
| API-MKT-005 | Funcional | `con_stock=true`. | Excluye agotados. |
| API-MKT-006 | Funcional | Obtener detalle por ID ACT. | No expone costos internos ni receta. |
| API-MKT-007 | Negativa | ID inexistente o producto INA. | 404. |
| API-MKT-008 | Validación | Indicadores distintos de S/N. | 400. |
| API-MKT-009 | Validación | Precio mínimo negativo o mayor al máximo. | 400. |
| API-MKT-010 | Paginación | Límites y página vacía. | Contrato coherente. |

### 6.21. Perfil del cliente

Casos `API-PERFIL-001` a `API-PERFIL-009`: consultar perfil propio; actualizar perfil propio; intentar enviar otro `id_cliente`; confirmar que se ignora/sobrescribe con el claim; Cliente A no puede consultar/modificar Cliente B; correo duplicado; campos inválidos; ADMIN/SUPERADMIN sin cliente recibe 403; token CLIENTE sin claim válido se rechaza; respuesta no expone usuario/hash.

### 6.22. Favoritos

| ID | Tipo | Caso | Resultado esperado |
|---|---|---|---|
| API-FAV-001 | Funcional | Agregar producto ACT con stock. | 201. |
| API-FAV-002 | Regla | Agregar producto ACT agotado. | 201; stock no es requisito. |
| API-FAV-003 | Funcional | Listar/buscar/consultar favorito propio. | 200. |
| API-FAV-004 | Conflicto | Agregar dos veces mismo producto. | Rechazo; una sola relación. |
| API-FAV-005 | Negativa | Producto inexistente o INA. | Respuesta conforme a regla vigente; documentar. |
| API-FAV-006 | Seguridad | Body contiene `id_cliente` de Cliente B. | API usa Cliente A del JWT. |
| API-FAV-007 | Seguridad | Cliente A intenta consultar favorito exclusivo de B. | 404/403 sin fuga de información. |
| API-FAV-008 | Funcional | Quitar favorito. | 200 y eliminación física de la relación. |
| API-FAV-009 | Negativa | Quitar nuevamente o ID inexistente. | 404. |
| API-FAV-010 | Seguridad | ADMIN intenta rutas del cliente. | 403. |
| API-FAV-011 | Paginación | Filtros y límites. | Metadatos correctos. |

### 6.23. Pedidos y pago simulado

Preparar un producto normal con stock, una sábana personalizable, descuentos activos, método de pago y dos clientes.

| ID | Tipo | Caso | Resultado esperado |
|---|---|---|---|
| API-PED-001 | Funcional | ADMIN crea pedido PEN válido. | 201; cabecera, detalles, subtotal, IVA 0 y total coherentes. |
| API-PED-002 | Funcional | CLIENTE crea pedido propio. | El `id_cliente` proviene del JWT. |
| API-PED-003 | Seguridad | Cliente A envía ID de Cliente B. | Pedido pertenece a A. |
| API-PED-004 | Funcional | Crear con `entrega_fisica='S'`. | Persiste S: retiro en tienda. |
| API-PED-005 | Funcional | Crear con `entrega_fisica='N'`. | Persiste N: entrega coordinada. |
| API-PED-006 | Validación | Entrega física fuera de S/N. | 400. |
| API-PED-007 | Validación | Cliente/método inexistente o inactivo. | 400; no crea. |
| API-PED-008 | Validación | Pedido sin detalles. | 400. |
| API-PED-009 | Validación | Cantidad 0/negativa. | 400. |
| API-PED-010 | Validación | Producto repetido en detalles. | 400. |
| API-PED-011 | Validación | Producto inexistente o INA. | 400. |
| API-PED-012 | Regla | Stock insuficiente al crear. | 400; no crea pedido. |
| API-PED-013 | Regla | Producto no personalizable recibe selección. | 400. |
| API-PED-014 | Regla | Falta personalización requerida. | 400. |
| API-PED-015 | Regla | Opción no pertenece al producto. | 400. |
| API-PED-016 | Regla | Opción repetida en un detalle. | 400. |
| API-PED-017 | Regla | LISTA/COLOR con código no permitido. | 400. |
| API-PED-018 | Regla | TEXTO fuera de longitud o patrón. | 400. |
| API-PED-019 | Regla | MEDIDA fuera de rango. | 400. |
| API-PED-020 | Regla | MATERIAL no permitido. | 400. |
| API-PED-021 | Regla | Sábana personalizada cantidad 2. | 400; mínimo 3. |
| API-PED-022 | Regla | Sábana personalizada cantidad 3. | 201. |
| API-PED-023 | Regla | Pedido alcanza tramo superior de descuento. | Guarda solo el mayor tramo aplicable y el histórico del porcentaje. |
| API-PED-024 | Regla | Personalización con costo adicional. | Precio unitario y subtotal incluyen el costo correcto. |
| API-PED-025 | Contrato | Revisar `personalizacion_selec`. | JSONB histórico versión 1; no cambia si luego se edita configuración. |
| API-PED-026 | Funcional | Listar, buscar y consultar pedido administrativo. | 200; filtros por cliente/estado/fecha/línea/método. |
| API-PED-027 | Seguridad | Cliente A lista/consulta pedidos. | Solo pedidos propios. |
| API-PED-028 | Seguridad | Cliente A solicita ID de pedido de B. | 403/404 sin devolver datos. |
| API-PED-029 | Funcional | Actualizar pedido PEN propio. | 200 y recálculo completo. |
| API-PED-030 | Regla | Actualizar pedido REA. | 400; solo PEN editable. |
| API-PED-031 | Pago | Pagar pedido PEN con stock suficiente. | 200; estado REA, fecha UTC, descuenta stock y crea egresos. |
| API-PED-032 | Pago | Confirmar monto/IVA al pagar. | IVA permanece 0; total no cambia. |
| API-PED-033 | Pago | Crear pedido, agotar stock y luego pagar. | 400; pedido continúa PEN, sin egresos parciales. |
| API-PED-034 | Pago | Pagar dos veces secuencialmente. | Primera 200; segunda 400. |
| API-PED-035 | Concurrencia | Enviar dos pagos simultáneos al mismo pedido. | Solo uno confirma; stock se descuenta una vez. |
| API-PED-036 | Transaccional | Forzar fallo durante creación de movimientos de pago. | Rollback de estado, fecha, stock y movimientos. |
| API-PED-037 | Seguridad | Cliente intenta pagar pedido de otro cliente. | 403/404. |
| API-PED-038 | Seguridad | ADMIN usa endpoint interno de pago. | Permitido. |
| API-PED-039 | Negativa | ID de pedido inexistente. | 404. |
| API-PED-040 | Alcance | Revisar Swagger. | No hay cancelar, envíos, pago real, DeUna, tarjetas ni facturación. |

## 7. Pruebas de integridad directamente en PostgreSQL

Estas verificaciones complementan Swagger. Deben ejecutarse con `SELECT`, nunca corrigiendo manualmente los resultados antes de documentarlos.

1. Un correo no debe repetirse entre usuarios/clientes según la regla global implementada.
2. Usuarios internos deben tener `id_cliente IS NULL`.
3. Usuarios CLIENTE deben apuntar a su cliente correcto.
4. Contraseñas deben comenzar con `PBKDF2-SHA256$210000$` y nunca coincidir con el texto enviado.
5. Bajas lógicas deben conservar la fila con estado INA.
6. Quitar un favorito debe eliminar físicamente solo la relación correspondiente.
7. No debe existir stock negativo.
8. Consumos de material deben conservar hasta tres decimales.
9. Una fabricación fallida no debe crear movimientos ni modificar existencias.
10. Un pago exitoso debe cambiar PEN a REA, registrar UTC y producir un egreso por producto.
11. Un pago fallido o concurrente no debe descontar dos veces.
12. `iva` debe permanecer en 0 aunque el campo siga presente.
13. `tiene_descuentos` debe ser S solo cuando exista al menos un tramo activo.
14. El histórico JSONB de un pedido debe conservarse aunque cambie la personalización original.

## 8. Ejecución y documentación

Para cada caso:

1. verificar precondiciones;
2. copiar el request exacto en `Datos de Entrada`;
3. ejecutar una sola vez, salvo casos de repetición o concurrencia;
4. copiar código y body reales en `Resultado Obtenido`;
5. consultar la base cuando el caso implique persistencia;
6. marcar solo una de las columnas `Aprobó` o `No Aprobó`;
7. registrar el `traceId` en errores;
8. registrar IDs creados para los casos dependientes;
9. si falla, no modificar el resultado esperado para hacer coincidir la implementación;
10. crear una incidencia indicando ID del caso, endpoint, request, response, evidencia y severidad.

### Severidad sugerida

| Severidad | Criterio |
|---|---|
| Crítica | Fuga de credenciales, acceso entre clientes, bypass de roles, pérdida de datos o doble cobro/descuento de stock. |
| Alta | Regla central incorrecta, transacción parcial, stock negativo o endpoint principal inutilizable. |
| Media | Validación incorrecta, código HTTP erróneo, filtro o paginación defectuosos. |
| Baja | Mensaje, orden, documentación o formato secundario. |

## 9. Limpieza

La limpieza se realiza después de conservar la evidencia:

- eliminar físicamente únicamente favoritos y relaciones de prueba cuando esté permitido;
- usar endpoints de baja lógica para entidades administrables;
- no borrar pedidos pagados ni movimientos para ocultar resultados;
- si se requiere reiniciar la campaña, borrar mediante un script aprobado y en orden de claves foráneas solamente los registros identificados con `CP-API-` o `@pruebas.retazomarket.local`;
- no borrar los usuarios reales Dana, Martín o Jaqueline;
- no reiniciar secuencias como parte de la prueba.

## 10. Criterios de cierre de la campaña

La API se considera aceptada para el alcance actual cuando:

- el 100 % de los casos críticos y de seguridad aprueba;
- el 100 % de los casos de atomicidad, fabricación y pago aprueba;
- no existen defectos críticos o altos abiertos;
- todos los endpoints de Swagger tienen al menos un caso exitoso y uno de acceso/autorización aplicable;
- todos los DTOs mutables tienen validaciones de requeridos, límites y valores inválidos;
- los resultados observados en PostgreSQL coinciden con las respuestas;
- los casos no aprobados tienen incidencia y evidencia;
- se confirma nuevamente que no se implementaron funcionalidades fuera del alcance.

## 11. Instrucciones para generar posteriormente el Excel

El modelo que genere el Excel deberá:

1. copiar el encabezado visual y las diez columnas del archivo de referencia;
2. crear una fila individual por cada ID explícito;
3. expandir también cada rango indicado, por ejemplo `API-LIN-001` a `API-LIN-012`, en doce filas individuales usando los escenarios enumerados;
4. no combinar varios resultados HTTP en una misma fila, excepto variantes que formen parte inseparable de una prueba de concurrencia;
5. incluir método, ruta, rol, precondiciones, parámetros y JSON concretos;
6. usar IDs dinámicos obtenidos de casos anteriores mediante marcadores `<ID_...>`;
7. dejar vacías las cuatro columnas de ejecución;
8. aplicar filtros, ajuste de texto, encabezado congelado y colores para aprobado/no aprobado;
9. incluir una hoja opcional `Datos de Ejecución` para tokens enmascarados e IDs, nunca contraseñas reales;
10. incluir una hoja opcional `Resumen` con totales por módulo, tipo, aprobación y severidad.
