using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

/*struct range
{
    public float start;
    public float end;
    public float midpoint;
}*/

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }



        public override void Run()
        {
            //throw new NotImplementedException();
            //check if user enter InputLevel && InputNumBits
            if (InputLevel == 0)
            {
                double bits = Convert.ToDouble(InputNumBits);
                InputLevel = Convert.ToInt32(Math.Pow(2, bits));
            }
            if (InputNumBits == 0)
            {
                double bits = Math.Log(Convert.ToDouble(InputLevel), 2);
                InputNumBits = Convert.ToInt32(bits);

            }

            float min = InputSignal.Samples.Min();
            float max = InputSignal.Samples.Max();
            float delta = (max - min) / InputLevel;

            List<float> start = new List<float>();
            List<float> end = new List<float>();
            List<float> midpoint = new List<float>();

            OutputIntervalIndices = new List<int>();
            OutputSamplesError = new List<float>();
            OutputEncodedSignal = new List<string>();
            List<float> samples = new List<float>();

            List<float> quantize_value = new List<float>();
            float s = min;
            int x = 0; // number of level
            while (s < max)
            {
                start.Add(s);
                s += delta;
                end.Add(s);
                midpoint.Add((start[x] + end[x]) / 2);
                x++;
            }

            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                for (int j = 0; j < InputLevel; j++)
                {
                    if (InputSignal.Samples[i] >= start[j] && InputSignal.Samples[i] <= (float)Math.Round((double)end[j], 2))
                    {

                        OutputIntervalIndices.Add(j + 1); // +1 because level start from 1 && index start from 0
                        quantize_value.Add((float)Math.Round((Decimal)midpoint[j], 3, MidpointRounding.AwayFromZero)); // Xq(n)
                        OutputSamplesError.Add(midpoint[j] - InputSignal.Samples[i]); // Eq(n) = Xq(n) - x(n)
                        break;
                    }
                }
            }

            for (int i = 0; i < OutputIntervalIndices.Count; i++)
            {
                string output = Convert.ToString(OutputIntervalIndices[i] - 1, 2).PadLeft(InputNumBits, '0');
                OutputEncodedSignal.Add(output);
            }
            OutputQuantizedSignal = new Signal(quantize_value, false);
        }

    }

}



    

