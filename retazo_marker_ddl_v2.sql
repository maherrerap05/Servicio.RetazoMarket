/*==============================================================*/
/* DBMS name:      PostgreSQL 8                                 */
/* Created on:     15/7/2026 10:07:03                           */
/*==============================================================*/

/* 
drop index AUDITORIA_PK;

drop table AUDITORIA;

drop index CAT_MAT_PK;

drop table CAT_MAT;

drop index CLIENTE_PK;

drop table CLIENTE;

drop index GENERA_FK;

drop index ENVIO_PK;

drop table ENVIO;

drop index ASOCIA_FK;

drop index FACTURAS_PK;

drop table FACTURAS;

drop index FAVORITOS_FK;

drop index FAVORITOS2_FK;

drop index FAVORITOS_PK;

drop table FAVORITOS;

drop index MUESTRA_FK;

drop index IMAGENES_PK;

drop table IMAGENES;

drop index LINEAS_PK;

drop table LINEAS;

drop index AGRUPA_FK;

drop index MATERIALES_PK;

drop table MATERIALES;

drop index METODO_PAGO_PK;

drop table METODO_PAGO;

drop index REGISTRA_FK;

drop index MOV_MATERIALES_PK;

drop table MOV_MATERIALES;

drop index PRESENTA_FK;

drop index MOV_PRODUCTOS_PK;

drop table MOV_PRODUCTOS;

drop index REALIZA_FK;

drop index CORRESPONDE_FK;

drop index PEDIDOS_PK;

drop table PEDIDOS;

drop index TIENE_FK;

drop index PERSONALIZACION_PK;

drop table PERSONALIZACION;

drop index PERTENECE_FK;

drop index PRODUCTOS_PK;

drop table PRODUCTOS;

drop index PROVEEDORES_PK;

drop table PROVEEDORES;

drop index PRO_X_MAT_FK;

drop index PRO_X_MAT2_FK;

drop index PRO_X_MAT_PK;

drop table PRO_X_MAT;

drop index PRO_X_PED_FK;

drop index PRO_X_PED2_FK;

drop index PRO_X_PED_PK;

drop table PRO_X_PED;

drop index PRV_X_MAT_FK;

drop index PRV_X_MAT2_FK;

drop index PRV_X_MAT_PK;

drop table PRV_X_MAT;

drop index ROL_PK;

drop table ROL;

drop index REPRESENTA_FK;

drop index TIENE_2_FK;

drop index USUARIO_PK;

drop table USUARIO;
*/

/*==============================================================*/
/* Table: AUDITORIA                                             */
/*==============================================================*/
create table AUDITORIA (
   ID_AUDITORIA         INT4                 not null,
   TABLA_AFECTADA       VARCHAR(100)         not null,
   ID_REGISTRO_AFECTADO INT4                 not null,
   TIPO_OPERACION       CHAR(6)              not null,
   FECHA_HORA           DATE                 not null,
   USUARIO_RESPONSABLE  VARCHAR(100)         not null,
   ROL_RESPONSABLE      VARCHAR(100)         not null,
   VALORES_ANTERIORES   VARCHAR(1)           not null,
   VALORES_NUEVOS       VARCHAR(1)           not null,
   constraint PK_AUDITORIA primary key (ID_AUDITORIA)
);

/*==============================================================*/
/* Index: AUDITORIA_PK                                          */
/*==============================================================*/
create unique index AUDITORIA_PK on AUDITORIA (
ID_AUDITORIA
);

/*==============================================================*/
/* Table: CAT_MAT                                               */
/*==============================================================*/
create table CAT_MAT (
   ID_CATEGORIA         INT4                 not null,
   CAT_NOMBRE           VARCHAR(100)         not null,
   CAT_ESTADO           CHAR(3)              not null,
   constraint PK_CAT_MAT primary key (ID_CATEGORIA)
);

