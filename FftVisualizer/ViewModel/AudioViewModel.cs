using FftVisualizerApp.MVVM;
using FftVisualizerApp.Services;
namespace FftVisualizerApp.ViewModel
{
    public class AudioViewModel : ViewModelBase
    {
        private AudioRecorderService _audioRecorderService;
        public TimePlotViewModel TimePlotViewModel { get; private set; }
        public SpectrogramPlotViewModel SpectrogramPlotViewModel { get; private set; }
        public RelayCommand ToggleRecordingCommand => new RelayCommand(execute => ToggleRecording());
        public string ButtonContent => !_audioRecorderService.IsRecording ? "Start" : "Stop";
        public AudioViewModel()
        {
            _audioRecorderService = new AudioRecorderService();
            TimePlotViewModel = new TimePlotViewModel();
            SpectrogramPlotViewModel = new SpectrogramPlotViewModel();
            _audioRecorderService.OnAudioDataReceived += TimePlotViewModel.UpdateTimePlotData;
            _audioRecorderService.OnSpectrogramDataReceived += SpectrogramPlotViewModel.UpdateSpectrogramPlotData;
        }
        private void ToggleRecording()
        {
            if (!_audioRecorderService.IsRecording)
            {
                _audioRecorderService.StartRecording();
            }
            else
            {
                _audioRecorderService.StopRecording();
            }
            OnPropertyChanged(nameof(ButtonContent));
        }
    }
}
