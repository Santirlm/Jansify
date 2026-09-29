using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jansify_Santiago_Ramirez.Models
{
    // Clase playlist la cual es creada por el usuario y esta relacionada con canciones
    public class Playlist
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; } = null!;

        // N:M con Cancion
        [InverseProperty("Playlists")]
        public virtual ICollection<Cancion> Canciones { get; set; } = new List<Cancion>();
    }
}