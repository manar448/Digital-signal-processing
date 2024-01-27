using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using MathNet.Numerics;
using System.Numerics;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            //throw new NotImplementedException();

            int sizeofindices = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1;
            if (InputSignal1.Samples.Count != sizeofindices)
            {
                while (InputSignal1.Samples.Count < sizeofindices)
                {
                    InputSignal1.Samples.Add(0);
                }
            }
            if (InputSignal2.Samples.Count != sizeofindices)
            {
                while (InputSignal2.Samples.Count < sizeofindices)
                {
                    InputSignal2.Samples.Add(0);
                }
            }

            DiscreteFourierTransform dft1 = new DiscreteFourierTransform();
            DiscreteFourierTransform dft2 = new DiscreteFourierTransform();
            InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();

            dft1.InputTimeDomainSignal = InputSignal1;
            dft2.InputTimeDomainSignal = InputSignal2;
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
                imagn1 = dft1.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * ((float)Math.Sin(dft1.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));
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
            OutputConvolvedSignal = idft.OutputTimeDomainSignal;
        }
    }
}