/*==============================================================*/
/* Index: CAT_MAT_PK                                            */
/*==============================================================*/
create unique index CAT_MAT_PK on CAT_MAT (
ID_CATEGORIA
);

/*==============================================================*/
/* Table: CLIENTE                                               */
/*==============================================================*/
create table CLIENTE (
   ID_CLIENTE           INT4                 not null,
   NOMBRE               VARCHAR(100)         not null,
   APELLIDOS            VARCHAR(100)         null,
   CORREO               VARCHAR(100)         not null,
   TELEFONO             CHAR(10)             not null,
   DIRECCION            VARCHAR(255)         not null,
   ORIGEN               CHAR(3)              not null,
   CLI_ESTADO           CHAR(3)              not null,
   FECHA_REGISTRO       DATE                 not null,
   constraint PK_CLIENTE primary key (ID_CLIENTE)
);

/*==============================================================*/
/* Index: CLIENTE_PK                                            */
/*==============================================================*/
create unique index CLIENTE_PK on CLIENTE (
ID_CLIENTE
);

/*==============================================================*/
/* Table: ENVIO                                                 */
/*==============================================================*/
create table ENVIO (
   ID_ENVIO             INT4                 not null,
   ID_PEDIDO            INT4                 not null,
   NUMERO_GUIA          VARCHAR(100)         not null,
   DEST_NOMBRE          VARCHAR(100)         not null,
   DEST_TELEFONO        CHAR(10)             not null,
   DIRECCION            VARCHAR(255)         not null,
   PROVINCIA            VARCHAR(100)         not null,
   CIUDAD               VARCHAR(100)         not null,
   COSTO                DECIMAL(10,2)        not null,
   ESTADO               CHAR(3)              not null,
   FECHA_GEN            DATE                 not null,
   constraint PK_ENVIO primary key (ID_ENVIO)
);

/*==============================================================*/
/* Index: ENVIO_PK                                              */
/*==============================================================*/
create unique index ENVIO_PK on ENVIO (
ID_ENVIO
);

/*==============================================================*/
/* Index: GENERA_FK                                             */
/*==============================================================*/
create  index GENERA_FK on ENVIO (
ID_PEDIDO
);

/*==============================================================*/
/* Table: FACTURAS                                              */
/*==============================================================*/
create table FACTURAS (
   ID_FACTURA           INT4                 not null,
   ID_PEDIDO            INT4                 not null,
   CLAVE_ACCESO         VARCHAR(255)         not null,
   ESTABLECIMIENTO      VARCHAR(100)         not null,
   PUNTO_EMISION        VARCHAR(100)         not null,
   NUMERO_SECUENCIAL    VARCHAR(255)         not null,
   AMBIENTE             CHAR(3)              not null,
   FECHA_EMISION        DATE                 not null,
   FECHA_AUTORIZACION   DATE                 not null,
   MOTIVO_RECHAZO       VARCHAR(255)         null,
   TIPO_IDENTIFICACION  CHAR(3)              not null,
   IDENTIFICACION_TRIBUTARIA VARCHAR(100)         not null,
   RAZON_SOCIAL         VARCHAR(100)         not null,
   DIRECCION_FACTURACION VARCHAR(255)         not null,
   URL_XML              VARCHAR(500)         not null,
   URL_RIDE             VARCHAR(500)         not null,
   constraint PK_FACTURAS primary key (ID_FACTURA)
);

/*==============================================================*/
/* Index: FACTURAS_PK                                           */
/*==============================================================*/
create unique index FACTURAS_PK on FACTURAS (
ID_FACTURA
);

/*==============================================================*/
/* Index: ASOCIA_FK                                             */
/*==============================================================*/
create  index ASOCIA_FK on FACTURAS (
ID_PEDIDO
);

/*==============================================================*/
/* Table: FAVORITOS                                             */
/*==============================================================*/
create table FAVORITOS (
   ID_CLIENTE           INT4                 not null,
   ID_PRODUCTO          INT4                 not null,
   constraint PK_FAVORITOS primary key (ID_CLIENTE, ID_PRODUCTO)
);

