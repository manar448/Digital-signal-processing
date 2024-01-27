using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class AccumulationSum : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            float sum = 0;
            List<float> accumulation = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                sum += InputSignal.Samples[i];
                accumulation.Add(sum);
            }
            OutputSignal = new Signal(accumulation, false);
        }
    }
}
