﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }


        public Signal Upsampling(Signal signal)
        {
            Signal upsampled_signal = new Signal(new List<float>(), false);
            int min_Index = signal.SamplesIndices.Min();
            upsampled_signal.Samples.Add(signal.Samples[0]);
            upsampled_signal.SamplesIndices.Add(min_Index);
            min_Index++;

            for (int i = 1; i < signal.Samples.Count; i++)
            {
                for(int j=1; j<L; j++)
                {
                    upsampled_signal.Samples.Add(0);
                    upsampled_signal.SamplesIndices.Add(min_Index);
                    min_Index++;
                }
                upsampled_signal.Samples.Add(signal.Samples[i]);
                upsampled_signal.SamplesIndices.Add(min_Index);
                min_Index++;
            }

            return upsampled_signal;
        }
        public Signal Downsampling(Signal signal)
        {
            Signal downsampled_signal = new Signal(new List<float>(), false);
            downsampled_signal.Samples.Add(signal.Samples[0]);
            downsampled_signal.SamplesIndices.Add(signal.SamplesIndices[0]);

            for (int i = 1; i < signal.Samples.Count; i++)
            {
                if (i % M == 0)
                {
                    downsampled_signal.Samples.Add(signal.Samples[i]);
                    downsampled_signal.SamplesIndices.Add(signal.SamplesIndices[i]);
                }
            }

            return downsampled_signal;
        }
        public Signal Apply_Low_Pass_Filter(Signal signal)
        {
            FIR Low_Pass_Filter = new FIR();
            Low_Pass_Filter.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
            Low_Pass_Filter.InputTimeDomainSignal = signal;
            Low_Pass_Filter.InputFS = 8000;
            Low_Pass_Filter.InputStopBandAttenuation = 50;
            Low_Pass_Filter.InputCutOffFrequency = 1500;
            Low_Pass_Filter.InputTransitionBand = 500;
            
            Low_Pass_Filter.Run();

            return Low_Pass_Filter.OutputYn;
        }

        public override void Run()
        {
            Console.WriteLine("L = " + L);
            Console.WriteLine("M = " + M);
            Signal Current_signal = InputSignal;

            if (L!=0 && M!=0)
            {
                Signal Upsampled_signal = Upsampling(Current_signal);    
                Signal Filtered_Upsampled_Signal = Apply_Low_Pass_Filter(Upsampled_signal);
                OutputSignal = Downsampling(Filtered_Upsampled_Signal);
            }
            else if(L != 0)
            {
                Signal Upsampled_signal = Upsampling(Current_signal);
                OutputSignal = Apply_Low_Pass_Filter(Upsampled_signal);
            }
            else if(M != 0)
            {
                Signal Filtered_Signal = Apply_Low_Pass_Filter(Current_signal);
                OutputSignal = Downsampling(Filtered_Signal);
            }
        }
    }

}