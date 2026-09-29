using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jansify_Santiago_Ramirez.Models
{
    // Clase usuario literalmente como cualquier clase User
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Rol { get; set; } = "USER";

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [InverseProperty("Usuario")]
        public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
    }
}