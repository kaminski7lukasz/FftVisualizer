using FftVisualizerApp.ViewModel;
using System.Windows;

namespace FftVisualizer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            AudioViewModel viewModel = new AudioViewModel();
            DataContext = viewModel;
        }
    }
}