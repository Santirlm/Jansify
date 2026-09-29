using Jansify_Santiago_Ramirez.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jansify_Santiago_Ramirez.Services
{
    public class ServiceCancion : IDisposable
    {
        bool disposed;

        public ServiceCancion() { disposed = false; }

        // Método que lista todas las canciones incluyendo su álbum y artista
        public async Task<List<Cancion>> Listar()
        {
            using (var _context = new JansifyDbContext())
            {
                return await _context.Canciones
                    .AsNoTracking()
                    .Include(c => c.Album).ThenInclude(a => a.Artista)
                    .OrderBy(c => c.Titulo)
                    .ToListAsync();
            }
        }

        // Método que lista canciones filtrando por un álbum específico
        public async Task<List<Cancion>> ListarPorAlbum(int albumId)
        {
            using (var _context = new JansifyDbContext())
            {
                return await _context.Canciones
                    .AsNoTracking()
                    .Include(c => c.Album).ThenInclude(a => a.Artista)
                    .Where(c => c.AlbumId == albumId)
                    .ToListAsync();
            }
        }

        // Método que inserta una nueva canción
        public async Task<Cancion> Insertar(Cancion cancion)
        {
            if (cancion == null) 
            {
                throw new ArgumentNullException(nameof(cancion));
            }
            using (var _context = new JansifyDbContext())
            {
                await _context.Canciones.AddAsync(cancion);
                await _context.SaveChangesAsync();
                return cancion;
            }
        }

        // Método que actualiza los datos de una canción existente
        public async Task<bool> Actualizar(Cancion cancion)
        {
            if (cancion == null)
            {
                throw new ArgumentNullException(nameof(cancion));
            }
            using (var _context = new JansifyDbContext())
            {
                var existing = await _context.Canciones.FirstOrDefaultAsync(c => c.Id == cancion.Id);
                if (existing is null)
                {
                    return false;
                }
                existing.Titulo = cancion.Titulo;
                existing.Duracion = cancion.Duracion;
                existing.ArchivoUrl = cancion.ArchivoUrl;
                existing.AlbumId = cancion.AlbumId;
                await _context.SaveChangesAsync();
                return true;
            }
        }

        // Método que elimina una canción por su ID
        public async Task<bool> Borrar(int id)
        {
            using (var _context = new JansifyDbContext())
            {
                var entity = await _context.Canciones.FirstOrDefaultAsync(c => c.Id == id);
                if (entity is null)
                {
                    return false;
                }
                _context.Canciones.Remove(entity);
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
        ~ServiceCancion()
        {
            Dispose(false);
        }
    }
}