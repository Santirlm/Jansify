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
    public class BuscarViewModel : BaseViewModel
    {
        #region Atributos

        private readonly ServiceCancion _serviceCancion;
        private readonly ServicePlaylist _servicePlaylist;
        private readonly ReproductorViewModel _reproductor;
        private ObservableCollection<Cancion> _canciones;
        private ObservableCollection<Cancion> _resultados;
        private ObservableCollection<Playlist> _playlists;
        private string _textoBusqueda;
        private bool _mostrarPlaylists;
        private Cancion _cancionParaAgregar;

        #endregion

        #region Propiedades

        public ObservableCollection<Cancion> Resultados
        {
            get => _resultados;
            set => SetProperty(ref _resultados, value);
        }

        public ObservableCollection<Playlist> Playlists
        {
            get => _playlists;
            set => SetProperty(ref _playlists, value);
        }

        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set
            {
                SetProperty(ref _textoBusqueda, value);
                // filtra en tiempo real al escribir
                ExecuteBuscar();
            }
        }

        public bool MostrarPlaylists
        {
            get => _mostrarPlaylists;
            set => SetProperty(ref _mostrarPlaylists, value);
        }

        #endregion

        #region Comandos

        public ICommand ReproducirCommand { get; }
        public ICommand AgregarAPlaylistCommand { get; }
        public ICommand SeleccionarPlaylistCommand { get; }
        public ICommand CancelarPlaylistCommand { get; }

        #endregion

        #region Constructores

        public BuscarViewModel(ReproductorViewModel reproductor)
        {
            _reproductor = reproductor;
            _serviceCancion = new ServiceCancion();
            _servicePlaylist = new ServicePlaylist();
            _canciones = new ObservableCollection<Cancion>();
            _resultados = new ObservableCollection<Cancion>();
            _playlists = new ObservableCollection<Playlist>();

            // reproduce solo esa cancion
            ReproducirCommand = new RelayCommand(param =>
            {
                if (param is Cancion cancion)
                {
                    _reproductor.CargarCola(new System.Collections.Generic.List<Cancion> { cancion });
                }
            });

            // carga las playlists del usuario y abre el modal
            AgregarAPlaylistCommand = new RelayCommand(async param =>
            {
                if (param is Cancion cancion)
                {
                    _cancionParaAgregar = cancion;
                    var lista = await _servicePlaylist.ListarPorUsuario(AuthService.UsuarioActivo.Id);
                    Playlists = new ObservableCollection<Playlist>(lista);
                    if (Playlists.Count == 0)
                    {
                        MessageBox.Show("No tienes playlists. Créa una primero en Tu biblioteca.", "Jansify",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    MostrarPlaylists = true;
                }
            });

            // añade la cancion a la playlist elegida
            SeleccionarPlaylistCommand = new RelayCommand(async param =>
            {
                if (param is Playlist playlist && _cancionParaAgregar != null)
                {
                    bool ok = await _servicePlaylist.AgregarCancion(playlist.Id, _cancionParaAgregar.Id);
                    MostrarPlaylists = false;
                    if (ok)
                    {
                        MessageBox.Show($"Canción añadida a {playlist.Nombre}.", "Jansify",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Esa canción ya está en la playlist.", "Jansify",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            });

            CancelarPlaylistCommand = new RelayCommand(_ => MostrarPlaylists = false);

            _ = CargarCanciones();
        }

        #endregion

        #region Metodos

        // carga todas las canciones al arrancar
        private async Task CargarCanciones()
        {
            var lista = await _serviceCancion.Listar();
            _canciones = new ObservableCollection<Cancion>(lista);
            Resultados = new ObservableCollection<Cancion>(lista);
        }

        // filtra por titulo, artista o album
        private void ExecuteBuscar()
        {
            if (string.IsNullOrWhiteSpace(TextoBusqueda))
            {
                Resultados = new ObservableCollection<Cancion>(_canciones);
                return;
            }
            var filtro = TextoBusqueda.ToLower();
            var filtrados = new ObservableCollection<Cancion>();
            foreach (var c in _canciones)
            {
                if (c.Titulo.ToLower().Contains(filtro) ||
                    (c.Album?.Artista?.Nombre.ToLower().Contains(filtro) ?? false) ||
                    (c.Album?.Titulo.ToLower().Contains(filtro) ?? false))
                    filtrados.Add(c);
            }
            Resultados = filtrados;
        }

        #endregion
    }
}