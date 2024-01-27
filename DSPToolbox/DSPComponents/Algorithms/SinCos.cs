using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class SinCos: Algorithm
    {
        //private const double V = 2.0;

        public string type { get; set; }
        public float A { get; set; }
        public float PhaseShift { get; set; }
        public float AnalogFrequency { get; set; }
        public float SamplingFrequency { get; set; }
        public List<float> samples { get; set; }
        public override void Run()
        {
            //throw new NotImplementedException();
            samples = new List<float>();
            if (SamplingFrequency >= (2 * AnalogFrequency))
            {
                for (int i = 0; i < SamplingFrequency; i++)
                {
                    if (type == "cos")
                    {
                        float res = (float)(A * Math.Cos(2 * Math.PI * (AnalogFrequency / SamplingFrequency) * i + PhaseShift));
                        samples.Add(res);
                    }
                    else
                    {
                        float res = (float)(A * Math.Sin(2 * Math.PI * (AnalogFrequency / SamplingFrequency) * i + PhaseShift));
                        samples.Add(res);

                    }
                }

            }
        }
    }
}
