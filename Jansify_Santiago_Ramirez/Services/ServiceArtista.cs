using Jansify_Santiago_Ramirez.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jansify_Santiago_Ramirez.Services
{
    public class ServiceArtista : IDisposable
    {
        bool disposed;

        public ServiceArtista() { disposed = false; }

        // Método que lista artistas ordenados por nombre
        public async Task<List<Artista>> Listar()
        {
            using (var _context = new JansifyDbContext())
            {
                return await _context.Artistas
                    .AsNoTracking()
                    .OrderBy(a => a.Nombre)
                    .ToListAsync();
            }
        }

        // Método que inserta un nuevo artista
        public async Task<Artista> Insertar(Artista artista)
        {
            if (artista == null) throw new ArgumentNullException(nameof(artista));
            using (var _context = new JansifyDbContext())
            {
                await _context.Artistas.AddAsync(artista);
                await _context.SaveChangesAsync();
                return artista;
            }
        }

        // Método que actualiza la información de un artista existente
        public async Task<bool> Actualizar(Artista artista)
        {
            if (artista == null)
            {
                throw new ArgumentNullException(nameof(artista));
            }
            using (var _context = new JansifyDbContext())
            {
                var existing = await _context.Artistas.FirstOrDefaultAsync(a => a.Id == artista.Id);
                if (existing is null)
                {
                    return false;

                }
                existing.Nombre = artista.Nombre;
                existing.Biografia = artista.Biografia;
                existing.ImagenUrl = artista.ImagenUrl;
                await _context.SaveChangesAsync();
                return true;
            }
        }

        // Método que elimina un artista por su ID
        public async Task<bool> Borrar(int id)
        {
            using (var _context = new JansifyDbContext())
            {
                var entity = await _context.Artistas.FirstOrDefaultAsync(a => a.Id == id);
                if (entity is null)
                {
                    return false;
                }
                _context.Artistas.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        // Métodos para la liberación de recursos (Patrón Dispose)
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
        ~ServiceArtista()
        {
            Dispose(false);
        }
    }
}