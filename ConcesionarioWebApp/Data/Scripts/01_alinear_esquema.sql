-- Alinea el esquema MySQL con los modelos EF Core. 1) PKs de pago, credito, garantia y mantenimiento
-- deben ser AUTO_INCREMENT ([DatabaseGenerated(Identity)]); sin esto los INSERT fallan. 2) Montos a decimal(12,2).
-- Se desactivan las FK temporalmente porque estas columnas son referenciadas por otras tablas.
USE concesionario;

SET FOREIGN_KEY_CHECKS = 0;
ALTER TABLE pago          MODIFY id_pago          INT NOT NULL AUTO_INCREMENT;
ALTER TABLE credito       MODIFY id_credito       INT NOT NULL AUTO_INCREMENT;
ALTER TABLE garantia      MODIFY id_garantia      INT NOT NULL AUTO_INCREMENT;
ALTER TABLE mantenimiento MODIFY id_mantenimiento INT NOT NULL AUTO_INCREMENT;
SET FOREIGN_KEY_CHECKS = 1;

-- pago.monto y credito.monto_aprobado eran decimal(10,2) (máx. 99.999.999,99), insuficiente para
-- vehículos de más de 100 millones; se igualan a venta.total_pagado / vehiculo.precio (decimal(12,2)).
ALTER TABLE pago    MODIFY monto          DECIMAL(12,2) NULL;
ALTER TABLE credito MODIFY monto_aprobado DECIMAL(12,2) NULL;
