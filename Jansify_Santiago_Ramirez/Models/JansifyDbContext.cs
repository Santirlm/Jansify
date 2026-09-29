using Microsoft.EntityFrameworkCore;

namespace Jansify_Santiago_Ramirez.Models
{
    public class JansifyDbContext : DbContext
    {
        // Declaracion necesaria de todos los modelos para crear las tablas
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Artista> Artistas { get; set; }
        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<Cancion> Canciones { get; set; }
        public virtual DbSet<Playlist> Playlists { get; set; }

        public JansifyDbContext() { }

        // Conexion con el vps 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // La cadena de conexión no va en el repositorio: se lee de la variable de entorno JANSIFY_DB
            var connectionString = Environment.GetEnvironmentVariable("JANSIFY_DB")
                ?? throw new InvalidOperationException("Falta la variable de entorno JANSIFY_DB con la cadena de conexión a MySQL.");
            optionsBuilder.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString)
            );
        }
        // M:N como en el ejemplo que nos diste

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Playlist>()
                .HasMany(p => p.Canciones)
                .WithMany(c => c.Playlists)
                .UsingEntity(j => j.ToTable("PlaylistCanciones"));

            base.OnModelCreating(modelBuilder);
        }
    }
}