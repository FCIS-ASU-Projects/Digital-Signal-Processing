﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }


        //USING BANDPASS FILERTING TO REMOVE THE NOISE
        public Signal Apply_Band_Pass_Filter(Signal signal)
        {
            FIR Low_Pass_Filter = new FIR();
            Low_Pass_Filter.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.BAND_PASS;
            Low_Pass_Filter.InputTimeDomainSignal = signal;
            Low_Pass_Filter.InputFS = Fs;
            Low_Pass_Filter.InputStopBandAttenuation = 50;
            Low_Pass_Filter.InputTransitionBand = 500;
            Low_Pass_Filter.InputF1 = miniF;
            Low_Pass_Filter.InputF2 = maxF;

            Low_Pass_Filter.Run();

            return Low_Pass_Filter.OutputYn;
        }
        //Resampling
        public Signal Apply_Resampling(Signal signal)
        {
            Sampling sampling = new Sampling();
            sampling.InputSignal = signal;
            sampling.L = L;
            sampling.M = M;

            sampling.Run();

            return sampling.OutputSignal;
        }
        //Removing DC_Component
        public Signal Remove_DC_Component(Signal signal)
        {
            DC_Component dc_Component = new DC_Component();
            dc_Component.InputSignal = signal;
            dc_Component.Run();

            return dc_Component.OutputSignal;
        }
        //Normalizing the Signal
        public Signal Apply_Normalization(Signal signal)
        {
            Normalizer normalizer = new Normalizer();
            normalizer.InputSignal = signal;
            normalizer.InputMinRange = -1;
            normalizer.InputMaxRange = 1;

            normalizer.Run();

            return normalizer.OutputNormalizedSignal;
        }
        //Compute DFT
        public Signal Compute_DFT(Signal signal)
        {
            DiscreteFourierTransform discreteFourierTransform = new DiscreteFourierTransform();
            discreteFourierTransform.InputTimeDomainSignal = signal;
            discreteFourierTransform.InputSamplingFrequency = Fs;

            discreteFourierTransform.Run();

            return discreteFourierTransform.OutputFreqDomainSignal;
        }
        //Write in File
        public void Save_In_File(string File_Path, int Is_Freq_Domain, int Is_Periodic, Signal signal)
        {
            try
            {
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(File_Path))
                    File.Delete(File_Path);
                
                // Create a new file
                using (StreamWriter writer = new StreamWriter(File_Path))
                {
                    writer.WriteLine(Is_Freq_Domain);
                    writer.WriteLine(Is_Periodic);
                    
                    if (Is_Freq_Domain == 1)
                    {
                        writer.WriteLine(signal.Frequencies.Count);
                        
                        for (int i = 0; i < signal.Frequencies.Count; i++)
                        {
                            writer.Write(signal.Frequencies[i]);
                            writer.Write(" ");
                            writer.Write(signal.FrequenciesAmplitudes[i]);
                            writer.Write(" ");
                            writer.WriteLine(signal.FrequenciesPhaseShifts[i]);
                        }
                    }
                    else if(Is_Freq_Domain == 0)
                    {
                        writer.WriteLine(signal.Samples.Count);
                        for (int i = 0; i < signal.Samples.Count; i++)
                        {
                            writer.Write(signal.SamplesIndices[i]);
                            writer.Write(" ");
                            writer.WriteLine(signal.Samples[i]);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }
        public override void Run()
        {
            Signal InputSignal = LoadSignal(SignalPath);
            
            //Filtering the Signal using BandPass Filter
            Signal Filtered_Signal = Apply_Band_Pass_Filter(InputSignal);

            //SAVING
            string File_Path = "C:/Huda/FCIS/4th Year/1st Term/Digital Signal Processing/Tasks/Digital-Signal-Processing/DSPToolbox/DSPComponentsUnitTest/TestingSignals/PracticalTask2Filtering.ds";
            Save_In_File(File_Path, 0, Convert.ToInt32(Filtered_Signal.Periodic), Filtered_Signal);

            //Resampling the signal to newFs
            bool Is_Resampled_Signal = false;
            Signal Resampled_Signal = new Signal(new List<float>(), false);

            if (newFs >= (2 * maxF))
            {
                Is_Resampled_Signal = true;
                Resampled_Signal = Apply_Resampling(Filtered_Signal);
            }
            else
                Console.WriteLine("NewFs is not valid");

            //Remove the DC component
            Signal Removed_DC_Component_Signal;
            if (Is_Resampled_Signal)
                Removed_DC_Component_Signal = Remove_DC_Component(Resampled_Signal);
            else
                Removed_DC_Component_Signal = Remove_DC_Component(Filtered_Signal);

            //SAVING
            File_Path = "C:/Huda/FCIS/4th Year/1st Term/Digital Signal Processing/Tasks/Digital-Signal-Processing/DSPToolbox/DSPComponentsUnitTest/TestingSignals/PracticalTask2RemovedDC.ds";
            Save_In_File(File_Path, 0, Convert.ToInt32(Removed_DC_Component_Signal.Periodic), Removed_DC_Component_Signal);

            //Normalize the signal to be from -1 to 1
            Signal Normalized_Signal = Apply_Normalization(Removed_DC_Component_Signal);

            //SAVING
            File_Path = "C:/Huda/FCIS/4th Year/1st Term/Digital Signal Processing/Tasks/Digital-Signal-Processing/DSPToolbox/DSPComponentsUnitTest/TestingSignals/PracticalTask2Normalized.ds";
            Save_In_File(File_Path, 0, Convert.ToInt32(Normalized_Signal.Periodic), Normalized_Signal);

            //Compute DFT
            OutputFreqDomainSignal = Compute_DFT(Normalized_Signal);

            //Rounding FrequenciesList to 1 decimal point:
            for (int i = 0; i < OutputFreqDomainSignal.Frequencies.Count; i++)
                OutputFreqDomainSignal.Frequencies[i] = (float)Math.Round(OutputFreqDomainSignal.Frequencies[i], 1);

            //SAVING
            File_Path = "C:/Huda/FCIS/4th Year/1st Term/Digital Signal Processing/Tasks/Digital-Signal-Processing/DSPToolbox/DSPComponentsUnitTest/TestingSignals/PracticalTask2DFT.ds";
            Save_In_File(File_Path, 1, Convert.ToInt32(OutputFreqDomainSignal.Periodic), OutputFreqDomainSignal);

        }

        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);

            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());

            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }

            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());

                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }
    }
}
