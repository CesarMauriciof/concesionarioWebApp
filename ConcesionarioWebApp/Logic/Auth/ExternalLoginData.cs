namespace Logic.Auth;

/// <summary>
/// Datos que devuelve el proveedor OAuth 2.0 (Google) después de que el usuario se autentica.
/// Presentation los lee de los claims externos y se los pasa a <see cref="AuthLog"/>.
/// </summary>
/// <param name="Proveedor">Nombre del proveedor, p. ej. "Google".</param>
/// <param name="ClaveProveedor">ID único del usuario en el proveedor (claim "sub" / NameIdentifier). No cambia nunca.</param>
/// <param name="Correo">Correo verificado que entrega el proveedor.</param>
/// <param name="Nombre">Nombre para mostrar que entrega el proveedor (puede venir vacío).</param>
public sealed record ExternalLoginData(string Proveedor, string ClaveProveedor, string Correo, string? Nombre);
