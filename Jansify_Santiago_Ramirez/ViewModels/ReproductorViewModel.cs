using Jansify_Santiago_Ramirez.Models;
using Jansify_Santiago_Ramirez.ViewModels.Base;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Jansify_Santiago_Ramirez.ViewModels
{
    public class ReproductorViewModel : BaseViewModel
    {
        #region Atributos

        private WaveOutEvent _waveOut;
        private AudioFileReader _audioReader;
        private DispatcherTimer _timer;
        private List<Cancion> _cola = new();
        private int _indiceActual = 0;
        private readonly Random _random = new();
        private bool _actualizandoProgreso = false;

        private Cancion _cancionActual;
        private bool _reproduciendo;
        private double _progreso;
        private double _duracionTotal;
        private double _volumen = 0.5;
        private string _tiempoActual = "0:00";
        private string _tiempoTotal = "0:00";

        #endregion

        #region Propiedades

        public Cancion CancionActual
        {
            get => _cancionActual;
            set => SetProperty(ref _cancionActual, value);
        }

        public bool Reproduciendo
        {
            get => _reproduciendo;
            set => SetProperty(ref _reproduciendo, value);
        }

        public double Progreso
        {
            get => _progreso;
            set
            {
                if (SetProperty(ref _progreso, value) && !_actualizandoProgreso && _audioReader != null)
                { 
                    _audioReader.CurrentTime = TimeSpan.FromSeconds(value);
                }
            }
        }

        public double DuracionTotal
        {
            get => _duracionTotal;
            set => SetProperty(ref _duracionTotal, value);
        }

        public double Volumen
        {
            get => _volumen;
            set
            {
                if (SetProperty(ref _volumen, value) && _waveOut != null)
                {
                    _waveOut.Volume = (float)value;
                }
            }
        }

        public string TiempoActual
        {
            get => _tiempoActual;
            set => SetProperty(ref _tiempoActual, value);
        }

        public string TiempoTotal
        {
            get => _tiempoTotal;
            set => SetProperty(ref _tiempoTotal, value);
        }

        #endregion

        #region Comandos

        public ICommand PlayPausaCommand { get; }
        public ICommand SiguienteCommand { get; }
        public ICommand AnteriorCommand { get; }

        #endregion

        #region Constructores

        public ReproductorViewModel()
        {
            PlayPausaCommand = new RelayCommand(_ => ExecutePlayPausa());
            SiguienteCommand = new RelayCommand(_ => ExecuteSiguiente());
            AnteriorCommand = new RelayCommand(_ => ExecuteAnterior());

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(500);
            _timer.Tick += TimerTick;
        }

        #endregion

        #region Metodos

        public void CargarCola(List<Cancion> canciones, int indice = 0)
        {
            _cola = canciones;
            _indiceActual = indice;
            ReproducirActual();
        }

        private void ReproducirActual()
        {
            if (_cola.Count == 0)
            {
                return;
            }
            CancionActual = _cola[_indiceActual];
            Detener();
            try
            {
                _audioReader = new AudioFileReader(CancionActual.ArchivoUrl);
                _waveOut = new WaveOutEvent();
                _waveOut.Init(_audioReader);
                _waveOut.Volume = (float)Volumen;
                _waveOut.PlaybackStopped += OnPlaybackStopped;
                _waveOut.Play();
                DuracionTotal = _audioReader.TotalTime.TotalSeconds;
                TiempoTotal = FormatearTiempo(_audioReader.TotalTime);
                Reproduciendo = true;
                _timer.Start();
            }
            catch { }
        }

        private void ExecutePlayPausa()
        {
            if (_waveOut == null)
            {
                return;
            }
            if (Reproduciendo)
            {
                _waveOut.Pause();
                _timer.Stop();
                Reproduciendo = false;
            }
            else
            {
                _waveOut.Play();
                _timer.Start();
                Reproduciendo = true;
            }
        }

        private void ExecuteSiguiente()
        {
            if (_cola.Count == 0)
            {
                return;
            }
            _indiceActual = (_indiceActual + 1) % _cola.Count;
            ReproducirActual();
        }

        private void ExecuteAnterior()
        {
            if (_cola.Count == 0)
            {
                return;
            }
            _indiceActual = (_indiceActual - 1 + _cola.Count) % _cola.Count;
            ReproducirActual();
        }

        private void Detener()
        {
            _timer.Stop();
            if (_waveOut != null)
            {
                _waveOut.PlaybackStopped -= OnPlaybackStopped;
                _waveOut.Stop();
                _waveOut.Dispose();
                _waveOut = null;
            }
            if (_audioReader != null)
            {
                _audioReader.Dispose();
                _audioReader = null;
            }
            Reproduciendo = false;
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (e.Exception != null)
                {
                    return;
                }
                ExecuteSiguiente();
            });
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (_audioReader == null)
            {
                return;
            }
            _actualizandoProgreso = true;
            Progreso = _audioReader.CurrentTime.TotalSeconds;
            _actualizandoProgreso = false;
            TiempoActual = FormatearTiempo(_audioReader.CurrentTime);
        }

        private string FormatearTiempo(TimeSpan t)
        {
            return t.Hours > 0
                ? $"{t.Hours}:{t.Minutes:D2}:{t.Seconds:D2}"
                : $"{t.Minutes}:{t.Seconds:D2}";
        }

        #endregion
    }
}