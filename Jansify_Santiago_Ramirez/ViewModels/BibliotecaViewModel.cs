using Jansify_Santiago_Ramirez.Helpers;
using Jansify_Santiago_Ramirez.Models;
using Jansify_Santiago_Ramirez.Services;
using Jansify_Santiago_Ramirez.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Jansify_Santiago_Ramirez.ViewModels
{
    public class BibliotecaViewModel : BaseViewModel
    {
        #region Atributos

        private readonly ServicePlaylist _servicePlaylist;
        private readonly ReproductorViewModel _reproductor;
        private ObservableCollection<Playlist> _playlists;
        private Playlist _playlistSeleccionada;
        private string _nombreNuevaPlaylist;
        private bool _mostrarNueva;

        #endregion

        #region Propiedades

        public ObservableCollection<Playlist> Playlists
        {
            get => _playlists;
            set => SetProperty(ref _playlists, value);
        }

        public Playlist PlaylistSeleccionada
        {
            get => _playlistSeleccionada;
            set => SetProperty(ref _playlistSeleccionada, value);
        }

        public string NombreNuevaPlaylist
        {
            get => _nombreNuevaPlaylist;
            set => SetProperty(ref _nombreNuevaPlaylist, value);
        }

        public bool MostrarNueva
        {
            get => _mostrarNueva;
            set => SetProperty(ref _mostrarNueva, value);
        }

        #endregion

        #region Comandos

        public ICommand NuevaPlaylistCommand { get; }
        public ICommand GuardarPlaylistCommand { get; }
        public ICommand CancelarPlaylistCommand { get; }
        public ICommand EliminarPlaylistCommand { get; }
        public ICommand ReproducirPlaylistCommand { get; }

        #endregion

        #region Constructores

        public BibliotecaViewModel(ReproductorViewModel reproductor)
        {
            _reproductor = reproductor;
            _servicePlaylist = new ServicePlaylist();
            _playlists = new ObservableCollection<Playlist>();

            NuevaPlaylistCommand = new RelayCommand(_ => ExecuteNueva());
            GuardarPlaylistCommand = new RelayCommand(async _ => await ExecuteGuardar());
            CancelarPlaylistCommand = new RelayCommand(_ => ExecuteCancelar());
            EliminarPlaylistCommand = new RelayCommand(async _ => await ExecuteEliminar(),
                _ => PlaylistSeleccionada != null);

            // carga las canciones de la playlist y arranca el reproductor
            ReproducirPlaylistCommand = new RelayCommand(param =>
            {
                if (param is Playlist pl && pl.Canciones?.Count > 0)
                {
                    var lista = new System.Collections.Generic.List<Cancion>(pl.Canciones);
                    _reproductor.CargarCola(lista);
                }
            });

            _ = CargarPlaylists();
        }

        #endregion

        #region Metodos

        // carga las playlists del usuario logado
        private async Task CargarPlaylists()
        {
            var lista = await _servicePlaylist.ListarPorUsuario(AuthService.UsuarioActivo.Id);
            Playlists = new ObservableCollection<Playlist>(lista);
        }

        // abre el modal para crear playlist
        private void ExecuteNueva()
        {
            NombreNuevaPlaylist = string.Empty;
            MostrarNueva = true;
        }

        // valida el nombre y guarda
        private async Task ExecuteGuardar()
        {
            if (string.IsNullOrWhiteSpace(NombreNuevaPlaylist))
            {
                MessageBox.Show("El nombre no puede estar vacío.", "Jansify",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            await _servicePlaylist.Insertar(new Playlist
            {
                Nombre = NombreNuevaPlaylist,
                UsuarioId = AuthService.UsuarioActivo.Id
            });
            MostrarNueva = false;
            await CargarPlaylists();
        }

        private void ExecuteCancelar()
        {
            MostrarNueva = false;
        }

        // pide confirmacion y borra
        private async Task ExecuteEliminar()
        {
            var result = MessageBox.Show(
                $"¿Eliminar la playlist {PlaylistSeleccionada.Nombre}?",
                "Jansify", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                await _servicePlaylist.Borrar(PlaylistSeleccionada.Id);
                await CargarPlaylists();
            }
        }

        #endregion
    }
}