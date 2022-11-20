using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {
            List<float> first = new List<float>();
            List<float> second = new List<float>();
            for (int i = 1; i < InputSignal.Samples.Count; ++i)
            {
                first.Add(InputSignal.Samples[i] - InputSignal.Samples[i - 1]);
            }
            for (int i = 1; i < InputSignal.Samples.Count - 1; ++i)
            {
                second.Add(InputSignal.Samples[i + 1] - (2 * InputSignal.Samples[i]) + InputSignal.Samples[i - 1]);
            }
            FirstDerivative = new Signal(first, false);
            SecondDerivative = new Signal(second, false);
        }
    }
}
