using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class SinCos: Algorithm
    {
        public string type { get; set; }
        public float A { get; set; }
        public float PhaseShift { get; set; }
        public float AnalogFrequency { get; set; }
        public float SamplingFrequency { get; set; }
        public List<float> samples { get; set; }
        public override void Run()
        {
            
            if (SamplingFrequency < AnalogFrequency * 2) return;

            samples = new List<float>();
            for (int i = 0; i < SamplingFrequency; ++i)
            {
                double x;
                float f = (float)AnalogFrequency / (float)SamplingFrequency;

                if (type == "cos")
                    x = A * Math.Cos(2 * Math.PI * f * i + PhaseShift);
                else
                    x = A * Math.Sin(2 * Math.PI * f * i + PhaseShift);


                samples.Add((float)x);
            }
        }
    }
}
