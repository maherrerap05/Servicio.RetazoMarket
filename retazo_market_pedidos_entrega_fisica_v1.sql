/*==============================================================*/
/* El Retazo Market - Entrega física en tienda para pedidos     */
/* Diseñado para ejecutarse sobre una base de datos ya creada.  */
/*==============================================================*/

BEGIN;

ALTER TABLE PEDIDOS
    ADD COLUMN IF NOT EXISTS ENTREGA_FISICA CHAR(1) NOT NULL DEFAULT 'N';

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint
        WHERE conname = 'ck_pedidos_entrega_fisica'
          AND conrelid = 'pedidos'::regclass
    ) THEN
        ALTER TABLE PEDIDOS
            ADD CONSTRAINT CK_PEDIDOS_ENTREGA_FISICA
            CHECK (ENTREGA_FISICA IN ('S','N'));
    END IF;
END
$$;

COMMIT;

