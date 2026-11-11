USE concesionario;

SET @correo_admin  = 'su.correo@gmail.com';   -- <<< CAMBIAR por su correo de Google
SET @usuario_admin = 'administrador';

INSERT INTO usuario (nombre_usuario, correo, contrasena_hash, rol, activo, fecha_creacion)
SELECT @usuario_admin, LOWER(@correo_admin), '', 'Administrador', 1, NOW()
    WHERE NOT EXISTS (SELECT 1 FROM usuario WHERE correo = LOWER(@correo_admin))
  AND NOT EXISTS (SELECT 1 FROM usuario WHERE nombre_usuario = @usuario_admin);

-- Verificación: debe aparecer el administrador
SELECT id_usuario, nombre_usuario, correo, rol, activo FROM usuario WHERE correo = LOWER(@correo_admin);
