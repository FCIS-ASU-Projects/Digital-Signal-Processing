using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Adder : Algorithm
    {
        public List<Signal> InputSignals { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> addedSamples = new List<float>();
            int max = 0;
            for (int i = 0; i < InputSignals.Count; i++)
            {
                if (InputSignals[i].Samples.Count > max)
                {
                    max = InputSignals[i].Samples.Count;
                }

            }
            for (int i = 0; i < InputSignals.Count; i++)
            {
                if (InputSignals[i].Samples.Count < max)
                {
                    int sub = max - InputSignals[i].Samples.Count;
                    for (int j = 0; j < sub; j++)
                    {
                        InputSignals[i].Samples.Add(0);
                    }
                }

            }
            //for(int i = 0; i < InputSignals[i].Samples.Count; i++)
            //{
            //    float x = 0;
            //    for(int j=0; j < InputSignals.Count; j++)
            //    {
            //        x= InputSignals[j].Samples[i];
            //    }
            //    addedSamples.Add(x);
            //}
            //OutputSignal = new Signal(addedSamples, false);
            for (int i = 0; i < InputSignals.Count; i++)
            {
                for (int j = 0; j < InputSignals[i].Samples.Count; j++)
                {
                    if (addedSamples.Count == j)
                    {
                        addedSamples.Add(InputSignals[i].Samples[j]);
                    }
                    else
                    {
                        addedSamples[j] += InputSignals[i].Samples[j];
                    }
                }
            }
            OutputSignal = new Signal(addedSamples, false);
        }
    }
}