/*==============================================================*/
/* Index: FAVORITOS_PK                                          */
/*==============================================================*/
create unique index FAVORITOS_PK on FAVORITOS (
ID_CLIENTE,
ID_PRODUCTO
);

/*==============================================================*/
/* Index: FAVORITOS2_FK                                         */
/*==============================================================*/
create  index FAVORITOS2_FK on FAVORITOS (
ID_CLIENTE
);

/*==============================================================*/
/* Index: FAVORITOS_FK                                          */
/*==============================================================*/
create  index FAVORITOS_FK on FAVORITOS (
ID_PRODUCTO
);

/*==============================================================*/
/* Table: IMAGENES                                              */
/*==============================================================*/
create table IMAGENES (
   ID_IMAGEN            INT4                 not null,
   ID_PRODUCTO          INT4                 not null,
   URL                  VARCHAR(500)         not null,
   ES_PRINCIPAL         CHAR(1)              not null,
   ORDEN                INT4                 not null,
   constraint PK_IMAGENES primary key (ID_IMAGEN)
);

/*==============================================================*/
/* Index: IMAGENES_PK                                           */
/*==============================================================*/
create unique index IMAGENES_PK on IMAGENES (
ID_IMAGEN
);

/*==============================================================*/
/* Index: MUESTRA_FK                                            */
/*==============================================================*/
create  index MUESTRA_FK on IMAGENES (
ID_PRODUCTO
);

/*==============================================================*/
/* Table: LINEAS                                                */
/*==============================================================*/
create table LINEAS (
   ID_LINEA             INT4                 not null,
   LIN_NOMBRE           VARCHAR(100)         null,
   LIN_ESTADO           CHAR(3)              null,
   constraint PK_LINEAS primary key (ID_LINEA)
);

/*==============================================================*/
/* Index: LINEAS_PK                                             */
/*==============================================================*/
create unique index LINEAS_PK on LINEAS (
ID_LINEA
);

/*==============================================================*/
/* Table: MATERIALES                                            */
/*==============================================================*/
create table MATERIALES (
   ID_MATERIAL          INT4                 not null,
   ID_CATEGORIA         INT4                 not null,
   MAT_NOMBRE           VARCHAR(100)         not null,
   UNIDAD_MEDIDA        VARCHAR(50)          not null,
   STOCK_ACTUAL         INT4                 not null,
   MAT_ESTADO           CHAR(3)              not null,
   constraint PK_MATERIALES primary key (ID_MATERIAL)
);

/*==============================================================*/
/* Index: MATERIALES_PK                                         */
/*==============================================================*/
create unique index MATERIALES_PK on MATERIALES (
ID_MATERIAL
);

/*==============================================================*/
/* Index: AGRUPA_FK                                             */
/*==============================================================*/
create  index AGRUPA_FK on MATERIALES (
ID_CATEGORIA
);

/*==============================================================*/
/* Table: METODO_PAGO                                           */
/*==============================================================*/
create table METODO_PAGO (
   ID_METODO            INT4                 not null,
   MET_NOMBRE           VARCHAR(100)         not null,
   CODIGO_SRI           VARCHAR(500)         not null,
   constraint PK_METODO_PAGO primary key (ID_METODO)
);

/*==============================================================*/
/* Index: METODO_PAGO_PK                                        */
/*==============================================================*/
create unique index METODO_PAGO_PK on METODO_PAGO (
ID_METODO
);

/*==============================================================*/
/* Table: MOV_MATERIALES                                        */
/*==============================================================*/
create table MOV_MATERIALES (
   ID_MOVIMIENTO        INT4                 not null,
   ID_MATERIAL          INT4                 not null,
   TIPO_MOVIMIENTO      CHAR(3)              not null,
   CANTIDAD             INT4                 not null,
   FECHA_MOV            DATE                 not null,
   MOTIVO_MOV           VARCHAR(100)         not null,
   METODO_PAGO          VARCHAR(50)          null,
   constraint PK_MOV_MATERIALES primary key (ID_MOVIMIENTO)
);

