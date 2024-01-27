using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
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

            float[] f = new float[InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1];
            List<float> result = new List<float>();
            List<int> result_ind = new List<int>();
            int sizeofindices = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1;
            List<int> indices = new List<int>();
            for (int i = 0; i < InputSignal1.Samples.Count; i++)
            {
                for (int j = 0; j < InputSignal2.Samples.Count; j++)
                {
                    f[i + j] += InputSignal1.Samples[i] * InputSignal2.Samples[j];
                }
            }
            for (int i = 0; i < f.Length; i++)
            {
                result.Add(f[i]);

            }
            for (int i = InputSignal2.SamplesIndices[0] - Math.Abs(InputSignal1.SamplesIndices[0]); i <= InputSignal2.SamplesIndices.Last() + Math.Abs(InputSignal1.SamplesIndices.Last()); i++)
            {
                indices.Add(i);
            }

            OutputConvolvedSignal = new Signal(result, indices, false);
        }
    }
}
