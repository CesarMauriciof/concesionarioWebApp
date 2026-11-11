using Data;
using Logic.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Logic;

/// <summary>Lógica de negocio de usuarios del sistema (incluye el manejo seguro de contraseñas).</summary>
public class UserLog(IDbContextFactory<DataContext> dbFactory, IPasswordHasher<UserEntity> passwordHasher)
    : BaseLog<UserEntity>(dbFactory)
{
    public const int MinPasswordLength = 8;

    /// <summary>Devuelve todos los usuarios con su empleado asociado.</summary>
    public Task<List<UserEntity>> UserListAsync() =>
        ListAsync(q => q.Include(u => u.Empleado).OrderBy(u => u.NombreUsuario));

    /// <summary>Busca un usuario por su ID. Devuelve null si no existe.</summary>
    public Task<UserEntity?> GetUserByIdAsync(int id) => GetByIdAsync(id);

    /// <summary>Registra un usuario guardando únicamente el hash de su contraseña.</summary>
    public async Task CreateUserAsync(UserEntity user, string password)
    {
        ValidatePassword(password);

        user.IdUsuario = 0;
        user.FechaCreacion = DateTime.Now;
        user.ContrasenaHash = passwordHasher.HashPassword(user, password);

        await CreateAsync(user);
    }

    /// <summary>
    /// Actualiza los datos del usuario. La contraseña solo cambia si se envía <paramref name="newPassword"/>;
    /// en caso contrario se conserva el hash actual.
    /// </summary>
    public async Task UpdateUserAsync(UserEntity user, string? newPassword = null)
    {
        Normalize(user);

        await using var db = await DbFactory.CreateDbContextAsync();
        var current = await db.Set<UserEntity>().FindAsync(user.IdUsuario)
                      ?? throw new BusinessException("El usuario que intenta actualizar no existe.");

        current.NombreUsuario = user.NombreUsuario;
        current.Correo = user.Correo;
        current.Rol = user.Rol;
        current.Activo = user.Activo;
        current.IdEmpleado = user.IdEmpleado;

        if (!string.IsNullOrWhiteSpace(newPassword))
        {
            ValidatePassword(newPassword);
            current.ContrasenaHash = passwordHasher.HashPassword(current, newPassword);
        }

        await SaveAsync(db);
    }

    /// <summary>Elimina un usuario por su ID.</summary>
    public Task DeleteUserAsync(int id) => DeleteAsync(id);

    /// <summary>
    /// Verifica las credenciales de un usuario activo. Devuelve el usuario si son correctas o null si no.
    /// Pensado para el futuro inicio de sesión.
    /// </summary>
    public async Task<UserEntity?> ValidateCredentialsAsync(string userName, string password)
    {
        await using var db = await DbFactory.CreateDbContextAsync();
        var user = await db.Set<UserEntity>()
            .FirstOrDefaultAsync(u => u.NombreUsuario == userName.Trim() && u.Activo);

        if (user == null)
        {
            return null;
        }

        var result = passwordHasher.VerifyHashedPassword(user, user.ContrasenaHash, password);
        if (result == PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.ContrasenaHash = passwordHasher.HashPassword(user, password);
            await SaveAsync(db);
        }

        return result == PasswordVerificationResult.Failed ? null : user;
    }

    protected override void Normalize(UserEntity entity)
    {
        entity.NombreUsuario = entity.NombreUsuario.Trim();
        entity.Correo = entity.Correo.Trim().ToLowerInvariant();
        if (entity.IdEmpleado <= 0)
        {
            entity.IdEmpleado = null;
        }
    }

    private static void ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < MinPasswordLength)
        {
            throw new BusinessException($"La contraseña debe tener al menos {MinPasswordLength} caracteres.");
        }

        if (!password.Any(char.IsLetter) || !password.Any(char.IsDigit))
        {
            throw new BusinessException("La contraseña debe contener al menos una letra y un número.");
        }
    }
}