/*==============================================================*/
/* Index: MOV_MATERIALES_PK                                     */
/*==============================================================*/
create unique index MOV_MATERIALES_PK on MOV_MATERIALES (
ID_MOVIMIENTO
);

/*==============================================================*/
/* Index: REGISTRA_FK                                           */
/*==============================================================*/
create  index REGISTRA_FK on MOV_MATERIALES (
ID_MATERIAL
);

/*==============================================================*/
/* Table: MOV_PRODUCTOS                                         */
/*==============================================================*/
create table MOV_PRODUCTOS (
   ID_MOVIMIENTO2       INT4                 not null,
   ID_PRODUCTO          INT4                 not null,
   TIPO_MOVIMIENTO      CHAR(3)              not null,
   CANTIDAD             INT4                 not null,
   FECHA_MOV            DATE                 not null,
   MOTIVO_MOV           VARCHAR(100)         not null,
   constraint PK_MOV_PRODUCTOS primary key (ID_MOVIMIENTO2)
);

/*==============================================================*/
/* Index: MOV_PRODUCTOS_PK                                      */
/*==============================================================*/
create unique index MOV_PRODUCTOS_PK on MOV_PRODUCTOS (
ID_MOVIMIENTO2
);

/*==============================================================*/
/* Index: PRESENTA_FK                                           */
/*==============================================================*/
create  index PRESENTA_FK on MOV_PRODUCTOS (
ID_PRODUCTO
);

/*==============================================================*/
/* Table: PEDIDOS                                               */
/*==============================================================*/
create table PEDIDOS (
   ID_PEDIDO            INT4                 not null,
   ID_METODO            INT4                 not null,
   ID_CLIENTE           INT4                 not null,
   FECHA_HORA           DATE                 not null,
   ESTADO               CHAR(3)              not null,
   ENTREGA_FISICA       CHAR(1)              not null default 'N',
   SUBTOTAL             DECIMAL(10,2)        not null,
   IVA                  DECIMAL(10,2)        not null,
   TOTAL                DECIMAL(10,2)        not null,
   FECHA_PAGO           DATE                 not null,
   constraint PK_PEDIDOS primary key (ID_PEDIDO)
);

/*==============================================================*/
/* Index: PEDIDOS_PK                                            */
/*==============================================================*/
create unique index PEDIDOS_PK on PEDIDOS (
ID_PEDIDO
);

/*==============================================================*/
/* Index: CORRESPONDE_FK                                        */
/*==============================================================*/
create  index CORRESPONDE_FK on PEDIDOS (
ID_METODO
);

/*==============================================================*/
/* Index: REALIZA_FK                                            */
/*==============================================================*/
create  index REALIZA_FK on PEDIDOS (
ID_CLIENTE
);

/*==============================================================*/
/* Table: PERSONALIZACION                                       */
/*==============================================================*/
create table PERSONALIZACION (
   ID_OPCION            INT4                 not null,
   ID_PRODUCTO          INT4                 not null,
   NOMBRE_ATR           VARCHAR(100)         not null,
   TIPO_VALOR           CHAR(10)             not null,
   VALORES_JSON         VARCHAR(1)           not null,
   COSTO_ADICIONAL      DECIMAL(10,2)        not null,
   constraint PK_PERSONALIZACION primary key (ID_OPCION)
);

/*==============================================================*/
/* Index: PERSONALIZACION_PK                                    */
/*==============================================================*/
create unique index PERSONALIZACION_PK on PERSONALIZACION (
ID_OPCION
);

/*==============================================================*/
/* Index: TIENE_FK                                              */
/*==============================================================*/
create  index TIENE_FK on PERSONALIZACION (
ID_PRODUCTO
);

