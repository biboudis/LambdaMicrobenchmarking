using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaMicrobenchmarking
{
    public class Run<T>
    {
        internal static string FORMAT =
            "{0,-25}\t{1,10:0.000000} {2,10:0.000} {3,6:0.000} {4,5} {5,6}";

        private string title;
        private double[] measurements;
        private Func<T> func;

        internal static int iterations = 10;
        internal static int warmups = 3;
        internal static double minimumSecs = 0.25;

        public Run(Tuple<String, Func<T>> runTuple)
        {
            this.measurements = new double[iterations];
            this.title = runTuple.Item1;
            this.func = runTuple.Item2;
        }

        public int GetN()
        {
            return iterations;
        }

        public double GetMean()
        {
            if (GetN() > 0)
            {
                return GetSum() / GetN();
            }
            else
            {
                return Double.NaN;
            }
        }

        public double GetSum()
        {
            if (GetN() > 0)
            {
                double s = 0;
                for (int i = 0; i < GetN(); i++)
                {
                    s += measurements[i];
                }
                return s;
            }
            else
            {
                return Double.NaN;
            }
        }

        public double GetStandardDeviation()
        {
            return Math.Sqrt(GetVariance());
        }

        public double GetVariance()
        {
            if (GetN() > 0)
            {
                double v = 0;
                double m = GetMean();
                for (int i = 0; i < iterations; i++)
                {
                    v += Math.Pow(measurements[i] - m, 2);
                }
                return v / (GetN() - 1);
            }
            else
            {
                return Double.NaN;
            }
        }

        public double GetMeanErrorAt(double confidence)
        {
            if (GetN() <= 2)
                return Double.NaN;
            var distribution = new StudentT(0, 1, GetN() - 1);
            double a = distribution.InverseCumulativeDistribution(1 - (1 - confidence) / 2);
            return a * GetStandardDeviation() / Math.Sqrt(GetN());
        }

        public void Measure()
        {
            var sw = new Stopwatch();

            // First invocation to compile method.
            for (int i = 0; i < warmups; i++)
                Compiler.ConsumeValue(func());

            int count = 1;
            double runningTime = 0;

            // This loop measures <code>iterations</code> benchmarks
            // and repeats measurement until either the entire
            // benchmark took at least <code>minimumSecs</code>
            // seconds or when it hits a repetition limit. This makes
            // measurement results highly stable, see Sestoft (2015),
            // Microbenchmarks in Java and C#.
            while (runningTime < minimumSecs && count < System.Int32.MaxValue / 2) {
                count *= 2;
                for (int j = 0; j < iterations; j++) {
                    // Force Full GC prior to execution
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();

                    // Measure <code>count</code> runs and report the
                    // average running time.
                    sw.Restart();
                    for (int i = 0; i < count; i++)
                        Compiler.ConsumeValue(func());
                    sw.Stop();

                    TimeSpan elapsed = sw.Elapsed;
                    measurements[j] = elapsed.TotalMilliseconds / count;
                    runningTime = elapsed.TotalSeconds;
                }
            }

            Console.WriteLine(FORMAT,
                title,
                GetMean(),
                GetMeanErrorAt(0.999),
                GetStandardDeviation(),
                "ms/op",
                count);
        }
    }
}
