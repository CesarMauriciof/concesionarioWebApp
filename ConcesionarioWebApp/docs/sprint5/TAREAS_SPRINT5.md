# Sprint 5 – Autenticación y autorización con OAuth 2.0

**Objetivo:** que solo los usuarios registrados en el sistema puedan entrar, iniciando sesión con su cuenta
de **Google** (OAuth 2.0 / OpenID Connect), y que cada uno vea solo lo que su **rol** permite.

## 1. Cómo funciona (léanlo todos antes de empezar)

```
 Navegador                 Presentation                    Logic                 Data (MySQL)
 ─────────                 ────────────                    ─────                 ────────────
 /login  ──clic Google──▶  GET /auth/login/google  ──▶  Google (OAuth 2.0)
                                                          │ usuario acepta
 ◀───────────────────────  /signin-google  ◀──────────────┘  (cookie TEMPORAL "External")
                           GET /auth/callback
                             lee claims de Google ──▶  AuthLog.LoginWithExternalProviderAsync
                                                         ¿correo registrado y activo? ──▶ usuario
                                                         vincula cuenta Google       ──▶ usuario_login_externo
                                                         actualiza último acceso     ──▶ usuario.ultimo_acceso
                             crea cookie DEFINITIVA ◀── AuthenticatedUser (id, nombre, correo, ROL)
 ◀── redirige a la página ──  (claims: Name, Email, Role)
 Cerrar sesión ──POST──▶   /auth/logout  → borra cookie → /login?logout=1
```

Reglas clave:

- **No hay auto-registro.** El administrador crea el usuario en `/user/create` con el correo de Google de
  la persona. Si alguien entra con una cuenta de Google que no está registrada, ve *"Su cuenta no está
  autorizada"*.
- **El rol** (`Administrador`, `Gerente`, `Vendedor`, `Tecnico`) viaja como claim y se usa en
  `[Authorize(Roles = "...")]`.
- **Ningún secreto en el código:** el ClientId y el ClientSecret de Google van en *user-secrets*.
- El login y el logout son **endpoints HTTP**, no código dentro de componentes: la app es
  InteractiveServer (SignalR) y desde ahí no se pueden escribir cookies.

## 2. Estructura que ya está lista (rama `feature-estructura-autenticacion-oauth`) – Adriana

**Cómo se trabaja:** cada integrante recibe por privado un mensaje con el **código completo** de sus
archivos. Los archivos ya existen en el proyecto con el aviso **`PEGAR AQUÍ - T# (Nombre)`**: se borra todo
su contenido y se pega el código del mensaje. Nadie necesita tocar `Program.cs`.

| Archivo | Responsable |
|---|---|
| `Presentation/Program.cs`, `Logic/DependencyInjection.cs`, `_Imports.razor`, `MainLayout.razor` | ✅ Listos (Adriana) |
| `Logic/Auth/AuthenticatedUser.cs`, `Logic/Auth/ExternalLoginData.cs`, `Model/ExternalLoginEntity.cs` | ✅ Listos (contratos) |
| `Model/UserEntity.cs`, `Data/DataContext.cs`, `Data/Scripts/03_crear_administrador_inicial.sql` + migración | Keiner (T3) |
| `Presentation/Auth/AuthenticationSetup.cs` | Mauricio (T2) |
| `Logic/Auth/AuthLog.cs`, `Presentation/Auth/UserClaims.cs`, `Presentation/Auth/AuthEndpoints.cs` | Yeison (T4) |
| `Pages/Auth/Login.razor`, `Layout/LoginLayout.razor`, `Pages/Auth/AccessDenied.razor`, `Shared/RedirectToLogin.razor` | Camilo (T5) |
| `Presentation/Auth/LogoutEndpoint.cs`, `Layout/LoginDisplay.razor` | Mauricio (T6) |
| `Routes.razor`, `_Imports.razor`, `NavMenu.razor`, `Pages/Home.razor`, `Pages/Error.razor`, `Pages/User/*` | Adriana (T7) |

Cada archivo tiene **un solo responsable**: así no hay conflictos al hacer merge.
**Importante:** cada uno pega su código en su turno (sección 4), después de `git pull origin develop`,
porque algunos archivos usan lo que pegaron los anteriores.