/*==============================================================*/
/* Table: PRODUCTOS                                             */
/*==============================================================*/
create table PRODUCTOS (
   ID_PRODUCTO          INT4                 not null,
   ID_LINEA             INT4                 not null,
   PROD_NOMBRE          VARCHAR(100)         not null,
   PROD_PESO            DECIMAL(10,2)        not null,
   PROD_DESCRIPCION     VARCHAR(500)         not null,
   COLORES              JSONB                not null default '[]'::jsonb,
   COSTO_MAT_PRIM       DECIMAL(10,2)        not null,
   COSTO_MANO_OBRA      DECIMAL(10,2)        not null,
   PORCENTAJE_MARGEN_GANANCIA DECIMAL(10,2)        not null,
   PRECIO_BASE          DECIMAL(10,2)        not null,
   ES_PERSONALIZABLE    CHAR(1)              not null,
   STOCK_ACTUAL         INT4                 not null,
   STOCK_DESCUENTO      INT4                 not null,
   PROD_ESTADO          CHAR(3)              not null,
   constraint PK_PRODUCTOS primary key (ID_PRODUCTO)
);

/*==============================================================*/
/* Index: PRODUCTOS_PK                                          */
/*==============================================================*/
create unique index PRODUCTOS_PK on PRODUCTOS (
ID_PRODUCTO
);

/*==============================================================*/
/* Index: PERTENECE_FK                                          */
/*==============================================================*/
create  index PERTENECE_FK on PRODUCTOS (
ID_LINEA
);

/*==============================================================*/
/* Table: PROVEEDORES                                           */
/*==============================================================*/
create table PROVEEDORES (
   ID_PROVEEDOR         INT4                 not null,
   PROV_NOMBRE          VARCHAR(100)         not null,
   PROV_TELEFONO        CHAR(10)             not null,
   PROV_CORREO          VARCHAR(100)         not null,
   PROV_DIRECCION       VARCHAR(255)         not null,
   PROV_ESTADO          CHAR(3)              not null,
   constraint PK_PROVEEDORES primary key (ID_PROVEEDOR)
);

/*==============================================================*/
/* Index: PROVEEDORES_PK                                        */
/*==============================================================*/
create unique index PROVEEDORES_PK on PROVEEDORES (
ID_PROVEEDOR
);

/*==============================================================*/
/* Table: PRO_X_MAT                                             */
/*==============================================================*/
create table PRO_X_MAT (
   ID_PRODUCTO          INT4                 not null,
   ID_MATERIAL          INT4                 not null,
   CANTIDAD_REQ         INT4                 not null,
   ES_PERSONALIZABLE    CHAR(1)              not null,
   constraint PK_PRO_X_MAT primary key (ID_PRODUCTO, ID_MATERIAL)
);

/*==============================================================*/
/* Index: PRO_X_MAT_PK                                          */
/*==============================================================*/
create unique index PRO_X_MAT_PK on PRO_X_MAT (
ID_PRODUCTO,
ID_MATERIAL
);

/*==============================================================*/
/* Index: PRO_X_MAT2_FK                                         */
/*==============================================================*/
create  index PRO_X_MAT2_FK on PRO_X_MAT (
ID_PRODUCTO
);

/*==============================================================*/
/* Index: PRO_X_MAT_FK                                          */
/*==============================================================*/
create  index PRO_X_MAT_FK on PRO_X_MAT (
ID_MATERIAL
);

/*==============================================================*/
/* Table: PRO_X_PED                                             */
/*==============================================================*/
create table PRO_X_PED (
   ID_PRODUCTO          INT4                 not null,
   ID_PEDIDO            INT4                 not null,
   CANTIDAD             INT4                 not null,
   PRECIO_UNITARIO      DECIMAL(10,2)        not null,
   PORCENTAJE_DESCUENTO DECIMAL(10,2)        not null,
   MONTO_DESCUENTO      DECIMAL(10,2)        not null,
   SUBTOTAL_ITEM        DECIMAL(10,2)        not null,
   PERSONALIZACION_SELEC VARCHAR(1)           not null,
   constraint PK_PRO_X_PED primary key (ID_PRODUCTO, ID_PEDIDO)
);

