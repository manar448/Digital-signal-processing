﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DC_Component: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            float sum = 0;

            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                sum += InputSignal.Samples[i];
            }
            float mean = sum / InputSignal.Samples.Count;
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                InputSignal.Samples[i] -= mean;
            }
            OutputSignal = new Signal(InputSignal.Samples, false);
        }
    }
}
