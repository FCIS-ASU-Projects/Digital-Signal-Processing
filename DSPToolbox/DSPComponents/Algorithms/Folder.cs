using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            InputSignal.Samples.Reverse();
            OutputFoldedSignal = new Signal(InputSignal.Samples, !InputSignal.Periodic);
            InputSignal.SamplesIndices.Reverse();
            for(int i=0; i< InputSignal.Samples.Count; i++)
            {
                OutputFoldedSignal.SamplesIndices[i] = InputSignal.SamplesIndices[i] * -1;
            }
            //Console.WriteLine("OutputFoldedSignal.Samples");
            //foreach (var i in OutputFoldedSignal.Samples)
            //    Console.WriteLine(i);
        }
    }
}
