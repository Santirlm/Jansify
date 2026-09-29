using Jansify_Santiago_Ramirez.Helpers;
using Jansify_Santiago_Ramirez.ViewModels.Base;
using System;
using System.Windows.Input;

namespace Jansify_Santiago_Ramirez.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Atributos

        private string _nombreUsuario = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;
        private readonly Action _onLoginSuccess;

        #endregion

        #region Propiedades

        // Propiedad que gestiona el nombre de usuario introducido
        public string NombreUsuario
        {
            get => _nombreUsuario;
            set => SetProperty(ref _nombreUsuario, value);
        }

        // Propiedad que gestiona la contraseña introducida
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        // Propiedad que almacena los mensajes de error para la interfaz
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        #endregion

        #region Comandos

        // Comando que dispara la ejecución del login
        public ICommand LoginCommand { get; }

        #endregion

        #region Constructores

        // Constructor que inicializa el comando y recibe el callback de éxito
        public LoginViewModel(Action onLoginSuccess)
        {
            _onLoginSuccess = onLoginSuccess;
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        #endregion

        #region Metodos

        // Método que ejecuta la lógica de autenticación contra el servicio
        private async void ExecuteLogin(object? parameter = null)
        {
            bool ok = await AuthService.Login(NombreUsuario, Password);
            if (ok)
            {
                _onLoginSuccess();
            }
            else
            {
                ErrorMessage = "Usuario o contraseña incorrectos.";
            }
        }

        #endregion
    }
}