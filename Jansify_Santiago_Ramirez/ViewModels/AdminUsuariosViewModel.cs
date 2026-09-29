using Jansify_Santiago_Ramirez.Models;
using Jansify_Santiago_Ramirez.Services;
using Jansify_Santiago_Ramirez.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Jansify_Santiago_Ramirez.ViewModels
{
    public class AdminUsuariosViewModel : BaseViewModel
    {
        #region Atributos

        private readonly ServiceUsuario _serviceUsuario;
        private ObservableCollection<Usuario> _usuarios;
        private Usuario _usuarioSeleccionado;
        private Usuario _usuarioEdicion;
        private bool _mostrarModal;
        private bool _esNuevo;

        #endregion

        #region Propiedades

        public ObservableCollection<Usuario> Usuarios
        {
            get => _usuarios;
            set => SetProperty(ref _usuarios, value);
        }

        public Usuario UsuarioSeleccionado
        {
            get => _usuarioSeleccionado;
            set => SetProperty(ref _usuarioSeleccionado, value);
        }

        public Usuario UsuarioEdicion
        {
            get => _usuarioEdicion;
            set => SetProperty(ref _usuarioEdicion, value);
        }

        public bool MostrarModal
        {
            get => _mostrarModal;
            set => SetProperty(ref _mostrarModal, value);
        }

        #endregion

        #region Comandos

        public ICommand NuevoCommand { get; }
        public ICommand EditarCommand { get; }
        public ICommand GuardarCommand { get; }
        public ICommand EliminarCommand { get; }
        public ICommand CancelarCommand { get; }

        #endregion

        #region Constructores

        public AdminUsuariosViewModel()
        {
            _serviceUsuario = new ServiceUsuario();
            _usuarios = new ObservableCollection<Usuario>();

            NuevoCommand = new RelayCommand(_ => ExecuteNuevo());
            EditarCommand = new RelayCommand(_ => ExecuteEditar(), _ => UsuarioSeleccionado != null);
            GuardarCommand = new RelayCommand(async _ => await ExecuteGuardar());
            EliminarCommand = new RelayCommand(async _ => await ExecuteEliminar(), _ => UsuarioSeleccionado != null);
            CancelarCommand = new RelayCommand(_ => ExecuteCancelar());

            _ = ExecuteCargar();
        }

        #endregion

        #region Metodos

        // carga todos los usuarios
        private async Task ExecuteCargar()
        {
            var lista = await _serviceUsuario.Listar();
            Usuarios = new ObservableCollection<Usuario>(lista);
        }

        // prepara el modal para crear
        private void ExecuteNuevo()
        {
            _esNuevo = true;
            UsuarioEdicion = new Usuario();
            MostrarModal = true;
        }

        // copia el seleccionado para editar sin tocar el original
        private void ExecuteEditar()
        {
            _esNuevo = false;
            UsuarioEdicion = new Usuario
            {
                Id = UsuarioSeleccionado.Id,
                NombreUsuario = UsuarioSeleccionado.NombreUsuario,
                Email = UsuarioSeleccionado.Email,
                Rol = UsuarioSeleccionado.Rol,
                PasswordHash = UsuarioSeleccionado.PasswordHash,
                FechaRegistro = UsuarioSeleccionado.FechaRegistro
            };
            MostrarModal = true;
        }

        // valida y guarda, distingue entre nuevo y edicion
        private async Task ExecuteGuardar()
        {
            if (string.IsNullOrWhiteSpace(UsuarioEdicion.NombreUsuario) ||
                string.IsNullOrWhiteSpace(UsuarioEdicion.Email))
            {
                MessageBox.Show("Nombre y email son obligatorios.", "Jansify",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (_esNuevo)
            {
                if (string.IsNullOrWhiteSpace(UsuarioEdicion.PasswordHash))
                {
                    MessageBox.Show("La contraseña es obligatoria.", "Jansify",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                await _serviceUsuario.Insertar(UsuarioEdicion);
            }
            else
            {
                await _serviceUsuario.Actualizar(UsuarioEdicion);
            }
            MostrarModal = false;
            await ExecuteCargar();
        }

        // pide confirmacion antes de borrar
        private async Task ExecuteEliminar()
        {
            var result = MessageBox.Show(
                $"¿Seguro que quieres eliminar a {UsuarioSeleccionado.NombreUsuario}?",
                "Jansify", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                await _serviceUsuario.Borrar(UsuarioSeleccionado.Id);
                await ExecuteCargar();
            }
        }

        // cierra el modal sin guardar
        private void ExecuteCancelar()
        {
            MostrarModal = false;
            UsuarioEdicion = null;
        }

        #endregion
    }
}