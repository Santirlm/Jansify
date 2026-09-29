using Jansify_Santiago_Ramirez.Helpers;
using Jansify_Santiago_Ramirez.Models;
using Jansify_Santiago_Ramirez.Services;
using Jansify_Santiago_Ramirez.ViewModels.Base;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Jansify_Santiago_Ramirez.ViewModels
{
    public class InformeViewModel : BaseViewModel
    {
        #region Atributos

        private readonly ServicePlaylist _servicePlaylist;
        private ObservableCollection<Playlist> _playlists;
        private Playlist _playlistSeleccionada;

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

        #endregion

        #region Comandos

        public ICommand GenerarInformeCommand { get; }

        #endregion

        #region Constructores

        public InformeViewModel()
        {
            _servicePlaylist = new ServicePlaylist();
            _playlists = new ObservableCollection<Playlist>();

            // solo se puede generar si hay playlist seleccionada
            GenerarInformeCommand = new RelayCommand(
                async _ => await ExecuteGenerarInforme(),
                _ => PlaylistSeleccionada != null);

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

        // abre el dialogo de guardado y genera el pdf
        private async Task ExecuteGenerarInforme()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "PDF|*.pdf",
                FileName = $"Jansify_{PlaylistSeleccionada.Nombre}_{DateTime.Now:yyyyMMdd}"
            };

            if (dialog.ShowDialog() == true)
            {
                InformeService.GenerarInformePlaylist(
                    AuthService.UsuarioActivo,
                    PlaylistSeleccionada,
                    dialog.FileName);

                MessageBox.Show("Informe generado correctamente.", "Jansify",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion
    }
}