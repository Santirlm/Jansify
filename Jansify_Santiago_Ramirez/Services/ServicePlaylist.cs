using Jansify_Santiago_Ramirez.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jansify_Santiago_Ramirez.Services
{
    public class ServicePlaylist : IDisposable
    {
        bool disposed;

        public ServicePlaylist() { disposed = false; }

        // playlists del usuario con canciones incluidas
        public async Task<List<Playlist>> ListarPorUsuario(int usuarioId)
        {
            using (var _context = new JansifyDbContext())
            {
                return await _context.Playlists
                    .AsNoTracking()
                    .Include(p => p.Canciones).ThenInclude(c => c.Album).ThenInclude(a => a.Artista)
                    .Where(p => p.UsuarioId == usuarioId)
                    .ToListAsync();
            }
        }

        public async Task<Playlist> Insertar(Playlist playlist)
        {
            if (playlist == null)
            {
                throw new ArgumentNullException(nameof(playlist));
            }
            using (var _context = new JansifyDbContext())
            { 
                await _context.Playlists.AddAsync(playlist);
                await _context.SaveChangesAsync();
                return playlist;
            }
        }

        // añade cancion si no esta ya
        public async Task<bool> AgregarCancion(int playlistId, int cancionId)
        {
            using (var _context = new JansifyDbContext())
            {
                var playlist = await _context.Playlists
                    .Include(p => p.Canciones)
                    .FirstOrDefaultAsync(p => p.Id == playlistId);
                var cancion = await _context.Canciones.FindAsync(cancionId);
                if (playlist is null || cancion is null)
                {
                    return false;
                }
                if (playlist.Canciones.Any(c => c.Id == cancionId))
                {
                    return false;
                }
                playlist.Canciones.Add(cancion);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> EliminarCancion(int playlistId, int cancionId)
        {
            using (var _context = new JansifyDbContext())
            {
                var playlist = await _context.Playlists
                    .Include(p => p.Canciones)
                    .FirstOrDefaultAsync(p => p.Id == playlistId);
                var cancion = playlist?.Canciones.FirstOrDefault(c => c.Id == cancionId);
                if (playlist is null || cancion is null)
                {
                    return false;
                }
                playlist.Canciones.Remove(cancion);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> Borrar(int id)
        {
            using (var _context = new JansifyDbContext())
            {
                var entity = await _context.Playlists.FirstOrDefaultAsync(p => p.Id == id);
                if (entity is null)
                {
                    return false;
                }
                _context.Playlists.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
        }

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
        ~ServicePlaylist()
        {
            Dispose(false);
        }
    }
}