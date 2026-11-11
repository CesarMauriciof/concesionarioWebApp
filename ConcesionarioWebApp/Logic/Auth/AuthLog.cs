using Data;
using Logic.Common;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Logic.Auth;

/// <summary>
/// Proceso de autenticación (capa Logic). Decide QUIÉN puede entrar al sistema.
///
/// Reglas de negocio:
///   1. Solo entran usuarios que YA existen en la tabla usuario y están ACTIVOS.
///      El administrador los registra antes en /user/create (no hay auto-registro).
///   2. Primer ingreso con Google: se busca el usuario por correo (sin distinguir mayúsculas)
///      y se crea el vínculo en usuario_login_externo.
///   3. Siguientes ingresos: se busca por (Proveedor, ClaveProveedor), NO por correo,
///      porque el correo de la cuenta externa puede cambiar.
///   4. Si el usuario no existe -> "Su cuenta no está autorizada para ingresar al sistema."
///      Si está inactivo     -> "Su usuario está inactivo. Comuníquese con el administrador."
///   5. En cada ingreso exitoso se actualiza usuario.ultimo_acceso.
/// </summary>
public class AuthLog(IDbContextFactory<DataContext> dbFactory, UserLog userLog)
{
    public const string MensajeNoAutorizado = "Su cuenta no está autorizada para ingresar al sistema.";
    public const string MensajeInactivo = "Su usuario está inactivo. Comuníquese con el administrador.";
    public const string MensajeCredenciales = "Usuario o contraseña incorrectos.";

    /// <summary>
    /// Procesa el ingreso con un proveedor OAuth 2.0 (Google) y devuelve el usuario autenticado.
    /// </summary>
    /// <exception cref="BusinessException">Si el correo no está registrado o el usuario está inactivo.</exception>
    public async Task<AuthenticatedUser> LoginWithExternalProviderAsync(ExternalLoginData data)
    {
        if (string.IsNullOrWhiteSpace(data.Proveedor) || string.IsNullOrWhiteSpace(data.ClaveProveedor))
        {
            throw new BusinessException("El proveedor no devolvió la identificación del usuario.");
        }

        await using var db = await dbFactory.CreateDbContextAsync();

        // Regla 3: ingresos siguientes -> se busca por el vínculo, no por el correo
        var login = await db.ExternalLogins
            .Include(l => l.Usuario)
            .FirstOrDefaultAsync(l => l.Proveedor == data.Proveedor && l.ClaveProveedor == data.ClaveProveedor);

        var user = login?.Usuario;

        // Regla 2: primer ingreso -> se busca por correo y se crea el vínculo
        if (user == null)
        {
            var correo = data.Correo.Trim().ToLowerInvariant();
            if (correo.Length == 0)
            {
                throw new BusinessException(MensajeNoAutorizado);
            }

            user = await db.Users.FirstOrDefaultAsync(u => u.Correo == correo)
                   ?? throw new BusinessException(MensajeNoAutorizado);   // Regla 1 y 4

            db.ExternalLogins.Add(new ExternalLoginEntity
            {
                Proveedor = data.Proveedor,
                ClaveProveedor = data.ClaveProveedor,
                CorreoProveedor = correo,
                FechaVinculacion = DateTime.Now,
                IdUsuario = user.IdUsuario
            });
        }

        if (!user.Activo)
        {
            throw new BusinessException(MensajeInactivo);                  // Regla 4
        }

        user.UltimoAcceso = DateTime.Now;                                   // Regla 5

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new BusinessException("No se pudo registrar el inicio de sesión. Intente de nuevo.", ex);
        }

        return ToAuthenticatedUser(user);
    }

    /// <summary>
    /// Ingreso con usuario y contraseña (respaldo, p. ej. para el administrador).
    /// </summary>
    /// <exception cref="BusinessException">Si las credenciales no son válidas.</exception>
    public async Task<AuthenticatedUser> LoginWithPasswordAsync(string userName, string password)
    {
        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrEmpty(password))
        {
            throw new BusinessException(MensajeCredenciales);
        }

        UserEntity? user;
        try
        {
            // Ya verifica el hash y que el usuario esté activo
            user = await userLog.ValidateCredentialsAsync(userName, password);
        }
        catch (FormatException)
        {
            // Hash vacío o inválido (p. ej. usuarios que solo entran con Google)
            user = null;
        }

        // Mismo mensaje si el usuario no existe o la clave es errada: no revelar cuál falló
        if (user == null)
        {
            throw new BusinessException(MensajeCredenciales);
        }

        await using var db = await dbFactory.CreateDbContextAsync();
        await db.Users.Where(u => u.IdUsuario == user.IdUsuario)
            .ExecuteUpdateAsync(s => s.SetProperty(u => u.UltimoAcceso, DateTime.Now));

        return ToAuthenticatedUser(user);
    }

    /// <summary>Convierte la entidad en el DTO que usa Presentation (sin datos sensibles).</summary>
    private static AuthenticatedUser ToAuthenticatedUser(UserEntity user) =>
        new(user.IdUsuario, user.NombreUsuario, user.Correo, user.Rol);
}