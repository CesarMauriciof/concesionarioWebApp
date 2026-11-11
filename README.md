# Concesionario WebApp

## Descripción del Proyecto

**Concesionario WebApp** es un sistema de información web diseñado para la gestión integral de un concesionario de vehículos multisede. La plataforma permite administrar el inventario de vehículos por sede, la gestión de clientes y empleados, el flujo completo de ventas, los pagos (incluyendo créditos bancarios), así como el seguimiento de garantías y mantenimientos programados.

El proyecto está desarrollado en **C# con .NET 9 (Blazor Server)**, estructurado bajo una **arquitectura en capas** (*Presentation*, *Logic*, *Model*, *Data*) y utiliza **MySQL** como motor de base de datos relacional.

## Requisitos Previos

Antes de comenzar, asegúrate de contar con los siguientes elementos instalados en tu entorno de desarrollo:

* **.NET SDK 9.0** (definido en `global.json`)
* **JetBrains Rider** (o Visual Studio / VS Code)
* **MySQL Server** (v8.0 o superior) y **MySQL Workbench**
* **Git** instalado en el sistema

## Tecnologías Utilizadas

* **Lenguaje:** C#
* **Framework:** .NET 9, Blazor Server y Entity Framework Core 9
* **Base de Datos:** MySQL
* **Conector de BD:** `Pomelo.EntityFrameworkCore.MySql` 9.0
* **Estilos:** Bootstrap 5.3 (incluido en `Presentation/wwwroot/lib`)
* **IDE Recomendado:** JetBrains Rider
* **Control de Versiones:** Git & Bitbucket (metodología **Git Flow**)
* **Gestión de Tareas:** Jira

## Modelo Relacional de Datos

El sistema está modelado a partir de las siguientes entidades principales:

1. **`sede`**: Sucursales del concesionario (`id_sede`, `nombre_sede`, `ciudad`, `direccion`).
2. **`empleado`**: Personal del concesionario asignado a una sede (`id_empleado`, `cedula`, `nombre`, `cargo`, `salario`, `id_sede`).
3. **`cliente`**: Clientes compradores (`id_cliente`, `cedula`, `nombre`, `telefono`, `correo`, `direccion`).
4. **`vehiculo`**: Inventario de vehículos vinculados a una sede (`id_vehiculo`, `vin`, `marca`, `modelo`, `precio`, `id_sede`).
5. **`venta`**: Encabezado de la transacción de venta (`id_venta`, `fecha`, `total_pagado`, `id_cliente`, `id_empleado`).
6. **`detalle_venta`**: Ítems/vehículos incluidos en cada venta (`id_detalle`, `id_venta`, `id_vehiculo`).
7. **`pago`**: Registro de pagos realizados a una venta (`id_pago`, `id_venta`, `monto`, `fecha_pago`, `metodo_pago`).
8. **`credito`**: Información sobre créditos bancarios asociados a un pago (`id_credito`, `id_pago`, `entidad_bancaria`, `monto_aprobado`, `numero_cuotas`).
9. **`garantia`**: Garantías asociadas a un detalle de venta (`id_garantia`, `id_detalle`, `fecha_inicio`, `condiciones`).
10. **`mantenimiento`**: Servicios posventa y mantenimientos asociados a una garantía (`id_mantenimiento`, `id_garantia`, `kilometraje`, `anio_mantenimiento`, `servicios_realizados`, `fecha_programada`).
11. **`usuario`**: Cuentas de acceso al sistema (`id_usuario`, `nombre_usuario`, `correo`, `contrasena_hash`, `rol`, `activo`, `fecha_creacion`, `id_empleado`). La contraseña nunca se guarda en texto plano: solo su hash (PBKDF2 con sal, `PasswordHasher` de ASP.NET Core Identity).

**Convención de nombres en la BD:** tablas y columnas en `snake_case` sin tildes ni `ñ`; índices únicos `uq_<tabla>_<columna>`, índices `ix_<tabla>_<columna>` y llaves foráneas `fk_<tabla>_<tabla_referenciada>`. En C# las propiedades usan `PascalCase` y se mapean con `[Column("...")]`.

## Estructura del Proyecto (Arquitectura en Capas)

La solución `ConcesionarioWebApp` sigue una separación de responsabilidades clara:

