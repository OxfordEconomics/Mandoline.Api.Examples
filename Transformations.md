### Copyright 2020 OXFORD ECONOMICS LTD.

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

# Example code in C# to perform data series transformations

```
using System;
using System.Linq;

namespace OxfordEconomicsMeasureTransformations
{
    public enum SeriesFrequencyEnum
    {
        Yearly = 0,
        Quarterly = 1
    }

    public class Transformation
    {
        public static float[] Transform(float[] variable, string measureCode, SeriesFrequencyEnum frequencyEnum)
        {
            var newData = TransformInternal(variable, frequencyEnum, measureCode);

            if (newData == null)
            {
                return null;
            }

            newData = Clean(newData);

            return newData;
        }

        private static float[] Clean(float[] newData)
        {
            for (int i = 0; i < newData.Length; i++)
            {
                if (float.IsInfinity(newData[i]))
                {
                    newData[i] = float.NaN;
                }
            }

            return newData;
        }

        private static float[] TransformInternal(float[] data, SeriesFrequencyEnum frequency, string measureCode)
        {
            switch (measureCode)
            {
                case "L": return data;
                case "D": return D(data, frequency);
                case "P": return P(data, frequency);
                case "PY": return PY(data, frequency);
                case "DY": return DY(data, frequency);
                case "GR": return GR(data, frequency);
                default: throw new Exception($"Unexpected measure: {measureCode}");
            }
        }

        private static float[] PY(float[] data, SeriesFrequencyEnum frequency)
        {
            switch (frequency)
            {
                case SeriesFrequencyEnum.Yearly:
                    return PY(data, 1);

                case SeriesFrequencyEnum.Quarterly:
                    return PY(data, 4);

                default:

                    throw new Exception($"Unexpected frequency: {frequency}");
            }
        }

        private static float[] DY(float[] data, SeriesFrequencyEnum frequency)
        {
            switch (frequency)
            {
                case SeriesFrequencyEnum.Yearly:
                    return DY(data, 1);

                case SeriesFrequencyEnum.Quarterly:
                    return DY(data, 4);

                default:
                    throw new Exception($"Unexpected frequency: {frequency}");
            }
        }

        private static float[] P(float[] data, SeriesFrequencyEnum frequency)
        {
            switch (frequency)
            {
                case SeriesFrequencyEnum.Yearly:
                    {
                        return null;
                    }
                case SeriesFrequencyEnum.Quarterly:
                    {
                        return P(data, 1);
                    }
                default:
                    throw new Exception($"Unexpected frequency: {frequency}");
            }
        }

        private static float[] D(float[] data, SeriesFrequencyEnum frequency)
        {
            switch (frequency)
            {
                case SeriesFrequencyEnum.Yearly:
                    return null;

                case SeriesFrequencyEnum.Quarterly:
                    return D(data, 1);

                default:
                    throw new Exception($"Unexpected frequency: {frequency}");
            }
        }

        private static float[] GR(float[] data, SeriesFrequencyEnum frequency)
        {
            switch (frequency)
            {
                case SeriesFrequencyEnum.Yearly:
                    {
                        return null;
                    }
                case SeriesFrequencyEnum.Quarterly:
                    {
                        float[] newData = new float[data.Length];
                        if (newData.Length > 0)
                        {
                            newData[0] = float.NaN;
                        }
                        for (int i = 1; i < data.Length; i++)
                        {
                            newData[i] = 100.0f * (SignedPow(data[i], 4) - SignedPow(data[i - 1], 4)) / Pow(data[i - 1], 4);
                        }

                        return newData;
                    }
                default:
                    throw new Exception($"Unexpected frequency: {frequency}");
            }
        }

        private static float[] DY(float[] data, int periodsInYear)
        {
            float[] newData = new float[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                newData[i] = i < periodsInYear
                    ? float.NaN
                    : data[i] - data[i - periodsInYear];
            }

            return newData;
        }

        private static float[] D(float[] data, int periodsInQuarter)
        {
            float[] newData = new float[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                newData[i] = i < periodsInQuarter
                    ? float.NaN
                    : data[i] - data[i - periodsInQuarter];
            }

            return newData;
        }

        private static float[] P(float[] data, int periodsInQuarter)
        {
            float[] newData = new float[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                newData[i] = i < periodsInQuarter
                    ? float.NaN
                    : (data[i] - data[i - periodsInQuarter]) * 100 / data[i - periodsInQuarter];
            }

            return newData;
        }

        private static float[] PY(float[] data, int periodsInYear)
        {
            float[] newData = new float[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                newData[i] = i < periodsInYear
                    ? float.NaN
                    : (data[i] - data[i - periodsInYear]) * 100 / data[i - periodsInYear];
            }

            return newData;
        }

        private static float Pow(float x, float y)
        {
            return (float)Math.Pow((double)x, (double)y);
        }

        private static float SignedPow(float x, float y)
        {
            return Pow(x, y - 1) * x;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Oxford Economics measure transformations");

            // 12 periods, ie 3 years of quarterly data
            var data = Enumerable.Range(1, 12).Select(i => (float)i).ToArray();
            WriteDataseries(data, "Raw");
            Console.WriteLine();

            foreach (var measure in new [] { "L", "DY", "PY", "D", "P", "GR" })
            {
                var transformed = Transformation.Transform(data, measure, SeriesFrequencyEnum.Quarterly);

                WriteDataseries(transformed, measure);
            }

        }

        private static void WriteDataseries(float[] data, string measure)
        {
            var formattedData = data.Select(v => $"{v,7:0.00}");

            Console.WriteLine($"{measure, 3} - {String.Join(", ", formattedData)}");
        }
    }
}

```