-- =====================================================================================================
-- 02 - Ajusta una BD "concesionario" EXISTENTE (creada desde MySQL Workbench) al esquema de la
--      migración EF Core "EsquemaInicial" y la registra en __EFMigrationsHistory.
--
-- Cuándo usarlo: solo si su BD ya existe y tiene datos. Si va a crear la BD desde cero, NO lo ejecute:
--                use "dotnet ef database update" (ver README).
--
-- Requisitos:   haber ejecutado antes 01_alinear_esquema.sql.
-- Antes:        haga un respaldo:  mysqldump -u root -p concesionario > respaldo_concesionario.sql
-- Después:      ejecute "dotnet ef database update --project Data --startup-project Presentation"
--               para crear la tabla usuario (migración AgregarTablaUsuario).
--
-- Cambios:
--   1. mantenimiento.año_mantenimiento -> anio_mantenimiento (se evita la "ñ" en identificadores).
--   2. Columnas obligatorias en el modelo pasan a NOT NULL (pago, credito, garantia, mantenimiento,
--      empleado.cargo/salario, sede.direccion). Las FK opcionales (id_sede, id_cliente, id_empleado)
--      siguen admitiendo NULL.
--   3. Nombres de índices y llaves foráneas con convención: uq_<tabla>_<col>, ix_<tabla>_<col>,
--      fk_<tabla>_<tabla_ref>. Las reglas ON DELETE se conservan.
-- =====================================================================================================
USE concesionario;

-- -----------------------------------------------------------------------------------------------------
-- 0. Verificación previa: TODAS las columnas deben devolver 0. Si alguna es > 0, corrija esos
--    registros antes de continuar (el paso 3 fallaría con "Data truncated" / "Invalid use of NULL").
-- -----------------------------------------------------------------------------------------------------
SELECT 'empleado'      AS tabla, SUM(cargo IS NULL) + SUM(salario IS NULL) AS nulos FROM empleado
UNION ALL SELECT 'sede',          SUM(direccion IS NULL) FROM sede
UNION ALL SELECT 'pago',          SUM(id_venta IS NULL) + SUM(monto IS NULL) + SUM(fecha_pago IS NULL) + SUM(metodo_pago IS NULL) FROM pago
UNION ALL SELECT 'credito',       SUM(id_pago IS NULL) + SUM(entidad_bancaria IS NULL) + SUM(monto_aprobado IS NULL) + SUM(numero_cuotas IS NULL) FROM credito
UNION ALL SELECT 'garantia',      SUM(fecha_inicio IS NULL) + SUM(condiciones IS NULL) FROM garantia
UNION ALL SELECT 'mantenimiento', SUM(id_garantia IS NULL) + SUM(kilometraje IS NULL) + SUM(`año_mantenimiento` IS NULL)
                                  + SUM(servicios_realizados IS NULL) + SUM(fecha_programada IS NULL) FROM mantenimiento;

-- -----------------------------------------------------------------------------------------------------
-- 1. Quitar las llaves foráneas con nombres antiguos (se recrean en el paso 4)
-- -----------------------------------------------------------------------------------------------------
ALTER TABLE credito       DROP FOREIGN KEY id_pago;
ALTER TABLE garantia      DROP FOREIGN KEY id_detalle;
ALTER TABLE mantenimiento DROP FOREIGN KEY id_garantia;
ALTER TABLE pago          DROP FOREIGN KEY id_venta;
ALTER TABLE detalle_venta DROP FOREIGN KEY detalle_venta_ibfk_1, DROP FOREIGN KEY detalle_venta_ibfk_2;
ALTER TABLE empleado      DROP FOREIGN KEY empleado_ibfk_1;
ALTER TABLE vehiculo      DROP FOREIGN KEY vehiculo_ibfk_1;
ALTER TABLE venta         DROP FOREIGN KEY venta_ibfk_1, DROP FOREIGN KEY venta_ibfk_2;

-- -----------------------------------------------------------------------------------------------------
-- 2. Renombrar índices
-- -----------------------------------------------------------------------------------------------------
ALTER TABLE cliente       RENAME INDEX cedula          TO uq_cliente_cedula;
ALTER TABLE empleado      RENAME INDEX cedula          TO uq_empleado_cedula,
                          RENAME INDEX id_sede         TO ix_empleado_id_sede;
ALTER TABLE vehiculo      RENAME INDEX vin             TO uq_vehiculo_vin,
                          RENAME INDEX id_sede         TO ix_vehiculo_id_sede;
ALTER TABLE venta         RENAME INDEX id_cliente      TO ix_venta_id_cliente,
                          RENAME INDEX id_empleado     TO ix_venta_id_empleado;
ALTER TABLE detalle_venta RENAME INDEX id_venta        TO ix_detalle_venta_id_venta,
                          RENAME INDEX id_vehiculo     TO ix_detalle_venta_id_vehiculo;