/*==============================================================*/
/* Index: PRO_X_PED_PK                                          */
/*==============================================================*/
create unique index PRO_X_PED_PK on PRO_X_PED (
ID_PRODUCTO,
ID_PEDIDO
);

/*==============================================================*/
/* Index: PRO_X_PED2_FK                                         */
/*==============================================================*/
create  index PRO_X_PED2_FK on PRO_X_PED (
ID_PRODUCTO
);

/*==============================================================*/
/* Index: PRO_X_PED_FK                                          */
/*==============================================================*/
create  index PRO_X_PED_FK on PRO_X_PED (
ID_PEDIDO
);

/*==============================================================*/
/* Table: PRV_X_MAT                                             */
/*==============================================================*/
create table PRV_X_MAT (
   ID_MATERIAL          INT4                 not null,
   ID_PROVEEDOR         INT4                 not null,
   ORIGEN               CHAR(3)              not null,
   PRECIO_COMPRA        DECIMAL(10,2)        not null,
   CANTIDAD_MIN         INT4                 not null,
   DIAS_ENTREGA         INT4                 not null,
   constraint PK_PRV_X_MAT primary key (ID_MATERIAL, ID_PROVEEDOR)
);

/*==============================================================*/
/* Index: PRV_X_MAT_PK                                          */
/*==============================================================*/
create unique index PRV_X_MAT_PK on PRV_X_MAT (
ID_MATERIAL,
ID_PROVEEDOR
);

/*==============================================================*/
/* Index: PRV_X_MAT2_FK                                         */
/*==============================================================*/
create  index PRV_X_MAT2_FK on PRV_X_MAT (
ID_MATERIAL
);

/*==============================================================*/
/* Index: PRV_X_MAT_FK                                          */
/*==============================================================*/
create  index PRV_X_MAT_FK on PRV_X_MAT (
ID_PROVEEDOR
);

/*==============================================================*/
/* Table: ROL                                                   */
/*==============================================================*/
create table ROL (
   ID_ROL               INT4                 not null,
   NOMBRE_ROL           VARCHAR(100)         not null,
   constraint PK_ROL primary key (ID_ROL)
);

/*==============================================================*/
/* Index: ROL_PK                                                */
/*==============================================================*/
create unique index ROL_PK on ROL (
ID_ROL
);

/*==============================================================*/
/* Table: USUARIO                                               */
/*==============================================================*/
create table USUARIO (
   ID_USUARIO           INT4                 not null,
   ID_CLIENTE           INT4                 not null,
   ID_ROL               INT4                 not null,
   NOMBRE               VARCHAR(100)         not null,
   CORREO               VARCHAR(100)         not null,
   CONTRASENA_HASH      VARCHAR(500)         not null,
   USR_ESTADO           CHAR(3)              not null,
   ULTIMO_ACCESO        DATE                 not null,
   constraint PK_USUARIO primary key (ID_USUARIO)
);

/*==============================================================*/
/* Index: USUARIO_PK                                            */
/*==============================================================*/
create unique index USUARIO_PK on USUARIO (
ID_USUARIO
);

/*==============================================================*/
/* Index: TIENE_2_FK                                            */
/*==============================================================*/
create  index TIENE_2_FK on USUARIO (
ID_CLIENTE
);

/*==============================================================*/
/* Index: REPRESENTA_FK                                         */
/*==============================================================*/
create  index REPRESENTA_FK on USUARIO (
ID_ROL
);

