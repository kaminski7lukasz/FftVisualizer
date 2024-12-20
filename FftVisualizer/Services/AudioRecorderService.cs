using FftVisualizerApp.Model;
using NAudio.Wave;
namespace FftVisualizerApp.Services
{
    public class AudioRecorderService
    {
        private readonly AudioDataModel _audioDataModel;
        private WaveInEvent _waveIn;
        public bool IsRecording { get; private set; }

        public event Action<List<float>> OnAudioDataReceived;
        public event Action<List<List<double>>> OnSpectrogramDataReceived;
        public AudioRecorderService()
        {
            _audioDataModel = new AudioDataModel();
            IsRecording = false;
            _waveIn = new WaveInEvent { WaveFormat = new WaveFormat(44100, 1) };
            _waveIn.DataAvailable += OnDataAvailable;
        }
        public void StartRecording()
        {
            _audioDataModel.SpectrogramData.Clear();
            _audioDataModel.TimeSignalData.Clear();
            _waveIn.StartRecording();
            IsRecording = true;
        }
        public void StopRecording()
        {
            _waveIn.StopRecording();
            IsRecording = false;
        }
        private async void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            var buffer = new float[e.BytesRecorded / 2];
            for (int i = 0; i < e.BytesRecorded; i += 2)
            {
                short sample = BitConverter.ToInt16(e.Buffer, i);
                buffer[i / 2] = sample / 32768f;
            }
            await Task.WhenAll(ProcessTimeSignalDataAsync(buffer), ProcessSpectrogramDataAsync(buffer));
        }
        private async Task ProcessTimeSignalDataAsync(float[] audioData)
        {
            await Task.Run(() =>
            {
                _audioDataModel.AddToTimeSignalData(audioData);
                OnAudioDataReceived?.Invoke(new List<float>(_audioDataModel.TimeSignalData));
            });
        }
        private async Task ProcessSpectrogramDataAsync(float[] audioData)
        {
            await Task.Run(() =>
            {
                _audioDataModel.AddToSpectrogramData(audioData);
                OnSpectrogramDataReceived?.Invoke(new List<List<double>>(_audioDataModel.SpectrogramData));
            });
        }
    }
}