## 3. Tareas para Jira

> Mauricio (T1) crea las tareas en Jira y **las ramas de cada uno** (desde `develop`, después de que
> Adriana mergee la estructura). Cada integrante solo hace `git fetch origin` y `git checkout <su-rama>`.

### T1 – Crear las tareas en Jira y las ramas feature · **Mauricio**
- Crear en Jira las tareas T1–T7 de este documento y asignarlas.
- Crear las ramas `feature-...` de cada tarea desde `develop` (después de mergear la estructura).
- **Criterio de aceptación:** tareas creadas y asignadas en Jira; las 6 ramas existen en Bitbucket.

> La estructura base del módulo (rama `feature-estructura-autenticacion-oauth`) la hizo Adriana,
> que además revisa y aprueba los pull requests en el orden de la sección 4.

### T2 – Configurar el proveedor OAuth 2.0 (Google) · **Mauricio**
Rama: `feature-configurar-proveedor-oauth`
1. Entrar a <https://console.cloud.google.com/> con una cuenta de Google y crear el proyecto
   **"Concesionario WebApp"**.
2. **APIs y servicios → Pantalla de consentimiento OAuth**: tipo *Externo*, nombre de la app, correo de
   soporte; agregar como **usuarios de prueba** los correos de los 5 integrantes y del profesor.
3. **Credenciales → Crear credenciales → ID de cliente de OAuth → Aplicación web**:
   - *Orígenes de JavaScript autorizados:* `http://localhost:5123` y `https://localhost:7137`
   - *URI de redireccionamiento autorizados:* `http://localhost:5123/signin-google` y
     `https://localhost:7137/signin-google`
4. Guardar ClientId y ClientSecret con user-secrets (y pasárselos al equipo **por privado**, nunca por Git):
   ```bash
   dotnet user-secrets set "Authentication:Google:ClientId" "xxxx.apps.googleusercontent.com" --project Presentation
   dotnet user-secrets set "Authentication:Google:ClientSecret" "GOCSPX-xxxx" --project Presentation
   ```
5. Completar el TODO de `Presentation/Auth/AuthenticationSetup.cs` (`AddGoogle` con `SignInScheme = ExternalScheme`).
- **Criterio de aceptación:** con los secretos configurados la app arranca; sin ellos, muestra un error claro
  de configuración.

### T3 – Persistencia y validación de usuarios en la capa Data · **Keiner**
Rama: `feature-persistencia-usuarios-data`
1. `Model/UserEntity.cs`: agregar `UltimoAcceso` (`ultimo_acceso`, datetime, opcional) y la colección `LoginsExternos`.
2. `Data/DataContext.cs`: registrar `ExternalLoginEntity` (DbSet + configuración del TODO: índice único
   `(proveedor, clave_proveedor)`, FK a usuario con cascada, nombres de índices/FK con la convención del proyecto).
3. Crear la migración y aplicarla:
   ```bash
   dotnet ef migrations add AgregarLoginExterno --project Data --startup-project Presentation
   dotnet ef database update --project Data --startup-project Presentation
   ```
4. Completar `Data/Scripts/03_crear_administrador_inicial.sql` (idempotente, correo como variable).
5. Regenerar el `concesionario.sql` compartido con la tabla nueva (¡revisar que `__EFMigrationsHistory`
   quede con mayúsculas!).
- **Criterio de aceptación:** `dotnet ef migrations has-pending-model-changes` dice que no hay cambios;
  no se puede insertar dos veces la misma cuenta de Google; al borrar un usuario se borran sus vínculos.

### T4 – Proceso de autenticación en la capa Logic · **Yeison**
Rama: `feature-proceso-autenticacion-logic` · **Depende de T3** (puede empezar con el contrato).
1. `Logic/Auth/AuthLog.cs`: implementar `LoginWithExternalProviderAsync` y `LoginWithPasswordAsync`
   siguiendo las 5 reglas escritas en el archivo.
2. `Presentation/Auth/UserClaims.cs`: crear el `ClaimsPrincipal` (Id, Name, Email, Role).
3. `Presentation/Auth/AuthEndpoints.cs`: `/auth/login/{provider}`, `/auth/callback`, `/auth/login-local`.
   **Validar que `returnUrl` sea una ruta local** (evita *open redirect*).
