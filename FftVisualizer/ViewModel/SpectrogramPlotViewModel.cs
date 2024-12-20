using FftVisualizerApp.MVVM;
using OxyPlot;
using OxyPlot.Series;
namespace FftVisualizerApp.ViewModel
{
    public class SpectrogramPlotViewModel : ViewModelBase
    {
        public PlotModel SpectrogramPlotModel { get; private set; }
        public SpectrogramPlotViewModel()
        {
            SpectrogramPlotModel = new PlotModel { Title = "Spectrogram" };
            SpectrogramPlotModel.Background = OxyColor.FromRgb(0, 0, 0);
            SpectrogramPlotModel.TextColor = OxyColors.Orange;
            var colorAxis = new OxyPlot.Axes.LinearColorAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Right,
                Palette = OxyPalettes.Hot(100),
                Minimum = 0,
                Maximum = 1
            };
            var heatmapSeries = new HeatMapSeries
            {
                X0 = 0,
                X1 = 4, // time span
                Y0 = 0,
                Y1 = 22050,
                Interpolate = true,
                RenderMethod = HeatMapRenderMethod.Bitmap,
                Data = new double[500, 2048 / 2]
            };
            SpectrogramPlotModel.Series.Add(heatmapSeries);
            SpectrogramPlotModel.Axes.Add(colorAxis);
            SpectrogramPlotModel.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                Title = "Time (seconds)",
                Minimum = 0,
                Maximum = 4
            });
            SpectrogramPlotModel.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Left,
                Title = "Frequency (Hz)",
                Minimum = 0,
                Maximum = 22050
            });
        }
        public void UpdateSpectrogramPlotData(List<List<double>> spectrogramData)
        {
            var heatmapSeries = SpectrogramPlotModel.Series[0] as HeatMapSeries;
            int timeLength = spectrogramData.Count;
            int frequencyLength = spectrogramData.FirstOrDefault()?.Count ?? 0;
            var heatmapData = new double[timeLength, frequencyLength];

            for (int t = 0; t < timeLength; t++)
            {
                for (int f = 0; f < frequencyLength; f++)
                {
                    heatmapData[t, f] = spectrogramData[t][f];
                }
            }
            heatmapSeries.Data = heatmapData;
            SpectrogramPlotModel.InvalidatePlot(true);
        }
    }
}