ALTER TABLE pago          RENAME INDEX id_venta_idx    TO ix_pago_id_venta;
ALTER TABLE credito       RENAME INDEX id_pago_idx     TO ix_credito_id_pago;
ALTER TABLE garantia      RENAME INDEX id_detalle_idx  TO ix_garantia_id_detalle;
ALTER TABLE mantenimiento RENAME INDEX id_garantia_idx TO ix_mantenimiento_id_garantia;

-- -----------------------------------------------------------------------------------------------------
-- 3. Renombrar columna con "ñ" y aplicar NOT NULL a las columnas obligatorias
-- -----------------------------------------------------------------------------------------------------
ALTER TABLE mantenimiento RENAME COLUMN `año_mantenimiento` TO anio_mantenimiento;

ALTER TABLE empleado      MODIFY cargo                VARCHAR(50)    NOT NULL,
                          MODIFY salario              DECIMAL(10,2)  NOT NULL;
ALTER TABLE sede          MODIFY direccion            VARCHAR(150)   NOT NULL;
ALTER TABLE pago          MODIFY id_venta             INT            NOT NULL,
                          MODIFY monto                DECIMAL(12,2)  NOT NULL,
                          MODIFY fecha_pago           DATETIME       NOT NULL,
                          MODIFY metodo_pago          VARCHAR(100)   NOT NULL;
ALTER TABLE credito       MODIFY id_pago              INT            NOT NULL,
                          MODIFY entidad_bancaria     VARCHAR(45)    NOT NULL,
                          MODIFY monto_aprobado       DECIMAL(12,2)  NOT NULL,
                          MODIFY numero_cuotas        INT            NOT NULL;
ALTER TABLE garantia      MODIFY fecha_inicio         DATE           NOT NULL,
                          MODIFY condiciones          VARCHAR(200)   NOT NULL;
ALTER TABLE mantenimiento MODIFY id_garantia          INT            NOT NULL,
                          MODIFY kilometraje          INT            NOT NULL,
                          MODIFY anio_mantenimiento   INT            NOT NULL,
                          MODIFY servicios_realizados VARCHAR(200)   NOT NULL,
                          MODIFY fecha_programada     DATE           NOT NULL;

-- -----------------------------------------------------------------------------------------------------
-- 4. Recrear las llaves foráneas con nombres claros (mismas reglas ON DELETE que antes)
-- -----------------------------------------------------------------------------------------------------
ALTER TABLE empleado      ADD CONSTRAINT fk_empleado_sede          FOREIGN KEY (id_sede)     REFERENCES sede (id_sede)                  ON DELETE SET NULL;
ALTER TABLE vehiculo      ADD CONSTRAINT fk_vehiculo_sede          FOREIGN KEY (id_sede)     REFERENCES sede (id_sede)                  ON DELETE SET NULL;
ALTER TABLE venta         ADD CONSTRAINT fk_venta_cliente          FOREIGN KEY (id_cliente)  REFERENCES cliente (id_cliente)            ON DELETE RESTRICT,
                          ADD CONSTRAINT fk_venta_empleado         FOREIGN KEY (id_empleado) REFERENCES empleado (id_empleado)          ON DELETE RESTRICT;
ALTER TABLE detalle_venta ADD CONSTRAINT fk_detalle_venta_venta    FOREIGN KEY (id_venta)    REFERENCES venta (id_venta)                ON DELETE CASCADE,
                          ADD CONSTRAINT fk_detalle_venta_vehiculo FOREIGN KEY (id_vehiculo) REFERENCES vehiculo (id_vehiculo)          ON DELETE RESTRICT;
ALTER TABLE pago          ADD CONSTRAINT fk_pago_venta             FOREIGN KEY (id_venta)    REFERENCES venta (id_venta)                ON DELETE RESTRICT;
ALTER TABLE credito       ADD CONSTRAINT fk_credito_pago           FOREIGN KEY (id_pago)     REFERENCES pago (id_pago)                  ON DELETE RESTRICT;
ALTER TABLE garantia      ADD CONSTRAINT fk_garantia_detalle_venta FOREIGN KEY (id_detalle)  REFERENCES detalle_venta (id_detalle)      ON DELETE RESTRICT;
ALTER TABLE mantenimiento ADD CONSTRAINT fk_mantenimiento_garantia FOREIGN KEY (id_garantia) REFERENCES garantia (id_garantia)          ON DELETE RESTRICT;

-- -----------------------------------------------------------------------------------------------------
-- 5. Registrar la migración base para que EF Core no intente crear de nuevo las tablas existentes
-- -----------------------------------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId`    VARCHAR(150) NOT NULL,
    `ProductVersion` VARCHAR(32)  NOT NULL,
    PRIMARY KEY (`MigrationId`)
) CHARACTER SET = utf8mb4;

INSERT IGNORE INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20261110235247_EsquemaInicial', '9.0.0');
