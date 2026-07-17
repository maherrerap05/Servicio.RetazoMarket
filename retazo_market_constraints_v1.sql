

/*==============================================================*/
/* El Retazo Market - Checks, Constraints y correcciones        */
/* Basado en: Especificacion de Requerimientos V1 (03-07-2026)  */
/* Fecha de creacion: 15/07/2026                                */
/*==============================================================*/


/*==============================================================*/
/* 1. PRODUCTOS                                                 */
/*==============================================================*/

-- RF-PRD-05: activar/desactivar sin eliminar
ALTER TABLE PRODUCTOS ADD CONSTRAINT CK_PRODUCTOS_ESTADO
    CHECK (PROD_ESTADO IN ('ACT','INA'));

-- Personalizacion es booleano (S/N) codificado en CHAR(1)
ALTER TABLE PRODUCTOS ADD CONSTRAINT CK_PRODUCTOS_PERSONALIZABLE
    CHECK (ES_PERSONALIZABLE IN ('S','N'));

-- Reglas de precios y costeo (seccion 7): ningun componente de costo negativo
ALTER TABLE PRODUCTOS ADD CONSTRAINT CK_PRODUCTOS_COSTOS_POSITIVOS
    CHECK (COSTO_MAT_PRIM >= 0 AND COSTO_MANO_OBRA >= 0 AND PRECIO_BASE >= 0 AND PROD_PESO > 0);

-- RF-INV-01/07: el stock nunca puede ser negativo
ALTER TABLE PRODUCTOS ADD CONSTRAINT CK_PRODUCTOS_STOCK
    CHECK (STOCK_ACTUAL >= 0 AND STOCK_DESCUENTO >= 0);

-- Regla de negocio (seccion 7): margen de ganancia fijo del sistema = 50%
ALTER TABLE PRODUCTOS ADD CONSTRAINT CK_PRODUCTOS_MARGEN
    CHECK (PORCENTAJE_MARGEN_GANANCIA = 50);


/*==============================================================*/
/* 2. LINEAS                                                    */
/*==============================================================*/

ALTER TABLE LINEAS ADD CONSTRAINT CK_LINEAS_ESTADO
    CHECK (LIN_ESTADO IN ('ACT','INA'));


/*==============================================================*/
/* 3. PERSONALIZACION                                           */
/*==============================================================*/

ALTER TABLE PERSONALIZACION ADD CONSTRAINT CK_PERSONALIZACION_COSTO
    CHECK (COSTO_ADICIONAL >= 0);

-- Confirmado: solo dos tipos de valor posibles
ALTER TABLE PERSONALIZACION ADD CONSTRAINT CK_PERSONALIZACION_TIPO_VALOR
    CHECK (TIPO_VALOR IN ('LISTA','MATERIAL'));


/*==============================================================*/
/* 4. MATERIALES / CAT_MAT                                      */
/*==============================================================*/

ALTER TABLE MATERIALES ADD CONSTRAINT CK_MATERIALES_STOCK
    CHECK (STOCK_ACTUAL >= 0);

ALTER TABLE MATERIALES ADD CONSTRAINT CK_MATERIALES_ESTADO
    CHECK (MAT_ESTADO IN ('ACT','INA'));

ALTER TABLE CAT_MAT ADD CONSTRAINT CK_CAT_MAT_ESTADO
    CHECK (CAT_ESTADO IN ('ACT','INA'));


/*==============================================================*/
/* 5. PROVEEDORES / MATERIALES ASOCIADOS                        */
/*==============================================================*/

ALTER TABLE PROVEEDORES ADD CONSTRAINT CK_PROVEEDORES_ESTADO
    CHECK (PROV_ESTADO IN ('ACT','INA'));

ALTER TABLE PROVEEDORES ADD CONSTRAINT CK_PROVEEDORES_TELEFONO
    CHECK (PROV_TELEFONO ~ '^[0-9]{10}$');

ALTER TABLE PROVEEDORES ADD CONSTRAINT CK_PROVEEDORES_CORREO
    CHECK (PROV_CORREO ~ '^[^@\s]+@[^@\s]+\.[^@\s]+$');

