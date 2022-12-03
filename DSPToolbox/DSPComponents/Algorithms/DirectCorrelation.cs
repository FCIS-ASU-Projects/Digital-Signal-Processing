using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
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
            double s1s = 0;
            double s2s = 0;
            if (InputSignal2 != null)
            {
                for (int i = 0; i < n; i++)
                {
                    s1s += Math.Pow(InputSignal1.Samples[i], 2);
                    s2s += Math.Pow(InputSignal2.Samples[i], 2);
                }
                double ss = (Math.Sqrt(s1s * s2s)) / n;
                for (int i = 0; i < n; i++)
                {

                    float r = 0;
                    for (int j = 0; j < n; j++)
                    {
                        int ij = i + j;
                        if (InputSignal2.Periodic == true)
                        {
                            if (ij >= n)
                            {
                                ij = ij - n;
                            }
                            r += InputSignal1.Samples[j] * InputSignal2.Samples[ij];
                        }
                        else
                        {
                            if (ij >= n)
                            {
                                r += 0;
                            }
                            else
                                r += InputSignal1.Samples[j] * InputSignal2.Samples[ij];
                        }
                    }
                    float r12 = r / n;
                    float p12 = (float)(r12 / ss);
                    OutputNonNormalizedCorrelation.Add(r12);
                    OutputNormalizedCorrelation.Add(p12);
                    //s2 = se;
                }

            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    s1s += Math.Pow(InputSignal1.Samples[i], 2);
                    s2s += Math.Pow(InputSignal1.Samples[i], 2);
                }
                double ss = (Math.Sqrt(s1s * s2s)) / n;
                for (int i = 0; i < n; i++)
                {

                    float r = 0;
                    for (int j = 0; j < n; j++)
                    {
                        int ij = i + j;
                        if (InputSignal1.Periodic == true)
                        {
                            if (ij >= n)
                            {
                                ij = ij - n;
                            }
                            r += InputSignal1.Samples[j] * InputSignal1.Samples[ij];
                        }
                        else
                        {
                            if (ij >= n)
                            {
                                r += 0;
                            }
                            else
                                r += InputSignal1.Samples[j] * InputSignal1.Samples[ij];
                        }
                    }
                    float r12 = r / n;
                    float p12 = (float)(r12 / ss);
                    OutputNonNormalizedCorrelation.Add(r12);
                    OutputNormalizedCorrelation.Add(p12);
                    //s2 = se;
                }
            }
        }
    }
}

//if (InputSignal2 == null)
//    InputSignal2 = new Signal(InputSignal1.Samples, InputSignal1.Periodic);

//OutputNonNormalizedCorrelation = new List<float>();
//OutputNormalizedCorrelation = new List<float>();
//int samples_size = InputSignal1.Samples.Count;
//double s1s = 0;
//double s2s = 0;
//for (int i = 0; i < samples_size; i++)
//{
//    s1s += Math.Pow(InputSignal1.Samples[i], 2);
//    s2s += Math.Pow(InputSignal2.Samples[i], 2);
//}
//float ss = (float)(Math.Sqrt(s1s * s2s)) / samples_size;

//for (int i = 0; i < InputSignal1.Samples.Count; i++)
//{
//    float num = 0;
//    for (int j = 0; j < InputSignal1.Samples.Count; j++)
//    {
//        if (InputSignal2.Periodic == false && i + j >= samples_size)
//            num += 0;
//        else
//            num += InputSignal1.Samples[j] * InputSignal2.Samples[(j + i) % samples_size];
//    }
//    num /= samples_size;
//    OutputNonNormalizedCorrelation.Add(num);
//    OutputNormalizedCorrelation.Add(num / ss);
//}