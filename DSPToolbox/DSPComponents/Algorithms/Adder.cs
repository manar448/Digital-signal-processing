using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Adder : Algorithm
    {
        public List<Signal> InputSignals { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            List<float> FinalResult = new List<float>();
            float res = 0;
            int maximum = 0;
            for (int i = 0; i < InputSignals.Count; i++)
            {
                if (maximum < InputSignals[i].Samples.Count)
                {
                    maximum = InputSignals[i].Samples.Count;
                }

            }
            for (int i = 0; i < InputSignals.Count; i++)
            {
                while (InputSignals[i].Samples.Count < maximum)
                {
                    InputSignals[i].Samples.Add(0);
                }
            }
            int k = 0;
            while (k < maximum)
            {
                for (int i = 0; i < InputSignals.Count; i++)
                {

                    res += InputSignals[i].Samples[k];

                }
                FinalResult.Add(res);
                res = 0;
                k++;
            };
            OutputSignal = new Signal(FinalResult, false);
        }
    }
}