-- RF-PRV-02: origen nacional/importado
ALTER TABLE PRV_X_MAT ADD CONSTRAINT CK_PRV_X_MAT_ORIGEN
    CHECK (ORIGEN IN ('NAC','IMP'));

-- RF-PRV-03: condiciones de compra validas
ALTER TABLE PRV_X_MAT ADD CONSTRAINT CK_PRV_X_MAT_CONDICIONES
    CHECK (PRECIO_COMPRA >= 0 AND CANTIDAD_MIN > 0 AND DIAS_ENTREGA >= 0);

ALTER TABLE PRO_X_MAT ADD CONSTRAINT CK_PRO_X_MAT_CANTIDAD
    CHECK (CANTIDAD_REQ > 0);

ALTER TABLE PRO_X_MAT ADD CONSTRAINT CK_PRO_X_MAT_PERSONALIZABLE
    CHECK (ES_PERSONALIZABLE IN ('S','N'));


/*==============================================================*/
/* 6. CLIENTE / USUARIO / ROL                                   */
/*==============================================================*/

-- RF-CLI-05: origen del cliente
ALTER TABLE CLIENTE ADD CONSTRAINT CK_CLIENTE_ORIGEN
    CHECK (ORIGEN IN ('MKT','FIS'));

-- RF-CLI-02: el cliente puede desactivar su propia cuenta
ALTER TABLE CLIENTE ADD CONSTRAINT CK_CLIENTE_ESTADO
    CHECK (CLI_ESTADO IN ('ACT','INA'));

ALTER TABLE CLIENTE ADD CONSTRAINT CK_CLIENTE_TELEFONO
    CHECK (TELEFONO ~ '^[0-9]{10}$');

ALTER TABLE CLIENTE ADD CONSTRAINT CK_CLIENTE_CORREO
    CHECK (CORREO ~ '^[^@\s]+@[^@\s]+\.[^@\s]+$');

-- RF-CLI-02 / RF-USR-01: correo unico por cliente y por usuario
ALTER TABLE CLIENTE ADD CONSTRAINT UQ_CLIENTE_CORREO
    UNIQUE (CORREO);

ALTER TABLE USUARIO ADD CONSTRAINT UQ_USUARIO_CORREO
    UNIQUE (CORREO);

ALTER TABLE USUARIO ADD CONSTRAINT CK_USUARIO_ESTADO
    CHECK (USR_ESTADO IN ('ACT','INA'));

ALTER TABLE USUARIO ADD CONSTRAINT CK_USUARIO_CORREO
    CHECK (CORREO ~ '^[^@\s]+@[^@\s]+\.[^@\s]+$');

-- Seccion 3.3: solo existen 3 roles definidos formalmente
ALTER TABLE ROL ADD CONSTRAINT CK_ROL_NOMBRE
    CHECK (NOMBRE_ROL IN ('SUPERADMINISTRADOR','ADMINISTRADOR','CLIENTE'));


/*==============================================================*/
/* 7. PEDIDOS                                                   */
/*==============================================================*/

-- RF-PED-03/04: los montos del pedido no pueden ser negativos
ALTER TABLE PEDIDOS ADD CONSTRAINT CK_PEDIDOS_MONTOS
    CHECK (SUBTOTAL >= 0 AND IVA >= 0 AND TOTAL >= 0);

-- RF-PED-04: el total debe ser la suma exacta de subtotal + IVA
ALTER TABLE PEDIDOS ADD CONSTRAINT CK_PEDIDOS_TOTAL_CONSISTENTE
    CHECK (TOTAL = SUBTOTAL + IVA);

-- RF-PED-05: confirmado, solo dos estados por ahora
ALTER TABLE PEDIDOS ADD CONSTRAINT CK_PEDIDOS_ESTADO
    CHECK (ESTADO IN ('PEN','REA'));

