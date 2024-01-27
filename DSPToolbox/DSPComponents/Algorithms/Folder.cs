using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            InputSignal.Samples.Reverse();
            InputSignal.SamplesIndices.Reverse();
            for (int i = 0; i < InputSignal.SamplesIndices.Count; i++)
            {
                InputSignal.SamplesIndices[i] *= -1;
            }

            OutputFoldedSignal = new Signal(InputSignal.Samples, InputSignal.SamplesIndices, false);
            if (InputSignal.Periodic == true)
            {
                OutputFoldedSignal.Periodic = false;
            }
            else
            {
                OutputFoldedSignal.Periodic = true;
            }
        }
    }
}
