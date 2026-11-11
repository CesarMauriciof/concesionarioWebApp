using Data;
using Logic.Common;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Logic.Auth;

/// <summary>
/// Proceso de autenticación (capa Logic). Decide QUIÉN puede entrar al sistema.
///
/// RESPONSABLE: Yeison - Tarea T4 (ver docs/sprint5/TAREAS_SPRINT5.md).
/// Depende de T3 (Keiner): tabla usuario_login_externo y columna usuario.ultimo_acceso.
///
/// Reglas de negocio que debe cumplir:
///   1. Solo entran usuarios que YA existen en la tabla usuario y están ACTIVOS.
///      El administrador los registra antes en /user/create (no hay auto-registro).
///   2. Primer ingreso con Google: se busca el usuario por correo (sin distinguir mayúsculas)
///      y se crea el vínculo en usuario_login_externo.
///   3. Siguientes ingresos: se busca por (Proveedor, ClaveProveedor), NO por correo,
///      porque el correo de la cuenta externa puede cambiar.
///   4. Si el usuario no existe -> BusinessException("Su cuenta no está autorizada para ingresar al sistema.")
///      Si está inactivo     -> BusinessException("Su usuario está inactivo. Comuníquese con el administrador.")
///   5. En cada ingreso exitoso se actualiza usuario.ultimo_acceso.
/// </summary>
public class AuthLog(IDbContextFactory<DataContext> dbFactory, UserLog userLog)
{
    /// <summary>
    /// Procesa el ingreso con un proveedor OAuth 2.0 (Google) y devuelve el usuario autenticado.
    /// </summary>
    /// <exception cref="BusinessException">Si el correo no está registrado o el usuario está inactivo.</exception>
    public async Task<AuthenticatedUser> LoginWithExternalProviderAsync(ExternalLoginData data)
    {
        // TODO [T4 - Yeison]:
        //  a) Buscar en usuario_login_externo por (data.Proveedor, data.ClaveProveedor) incluyendo el Usuario.
        //  b) Si no existe: buscar el usuario por correo (data.Correo) y crear el vínculo (FechaVinculacion = ahora).
        //  c) Validar que el usuario exista y esté Activo (reglas 1 y 4).
        //  d) Actualizar UltimoAcceso (regla 5) y guardar con un solo SaveChangesAsync.
        //  e) Devolver ToAuthenticatedUser(usuario).
        await using var db = await dbFactory.CreateDbContextAsync();
        throw new NotImplementedException("T4: LoginWithExternalProviderAsync pendiente de implementar.");
    }

    /// <summary>
    /// Ingreso con usuario y contraseña (respaldo, p. ej. para el administrador inicial).
    /// </summary>
    /// <exception cref="BusinessException">Si las credenciales no son válidas.</exception>
    public async Task<AuthenticatedUser> LoginWithPasswordAsync(string userName, string password)
    {
        // TODO [T4 - Yeison]:
        //  a) Usar userLog.ValidateCredentialsAsync(userName, password) (ya existe y verifica el hash).
        //  b) Si devuelve null -> BusinessException("Usuario o contraseña incorrectos.")
        //     (mismo mensaje para usuario inexistente y clave errada: no revelar cuál falló).
        //  c) Actualizar UltimoAcceso y devolver ToAuthenticatedUser(usuario).
        _ = userLog;
        await Task.CompletedTask;
        throw new NotImplementedException("T4: LoginWithPasswordAsync pendiente de implementar.");
    }

    /// <summary>Convierte la entidad en el DTO que usa Presentation. Ya implementado.</summary>
    private static AuthenticatedUser ToAuthenticatedUser(UserEntity user) =>
        new(user.IdUsuario, user.NombreUsuario, user.Correo, user.Rol);
}