-- Modalidad de entrega: retiro físico en tienda (S/N)
ALTER TABLE PEDIDOS ADD CONSTRAINT CK_PEDIDOS_ENTREGA_FISICA
    CHECK (ENTREGA_FISICA IN ('S','N'));

-- Correccion: FECHA_PAGO debe permitir NULL (pedido pendiente aun no tiene pago confirmado)
ALTER TABLE PEDIDOS ALTER COLUMN FECHA_PAGO DROP NOT NULL;


/*==============================================================*/
/* 8. PRO_X_PED                                                 */
/*==============================================================*/

ALTER TABLE PRO_X_PED ADD CONSTRAINT CK_PRO_X_PED_CANTIDAD
    CHECK (CANTIDAD > 0);

ALTER TABLE PRO_X_PED ADD CONSTRAINT CK_PRO_X_PED_MONTOS
    CHECK (PRECIO_UNITARIO >= 0 AND MONTO_DESCUENTO >= 0 AND SUBTOTAL_ITEM >= 0);

ALTER TABLE PRO_X_PED ADD CONSTRAINT CK_PRO_X_PED_DESCUENTO
    CHECK (PORCENTAJE_DESCUENTO >= 0 AND PORCENTAJE_DESCUENTO <= 100);

-- Consistencia del calculo de linea de pedido
ALTER TABLE PRO_X_PED ADD CONSTRAINT CK_PRO_X_PED_SUBTOTAL_CONSISTENTE
    CHECK (SUBTOTAL_ITEM = (PRECIO_UNITARIO * CANTIDAD) - MONTO_DESCUENTO);


/*==============================================================*/
/* 9. FACTURAS                                                  */
/*==============================================================*/

-- Glosario: la clave de acceso SRI tiene exactamente 49 digitos numericos
ALTER TABLE FACTURAS ADD CONSTRAINT CK_FACTURAS_CLAVE_ACCESO
    CHECK (CLAVE_ACCESO ~ '^[0-9]{49}$');

-- Seccion 6.2 / Glosario: ambiente de Pruebas o Produccion
ALTER TABLE FACTURAS ADD CONSTRAINT CK_FACTURAS_AMBIENTE
    CHECK (AMBIENTE IN ('PRU','PRO'));

-- La autorizacion no puede ocurrir antes de la emision
ALTER TABLE FACTURAS ADD CONSTRAINT CK_FACTURAS_FECHAS
    CHECK (FECHA_AUTORIZACION >= FECHA_EMISION);

-- NOTA: no se agrega CHECK para TIPO_IDENTIFICACION - dominio de valores
-- SRI aun pendiente de confirmar. Agregar en una siguiente iteracion.


/*==============================================================*/
/* 10. ENVIO                                                    */
/*==============================================================*/

ALTER TABLE ENVIO ADD CONSTRAINT CK_ENVIO_COSTO
    CHECK (COSTO >= 0);

ALTER TABLE ENVIO ADD CONSTRAINT CK_ENVIO_TELEFONO
    CHECK (DEST_TELEFONO ~ '^[0-9]{10}$');

-- Confirmado: En Proceso (PRO), Enviado (ENV), Realizado (REA)
ALTER TABLE ENVIO ADD CONSTRAINT CK_ENVIO_ESTADO
    CHECK (ESTADO IN ('PRO','ENV','REA'));


/*==============================================================*/
/* 11. AUDITORIA                                                */
/*==============================================================*/

-- RF-AUD-01/02/03: solo estas tres operaciones se auditan
ALTER TABLE AUDITORIA ADD CONSTRAINT CK_AUDITORIA_TIPO_OPERACION
    CHECK (TIPO_OPERACION IN ('INSERT','UPDATE','DELETE'));


/*==============================================================*/
/* 12. IMAGENES                                                 */
/*==============================================================*/

ALTER TABLE IMAGENES ADD CONSTRAINT CK_IMAGENES_PRINCIPAL
    CHECK (ES_PRINCIPAL IN ('S','N'));

ALTER TABLE IMAGENES ADD CONSTRAINT CK_IMAGENES_ORDEN
    CHECK (ORDEN >= 0);
