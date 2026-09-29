using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jansify_Santiago_Ramirez.Models
{
    // Clase la cual se encarga de gestionar los almbunes de los artistas
    public class Album
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Titulo { get; set; } = string.Empty;

        public int Ano { get; set; }

        public string PortadaUrl { get; set; } = string.Empty;

        public int ArtistaId { get; set; }

        [ForeignKey("ArtistaId")]
        public virtual Artista Artista { get; set; } = null!;

        [InverseProperty("Album")]
        public virtual ICollection<Cancion> Canciones { get; set; } = new List<Cancion>();
    }
}