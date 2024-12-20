using MathNet.Numerics.IntegralTransforms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FftVisualizerApp.Services
{
    public class FFTProcessorService
    {
        public int FftWindowSize { get; private set; } = 2048;
        public List<double> CalculateFFT(float[] audioData)
        {
            var complexData = new Complex[FftWindowSize];
            for (int i = 0; i < audioData.Length; i++)
            {
                complexData[i] = new Complex(audioData[i], 0);
            }
            Fourier.Forward(complexData, FourierOptions.Matlab);
            var frequencyData = new List<double>();
            for (int i = 0; i < FftWindowSize / 2; i++)
            {
                frequencyData.Add(complexData[i].Magnitude);
            }
            return frequencyData;
        }
    }
}
