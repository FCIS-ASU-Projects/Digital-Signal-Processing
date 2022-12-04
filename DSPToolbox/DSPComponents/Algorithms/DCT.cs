using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            float N = InputSignal.Samples.Count;
            OutputSignal = new Signal(new List<float>(), InputSignal.Periodic);
            
            for (int k=0; k<N; k++)
            {
                OutputSignal.Samples.Add(0.0f);

                for (int n=0; n<N; n++)
                    OutputSignal.Samples[k] += (float) (InputSignal.Samples[n] * Math.Cos((Math.PI / (4 * N))*(2 * n - 1)*(2 * k - 1)));

                OutputSignal.Samples[k] *= (float)Math.Sqrt(2.0f / N);
            }
        }
    }
}