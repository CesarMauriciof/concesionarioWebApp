# Documento técnico – Módulo de autenticación y autorización (OAuth 2.0)

> **Anexo del documento técnico de Concesionario WebApp – Sprint 5.**
> Consolida: **Keiner (T8)**. Cada integrante completa la sección de su tarea.
> ⚠️ No incluir ClientSecret, contraseñas ni cadenas de conexión reales; en las capturas, taparlos.

## 1. Introducción
<!-- Keiner: objetivo del módulo, alcance del sprint y por qué se eligió OAuth 2.0 con Google. -->

## 2. Conceptos
<!-- Keiner: explicar en pocas líneas OAuth 2.0, OpenID Connect, autenticación vs. autorización,
     claims, cookie de sesión y flujo "Authorization Code". -->

## 3. Arquitectura del módulo
<!-- Keiner: diagrama del flujo (puede partir del de TAREAS_SPRINT5.md, sección 1) y tabla de qué
     hace cada capa (Presentation, Logic, Data, Model). -->

## 4. Configuración del proveedor OAuth 2.0 (Google) – *Mauricio (T2)*
<!-- Paso a paso en Google Cloud Console con capturas: proyecto, pantalla de consentimiento,
     usuarios de prueba, credenciales, URIs de redirección. Cómo se guardan ClientId/ClientSecret
     con user-secrets y por qué no van en appsettings.json ni en Git. -->

## 5. Persistencia y validación de usuarios (capa Data) – *Keiner (T3)*
<!-- Modelo de la tabla usuario_login_externo y la columna ultimo_acceso (diagrama E-R),
     índices y restricciones, migración AgregarLoginExterno, script del administrador inicial. -->

## 6. Proceso de autenticación (capa Logic) – *Yeison (T4)*
<!-- Reglas de negocio de AuthLog, vinculación de cuentas, claims que se generan,
     endpoints /auth/login/{provider}, /auth/callback y /auth/login-local, validación de returnUrl. -->

## 7. Formulario de inicio de sesión (capa Presentation) – *Camilo (T5)*
<!-- Capturas de la pantalla de login (PC y celular), mensajes de error, redirección con returnUrl,
     página de acceso denegado. -->

## 8. Cierre de sesión – *Mauricio (T6)*
<!-- Cómo funciona el logout, por qué es POST con antiforgery, capturas. -->

## 9. Autorización por roles – *Adriana (T7)*
<!-- Tabla rol → páginas permitidas, uso de [Authorize] y AuthorizeView, comportamiento sin sesión. -->

## 10. Seguridad
<!-- Keiner con aportes de todos: cookie HttpOnly/SameSite, secretos en user-secrets, sin auto-registro,
     protección open redirect, antiforgery, contraseñas con hash PBKDF2, mensajes de error que no
     revelan si existe el usuario. -->

## 11. Pruebas realizadas
<!-- Adriana: resultado de las 8 pruebas de aceptación de TAREAS_SPRINT5.md (sección 5) con capturas. -->

## 12. Instalación para un nuevo integrante
<!-- Pasos: git pull, user-secrets (BD + Google), dotnet ef database update, script 03, correr la app. -->

## 13. Referencias
- Autenticación con Google en ASP.NET Core: <https://learn.microsoft.com/aspnet/core/security/authentication/social/google-logins>
- Autenticación y autorización en Blazor: <https://learn.microsoft.com/aspnet/core/blazor/security/>
- Google Identity – OAuth 2.0: <https://developers.google.com/identity/protocols/oauth2>