alter table ENVIO
   add constraint FK_ENVIO_GENERA_PEDIDOS foreign key (ID_PEDIDO)
      references PEDIDOS (ID_PEDIDO)
      on delete restrict on update restrict;

alter table FACTURAS
   add constraint FK_FACTURAS_ASOCIA_PEDIDOS foreign key (ID_PEDIDO)
      references PEDIDOS (ID_PEDIDO)
      on delete restrict on update restrict;

alter table FAVORITOS
   add constraint FK_FAVORITO_FAVORITOS_PRODUCTO foreign key (ID_PRODUCTO)
      references PRODUCTOS (ID_PRODUCTO)
      on delete restrict on update restrict;

alter table FAVORITOS
   add constraint FK_FAVORITO_FAVORITOS_CLIENTE foreign key (ID_CLIENTE)
      references CLIENTE (ID_CLIENTE)
      on delete restrict on update restrict;

alter table IMAGENES
   add constraint FK_IMAGENES_MUESTRA_PRODUCTO foreign key (ID_PRODUCTO)
      references PRODUCTOS (ID_PRODUCTO)
      on delete restrict on update restrict;

alter table MATERIALES
   add constraint FK_MATERIAL_AGRUPA_CAT_MAT foreign key (ID_CATEGORIA)
      references CAT_MAT (ID_CATEGORIA)
      on delete restrict on update restrict;

alter table MOV_MATERIALES
   add constraint FK_MOV_MATE_REGISTRA_MATERIAL foreign key (ID_MATERIAL)
      references MATERIALES (ID_MATERIAL)
      on delete restrict on update restrict;

alter table MOV_PRODUCTOS
   add constraint FK_MOV_PROD_PRESENTA_PRODUCTO foreign key (ID_PRODUCTO)
      references PRODUCTOS (ID_PRODUCTO)
      on delete restrict on update restrict;

alter table PEDIDOS
   add constraint FK_PEDIDOS_CORRESPON_METODO_P foreign key (ID_METODO)
      references METODO_PAGO (ID_METODO)
      on delete restrict on update restrict;

alter table PEDIDOS
   add constraint FK_PEDIDOS_REALIZA_CLIENTE foreign key (ID_CLIENTE)
      references CLIENTE (ID_CLIENTE)
      on delete restrict on update restrict;

alter table PERSONALIZACION
   add constraint FK_PERSONAL_TIENE_PRODUCTO foreign key (ID_PRODUCTO)
      references PRODUCTOS (ID_PRODUCTO)
      on delete restrict on update restrict;

alter table PRODUCTOS
   add constraint FK_PRODUCTO_PERTENECE_LINEAS foreign key (ID_LINEA)
      references LINEAS (ID_LINEA)
      on delete restrict on update restrict;

alter table PRO_X_MAT
   add constraint FK_PRO_X_MA_PRO_X_MAT_MATERIAL foreign key (ID_MATERIAL)
      references MATERIALES (ID_MATERIAL)
      on delete restrict on update restrict;

alter table PRO_X_MAT
   add constraint FK_PRO_X_MA_PRO_X_MAT_PRODUCTO foreign key (ID_PRODUCTO)
      references PRODUCTOS (ID_PRODUCTO)
      on delete restrict on update restrict;

alter table PRO_X_PED
   add constraint FK_PRO_X_PE_PRO_X_PED_PEDIDOS foreign key (ID_PEDIDO)
      references PEDIDOS (ID_PEDIDO)
      on delete restrict on update restrict;

alter table PRO_X_PED
   add constraint FK_PRO_X_PE_PRO_X_PED_PRODUCTO foreign key (ID_PRODUCTO)
      references PRODUCTOS (ID_PRODUCTO)
      on delete restrict on update restrict;

alter table PRV_X_MAT
   add constraint FK_PRV_X_MA_PRV_X_MAT_PROVEEDO foreign key (ID_PROVEEDOR)
      references PROVEEDORES (ID_PROVEEDOR)
      on delete restrict on update restrict;

