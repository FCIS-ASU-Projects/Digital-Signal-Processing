using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();
            int n = InputSignal1.Samples.Count;
            double sum1 = 0;
            double sum2 = 0;
            float r = 0;
            if (InputSignal2 == null)
                InputSignal2 = InputSignal1;
            if (InputSignal1.Samples.Count == InputSignal2.Samples.Count)
            {
                for (int i = 0; i < n; i++)
                {
                    sum1 += Math.Pow(InputSignal1.Samples[i], 2);
                    sum2 += Math.Pow(InputSignal2.Samples[i], 2);
                }
                DiscreteFourierTransform dft = new DiscreteFourierTransform();
                dft.InputTimeDomainSignal = InputSignal1;
                dft.Run();
                DiscreteFourierTransform dft2 = new DiscreteFourierTransform();
                dft2.InputTimeDomainSignal = InputSignal2;
                dft2.Run();
                List<Complex> c1 = new List<Complex>();
                List<Complex> c2 = new List<Complex>();
                for (int i = 0; i < dft.OutputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
                {
                    Complex c = new Complex(dft.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Cos(dft.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]),
                                           -dft.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Sin(dft.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                    c1.Add(c);
                    Complex cc = new Complex(dft2.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Cos(dft.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]),
                                          dft2.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Sin(dft.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                    c2.Add(cc);
                }
                List<Complex> resultOfMultiply = new List<Complex>();
                InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();
                idft.InputFreqDomainSignal.FrequenciesPhaseShifts = new List<float>();
                idft.InputFreqDomainSignal.FrequenciesAmplitudes = new List<float>();
                for (int i = 0; i < dft2.OutputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
                {
                    resultOfMultiply.Add(c1[i] * c2[i]);
                    idft.InputFreqDomainSignal.FrequenciesPhaseShifts.Add((float)resultOfMultiply[i].Phase);
                    idft.InputFreqDomainSignal.FrequenciesAmplitudes.Add((float)resultOfMultiply[i].Magnitude);
                }
                idft.Run();
                for (int i = 0; i < idft.OutputTimeDomainSignal.Samples.Count; i++)
                {
                    r += idft.OutputTimeDomainSignal.Samples[i];
                }
                r = r / n;
                double ss = (Math.Sqrt(sum1 * sum2)) / n;
                float p = (float)(r / ss);
                OutputNonNormalizedCorrelation.Add(r);
                OutputNormalizedCorrelation.Add(p);
            }
        }
    }
}