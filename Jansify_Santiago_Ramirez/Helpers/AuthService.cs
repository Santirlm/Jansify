using Jansify_Santiago_Ramirez.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Jansify_Santiago_Ramirez.Helpers
{   
    // Clase de ayuda la cual me permita gestionar parte de la ui y cosas relacionadas aparte de gestionar el user pedido en el enunciado
    public class AuthService
    {
        private static Usuario? _usuarioActivo;
        public static Usuario? UsuarioActivo => _usuarioActivo;
        public static bool EsAdmin() => _usuarioActivo?.Rol == "ADMIN";
        public static async Task<bool> Login(string nombreUsuario, string password)
        {
            using (var _context = new JansifyDbContext())
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);

                if (usuario == null)
                {
                    return false;
                }

                bool valido = BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash);
                if (valido)
                {
                    _usuarioActivo = usuario;
                }
                return valido;
            }
        }

        public static void Logout()
        {
            _usuarioActivo = null;
        }

        // garantiza que siempre exista Admin/Admin en la bd
        public static async Task SeedAdmin()
        {
            using (var _context = new JansifyDbContext())
            {
                var admin = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.NombreUsuario == "Admin");

                if (admin == null)
                {
                    await _context.Usuarios.AddAsync(new Usuario
                    {
                        NombreUsuario = "Admin",
                        Email = "admin@jansify.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin"),
                        Rol = "ADMIN"
                    });
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}