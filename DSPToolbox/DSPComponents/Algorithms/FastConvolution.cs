using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            int zeros = (InputSignal1.Samples.Count + InputSignal2.Samples.Count) - 1;
            for (int i = 0; i < (zeros - InputSignal1.Samples.Count); i++)
            {
                InputSignal1.Samples.Add(0);
            }
            for (int i = 0; i < (zeros - InputSignal2.Samples.Count); i++)
            {
                InputSignal2.Samples.Add(0);
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
                                       dft.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Sin(dft.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                c1.Add(c);
            }
            for (int i = 0; i < dft2.OutputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
            {
                Complex c = new Complex(dft2.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Cos(dft.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]),
                                       dft2.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Sin(dft.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                c2.Add(c);
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
            OutputConvolvedSignal = new Signal(idft.OutputTimeDomainSignal.Samples, false);
        }
    }
}
