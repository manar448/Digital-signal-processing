using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class TimeDelay : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public float InputSamplingPeriod { get; set; }
        public float OutputTimeDelay { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            List<float> non_norm = new List<float>();
            List<float> norm = new List<float>();
            float max = 0;
            //float max1 = 0;
            TimeDelay ti = new TimeDelay();
            DirectCorrelation corr = new DirectCorrelation();
            corr.InputSignal1 = ti.InputSignal1;
            corr.InputSignal2 = ti.InputSignal2;
            corr.Run();
            max = corr.OutputNonNormalizedCorrelation.Max();
            //max1 = corr.OutputNormalizedCorrelation.Max();
            OutputTimeDelay = InputSamplingPeriod * corr.OutputNonNormalizedCorrelation.IndexOf(max);
        }
    }
}