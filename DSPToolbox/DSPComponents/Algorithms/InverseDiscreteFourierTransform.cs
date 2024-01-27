using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            List<float> Samples = new List<float>();
            List<float> Frequencies = new List<float>();
            List<int> SamplesIndices = new List<int>();

            OutputTimeDomainSignal = new Signal(Samples, SamplesIndices, false, Frequencies, InputFreqDomainSignal.FrequenciesAmplitudes, InputFreqDomainSignal.FrequenciesPhaseShifts);

            for (int k = 0; k < InputFreqDomainSignal.FrequenciesAmplitudes.Count; k++)
            {
                float coss = 0;
                float sinn = 0;

                float a = 0;
                float b = 0;
                for (int n = 0; n < InputFreqDomainSignal.FrequenciesAmplitudes.Count; n++)
                {
                    a = InputFreqDomainSignal.FrequenciesAmplitudes[n] * (float)Math.Cos(InputFreqDomainSignal.FrequenciesPhaseShifts[n]);
                    b = InputFreqDomainSignal.FrequenciesAmplitudes[n] * (float)Math.Sin(InputFreqDomainSignal.FrequenciesPhaseShifts[n]);

                    coss += a * ((float)Math.Cos((k * 2 * 180 * n / InputFreqDomainSignal.FrequenciesAmplitudes.Count * Math.PI) / 180));
                    sinn += -1 * b * ((float)Math.Sin((k * 2 * 180 * n / InputFreqDomainSignal.FrequenciesAmplitudes.Count * Math.PI) / 180));
                }
                OutputTimeDomainSignal.Samples.Add((coss + sinn) / InputFreqDomainSignal.FrequenciesAmplitudes.Count);
                OutputTimeDomainSignal.SamplesIndices.Add(k);
            }
        }
    }
}
