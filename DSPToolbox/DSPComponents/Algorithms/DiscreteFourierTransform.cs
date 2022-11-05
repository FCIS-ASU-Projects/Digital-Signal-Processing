using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            List<KeyValuePair<float, float>> harmonics = new List<KeyValuePair<float, float>>();
            List<float> freq = new List<float>();
            List<float> amplitudes = new List<float>();
            List<float> phase_shifts = new List<float>();
            int N = InputTimeDomainSignal.Samples.Count;

            for (int k=0; k< N; k++)//Iterations on harmonics
            {
                float a =0, b=0;
                for (int n=0; n<N; n++)
                {
                    a += InputTimeDomainSignal.Samples[n] * (float)Math.Cos((k * 2 * Math.PI * n) / N);
                    b += InputTimeDomainSignal.Samples[n] * -1 * (float)(Math.Sin((k * 2 * Math.PI * n) / N));
                }
                freq.Add(k);
                harmonics.Add(new KeyValuePair<float, float>(a, b));
                amplitudes.Add((float)Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2)));
                phase_shifts.Add((float)Math.Atan2(b, a));
            }
            OutputFreqDomainSignal = new Signal(new List<float>(), false);
            OutputFreqDomainSignal.Frequencies = freq;
            OutputFreqDomainSignal.FrequenciesAmplitudes = amplitudes;
            OutputFreqDomainSignal.FrequenciesPhaseShifts = phase_shifts;
        }
    }
}
