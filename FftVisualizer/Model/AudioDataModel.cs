using FftVisualizerApp.Services;
namespace FftVisualizerApp.Model
{
    public class AudioDataModel
    {
        public List<float> RecordedData { get; private set; }
        public Queue<float> TimeSignalData { get; private set; }
        public List<List<double>> SpectrogramData { get; private set; }

        public FFTProcessorService fftProcessor;
        private const int MaxSamples = 176400;
        private const int Overlap = 1024;
        public AudioDataModel()
        {
            fftProcessor = new FFTProcessorService();
            RecordedData = new List<float>();
            TimeSignalData = new Queue<float>();
            SpectrogramData = new List<List<double>>();
        }
        public void AddToTimeSignalData(float[] audioData)
        {
            foreach (var sample in audioData)
            {
                if (TimeSignalData.Count >= MaxSamples)
                {
                    TimeSignalData.Dequeue();
                }
                TimeSignalData.Enqueue(sample);
            }
        }
        public void AddToSpectrogramData(float[] audioData)
        {
            RecordedData.AddRange(audioData);
            while (RecordedData.Count >= fftProcessor.FftWindowSize)
            {
                var window = RecordedData.Take(fftProcessor.FftWindowSize).ToArray();
                RecordedData.RemoveRange(0, fftProcessor.FftWindowSize - Overlap);
                var fftResult = fftProcessor.CalculateFFT(window);
                SpectrogramData.Add(fftResult);
                if (SpectrogramData.Count > MaxSamples / Overlap)
                {
                    SpectrogramData.RemoveAt(0);
                }
            }
        }
    }
}