- **Criterio de aceptación:** usuario registrado y activo entra; no registrado → mensaje *"Su cuenta no está
  autorizada"*; inactivo → *"Su usuario está inactivo"*; se guarda `ultimo_acceso`; el segundo ingreso usa
  el vínculo aunque el correo haya cambiado.

### T5 – Formulario de inicio de sesión en la capa Presentation · **Camilo**
Rama: `feature-formulario-inicio-sesion`
1. `Pages/Auth/Login.razor`: botón *"Iniciar sesión con Google"*, formulario usuario/contraseña
   (`POST /auth/login-local`, campos `usuario`, `contrasena`, `returnUrl` y `<AntiforgeryToken />`),
   mensajes de error y de sesión cerrada.
2. `Layout/LoginLayout.razor`: diseño de la pantalla de login (tarjeta centrada, Bootstrap 5.3).
3. `Pages/Auth/AccessDenied.razor` y `Shared/RedirectToLogin.razor` (con `returnUrl`).
- **Criterio de aceptación:** se ve bien en celular y PC; los errores que mandan los endpoints se muestran;
  después de iniciar sesión se vuelve a la página que se intentaba abrir.

### T6 – Cierre de sesión (Logout) · **Mauricio**
Rama: `feature-cierre-de-sesion`
1. `Presentation/Auth/LogoutEndpoint.cs`: `SignOutAsync` y redirección a `/login?logout=1`.
2. `Layout/LoginDisplay.razor`: nombre y rol del usuario + botón *Cerrar sesión* (formulario **POST** con
   `<AntiforgeryToken />`, no un enlace).
- **Criterio de aceptación:** al cerrar sesión la cookie desaparece y al presionar *Atrás* en el navegador
  no se puede volver a ver información protegida.

### T7 – Autorización por roles · **Adriana**
Rama: `feature-autorizacion-por-roles` · **Va de último** (depende de T4 y T5).
1. `Routes.razor`: `AuthorizeRouteView` + `<RedirectToLogin />`.
2. `_Imports.razor`: `@attribute [Authorize]` (todo exige sesión salvo Login y AccessDenied).
3. `Pages/User/*`: `[Authorize(Roles = "Administrador")]`.
4. `NavMenu.razor`: ocultar *Usuarios* a quien no sea Administrador.
- **Criterio de aceptación:** sin sesión cualquier URL lleva al login; un Vendedor que abre `/user/list`
  ve *Acceso denegado*; el Administrador ve todo.

## 4. Orden de merge a `develop`

```
T1 estructura ──▶ T3 Data (Keiner) ──▶ T2 Google (Mauricio) ──▶ T4 Logic+endpoints (Yeison)
                                                             ──▶ T5 Login (Camilo) ──▶ T6 Logout (Mauricio)
                                                                                    ──▶ T7 Roles (Adriana)
```

- Cada uno pega su código **en su turno**, después de que el anterior haya hecho merge
  (`git pull origin develop` en su rama antes de pegar), porque algunos archivos dependen de los anteriores.
- Antes de abrir el pull request: `git pull origin develop` en su rama, compilar y probar.
- Commits con la convención del equipo: `feat:` / `fix:` / `docs:`.

## 5. Pruebas de aceptación del sprint (las hace Adriana al final)

1. Abrir `http://localhost:5123/sale/list` sin sesión → redirige al login con `returnUrl`.
2. Entrar con Google (usuario registrado y activo) → vuelve a `/sale/list` y arriba aparece su nombre y rol.
3. Entrar con un Google **no registrado** → mensaje *"Su cuenta no está autorizada"* y no entra.
4. Desactivar el usuario en `/user/update` → al volver a entrar, *"Su usuario está inactivo"*.
5. Usuario Vendedor abre `/user/list` → *Acceso denegado*; no ve *Usuarios* en el menú.
6. Cerrar sesión → vuelve al login con *"Sesión cerrada"*; *Atrás* no muestra datos.
7. Revisar con F12 → Application → Cookies: `Concesionario.Auth` es **HttpOnly**.
8. `git grep -i "GOCSPX\|client_secret"` no encuentra secretos en el repositorio.
