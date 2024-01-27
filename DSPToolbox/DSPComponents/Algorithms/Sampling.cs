using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }




        public override void Run()
        {
            // throw new NotImplementedException();
            int count = InputSignal.SamplesIndices.Count;
            // up sample method 2
            List<float> new_list = new List<float>();
            List<int> indicies = new List<int>();
            if (M == 0 && L != 0)
            {
                int count1 = InputSignal.SamplesIndices.Count;
                int k = InputSignal.SamplesIndices[0];
                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    new_list.Add(InputSignal.Samples[i]);
                    indicies.Add(k);
                    k++;
                    for (int j = 0; j < (L - 1); j++)
                    {
                        new_list.Add(0);
                        indicies.Add(k);
                        k++;
                    }
                }
                InputSignal = new Signal(new_list, indicies, false);
                FIR f = new FIR();
                f.InputTimeDomainSignal = InputSignal;
                f.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
                f.InputFS = 8000;
                f.InputStopBandAttenuation = 50;
                f.InputCutOffFrequency = 1500;
                f.InputTransitionBand = 500;
                f.Run();
                f.OutputYn.Samples.RemoveAt(f.OutputYn.Samples.Count() - 1);
                f.OutputYn.Samples.RemoveAt(f.OutputYn.Samples.Count() - 1);
                OutputSignal = new Signal(f.OutputYn.Samples, false);
            }
            // down sample method 3
            else if (M != 0 && L == 0)
            {

                FIR f = new FIR();
                f.InputTimeDomainSignal = InputSignal;
                f.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
                f.InputFS = 8000;
                f.InputStopBandAttenuation = 50;
                f.InputCutOffFrequency = 1500;
                f.InputTransitionBand = 500;
                f.Run();

                List<float> c = new List<float>();
                for (int i = 0; i < f.OutputYn.Samples.Count; i += M)
                {
                    c.Add(f.OutputYn.Samples[i]);
                }
                OutputSignal = new Signal(c, false);
            }
            // fraction method 1
            else if (M != 0 && L != 0)
            {
                int count1 = InputSignal.SamplesIndices.Count;
                int k = InputSignal.SamplesIndices[0];
                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    new_list.Add(InputSignal.Samples[i]);
                    indicies.Add(k);
                    k++;
                    for (int j = 0; j < (L - 1); j++)
                    {
                        new_list.Add(0);
                        indicies.Add(k);
                        k++;
                    }
                }
                InputSignal = new Signal(new_list, indicies, false);
                FIR f = new FIR();
                f.InputTimeDomainSignal = InputSignal;
                f.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
                f.InputFS = 8000;
                f.InputStopBandAttenuation = 50;
                f.InputCutOffFrequency = 1500;
                f.InputTransitionBand = 500;
                f.Run();
                f.OutputYn.Samples.RemoveAt(f.OutputYn.Samples.Count() - 1);
                f.OutputYn.Samples.RemoveAt(f.OutputYn.Samples.Count() - 1);

                //count1 = f.OutputYn.SamplesIndices.Count;
                List<float> c = new List<float>();
                for (int i = 0; i < f.OutputYn.Samples.Count; i += M)
                {
                    c.Add(f.OutputYn.Samples[i]);
                }
                OutputSignal = new Signal(c, false);
            }
        }
    }
}