```text
ConcesionarioWebApp/
├── Model/          # Entidades (ClientEntity, VehicleEntity, UserEntity...) con DataAnnotations
├── Data/           # DataContext (EF Core), migraciones y scripts SQL
├── Logic/          # Clases *Log con la lógica de negocio, BaseLog (CRUD común) y registro de DI
└── Presentation/   # Blazor Server: páginas (Pages/), componentes por módulo y Program.cs
```

Las rutas de la interfaz siguen la convención `/<modulo>/list`, `/<modulo>/create` y `/<modulo>/update/{id}`
(por ejemplo `/vehicle/list`, `/user/create`).

## Instalación y Configuración

### 1. Clonar el repositorio

```bash
git clone git@bitbucket.org:1152471325/concesionariowebapp.git
cd concesionariowebapp/ConcesionarioWebApp
dotnet tool restore     # instala dotnet-ef 9.0 (definido en .config/dotnet-tools.json)
```

### 2. Configurar la cadena de conexión (sin subir la contraseña)

`Presentation/appsettings.json` trae la cadena **sin contraseña real**. Cada integrante guarda la suya con
*user-secrets*, que se almacena fuera del repositorio:

```bash
dotnet user-secrets set "ConnectionStrings:Concesionario" "Server=localhost;Port=3306;Database=concesionario;User=root;Password=TU_CONTRASEÑA;" --project Presentation
```

En producción se usa la variable de entorno `ConnectionStrings__Concesionario`.

### 3. Crear o actualizar la base de datos

**Opción A – BD nueva (vacía):** EF Core crea todas las tablas:

```bash
dotnet ef database update --project Data --startup-project Presentation
```

**Opción B – BD existente con datos (creada desde MySQL Workbench):**

1. Haga un respaldo: `mysqldump -u root -p concesionario > respaldo.sql`
2. Ejecute en orden `Data/Scripts/01_alinear_esquema.sql` y `Data/Scripts/02_ajustar_esquema_y_registrar_migracion.sql`
   (el script 02 muestra primero una verificación: todas las filas deben indicar `0` nulos).
3. Aplique las migraciones pendientes (crea la tabla `usuario`):

```bash
dotnet ef database update --project Data --startup-project Presentation
```

Para agregar cambios al modelo en el futuro:

```bash
dotnet ef migrations add NombreDelCambio --project Data --startup-project Presentation
```

### 4. Iniciar la aplicación

```bash
dotnet run --project Presentation
```

## Ejemplos de Funcionamiento

### 1. Registro de Vehículos por Sede
Permite dar de alta un vehículo asociándolo a su correspondiente sede física mediante su número de identificación único (`VIN`), marca, modelo y precio de venta.

### 2. Proceso de Venta y Pagos
* Se registra la venta vinculando al `cliente` comprador y al `empleado` asesor.
* Se adjuntan uno o más vehículos mediante `detalle_venta`.
* Se efectúan los registros de `pago` (efectivo, transferencia o mediante financiación en `credito` con entidad bancaria y cuotas).

### 3. Posventa: Garantía y Mantenimiento
Una vez vendido el vehículo, se genera la `garantia` asociada al `detalle_venta`, lo que habilita la programación periódica de `mantenimiento` preventivo o correctivo especificando el kilometraje y los servicios a realizar.

### 4. Flujo de Trabajo (Git Flow)
Todas las características se desarrollan en ramas `feature/` nombradas según las tareas registradas en **Jira**:

```bash
git checkout develop
git checkout -b feature/CON-101-crear-entidad-vehiculo
# ... desarrollo y pruebas ...
git commit -m "feat: se crea la entidad Vehiculo y configuracion en DataContext"
git push origin feature/CON-101-crear-entidad-vehiculo
```

## Convención de Commits

* `feat:` Nueva funcionalidad para el sistema.
* `fix:` Corrección de errores.
* `docs:` Cambios en la documentación (ej. `README.md`).
* `style:` Formateo o cambios cosméticos de código sin alterar lógica.
* `refactor:` Reestructuración de código existente.

## Autores

* **Keiner Alex Buitron Pareja** 
* **Cesar Mauricio Florez Ruiz**  
* **Camilo Serna** 
* **Adriana Pacheco Canchila** 
* **Yeison Andres Anaya Camacho** 

## Licencia

Este proyecto se distribuye bajo la licencia **MIT**. Consulta el archivo `LICENSE` para más información.