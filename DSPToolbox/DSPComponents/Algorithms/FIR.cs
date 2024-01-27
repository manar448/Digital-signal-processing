using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; }
        public float? InputCutOffFrequency { get; set; }
        public float? InputF1 { get; set; }
        public float? InputF2 { get; set; }
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; }
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            int N = 0;
            float transation_table = 0.0f;
            String windowName = "";
            OutputHn = new Signal(new List<float>(), new List<int>(), false);
            if (InputStopBandAttenuation <= 21)
            {
                transation_table = 0.9f;
                windowName = "rectangle";
            }
            else if (InputStopBandAttenuation <= 44)
            {
                transation_table = 3.1f;
                windowName = "hanning";
            }
            else if (InputStopBandAttenuation <= 53)
            {
                transation_table = 3.3f;
                windowName = "hamming";

            }
            else
            {
                transation_table = 5.5f;
                windowName = "blackman";

            }
            N = (int)Math.Floor((transation_table / (InputTransitionBand / InputFS)) + 1);

            for (int i = 0, n = (int)-N / 2; i < N; i++, n++)
            {
                OutputHn.SamplesIndices.Add(n);
            }
            // Low filter
            if (InputFilterType == FILTER_TYPES.LOW)
            {
                float res = (float)(InputCutOffFrequency + (InputTransitionBand / 2));
                float normalized_res = res / InputFS;

                for (int i = 0; i < N; i++)
                {
                    int index = Math.Abs(OutputHn.SamplesIndices[i]);
                    if (OutputHn.SamplesIndices[i] == 0)
                    {
                        float hn = 2 * normalized_res;
                        float wn = window_function(windowName, index, N);
                        OutputHn.Samples.Add(hn * wn);
                    }
                    else
                    {
                        float wc = (float)(2 * Math.PI * normalized_res * index);
                        float hn = (float)(2 * normalized_res * Math.Sin(wc) / wc);
                        float wn = window_function(windowName, index, N);
                        OutputHn.Samples.Add(hn * wn);
                    }
                }
            }
            // High filter
            else if (InputFilterType == FILTER_TYPES.HIGH)
            {
                for (int i = 0; i < N; i++)
                {
                    float res = (float)(InputCutOffFrequency - (InputTransitionBand / 2));
                    float normalized_res = res / InputFS;

                    int index = Math.Abs(OutputHn.SamplesIndices[i]);
                    if (OutputHn.SamplesIndices[i] == 0)
                    {
                        float hn = 1 - (2 * normalized_res);
                        float wn = window_function(windowName, index, N);
                        OutputHn.Samples.Add(hn * wn);
                    }
                    else
                    {
                        float wc = (float)(2 * Math.PI * normalized_res * index);
                        float hn = (float)(-2 * normalized_res * Math.Sin(wc) / (wc));
                        float wn = window_function(windowName, index, N);
                        OutputHn.Samples.Add(hn * wn);
                    }
                }

            }
            // Band Pass
            else if (InputFilterType == FILTER_TYPES.BAND_PASS)
            {
                for (int i = 0; i < N; i++)
                {
                    float res1 = (float)(InputF1 - (InputTransitionBand / 2));
                    float res2 = (float)(InputF2 + (InputTransitionBand / 2));
                    float normalized_res1 = res1 / InputFS;
                    float normalized_res2 = res2 / InputFS;

                    int index = Math.Abs(OutputHn.SamplesIndices[i]);
                    if (OutputHn.SamplesIndices[i] == 0)
                    {
                        float hn = 2 * (normalized_res2 - normalized_res1);
                        float wn = window_function(windowName, index, N);
                        OutputHn.Samples.Add(hn * wn);
                    }
                    else
                    {
                        float w1 = (float)(2 * Math.PI * normalized_res1 * index);
                        float w2 = (float)(2 * Math.PI * normalized_res2 * index);
                        float hn = (float)((2 * normalized_res2 * Math.Sin(w2) / w2) - (2 * normalized_res1 * Math.Sin(w1) / w1));

                        float wn = (window_function(windowName, index, N));
                        OutputHn.Samples.Add(hn * wn);
                    }
                }
            }
            // Band Stop
            else
            {
                float res1 = (float)(InputF1 + (InputTransitionBand / 2));
                float res2 = (float)(InputF2 - (InputTransitionBand / 2));
                float normalized_res1 = res1 / InputFS;
                float normalized_res2 = res2 / InputFS;

                for (int i = 0; i < N; i++)
                {
                    int index = Math.Abs(OutputHn.SamplesIndices[i]);
                    if (OutputHn.SamplesIndices[i] == 0)
                    {
                        float hn = 1 - (2 * (normalized_res2 - normalized_res1));
                        float wn = window_function(windowName, index, N);
                        OutputHn.Samples.Add(hn * wn);
                    }
                    else
                    {
                        float w1 = (float)(2 * Math.PI * normalized_res1 * index);
                        float w2 = (float)(2 * Math.PI * normalized_res2 * index);
                        float hn = (float)((2 * normalized_res1 * Math.Sin(w1) / w1) - (2 * normalized_res2 * Math.Sin(w2) / w2));

                        float wn = (window_function(windowName, index, N));
                        OutputHn.Samples.Add(hn * wn);
                    }
                }
            }



            DirectConvolution conv = new DirectConvolution();
            conv.InputSignal1 = InputTimeDomainSignal;
            conv.InputSignal2 = OutputHn;
            conv.Run();
            OutputYn = conv.OutputConvolvedSignal;
        }




        public float window_function(String windowName, int n, int N)
        {
            float res = 0.0f;
            if (windowName == "rectangle")
            {
                res = 1;
            }
            else if (windowName == "hanning")
            {
                res = (float)0.5 + (float)(0.5 * Math.Cos((2 * Math.PI * n) / N));
            }
            else if (windowName == "hamming")
            {
                res = (float)0.54 + (float)(0.46 * Math.Cos((2 * Math.PI * n) / N));
            }
            else if (windowName == "blackman")
            {


                float term1 = (float)(0.5 * Math.Cos((2 * Math.PI * n) / (N - 1)));
                float term2 = (float)(0.08 * Math.Cos((4 * Math.PI * n) / (N - 1)));
                res = (float)(0.42 + term1 + term2);
            }

            return res;
        }
    }
}
