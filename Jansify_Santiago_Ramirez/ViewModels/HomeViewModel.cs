using Jansify_Santiago_Ramirez.Models;
using Jansify_Santiago_Ramirez.Services;
using Jansify_Santiago_Ramirez.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Jansify_Santiago_Ramirez.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        #region Atributos

        private readonly ServiceArtista _serviceArtista;
        private readonly ServiceAlbum _serviceAlbum;
        private readonly ServiceCancion _serviceCancion;
        private readonly ReproductorViewModel _reproductor;
        private ObservableCollection<Artista> _artistas;
        private ObservableCollection<Album> _albums;
        private ObservableCollection<Cancion> _canciones;

        #endregion

        #region Propiedades

        public ObservableCollection<Artista> Artistas
        {
            get => _artistas;
            set => SetProperty(ref _artistas, value);
        }

        public ObservableCollection<Album> Albums
        {
            get => _albums;
            set => SetProperty(ref _albums, value);
        }

        public ObservableCollection<Cancion> Canciones
        {
            get => _canciones;
            set => SetProperty(ref _canciones, value);
        }

        #endregion

        #region Comandos

        public ICommand ReproducirAlbumCommand { get; }
        public ICommand ReproducirCancionCommand { get; }

        #endregion

        #region Constructores

        public HomeViewModel(ReproductorViewModel reproductor)
        {
            _reproductor = reproductor;
            _serviceArtista = new ServiceArtista();
            _serviceAlbum = new ServiceAlbum();
            _serviceCancion = new ServiceCancion();

            // carga las canciones del album y arranca
            ReproducirAlbumCommand = new RelayCommand(async param =>
            {
                if (param is Album album)
                {
                    var canciones = await _serviceCancion.ListarPorAlbum(album.Id);
                    _reproductor.CargarCola(canciones);
                }
            });

            // carga todas las canciones y arranca desde la seleccionada
            ReproducirCancionCommand = new RelayCommand(async param =>
            {
                if (param is Cancion cancion)
                {
                    var todas = await _serviceCancion.Listar();
                    var indice = todas.FindIndex(c => c.Id == cancion.Id);
                    _reproductor.CargarCola(todas, indice < 0 ? 0 : indice);
                }
            });

            _ = CargarDatos();
        }

        #endregion

        #region Metodos

        // carga artistas, albums y canciones de la bd
        private async Task CargarDatos()
        {
            var artistas = await _serviceArtista.Listar();
            Artistas = new ObservableCollection<Artista>(artistas);

            var albums = await _serviceAlbum.Listar();
            Albums = new ObservableCollection<Album>(albums);

            var canciones = await _serviceCancion.Listar();
            Canciones = new ObservableCollection<Cancion>(canciones);
        }

        #endregion
    }
}