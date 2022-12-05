using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public override void Run()
        {
            List<float> harmonics = new List<float>();
            int N = InputFreqDomainSignal.FrequenciesAmplitudes.Count;
            for (int k=0; k<N; k++)//Iterations on harmonics
            {
                Complex harmonic = new Complex(0, 0);
                for (int n=0; n<N; n++)
                {
                    Complex x = new Complex(
                        InputFreqDomainSignal.FrequenciesAmplitudes[n] * Math.Cos((float)InputFreqDomainSignal.FrequenciesPhaseShifts[n]),
                        InputFreqDomainSignal.FrequenciesAmplitudes[n] * Math.Sin((float)InputFreqDomainSignal.FrequenciesPhaseShifts[n])
                        );
                    harmonic += x * (new Complex(Math.Cos(k * 2 * Math.PI * n / N), Math.Sin(k * 2 * Math.PI * n / N)));
                }
                harmonic /= N;
                harmonics.Add((float)harmonic.Real);
            }
            OutputTimeDomainSignal = new Signal(harmonics, false);
        }
    }
}
