using Jansify_Santiago_Ramirez.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jansify_Santiago_Ramirez.Services
{
    public class ServiceUsuario : IDisposable
    {
        bool disposed;

        public ServiceUsuario() { disposed = false; }

        // Método que lista todos los usuarios del sistema
        public async Task<List<Usuario>> Listar()
        {
            using (var _context = new JansifyDbContext())
            {
                return await _context.Usuarios
                    .AsNoTracking()
                    .OrderBy(u => u.Id)
                    .ToListAsync();
            }
        }

        // Método que busca y devuelve un usuario por su ID
        public async Task<Usuario?> Listar(int id)
        {
            using (var _context = new JansifyDbContext())
            {
                return await _context.Usuarios
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == id);
            }
        }

        // Método que inserta un nuevo usuario
        public async Task<Usuario> Insertar(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario));
            }
            using (var _context = new JansifyDbContext())
            {
                // hashea la contraseña antes de guardar
                usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuario.PasswordHash);
                usuario.FechaRegistro = DateTime.Now;
                await _context.Usuarios.AddAsync(usuario);
                await _context.SaveChangesAsync();
                return usuario;
            }
        }

        // Método que actualiza los datos de un usuario existente
        public async Task<bool> Actualizar(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario)); 
            }
            using (var _context = new JansifyDbContext())
            {
                var existing = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == usuario.Id);
                if (existing is null)
                {
                    return false;
                }
                existing.NombreUsuario = usuario.NombreUsuario;
                existing.Email = usuario.Email;
                existing.Rol = usuario.Rol;
                await _context.SaveChangesAsync();
                return true;
            }
        }

        // Método que borra un usuario por su ID
        public async Task<bool> Borrar(int id)
        {
            using (var _context = new JansifyDbContext())
            {
                var entity = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
                if (entity is null)
                {
                    return false;
                }
                _context.Usuarios.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        // Métodos para la liberación de recursos
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return; disposed = true;
            }
        }
        ~ServiceUsuario()
        {
            Dispose(false);
        }
    }
}