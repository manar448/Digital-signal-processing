using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }//Sample Freq
        public float miniF { get; set; }//Min Freq
        public float maxF { get; set; }//Max Freq
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            Signal InputSignal = LoadSignal(SignalPath);

            List<float> fir = new List<float>();
            List<float> sampling = new List<float>();
            List<float> DC = new List<float>();
            List<float> Normalize = new List<float>();

            // step 2
            FIR f = new FIR();
            f.InputTimeDomainSignal = InputSignal;
            f.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.BAND_PASS;
            f.InputFS = Fs;
            f.InputF1 = miniF;
            f.InputF2 = maxF;
            f.InputStopBandAttenuation = 50;
            f.InputTransitionBand = 500;
            f.Run();
            for (int i = 0; i < f.OutputYn.Samples.Count; i++)
            {
                fir.Add(f.OutputYn.Samples[i]);
            }

            using (StreamWriter writer = new StreamWriter("C:/Users/Malak/Documents/Semster5/Digital Signal Processing/Tasks/fcisdsp-dsp.toolbox-78ddd969882b/DSPToolbox/fir.txt"))
            {
                writer.WriteLine(0); //freq. or time Domain
                writer.WriteLine(0); //periodic or not
                writer.WriteLine(f.OutputYn.Samples.Count);
                for (int i = 0; i < f.OutputYn.Samples.Count; i++)
                {
                    writer.Write(f.OutputYn.SamplesIndices[i]);
                    writer.Write(" ");
                    writer.WriteLine(f.OutputYn.Samples[i]);
                }
            }

            // if step 3
            if (newFs >= 2 * maxF)
            {
                // step3
                Sampling s = new Sampling();
                s.L = L;
                s.M = M;
                s.InputSignal = new Signal(fir, false);
                s.Run();
                for (int i = 0; i < s.OutputSignal.Samples.Count; i++)
                {
                    sampling.Add(s.OutputSignal.Samples[i]);
                }

                using (StreamWriter writer = new StreamWriter("C:/Users/Malak/Documents/Semster5/Digital Signal Processing/Tasks/fcisdsp-dsp.toolbox-78ddd969882b/DSPToolbox/sampling.txt"))
                {
                    writer.WriteLine(0); //freq. or time Domain
                    writer.WriteLine(0); //periodic or not
                    writer.WriteLine(s.OutputSignal.Samples.Count);
                    for (int i = 0; i < s.OutputSignal.Samples.Count; i++)
                    {
                        writer.Write(s.OutputSignal.SamplesIndices[i]);
                        writer.Write(" ");
                        writer.WriteLine(s.OutputSignal.Samples[i]);
                    }
                }

                // step 4
                DC_Component dc_obj = new DC_Component();
                dc_obj.InputSignal = new Signal(sampling, false);
                dc_obj.Run();
                for (int i = 0; i < dc_obj.OutputSignal.Samples.Count; i++)
                {
                    DC.Add(dc_obj.OutputSignal.Samples[i]);
                }

                using (StreamWriter writer = new StreamWriter("C:/Users/Malak/Documents/Semster5/Digital Signal Processing/Tasks/fcisdsp-dsp.toolbox-78ddd969882b/DSPToolbox/dc_component.txt"))
                {
                    writer.WriteLine(0); //freq. or time Domain
                    writer.WriteLine(0); //periodic or not
                    writer.WriteLine(dc_obj.OutputSignal.Samples.Count);
                    for (int i = 0; i < dc_obj.OutputSignal.Samples.Count; i++)
                    {
                        writer.Write(dc_obj.OutputSignal.SamplesIndices[i]);
                        writer.Write(" ");
                        writer.WriteLine(dc_obj.OutputSignal.Samples[i]);
                    }
                }

                // step 6
                Normalizer norm = new Normalizer();
                norm.InputSignal = new Signal(DC, false);
                norm.InputMinRange = -1;
                norm.InputMaxRange = 1;
                norm.Run();
                for (int i = 0; i < norm.OutputNormalizedSignal.Samples.Count; i++)
                {
                    Normalize.Add(norm.OutputNormalizedSignal.Samples[i]);
                }

                using (StreamWriter writer = new StreamWriter("C:/Users/Malak/Documents/Semster5/Digital Signal Processing/Tasks/fcisdsp-dsp.toolbox-78ddd969882b/DSPToolbox/normalizer.txt"))
                {
                    writer.WriteLine(0); //freq. or time Domain
                    writer.WriteLine(0); //periodic or not
                    writer.WriteLine(norm.OutputNormalizedSignal.Samples.Count);
                    for (int i = 0; i < norm.OutputNormalizedSignal.Samples.Count; i++)
                    {
                        writer.Write(norm.OutputNormalizedSignal.SamplesIndices[i]);
                        writer.Write(" ");
                        writer.WriteLine(norm.OutputNormalizedSignal.Samples[i]);
                    }
                }

                // step 8
                DiscreteFourierTransform DFT = new DiscreteFourierTransform();
                DFT.InputTimeDomainSignal = new Signal(Normalize, false);
                DFT.InputSamplingFrequency = Fs;
                DFT.Run();
                OutputFreqDomainSignal = DFT.OutputFreqDomainSignal;
                using (StreamWriter writer = new StreamWriter("C:/Users/Malak/Documents/Semster5/Digital Signal Processing/Tasks/fcisdsp-dsp.toolbox-78ddd969882b/DSPToolbox/DFT.txt"))
                {
                    writer.WriteLine(1); //freq. or time Domain
                    writer.WriteLine(0); //periodic or not
                    writer.WriteLine(DFT.OutputFreqDomainSignal.Frequencies.Count);
                    for (int i = 0; i < DFT.OutputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
                    {
                        writer.Write(DFT.OutputFreqDomainSignal.Frequencies[i]);
                        writer.Write(" ");
                        writer.Write(DFT.OutputFreqDomainSignal.FrequenciesAmplitudes[i]);
                        writer.Write(" ");
                        writer.WriteLine(DFT.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]);
                    }
                }
            }
            else
            {
                // step 4
                DC_Component dc_obj = new DC_Component();
                dc_obj.InputSignal = new Signal(fir, false);
                dc_obj.Run();
                for (int i = 0; i < dc_obj.OutputSignal.Samples.Count; i++)
                {
                    DC.Add(dc_obj.OutputSignal.Samples[i]);
                }

                using (StreamWriter writer = new StreamWriter("C:/Users/Malak/Documents/Semster5/Digital Signal Processing/Tasks/fcisdsp-dsp.toolbox-78ddd969882b/DSPToolbox/dc_component.txt"))
                {
                    writer.WriteLine(0); //freq. or time Domain
                    writer.WriteLine(0); //periodic or not
                    writer.WriteLine(dc_obj.OutputSignal.Samples.Count);
                    for (int i = 0; i < dc_obj.OutputSignal.Samples.Count; i++)
                    {
                        writer.Write(dc_obj.OutputSignal.SamplesIndices[i]);
                        writer.Write(" ");
                        writer.WriteLine(dc_obj.OutputSignal.Samples[i]);
                    }
                }

                // step 6
                Normalizer norm = new Normalizer();
                norm.InputSignal = new Signal(DC, false);
                norm.InputMinRange = -1;
                norm.InputMaxRange = 1;
                norm.Run();
                for (int i = 0; i < norm.OutputNormalizedSignal.Samples.Count; i++)
                {
                    Normalize.Add(norm.OutputNormalizedSignal.Samples[i]);
                }

                using (StreamWriter writer = new StreamWriter("C:/Users/Malak/Documents/Semster5/Digital Signal Processing/Tasks/fcisdsp-dsp.toolbox-78ddd969882b/DSPToolbox/normalizer.txt"))
                {
                    writer.WriteLine(0); //freq. or time Domain
                    writer.WriteLine(0); //periodic or not
                    writer.WriteLine(norm.OutputNormalizedSignal.Samples.Count);
                    for (int i = 0; i < norm.OutputNormalizedSignal.Samples.Count; i++)
                    {
                        writer.Write(norm.OutputNormalizedSignal.SamplesIndices[i]);
                        writer.Write(" ");
                        writer.WriteLine(norm.OutputNormalizedSignal.Samples[i]);
                    }
                }

                // step 8
                DiscreteFourierTransform DFT = new DiscreteFourierTransform();
                DFT.InputTimeDomainSignal = new Signal(Normalize, false);
                DFT.InputSamplingFrequency = Fs;
                DFT.Run();

                OutputFreqDomainSignal = DFT.OutputFreqDomainSignal;
                using (StreamWriter writer = new StreamWriter("C:/Users/Malak/Documents/Semster5/Digital Signal Processing/Tasks/fcisdsp-dsp.toolbox-78ddd969882b/DSPToolbox/DFT.txt"))
                {
                    writer.WriteLine(1); //freq. or time Domain
                    writer.WriteLine(0); //periodic or not
                    writer.WriteLine(DFT.OutputFreqDomainSignal.Frequencies.Count);
                    for (int i = 0; i < DFT.OutputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
                    {
                        writer.Write(DFT.OutputFreqDomainSignal.Frequencies[i]);
                        writer.Write(" ");
                        writer.Write(DFT.OutputFreqDomainSignal.FrequenciesAmplitudes[i]);
                        writer.Write(" ");
                        writer.WriteLine(DFT.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]);
                    }
                }
            }
        }
        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);

            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());

            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }

            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());

                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }
    }
}
