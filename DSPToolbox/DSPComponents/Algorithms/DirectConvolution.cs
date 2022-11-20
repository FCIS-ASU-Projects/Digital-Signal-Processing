using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            List<float> convoluted_s = new List<float>();
            List<int> convoluted_s_index = new List<int>();
            int start = InputSignal1.SamplesIndices.Min() + InputSignal2.SamplesIndices.Min();
            int end = InputSignal1.SamplesIndices.Max() + InputSignal2.SamplesIndices.Max();
            for (int numIterations = start; numIterations <= end; ++numIterations)
            {
                double resultHarmonic = 0;
                int k;
                for (k = start; k < InputSignal1.Samples.Count; ++k)
                {
                    if (numIterations - k < InputSignal2.SamplesIndices.Min() || numIterations - k > InputSignal2.SamplesIndices.Max())
                        continue;
                    if (k < InputSignal1.SamplesIndices.Min() || k > InputSignal1.SamplesIndices.Max())
                        continue;
                    int x_indx = InputSignal1.SamplesIndices.IndexOf(k);
                    int h_indx = InputSignal2.SamplesIndices.IndexOf(numIterations - k);
                    resultHarmonic += InputSignal1.Samples[x_indx] * InputSignal2.Samples[h_indx];
                }
                if (resultHarmonic == 0 && numIterations == end) continue;
                convoluted_s.Add((float)resultHarmonic);
                convoluted_s_index.Add(numIterations);
            }
            OutputConvolvedSignal = new Signal(convoluted_s, convoluted_s_index, false);
        }
    }
}