alter table PRV_X_MAT
   add constraint FK_PRV_X_MA_PRV_X_MAT_MATERIAL foreign key (ID_MATERIAL)
      references MATERIALES (ID_MATERIAL)
      on delete restrict on update restrict;

alter table USUARIO
   add constraint FK_USUARIO_REPRESENT_ROL foreign key (ID_ROL)
      references ROL (ID_ROL)
      on delete restrict on update restrict;

alter table USUARIO
   add constraint FK_USUARIO_TIENE_2_CLIENTE foreign key (ID_CLIENTE)
      references CLIENTE (ID_CLIENTE)
      on delete restrict on update restrict;

/* CORRECCIONES PARA DIAGRAMA
REALIZADO POR: MARTÍN ALEJANDRO HERRERA PACHECO
FECHA: 2026-07-15*/

/*1. CORRECCIÓN DE CAMPOS JSON*/
-- PERSONALIZACION
ALTER TABLE PERSONALIZACION
    ALTER COLUMN VALORES_JSON TYPE JSONB USING VALORES_JSON::jsonb;

-- PRO_X_PED
ALTER TABLE PRO_X_PED
    ALTER COLUMN PERSONALIZACION_SELEC TYPE JSONB USING PERSONALIZACION_SELEC::jsonb;

-- AUDITORIA
ALTER TABLE AUDITORIA
    ALTER COLUMN VALORES_ANTERIORES TYPE JSONB USING VALORES_ANTERIORES::jsonb;

ALTER TABLE AUDITORIA
    ALTER COLUMN VALORES_NUEVOS TYPE JSONB USING VALORES_NUEVOS::jsonb;


/*2. CORRECIÓN DE CAMPOS DE FECHA*/
-- PEDIDOS
ALTER TABLE PEDIDOS ALTER COLUMN FECHA_HORA TYPE TIMESTAMP USING FECHA_HORA::timestamp;
ALTER TABLE PEDIDOS ALTER COLUMN FECHA_PAGO TYPE TIMESTAMP USING FECHA_PAGO::timestamp;

-- CLIENTE
ALTER TABLE CLIENTE ALTER COLUMN FECHA_REGISTRO TYPE TIMESTAMP USING FECHA_REGISTRO::timestamp;

-- ENVIO
ALTER TABLE ENVIO ALTER COLUMN FECHA_GEN TYPE TIMESTAMP USING FECHA_GEN::timestamp;

-- FACTURAS
ALTER TABLE FACTURAS ALTER COLUMN FECHA_EMISION TYPE TIMESTAMP USING FECHA_EMISION::timestamp;
ALTER TABLE FACTURAS ALTER COLUMN FECHA_AUTORIZACION TYPE TIMESTAMP USING FECHA_AUTORIZACION::timestamp;

-- USUARIO
ALTER TABLE USUARIO ALTER COLUMN ULTIMO_ACCESO TYPE TIMESTAMP USING ULTIMO_ACCESO::timestamp;

-- MOV_MATERIALES
ALTER TABLE MOV_MATERIALES ALTER COLUMN FECHA_MOV TYPE TIMESTAMP USING FECHA_MOV::timestamp;

-- MOV_PRODUCTOS
ALTER TABLE MOV_PRODUCTOS ALTER COLUMN FECHA_MOV TYPE TIMESTAMP USING FECHA_MOV::timestamp;

-- AUDITORIA
ALTER TABLE AUDITORIA ALTER COLUMN FECHA_HORA TYPE TIMESTAMP USING FECHA_HORA::timestamp;

/*3. CORRECCIÓN DE CAMPOS DE USUARIO (ID_CLIENTE OPCIONAL)*/
-- USUARIO: permitir ID_CLIENTE nulo (solo aplica a usuarios con rol Cliente)
ALTER TABLE USUARIO
    ALTER COLUMN ID_CLIENTE DROP NOT NULL;
