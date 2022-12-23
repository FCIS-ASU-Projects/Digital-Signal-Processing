using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; }
        public float? InputCutOffFrequency { get; set; }
        public float? InputF1 { get; set; }
        public float? InputF2 { get; set; }
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; }
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; } //Convolution of InputTimeDomainSignal & OutputHn

        //IDEAL FILTERS
        public float LowPass_Hd(float Fc, int n)
        {
            float result;
            if (n == 0)
                result = 2 * Fc;
            else
                result = (float)((2 * Fc * Math.Sin(n * 2 * Math.PI * Fc)) / (n * 2 * Math.PI * Fc));

            return result;
        }
        public float HighPass_Hd(float Fc, int n)
        {
            float result;
            if (n == 0)
                result = 1 - (2 * Fc);
            else
                result = (float)((-2 * Fc * Math.Sin(n * 2 * Math.PI * Fc)) / (n * 2 * Math.PI * Fc));

            return result;
        }
        public float BandPass_Hd(float Fc1, float Fc2, int n)
        {
            float result;
            if (n == 0)
                result = 2 * (Fc2 - Fc1);
            else
                result = (float)(((2 * Fc2 * Math.Sin(n * 2 * Math.PI * Fc2)) / (n * 2 * Math.PI * Fc2)) -
                    ((2 * Fc1 * Math.Sin(n * 2 * Math.PI * Fc1)) / (n * 2 * Math.PI * Fc1)));
            //Console.WriteLine("result = " + result);
            return result;
        }
        public float BandStop_Hd(float Fc1, float Fc2, int n)
        {
            float result;
            if (n == 0)
                result = 1 - 2 * (Fc2 - Fc1);
            else
                result = (float)(((2 * Fc1 * Math.Sin(n * 2 * Math.PI * Fc1)) / (n * 2 * Math.PI * Fc1)) -
                    ((2 * Fc2 * Math.Sin(n * 2 * Math.PI * Fc2)) / (n * 2 * Math.PI * Fc2)));

            return result;
        }
        public float Filter_Type(int n)
        {
            if (InputFilterType == FILTER_TYPES.HIGH)
                return HighPass_Hd((float)InputCutOffFrequency, n);
            else if (InputFilterType == FILTER_TYPES.LOW)
                return LowPass_Hd((float)InputCutOffFrequency, n);
            else if (InputFilterType == FILTER_TYPES.BAND_PASS)
                return BandPass_Hd((float)InputF1, (float)InputF2, n);
            else if (InputFilterType == FILTER_TYPES.BAND_STOP)
                return BandStop_Hd((float)InputF1, (float)InputF2, n);
            return 0.0f;
        }
        //CUT OFF FREQUENCY VALUE
        public void Calc_CutOffFreq()
        {
            if (InputFilterType == FILTER_TYPES.HIGH)
            {
                InputCutOffFrequency -= (InputTransitionBand / 2);
                InputCutOffFrequency /= InputFS;
            }
            else if (InputFilterType == FILTER_TYPES.LOW)
            {
                InputCutOffFrequency += (InputTransitionBand / 2);
                InputCutOffFrequency /= InputFS;
            }
            else if (InputFilterType == FILTER_TYPES.BAND_PASS)
            {
                InputF1 -= (InputTransitionBand / 2);
                InputF1 /= InputFS;

                InputF2 += (InputTransitionBand / 2);
                InputF2 /= InputFS;
            }
            else if (InputFilterType == FILTER_TYPES.BAND_STOP)
            {
                InputF1 += (InputTransitionBand / 2);
                InputF1 /= InputFS;

                InputF2 -= (InputTransitionBand / 2);
                InputF2 /= InputFS;
            }
        }
        //GET N
        public int RoundUp_to_nearest_Odd(float N)
        {
            int Int_N = (int)N;

            if (Int_N == N)
            {
                if (Int_N % 2 != 0)
                    return Int_N;
                else
                    return (Int_N + 1);
            }
            else
            {
                Int_N += 1;
                if (Int_N % 2 != 0)
                    return Int_N;
                else
                    return (Int_N + 1);
            }
        }
        public float Get_N(Double num, float NormalizedTransitionBand)
        {
            float N = (float)(num / NormalizedTransitionBand);
            N = (float)RoundUp_to_nearest_Odd(N);
            return N;
        }
        //WINDOW FUNCTIONS
        public float Window_Function(string window_name, float N, int n)
        {
            if (window_name == "Rectangular")
                return 1.0f;
            else if (window_name == "Hanning")
                return (float)(0.5 + (0.5 * Math.Cos((2 * Math.PI * n) / N)));
            else if (window_name == "Hamming")
                return (float)(0.54 + (0.46 * Math.Cos((2 * Math.PI * n) / N)));
            else if (window_name == "BlackMan")
                return (float)(0.42 + (0.5 * Math.Cos((2 * Math.PI * n) / (N - 1))) + (0.08 * Math.Cos((4 * Math.PI * n) / (N - 1))));

            return 0.0f;
        }
        //RUN
        public override void Run()
        {
            float NormalizedTransitionBand = InputTransitionBand / InputFS;
            float N = 0;
            float Half_N = 0; // -Half_N <= n <= Half_N
            string WindowName = "";
            //RECTANGULAR WINDOW
            if (InputStopBandAttenuation <= 21)
            {
                N = Get_N(0.9, NormalizedTransitionBand);
                WindowName = "Rectangular";
            }
            //HANNING WINDOW
            else if (InputStopBandAttenuation > 21 && InputStopBandAttenuation <= 44)
            {
                N = Get_N(3.1, NormalizedTransitionBand);
                WindowName = "Hanning";
            }
            //HAMMING WINDOW
            else if (InputStopBandAttenuation > 44 && InputStopBandAttenuation <= 53)
            {
                N = Get_N(3.3, NormalizedTransitionBand);
                WindowName = "Hamming";
            }
            //BLACKMAN WINDOW
            else if (InputStopBandAttenuation > 53 && InputStopBandAttenuation <= 74)
            {
                N = Get_N(5.5, NormalizedTransitionBand);
                WindowName = "BlackMan";
            }

            Half_N = (N - 1) / 2;
            Calc_CutOffFreq();

            OutputHn = new Signal(new List<float>(), false);
            for (int n = (int)(-Half_N); n <= Half_N; n++)
            {
                float Hd = Filter_Type(Math.Abs(n));
                float Window = Window_Function(WindowName, N, Math.Abs(n));
                OutputHn.Samples.Add(Hd * Window);
                OutputHn.SamplesIndices.Add(n);
            }

            DirectConvolution directConvolution = new DirectConvolution();
            Signal Copy_Of_Hn = OutputHn;
            directConvolution.InputSignal1 = Copy_Of_Hn;
            directConvolution.InputSignal2 = InputTimeDomainSignal;
            
            directConvolution.Run();

            OutputYn = directConvolution.OutputConvolvedSignal;
        }
    }
}
