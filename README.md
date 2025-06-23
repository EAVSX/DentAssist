# 🦷 DentAssist

---

Plataforma de gestión clínica dental que simplifica la agenda diaria, el historial de pacientes y la administración de tratamientos.
---

## Características
- Agenda inteligente y vista semanal de turnos  
- Gestión de pacientes con historial clínico  
- Planes de tratamiento con seguimiento paso a paso  
- Exportación de planes a PDF (DinkToPdf)  
- Sistema de roles y autenticación

---

## Requisitos

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [MySQL](https://dev.mysql.com/downloads/mysql/) o [MariaDB](https://mariadb.org/)
* Visual Studio 2022 (o Visual Studio Code)
* Git (opcional, para clonar el repositorio)

---

## Instalación

### 1 · Clona el repositorio

```bash
git clone https://github.com/eavsx/DentAssist.git
cd DentAssist
```

### 2 · Configura la base de datos

1. Descarga y ejecuta el script SQL para crear la base `DentAssistDb` y sus tablas.
2. Ajusta la cadena de conexión en **`appsettings.json`** con tus credenciales:

   ```json
   "ConnectionStrings": {
       "DefaultConnection": "server=localhost;port=3306;database=DentAssistDb;user=root;password=admin;"
   }
   ```

### 3 · Restaura las dependencias

```bash
dotnet restore
```

### 4 · Ejecuta la aplicación

```bash
dotnet run
```

La aplicación estará disponible en (https://localhost:7157/) (o el puerto asignado por Kestrel).

---

## Primer acceso

| Rol           | Email                                               | Contraseña |
| ------------- | --------------------------------------------------- | ---------- |
| Administrador | [admin@dentassist.com](mailto:admin@dentassist.com) | Admin123!  |
| Odontologo    | [Deer@dentassist.cl](mailto:Deer@dentassist.com)    | Admin123!  |
| Recepcionista | [Maria.a@dentassist.cl](mailto:Deer@dentassist.com) | Admin123!  |

---

## Generación de PDFs

El sistema usa **DinkToPdf** con la librería nativa `libwkhtmltox.dll` incluida en `runtimes/`.
En Windows no se requiere instalación adicional; para Linux/macOS debes sustituir la librería por la versión correspondiente.

---

## Estructura del proyecto

```
DentAssist/
├── Controllers/     Lógica MVC
├── Datos/           DbContext (DentAssistContext)
├── Helpers/         Utilidades (PDF, Razor → string)
├── Models/          Entidades y ViewModels
├── Views/           Vistas Razor
├── wwwroot/         Recursos estáticos (CSS, JS)
└── Program.cs       Configuración y arranque
```

---

## Notas adicionales

* **Despliegue en producción:** cambia la contraseña del usuario administrador.
* **Restaurar paquetes:** ejecuta `dotnet restore` si aparecen errores de dependencias.
* **Soporte multiplataforma:** usa la versión correcta de `wkhtmltopdf` para Linux/macOS cuando generes PDF.

---

## Capturas de pantalla

![Página de inicio](https://github.com/user-attachments/assets/fd031c35-6cb1-47ce-b954-2f9a8393ece6)

Página de Inicio:
Vista pública con llamado a la acción, registro y sell points de la plataforma.

---
![Inicio de sesión](https://github.com/user-attachments/assets/9fce4f61-2fea-4403-b901-df6651ba3d55)

Inicio de Sesión:
Formulario minimalista de acceso con opción “Recordarme”.

---

![Panel de administración](https://github.com/user-attachments/assets/075f1c48-bef1-4ca0-bbf3-4bd4e3b0da1b)

Panel del Administrador:
Accesos directos para gestionar Odontólogos, Recepcionistas, Tratamientos y Configuración General.

---
![Panel de recepcionista](https://github.com/user-attachments/assets/aabb7ad8-b199-4915-bbcf-268a9d4fa03b)

Panel de Recepcionista:
Resumen de turnos y pacientes con botones a las secciones clave.

---
![panelOdontologo](https://github.com/user-attachments/assets/34c61f9b-734a-4be6-826d-610b56aa5424)

Panel del Odontólogo:
Acceso a Mi Agenda y Planes de Tratamiento asignados.

---
![AgendaOdontologo](https://github.com/user-attachments/assets/3fd36365-47f8-47db-a512-2673ead18cf2)

Agenda Semanal del Odontólogo:
Tarjetas por día con hora, paciente, estado y observaciones.
---
![Planes de tratamiento](https://github.com/user-attachments/assets/f456c5cb-c4e1-4895-851a-40ba16969af2)

Listado de Planes de Tratamiento:
Tabla con acciones de detalle y eliminación.
---
![Nuevo Plan](https://github.com/user-attachments/assets/f98151e7-65bb-470e-85ef-ce436b1b54fc)

Formulario “Nuevo Plan de Tratamiento”:
Selección de paciente, tratamiento base y observaciones.

---
![Detalle Plan](https://github.com/user-attachments/assets/d10fb905-06f5-412b-b79e-1b2fd5426aec)

Detalle de un Plan de Tratamiento:
Barra de progreso y tabla de Pasos con actualización de estado individual y exportación a PDF.



---
## Licencia

Proyecto de uso académico; puede adaptarse libremente según las necesidades de la institución.

---

## Autor

**Gabriel Montoya – Rodrigo Araya - Eugenio Vergara - 2025**
