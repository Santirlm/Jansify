using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jansify_Santiago_Ramirez.Models
{
    // Cancion la cual esta relacionada con album y playlist
    public class Cancion
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Titulo { get; set; } = string.Empty;

        public TimeSpan Duracion { get; set; }

        public string ArchivoUrl { get; set; } = string.Empty;

        public int AlbumId { get; set; }

        [ForeignKey("AlbumId")]
        public virtual Album Album { get; set; } = null!;

        // N:M con Playlist
        [InverseProperty("Canciones")]
        public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
    }
}