using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }
 
        public override void Run()
        {
            List<float> s = new List<float>();
            for (int i = 0; i <= InputSignal.Samples.Count - InputWindowSize; i++)
            {
                float sum = 0;
                for (int j = i; j < i + InputWindowSize; ++j)
                    sum += InputSignal.Samples[j];
                sum /= InputWindowSize;
                s.Add(sum);
                //Console.WriteLine("sum = " + sum);
            }
            OutputAverageSignal = new Signal(s, false);
        }
    }
}
