using FftVisualizerApp.MVVM;
using OxyPlot;
using OxyPlot.Series;
namespace FftVisualizerApp.ViewModel
{
    public class TimePlotViewModel : ViewModelBase
    {
        public PlotModel TimePlotModel { get; private set; }
        public TimePlotViewModel()
        {
            TimePlotModel = new PlotModel { Title = "Audio Signal" };
            TimePlotModel.Background = OxyColor.FromRgb(0, 0, 0);
            TimePlotModel.TextColor = OxyColors.Orange;
            TimePlotModel.Series.Add(new LineSeries { Title = "Signal", MarkerType = MarkerType.None, Color = OxyColors.Orange });
            TimePlotModel.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                Title = "Time (seconds)",  // Etykieta osi X
                Minimum = 0,
                Maximum = 4  // Możesz dostosować w zależności od długości sygnału
            });
            TimePlotModel.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Left,
                Title = "Amplitude",
                Minimum = -1,
                Maximum = 1,
                MajorStep = 0.5,
                MinorStep = 0.1
            });
        }
        public void UpdateTimePlotData(List<float> audioData)
        {
            var series = TimePlotModel.Series[0] as LineSeries;
            series.Points.Clear();
            for (int i = 0; i < audioData.Count; i++)
            {
                double timeInSeconds = i / 44100.0;
                double amplitude = Math.Abs(audioData[i]) < 0.005f ? 0 : audioData[i];
                series.Points.Add(new DataPoint(timeInSeconds, amplitude));
            }
            TimePlotModel.InvalidatePlot(true);
        }
    }
}