# Decisiones de diseño compartidas

## Propósito

Este documento registra decisiones funcionales y de diseño que deben conocer los equipos de backend y frontend. Su objetivo es mantener una interpretación uniforme del comportamiento del sistema.

## 1. Creación de productos y consumo de materiales

### Decisión

Crear un producto en el catálogo no consume materiales ni incrementa el stock de productos terminados.

La creación del producto únicamente define:

- Su información comercial.
- La línea a la que pertenece.
- Si admite personalizaciones.
- Su precio y componentes de costo.
- Los materiales que necesita.
- La cantidad requerida de cada material por unidad mediante `PRO_X_MAT`.

### Motivo

`PRO_X_MAT` representa una receta de fabricación. Por ejemplo, puede indicar que una unidad de producto requiere dos metros de tela y una unidad de relleno. Al crear la ficha todavía no existe una cantidad fabricada sobre la cual calcular el consumo.

### Flujo futuro de producción

La fabricación se manejará como una operación independiente:

1. El administrador selecciona el producto.
2. Indica la cantidad de unidades fabricadas.
3. El sistema calcula por cada material:

```text
cantidad_consumida = cantidad_req × cantidad_fabricada
```

4. Valida que exista material suficiente.
5. Descuenta los materiales.
6. Registra movimientos de egreso de materiales.
7. Incrementa el stock del producto terminado.
8. Registra un movimiento de ingreso del producto.
9. Confirma todos los cambios en una sola transacción.

Si cualquier material es insuficiente, no se realiza ningún cambio parcial.

### Consideración para frontend

El formulario de creación de productos define la receta, pero no debe presentar esa acción como una fabricación ni asumir que modificará existencias. La futura pantalla de producción solicitará una cantidad concreta de unidades.

## 2. Correo de clientes y usuarios

### Decisión

- El correo es único dentro de `CLIENTE`.
- El correo es único dentro de `USUARIO`.
- Un usuario del Marketplace puede compartir correo con su propio registro de cliente asociado.
- Ningún cliente o usuario diferente puede reutilizar ese correo.

### Autorregistro

El autorregistro crea o vincula coherentemente:

- Un cliente con origen `MKT`.
- Un usuario con rol `Cliente`.
- La relación entre ambos.

El correo de los dos registros representa a la misma persona.

## 3. Administrador y Super Administrador

### Super Administrador

Tiene acceso a:

- Todos los módulos administrativos.
- Gestión de usuarios internos.
- Gestión y asignación de roles.
- Configuraciones técnicas expuestas por la aplicación.
- Funciones de mantenimiento que se incorporen posteriormente.

### Administrador

Tiene acceso a los módulos administrativos comerciales, entre ellos:

- Clientes.
- Proveedores.
- Categorías, líneas y métodos de pago.
- Materiales y productos.
- Inventario y movimientos.
- Pedidos.
- Consultas y paneles administrativos.

No tiene acceso a:

- Gestión de roles.
- Creación o administración de Super Administradores.
- Configuraciones técnicas críticas.
- Acceso directo a la base de datos.

### Cliente

Tiene acceso únicamente al Marketplace y a sus propios recursos:

- Catálogo público.
- Perfil propio.
- Favoritos propios.
- Creación y pago de pedidos propios.
- Historial de pedidos propio.

### Consideración para frontend

Ocultar una opción visual no reemplaza la autorización del backend. El frontend adapta menús y rutas según el rol, pero cada endpoint también debe validar permisos.

## 4. Entrega física

### Valores

- `S`: retiro físico en la tienda.
- `N`: entrega coordinada externamente.

### Alcance actual

- El retiro en tienda no crea un registro de envío.
- La entrega externa se coordina fuera del sistema mientras el módulo de envíos permanezca fuera del alcance.
- Seleccionar `N` tampoco crea por ahora un registro en `ENVIO`.
- El pedido conserva la elección para información administrativa.

### Consideración para frontend

La interfaz debe mostrar opciones comprensibles:

- `Retiro en tienda` → envía `S`.
- `Entrega coordinada` → envía `N`.

Para `N` debe indicarse que la coordinación y cualquier costo adicional se confirmarán por un medio externo mientras no exista el módulo de envíos.

## 5. Producto inactivo y producto agotado

Son estados funcionalmente diferentes.

### Producto inactivo

- Tiene `PROD_ESTADO = 'INA'`.
- Fue retirado administrativamente del catálogo público.
- Puede permanecer en favoritos e historiales.
- No debe mostrarse como disponible para una nueva compra.
- En favoritos debe mostrarse como `Producto no disponible`.

### Producto agotado

- Tiene `PROD_ESTADO = 'ACT'`.
- Tiene `stock_actual = 0`.
- Continúa formando parte del catálogo.
- En el catálogo y favoritos debe mostrarse como `Agotado`.
- No puede añadirse a un pedido nuevo.

### Producto disponible

Solo puede comprarse cuando:

```text
PROD_ESTADO = 'ACT' AND stock_actual > 0
```

### Prioridad visual sugerida

1. Si está inactivo: `Producto no disponible`.
2. Si está activo y sin stock: `Agotado`.
3. Si está activo y tiene stock: `Disponible`.

## 6. Vigencia

Estas decisiones se consideran confirmadas para la implementación. Cualquier modificación posterior deberá actualizar este documento y las reglas de Business relacionadas.
