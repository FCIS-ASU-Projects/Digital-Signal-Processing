using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Shifter : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int ShiftingValue { get; set; }
        public Signal OutputShiftedSignal { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            OutputShiftedSignal = new Signal(InputSignal.Samples, false);
            for (int i = 0; i < InputSignal.SamplesIndices.Count; i++)
            {
                OutputShiftedSignal.SamplesIndices[i] = InputSignal.SamplesIndices[i] - ShiftingValue;
            }

            //Console.WriteLine("OutputShiftedSignal.SamplesIndices");
            //foreach (var i in OutputShiftedSignal.SamplesIndices)
            //    Console.WriteLine(i);
        }
    }
}
