using Jansify_Santiago_Ramirez.Helpers;
using Jansify_Santiago_Ramirez.ViewModels.Base;
using System.Windows.Input;

namespace Jansify_Santiago_Ramirez.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Atributos

        private object _currentView;
        private bool _isLoggedIn;

        #endregion

        #region Propiedades

        public object CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set => SetProperty(ref _isLoggedIn, value);
        }

        // instancia unica del reproductor compartida entre vistas
        public ReproductorViewModel Reproductor { get; } = new ReproductorViewModel();

        // true si el usuario logado es admin
        public bool EsAdmin => AuthService.EsAdmin();

        #endregion

        #region Comandos

        public ICommand ShowHomeCommand { get; }
        public ICommand ShowAdminCommand { get; }
        public ICommand ShowInformeCommand { get; }
        public ICommand ShowBuscarCommand { get; }
        public ICommand ShowBibliotecaCommand { get; }
        public ICommand LogoutCommand { get; }

        #endregion

        #region Constructores

        public MainWindowViewModel()
        {
            ShowHomeCommand = new RelayCommand(_ => ExecuteShowHome());
            ShowAdminCommand = new RelayCommand(_ => ExecuteShowAdmin());
            ShowInformeCommand = new RelayCommand(_ => ExecuteShowInforme());
            ShowBuscarCommand = new RelayCommand(_ => ExecuteShowBuscar());
            ShowBibliotecaCommand = new RelayCommand(_ => ExecuteShowBiblioteca());
            LogoutCommand = new RelayCommand(_ => ExecuteLogout());

            // arranca en el login
            ExecuteShowLogin();
        }

        #endregion

        #region Metodos

        private void ExecuteShowLogin()
        {
            CurrentView = new LoginViewModel(OnLoginSuccess);
            IsLoggedIn = false;
        }

        private void ExecuteShowHome()
        {
            CurrentView = new HomeViewModel(Reproductor);
            IsLoggedIn = true;
        }

        private void ExecuteShowAdmin()
        {
            CurrentView = new AdminUsuariosViewModel();
            IsLoggedIn = true;
        }

        private void ExecuteShowInforme()
        {
            CurrentView = new InformeViewModel();
            IsLoggedIn = true;
        }

        private void ExecuteShowBuscar()
        {
            CurrentView = new BuscarViewModel(Reproductor);
            IsLoggedIn = true;
        }

        private void ExecuteShowBiblioteca()
        {
            CurrentView = new BibliotecaViewModel(Reproductor);
            IsLoggedIn = true;
        }

        private void ExecuteLogout()
        {
            AuthService.Logout();
            ExecuteShowLogin();
        }

        // callback que llama el loginviewmodel cuando el login es correcto
        private void OnLoginSuccess()
        {
            // notifica el cambio de rol al sidebar
            OnPropertyChanged(nameof(EsAdmin));
            ExecuteShowHome();
        }

        #endregion
    }
}