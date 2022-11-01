using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }

        public override void Run()
        {
            if (InputLevel <= 0)
                InputLevel = (int)Math.Pow(2, (double)InputNumBits);
            else if (InputNumBits <= 0)
                InputNumBits = (int)Math.Log(InputLevel, 2);

            float max = InputSignal.Samples[0], min = InputSignal.Samples[0];
            for (int i = 0; i < InputSignal.Samples.Count; ++i)
            {
                if (max < InputSignal.Samples[i])
                    max = InputSignal.Samples[i];
                if (min > InputSignal.Samples[i])
                    min = InputSignal.Samples[i];
            }
            float result = (max - min) / (float)InputLevel;
            List<float> borders = new List<float>();
            List<float> midPoint = new List<float>();
            float x = min;
            for (int i = 0; i < InputLevel; ++i)
            {
                if (i == 0)
                    borders.Add(x);
                float a = x;
                x += (float)result;
                borders.Add(x);
                float b = x;
                midPoint.Add(((float)(a + b) / (float)2));
            }

            List<float> quantized = new List<float>();
            OutputSamplesError = new List<float>();
            OutputIntervalIndices = new List<int>();
            for (int i = 0; i < InputSignal.Samples.Count; ++i)
            {
                for (int j = 0; j < borders.Count; ++j)
                {
                    if (j < borders.Count - 1 && (((float)borders[j] < (float)InputSignal.Samples[i] && (float)borders[j + 1] >= (float)InputSignal.Samples[i]) ||
                        ((float)borders[j] <= (float)InputSignal.Samples[i] && (float)borders[j + 1] > (float)InputSignal.Samples[i])))
                    {
                        float mid = midPoint[(j + j + 1) / 2];
                        quantized.Add(mid);
                        OutputSamplesError.Add((float)((float)mid - (float)InputSignal.Samples[i]));
                        int indics = ((j + j + 1) / 2) + 1;
                        OutputIntervalIndices.Add(indics);
                        break;
                    }
                    else if (j == borders.Count - 1 && InputSignal.Samples[i] > borders[j])
                    {
                        float mid = midPoint[midPoint.Count - 1];
                        quantized.Add(mid);
                        OutputSamplesError.Add(mid - InputSignal.Samples[i]);
                        OutputIntervalIndices.Add((midPoint.Count));
                        break;
                    }
                }
            }
            OutputQuantizedSignal = new Signal(quantized, false);

            OutputEncodedSignal = new List<string>();
            for (int i = 0; i < OutputIntervalIndices.Count; ++i)
            {
                string conv = "";
                int a = OutputIntervalIndices[i] - 1;
                while (a != 0)
                {
                    int r = a % 2;
                    if (r == 1) conv += '1';
                    else conv += '0';
                    a /= 2;
                }
                string res = "";
                if (conv.Length < InputNumBits)
                {
                    for (int j = 0; j < InputNumBits - conv.Length; ++j)
                        res += '0';
                }
                if (conv.Length > 0)
                {
                    for (int j = conv.Length - 1; j >= 0; --j)
                    {
                        res += conv[j];
                    }
                }
                OutputEncodedSignal.Add(res);
            }
        }
    }
}
