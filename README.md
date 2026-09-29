# Jansify

Reproductor de música de escritorio en WPF (.NET 8) con patrón MVVM: usuarios con login y rol de administrador, artistas, álbumes, canciones, playlists, biblioteca, búsqueda e informes en PDF.

**Tecnologías:** C#, WPF, Entity Framework Core (Pomelo MySQL), NAudio, QuestPDF, BCrypt.

## Puesta en marcha

1. Crea una base de datos MySQL.
2. Define la variable de entorno `JANSIFY_DB` con la cadena de conexión:
   ```
   setx JANSIFY_DB "Server=localhost;Database=jansify_db;User=usuario;Password=contraseña;"
   ```
3. Aplica las migraciones: `dotnet ef database update`
4. Abre `Jansify_Santiago_Ramirez.sln` en Visual Studio y ejecuta.
