using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            //initializations
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();

            //auto-correlation
            if (InputSignal2 == null)
            {
                //initializations
                List<float> auto_correlation = new List<float>();
                List<double> signal1_samples = new List<double>();
                List<double> signal1_samples_copy = new List<double>();
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                {
                    signal1_samples.Add(InputSignal1.Samples[i]);
                    signal1_samples_copy.Add(InputSignal1.Samples[i]);
                }

                //normalization summation
                double normalization_summation = 0, signal_samples_summation = 0, signal_samples_copy_summation = 0;
                for (int i = 0; i < signal1_samples.Count; i++)
                {
                    signal_samples_summation += Math.Pow(signal1_samples[i], 2);
                    signal_samples_copy_summation += Math.Pow(signal1_samples_copy[i], 2);
                }
                normalization_summation = signal_samples_summation * signal_samples_copy_summation;
                normalization_summation = Math.Sqrt(normalization_summation);
                normalization_summation /= signal1_samples.Count;

                //non-periodic
                if (InputSignal1.Periodic == false)
                {
                    for (int i = 0; i < signal1_samples_copy.Count; i++)
                    {
                        double sum = 0;
                        if (i != 0) // so shift 
                        {
                            double first_element = 0;
                            for (int j = 0; j < signal1_samples_copy.Count - 1; j++)
                            {
                                signal1_samples_copy[j] = signal1_samples_copy[j + 1];
                                sum += signal1_samples_copy[j] * signal1_samples[j];
                            }
                            signal1_samples_copy[signal1_samples_copy.Count - 1] = first_element;

                        }
                        else
                        {
                            for (int j = 0; j < signal1_samples_copy.Count; j++)
                                sum += signal1_samples_copy[j] * signal1_samples_copy[j];
                        }
                        auto_correlation.Add((float)sum / signal1_samples_copy.Count);
                    }
                }

                //periodic
                else
                {
                    for (int i = 0; i < signal1_samples_copy.Count; i++)
                    {
                        double sum = 0;
                        if (i != 0) // so shift 
                        {
                            double first_element = signal1_samples_copy[0];
                            for (int j = 0; j < signal1_samples_copy.Count - 1; j++)
                            {
                                signal1_samples_copy[j] = signal1_samples_copy[j + 1];
                                sum += signal1_samples_copy[j] * signal1_samples[j];
                            }
                            signal1_samples_copy[signal1_samples_copy.Count - 1] = first_element;
                            sum += signal1_samples_copy[signal1_samples_copy.Count - 1] * signal1_samples[signal1_samples.Count - 1];
                        }
                        else
                        {
                            for (int j = 0; j < signal1_samples_copy.Count; j++)
                                sum += signal1_samples_copy[j] * signal1_samples_copy[j];
                        }
                        auto_correlation.Add((float)sum / signal1_samples_copy.Count);
                    }

                }

                //output
                OutputNonNormalizedCorrelation = auto_correlation;

                for (int i = 0; i < OutputNonNormalizedCorrelation.Count; i++)
                    OutputNormalizedCorrelation.Add((float)(OutputNonNormalizedCorrelation[i] / normalization_summation));
            }

            //cross-correlation
            else
            {
                //initializations
                List<float> cross_correlation = new List<float>();
                List<double> signal1_samples = new List<double>();
                List<double> signal2_samples = new List<double>();
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    signal1_samples.Add(InputSignal1.Samples[i]);
                for (int i = 0; i < InputSignal2.Samples.Count; i++)
                    signal2_samples.Add(InputSignal2.Samples[i]);

                //normalization summation
                double normalization_summation = 0, signal1_samples_summation = 0, signal2_samples_summation = 0;
                for (int i = 0; i < signal1_samples.Count; i++)
                {
                    signal1_samples_summation += Math.Pow(signal1_samples[i], 2);
                    signal2_samples_summation += Math.Pow(signal2_samples[i], 2);
                }
                normalization_summation = signal1_samples_summation * signal2_samples_summation;
                normalization_summation = Math.Sqrt(normalization_summation);
                normalization_summation /= signal1_samples.Count;

                //non-periodic
                if (InputSignal1.Periodic == false)
                {
                    for (int i = 0; i < signal2_samples.Count; i++)
                    {
                        double sum = 0;
                        if (i != 0) // so shift 
                        {
                            double first_element = 0;
                            for (int j = 0; j < signal2_samples.Count - 1; j++)
                            {
                                signal2_samples[j] = signal2_samples[j + 1];
                                sum += signal2_samples[j] * signal1_samples[j];
                            }
                            signal2_samples[signal2_samples.Count - 1] = first_element;
                        }
                        else
                        {
                            for (int j = 0; j < signal2_samples.Count; j++)
                                sum += signal1_samples[j] * signal2_samples[j];
                        }
                        cross_correlation.Add((float)sum / signal2_samples.Count);
                    }
                }

                //periodic
                else
                {
                    for (int i = 0; i < signal2_samples.Count; i++)
                    {
                        double sum = 0;
                        if (i != 0) // so shift 
                        {
                            double first_element = signal2_samples[0];
                            for (int j = 0; j < signal2_samples.Count - 1; j++)
                            {
                                signal2_samples[j] = signal2_samples[j + 1];
                                sum += signal2_samples[j] * signal1_samples[j];
                            }
                            signal2_samples[signal2_samples.Count - 1] = first_element;
                            sum += signal2_samples[signal2_samples.Count - 1] * signal1_samples[signal1_samples.Count - 1];
                        }
                        else
                        {
                            for (int j = 0; j < signal2_samples.Count; j++)
                                sum += signal1_samples[j] * signal2_samples[j];
                        }
                        cross_correlation.Add((float)sum / signal2_samples.Count);
                    }
                }

                //output
                OutputNonNormalizedCorrelation = cross_correlation;

                for (int i = 0; i < OutputNonNormalizedCorrelation.Count; i++)
                    OutputNormalizedCorrelation.Add((float)(OutputNonNormalizedCorrelation[i] / normalization_summation));
            }
        }
    }
}
  