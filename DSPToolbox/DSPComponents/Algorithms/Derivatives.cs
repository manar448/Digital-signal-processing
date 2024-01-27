using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives : Algorithm
    {
        private float f;
        private float s;

        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            List<float> First_D = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                if (i == 0)
                    First_D.Add(InputSignal.Samples[i]);
                else
                    First_D.Add(InputSignal.Samples[i] - InputSignal.Samples[i - 1]);
            }
            List<float> Second_D = new List<float>();
            for (int i = 0; i < First_D.Count - 1; i++)
            {
                if (i == 0)
                    Second_D.Add(First_D[i]);
                else
                    Second_D.Add(First_D[i] - First_D[i - 1]);
            }
            First_D.RemoveAt(0);
            Second_D.RemoveAt(0);
            FirstDerivative = new Signal(First_D, false);
            SecondDerivative = new Signal(Second_D, false);
        }
    }
}