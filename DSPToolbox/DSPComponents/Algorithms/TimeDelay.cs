using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class TimeDelay:Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public float InputSamplingPeriod { get; set; }
        public float OutputTimeDelay { get; set; }

        public override void Run()
        {
            //Find the correlation
            if (InputSignal2 == null)
                InputSignal2 = new Signal(InputSignal1.Samples, InputSignal1.Periodic);

            List<float> OutputNonNormalizedCorrelation = new List<float>();
            List<float> OutputNormalizedCorrelation = new List<float>();
            int samples_size = InputSignal1.Samples.Count;
            double s1 = 0;
            double s2 = 0;
            for (int i = 0; i < samples_size; i++)
            {
                s1 += Math.Pow(InputSignal1.Samples[i], 2);
                s2 += Math.Pow(InputSignal2.Samples[i], 2);
            }
            float ss = (float)(Math.Sqrt(s1 * s2)) / samples_size;

            for (int i = 0; i < InputSignal1.Samples.Count; i++)
            {
                float num = 0;
                for (int j = 0; j < InputSignal1.Samples.Count; j++)
                {
                    if (InputSignal2.Periodic == false && i + j >= samples_size)
                        num += 0;
                    else
                        num += InputSignal1.Samples[j] * InputSignal2.Samples[(j + i) % samples_size];
                }
                num /= samples_size;
                OutputNonNormalizedCorrelation.Add(num);
                OutputNormalizedCorrelation.Add(num / ss);
            }

            //Find maximum maximum absolute value
            float max_val = Math.Abs(OutputNormalizedCorrelation[0]);
            int index_of_max_val = 0;
            for (int i=1; i< OutputNormalizedCorrelation.Count; i++)
            {
                if(Math.Abs(OutputNormalizedCorrelation[i]) > max_val)
                {
                    max_val = Math.Abs(OutputNormalizedCorrelation[i]);
                    index_of_max_val = i;
                }
            }
            OutputTimeDelay = index_of_max_val * InputSamplingPeriod;
        }
    }
}
