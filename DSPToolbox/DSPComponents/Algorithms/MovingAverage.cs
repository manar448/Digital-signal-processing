using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }
 
        public override void Run()
        {
            //throw new NotImplementedException(); 
            List<float> result = new List<float>();

            for (int i = 0; i <= InputSignal.Samples.Count - InputWindowSize; i++)
            {
                //samples = 10  window size = 3  
                //samples[0] = 0+1 +2 /3
                int j = i;
                float sum = 0;
                int counter = 0;
                while (counter < InputWindowSize)
                {
                    sum += InputSignal.Samples[j];
                    j++;
                    counter++;
                }

                result.Add(sum / InputWindowSize);
            }
            OutputAverageSignal = new Signal(result, false);
        }
    }
}
