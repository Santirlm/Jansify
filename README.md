# Jansify

Reproductor de música de escritorio para Windows, inspirado en Spotify. Desarrollado en C# con WPF siguiendo el patrón **MVVM**, con base de datos MySQL a través de Entity Framework Core.

## Funcionalidades

- **Login con roles.** Contraseñas cifradas con BCrypt y dos roles, usuario y administrador.
- **Reproductor.** Reproduce, pausa y pasa a la canción siguiente o anterior (NAudio).
- **Buscador** de canciones con filtro en tiempo real, y opción de añadir el resultado a una playlist.
- **Biblioteca.** Crear, reproducir y borrar playlists propias.
- **Informes en PDF.** Exporta una playlist a PDF con su listado de canciones, artistas, álbumes y duración (QuestPDF).
- **Panel de administración** para gestionar usuarios y sus roles.

## Tecnologías

| | |
|---|---|
| Lenguaje | C# (.NET 8) |
| Interfaz | WPF, XAML, MVVM (`BaseViewModel`, `RelayCommand`) |
| Datos | Entity Framework Core 9, Pomelo MySQL, migraciones |
| Librerías | NAudio, QuestPDF, BCrypt.Net, Xaml.Behaviors |

## Estructura

```
Jansify_Santiago_Ramirez/
├── Models/       Entidades (Usuario, Artista, Album, Cancion, Playlist) y DbContext
├── Services/     Acceso a datos, un servicio por entidad
├── Helpers/      Autenticación e informes PDF
├── ViewModels/   Lógica de cada pantalla y del reproductor
├── Views/        Vistas XAML
└── Migrations/   Migraciones de EF Core
```

## Puesta en marcha

1. Crea una base de datos MySQL.
2. Define la variable de entorno `JANSIFY_DB` con la cadena de conexión:
   ```
   setx JANSIFY_DB "Server=localhost;Database=jansify_db;User=usuario;Password=contraseña;"
   ```
3. Aplica las migraciones:
   ```
   dotnet ef database update
   ```
4. Abre `Jansify_Santiago_Ramirez.sln` en Visual Studio y ejecuta.

La primera vez se crea un administrador con usuario `Admin` y contraseña `Admin`.

---

Proyecto del ciclo DAM/DAW · **Santiago Ramírez**
