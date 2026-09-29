using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jansify_Santiago_Ramirez.Models
{
    // Clase Artista la cual identifica cada uno con biografia y imagen
    public class Artista
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        public string Biografia { get; set; } = string.Empty;

        public string ImagenUrl { get; set; } = string.Empty;

        [InverseProperty("Artista")]
        public virtual ICollection<Album> Albums { get; set; } = new List<Album>();
    }
}