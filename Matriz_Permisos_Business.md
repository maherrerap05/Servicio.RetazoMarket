# Matriz de permisos de Business

Esta matriz define autorización funcional sin depender de `HttpContext`, atributos HTTP ni códigos de respuesta. Api construirá `ActorContext` desde los claims del JWT y entregará el contexto a la validación de Business.

| Recurso u operación | SUPERADMINISTRADOR | ADMINISTRADOR | CLIENTE |
|---|---:|---:|---:|
| Roles | Sí | No | No |
| Usuarios internos | Sí | No | No |
| Categorías de materiales | Sí | No | No |
| Líneas | Sí | No | No |
| Métodos de pago | Sí | No | No |
| Configuración de personalizaciones | Sí | No | No |
| Clientes administrativos | Sí | Sí | Solo su perfil |
| Proveedores y relaciones con materiales | Sí | Sí | No |
| Materiales e inventario | Sí | Sí | No |
| Productos, recetas, imágenes y descuentos | Sí | Sí | Solo consulta pública |
| Pedidos administrativos | Sí | Sí | Solo los propios |
| Pago simulado administrativo | Sí | Sí | Solo pedido propio cuando Api lo habilite |
| Favoritos | No como operación del cliente | No como operación del cliente | Solo los propios |
| Catálogo Marketplace | Público | Público | Público |

## Reglas de aplicación

- `SUPERADMINISTRADOR` incluye las capacidades administrativas generales.
- `ADMINISTRADOR` no puede administrar roles, usuarios internos ni configuración técnica.
- `CLIENTE` requiere que `ActorContext.id_cliente` coincida con el propietario del recurso.
- Ocultar controles en el frontend no reemplaza la validación de Business.
- Api conserva la responsabilidad de `[Authorize]`, policies, lectura de claims y traducción a códigos HTTP.
- Los endpoints públicos de catálogo no requieren `ActorContext`.
