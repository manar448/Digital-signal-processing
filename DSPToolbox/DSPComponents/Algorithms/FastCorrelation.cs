using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics;
using System.Threading.Tasks;
using System.Numerics;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();
            Signal signal2 = InputSignal1;
            if (InputSignal2 != null)
                signal2 = InputSignal2;

            double normalization_summation = 0, signal_samples_summation = 0, signal_samples_copy_summation = 0;
            for (int i = 0; i < InputSignal1.Samples.Count; i++)
            {
                signal_samples_summation += Math.Pow(InputSignal1.Samples[i], 2);
                signal_samples_copy_summation += Math.Pow(signal2.Samples[i], 2);
            }
            normalization_summation = signal_samples_summation * signal_samples_copy_summation;
            normalization_summation = Math.Sqrt(normalization_summation);
            normalization_summation /= InputSignal1.Samples.Count;


            DiscreteFourierTransform dft1 = new DiscreteFourierTransform();
            DiscreteFourierTransform dft2 = new DiscreteFourierTransform();
            InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();

            dft1.InputTimeDomainSignal = InputSignal1;
            dft2.InputTimeDomainSignal = signal2;
            dft1.Run();
            dft2.Run();

            List<float> amplitude = new List<float>();
            List<float> phase = new List<float>();

            for (int i = 0; i < InputSignal1.Samples.Count; i++)
            {
                float real1 = 0;
                float imagn1 = 0;
                float real2 = 0;
                float imagn2 = 0;

                real1 = dft1.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * ((float)Math.Cos(dft1.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                imagn1 = -1 * dft1.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * ((float)Math.Sin(dft1.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                real2 = dft2.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * ((float)Math.Cos(dft2.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                imagn2 = dft2.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * ((float)Math.Sin(dft2.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));

                Complex c1 = new Complex(real1, imagn1);
                Complex c2 = new Complex(real2, imagn2);
                Complex complex = c1 * c2;

                amplitude.Add((float)complex.Magnitude);
                phase.Add((float)Math.Atan2(complex.Imaginary, complex.Real));
            }

            List<float> no = new List<float>();
            List<float> samples = new List<float>();
            Signal amp = new Signal(samples, false, no, amplitude, phase);

            idft.InputFreqDomainSignal = amp;
            idft.Run();

            List<float> output = new List<float>();
            float N = idft.OutputTimeDomainSignal.Samples.Count;

            for (int i = 0; i < idft.OutputTimeDomainSignal.Samples.Count; i++)
            {
                float res = idft.OutputTimeDomainSignal.Samples[i] * (1 / N);
                output.Add(res);
            }
            OutputNonNormalizedCorrelation = output;
            for (int i = 0; i < OutputNonNormalizedCorrelation.Count; i++)
                OutputNormalizedCorrelation.Add((float)(OutputNonNormalizedCorrelation[i] / normalization_summation));
        }
    }
}
