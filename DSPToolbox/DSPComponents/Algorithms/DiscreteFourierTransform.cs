using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            
            List<float> sins = new List<float>();
            List<float> cosin = new List<float>();
            List<float> Samples = new List<float>();
            List<float> FrequenciesAmplitudes = new List<float>();
            List<float> Frequencies = new List<float>();
            List<float> FrequenciesPhaseShift = new List<float>();
            OutputFreqDomainSignal = new Signal(Samples, false, Frequencies, FrequenciesAmplitudes, FrequenciesPhaseShift);

            for (int k = 0; k < InputTimeDomainSignal.Samples.Count; k++)
            {
                float sinn = 0;
                float coss = 0;

                for (int n = 0; n < InputTimeDomainSignal.Samples.Count; n++)
                {
                    double x = ((2 * Math.PI * n * k) / InputTimeDomainSignal.Samples.Count);
                    coss += (float)(InputTimeDomainSignal.Samples[n] * Math.Cos(x));
                    sinn += (float)((-1 * InputTimeDomainSignal.Samples[n]) * Math.Sin(x));
                }
                sins.Add(sinn);
                sins.Add(coss);
                OutputFreqDomainSignal.FrequenciesAmplitudes.Add((float)(Math.Sqrt(Math.Pow(coss, 2) + Math.Pow(sinn, 2))));
                OutputFreqDomainSignal.FrequenciesPhaseShifts.Add((float)Math.Atan2(sinn, coss));
                float omega = (float)Math.Round(((2 * Math.PI) / (InputTimeDomainSignal.Samples.Count * (1 / InputSamplingFrequency))) * k, 1);
                OutputFreqDomainSignal.Frequencies.Add(omega);
            }
        }
    }
}
