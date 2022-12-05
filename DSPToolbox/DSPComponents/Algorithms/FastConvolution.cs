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
            for (int i = InputSignal1.Samples.Count; i < zeros ; i++)
            {
                InputSignal1.Samples.Add(0);
            }
            for (int i = InputSignal2.Samples.Count; i < zeros; i++)
            {
                InputSignal2.Samples.Add(0);
            }
            Console.WriteLine(InputSignal1.Samples.Count);
            Console.WriteLine(InputSignal2.Samples.Count);
            DiscreteFourierTransform dft = new DiscreteFourierTransform();
            dft.InputTimeDomainSignal = new DSPAlgorithms.DataStructures.Signal(InputSignal1.Samples, false);
            dft.Run();
            DiscreteFourierTransform dft2 = new DiscreteFourierTransform();
            dft2.InputTimeDomainSignal = new DSPAlgorithms.DataStructures.Signal(InputSignal2.Samples, false);
            dft2.Run();
            List<Complex> c1 = new List<Complex>();
            List<Complex> c2 = new List<Complex>();
            List<float> Ampletudes = new List<float>();
            List<float> phasese = new List<float>();
            List<float> frequences = new List<float>();
            for (int i = 0; i < dft.OutputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
            {
                Complex c = new Complex(dft.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Cos(dft.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]),
                                       dft.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Sin(dft.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                c1.Add(c);
            }
            for (int i = 0; i < dft2.OutputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
            {
                Complex cc = new Complex(dft2.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Cos(dft2.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]),
                                      dft2.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Sin(dft2.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                c2.Add(cc);
            }
            Console.WriteLine(c1.Count);
            Console.WriteLine(c2.Count);
            List<Complex> resultOfMultiply = new List<Complex>();
            InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();
            for (int i = 0; i < dft2.OutputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
            {
                Console.WriteLine(c1[i]);
                Console.WriteLine(c2[i]);

                resultOfMultiply.Add(c1[i] * c2[i]);
                Ampletudes.Add((float)resultOfMultiply[i].Magnitude);
                phasese.Add((float)resultOfMultiply[i].Phase);
            }
            idft.InputFreqDomainSignal = new DSPAlgorithms.DataStructures.Signal(true, frequences, Ampletudes, phasese);
            idft.Run();
            OutputConvolvedSignal = new Signal(idft.OutputTimeDomainSignal.Samples, false);
        }
    }
}
