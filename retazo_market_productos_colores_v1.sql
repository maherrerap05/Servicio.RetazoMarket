/*==============================================================*/
/* El Retazo Market - Campo de colores para productos           */
/* Diseñado para ejecutarse sobre una base de datos ya creada.  */
/*==============================================================*/

BEGIN;

ALTER TABLE PRODUCTOS
    ADD COLUMN IF NOT EXISTS COLORES JSONB NOT NULL DEFAULT '[]'::jsonb;

COMMIT;

