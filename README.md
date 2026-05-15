# Sistema de gestión El Fogón 🍗

> Sistema web que que gestiona las ventas, inventario, compras, caja, usuarios, reportes para el funcionamiento integral del negocio.

## 📸 Capturas de Pantalla
<img width="1359" height="552" alt="image" src="https://github.com/user-attachments/assets/c7727c01-64d9-4457-9878-11a2ff485642" />

## ✨ Funcionalidades

- [ ✅ ] **Autenticación y Roles** — Login con BCrypt, roles con permisos por formulario/web
- [ ] **Ventas** — Registro, edición y anulación de ventas con detalle
- [ ] **Caja** — Apertura/cierre de turno, gastos operativos, ingresos y egresos manuales
- [ ] **Inventario** — Insumos, stock automático por venta/compra, movimientos
- [ ] **Compras** — Registro de compras a proveedores con actualización de stock
- [ ] **Productos** — Productos con receta de insumos y stock calculado
- [ ] **Clientes** — Clientes normales y comerciantes (local + pasillo)
- [ ] **Dashboard** — Resumen de ventas del mes, top productos, gráfico semanal

---

## 🛠️ Stack Tecnológico

| Capa | Tecnología |
|------|------------|
| Frontend | ASP.NET Core Razor Pages + Tailwind CSS |
| Backend | C# / ASP.NET Core |
| Base de Datos | SQL Server |
| ORM / Acceso a datos | ADO.NET + Stored Procedures |
| Autenticación | BCrypt.Net |
| Estilos | Tailwind CSS v4 |

---

## 🗂️ Estructura del Proyecto

```
CapaDatos/
├── CD_Conexion.cs
├── CD_Usuario.cs
├── CD_Rol.cs
├── CD_Permiso.cs
└── CD_RolPermiso.cs
CapaNegocio/
├── CN_Usuario.cs
├── CN_Rol.cs
├── CN_Permiso.cs
└── CN_RolPermiso.cs
Entidades/
├── Usuario.cs
├── Rol.cs
├── Permiso.cs
├── RolPermiso.cs
└── MenuItem.cs
CapaPresentacion/
├── Filters/
│   └── PermisoRequeridoAttribute.cs
├── wwwroot/
│   ├── css/
│   └── js/
├── Pages/
|   |   ├── Shared/
│   |   └── _Layout.cshtml
│   ├── Acceso/
│   │   ├── Login.cshtml
│   │   ├── AccesoDenegado.cshtml
│   │   └── Logout.cshtml
|   ├── Ventas/
│   ├── Caja/
│   ├── Productos/
│   ├── Insumos/
│   ├── Compras/
│   ├── Clientes/
│   ├── Usuarios/
│   ├── Rol/
│   └── ...
└── appsettings.json
```

## ⚙️ Instalación y Configuración

### Requisitos previos

- .NET 8 SDK o superior
- SQL Server
- Node.js + npm (para Tailwind)

### 1. Clonar el repositorio

```bash
git clone https://github.com/tu-usuario/sigef.git
cd sigef
```

### 2. Configurar la cadena de conexión

En `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=BDElFogon;Trusted_Connection=True;"
}
```

### 3. Crear la base de datos

Ejecutar en SQL Server en este orden:
1. `Tablas.sql`
2. `InserccionDeDatos.sql`
3. `Procedimientos.sql`

### 4. Instalar dependencias de Tailwind y compilar CSS

```bash
npm install
npm run css:build
```

### 5. Correr el proyecto

```bash
dotnet run
```

---

## 🗃️ Base de Datos — Diagrama
<img width="4338" height="2686" alt="DiagramaElFogon" src="https://github.com/user-attachments/assets/071dfc32-f37d-421b-96a0-838c0c84e259" />
