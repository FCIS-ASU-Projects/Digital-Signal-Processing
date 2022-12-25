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
                                ij = ij - n;
                            r += InputSignal1.Samples[j] * InputSignal2.Samples[ij];
                        }
                        else
                        {
                            if (ij >= n)
                                r += 0;
                            else
                                r += InputSignal1.Samples[j] * InputSignal2.Samples[ij];
                        }
                    }
                    float r12 = r / n;
                    float p12 = (float)(r12 / ss);
                    OutputNonNormalizedCorrelation.Add(r12);
                    OutputNormalizedCorrelation.Add(p12);
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
                                ij = ij - n;
                            r += InputSignal1.Samples[j] * InputSignal1.Samples[ij];
                        }
                        else
                        {
                            if (ij >= n)
                                r += 0;
                            else
                                r += InputSignal1.Samples[j] * InputSignal1.Samples[ij];
                        }
                    }
                    float r12 = r / n;
                    float p12 = (float)(r12 / ss);
                    OutputNonNormalizedCorrelation.Add(r12);
                    OutputNormalizedCorrelation.Add(p12);
                }
            }
        }
    }
}