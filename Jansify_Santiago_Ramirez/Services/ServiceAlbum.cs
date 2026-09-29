using Jansify_Santiago_Ramirez.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jansify_Santiago_Ramirez.Services
{
    public class ServiceAlbum : IDisposable
    {
        bool disposed;

        public ServiceAlbum() { disposed = false; }

        // Método que lista albumes
        public async Task<List<Album>> Listar()
        {
            using (var _context = new JansifyDbContext())
            {
                return await _context.Albums
                    .AsNoTracking()
                    .Include(a => a.Artista)
                    .OrderBy(a => a.Titulo)
                    .ToListAsync();
            }
        }

        // Método que lista albumens filtrando por artista
        public async Task<List<Album>> ListarPorArtista(int artistaId)
        {
            using (var _context = new JansifyDbContext())
            {
                return await _context.Albums
                    .AsNoTracking()
                    .Include(a => a.Artista)
                    .Where(a => a.ArtistaId == artistaId)
                    .ToListAsync();
            }
        }

        // Método que inserta un nuevo álbum
        public async Task<Album> Insertar(Album album)
        {
            if (album == null)
            {
                throw new ArgumentNullException(nameof(album));
            }
            using (var _context = new JansifyDbContext())
            {
                await _context.Albums.AddAsync(album);
                await _context.SaveChangesAsync();
                return album;
            }
        }

        // Método que actualiza un álbum existente
        public async Task<bool> Actualizar(Album album)
        {
            if (album == null)
            {
                throw new ArgumentNullException(nameof(album));

            }
            using (var _context = new JansifyDbContext())
            {
                var existing = await _context.Albums.FirstOrDefaultAsync(a => a.Id == album.Id);
                if (existing is null)
                {
                    return false;
                }
                existing.Titulo = album.Titulo;
                existing.Ano = album.Ano;
                existing.PortadaUrl = album.PortadaUrl;
                existing.ArtistaId = album.ArtistaId;
                await _context.SaveChangesAsync();
                return true;
            }
        }

        // Método que elimina un álbum por su ID
        public async Task<bool> Borrar(int id)
        {
            using (var _context = new JansifyDbContext())
            {
                var entity = await _context.Albums.FirstOrDefaultAsync(a => a.Id == id);
                if (entity is null)
                {
                    return false;
                }
                _context.Albums.Remove(entity);
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
        ~ServiceAlbum() 
        {
            Dispose(false); 
        }
    }
}