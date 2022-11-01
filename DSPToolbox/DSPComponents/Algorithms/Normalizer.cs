using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Normalizer : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputMinRange { get; set; }
        public float InputMaxRange { get; set; }
        public Signal OutputNormalizedSignal { get; set; }

        public override void Run()
        {
            float min = 100000.0f;
            float max = 0;
            List<float> normalizedsamples = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                if (InputSignal.Samples[i] < min)
                {
                    min = InputSignal.Samples[i];
                }
                if (InputSignal.Samples[i] > max)
                {
                    max = InputSignal.Samples[i];
                }
            }
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                normalizedsamples.Add(((InputSignal.Samples[i] - min) / (max - min)) * (InputMaxRange - InputMinRange) + InputMinRange);
            }
            OutputNormalizedSignal = new Signal(normalizedsamples, false);
        }
    }
}
