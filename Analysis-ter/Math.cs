using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Analysistem.Math
{
    internal static class Math
    {
        public class Complex
        {
            public double Real { get; set; }
            public double Imaginary { get; set; }

            public Complex(double real, double imaginary = 0)
            {
                Real = real;
                Imaginary = imaginary;
            }

            public Complex Conj()
            {
                return new Complex(Real - Imaginary, 0);
            }

            public Complex Mult(Complex a)
            {
                return new Complex(
                    Real * a.Real - Imaginary * a.Imaginary,
                    Real * a.Imaginary + Imaginary * a.Real
                );
            }

            public static Complex Add(Complex a, Complex b)
            {
                return new Complex(a.Real + b.Real, a.Imaginary + b.Imaginary);
            }

            public static Complex Subtract(Complex a, Complex b)
            {
                return new Complex(a.Real - b.Real, a.Imaginary - b.Imaginary);
            }

            public static Complex Multiply(Complex a, Complex b)
            {
                return new Complex(
                    a.Real * b.Real - a.Imaginary * b.Imaginary,
                    a.Real * b.Imaginary + a.Imaginary * b.Real
                );
            }

            public static Complex Divide(Complex a, Complex b)
            {
                double denominator = b.Real * b.Real + b.Imaginary * b.Imaginary;
                return new Complex(
                    (a.Real * b.Real + a.Imaginary * b.Imaginary) / denominator,
                    (a.Imaginary * b.Real - a.Real * b.Imaginary) / denominator
                );
            }

            public static Complex Conjugate(Complex a)
            {
                return new Complex(a.Real, -a.Imaginary);
            }
        }

        static List<double> Fft(List<double> signal)
        {
            var length = signal.Count;
            var fft = new List<double>((IEnumerable<double>)Enumerable.Range(0, length));

            var omega = -2 * System.Math.PI / length;

            // iterate over each stage of the FFT
            for (int stage = 1; stage < length; stage *= 2)
            {
                // split FFT into even and odd components
                var even = new List<double>();
                var odd = new List<double>();
                for (int i = 0; i < length; i++)
                {
                    if (i % (2 * stage) < stage)
                    {
                        even.Add(fft[i]);
                    }
                    else
                    {
                        odd.Add(fft[i]);
                    }
                }

                // combine even and odd components
                for (int i = 0; i < length / 2; i++)
                {
                    var oddComponent = odd[i] * System.Math.Exp(omega * i * i);
                    fft[i] = even[i] + oddComponent;
                    fft[i + length / 2] = even[i] - oddComponent;
                }
            }

            return fft;
        }

        static List<double> Ifft(List<double> signal)
        {
            var length = signal.Count;
            var ifft = Enumerable.Repeat(0.0, length).ToList();

            // use Parallel.ForEach to perform the calculation in parallel
            Parallel.ForEach(Partitioner.Create(0, length / 2), range =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    var value = signal[i] / length;
                    ifft[i] = value;
                    ifft[length - i - 1] = value;
                }
            });

            return ifft;
        }

        public static double[] CalculateCrossCorrelation(List<double> signal1, List<double> signal2)
        {
            int signal1Length = signal1.Count;
            int signal2Length = signal2.Count;
            int length = signal1Length + signal2Length - 1;

            // pad signal1 and signal2 with zeros to make them the same length
            double[] signal1Padded = new double[length];
            signal1.CopyTo(signal1Padded, 0);
            double[] signal2Padded = new double[length];
            signal2.CopyTo(signal2Padded, 0);

            // calculate Fourier transforms of signal1 and signal2
            List<double> signal1Fft = Fft(signal1Padded.ToList());
            List<double> signal2Fft = Fft(signal2Padded.ToList());

            // calculate cross-correlation by taking the inverse Fourier transform of the product of the Fourier transforms of signal1 and signal2
            List<double> crossCorrelation = Ifft(
                (List<double>)signal1Fft.Select((s, i) => new Complex(s).Mult(new Complex(signal2Fft[i]).Conj()).Real)
            );

            // return real part of cross-correlation
            return crossCorrelation.ToArray();
        }

        public static List<double> CalculateTimeVector(int length, double samplingFrequency)
        {
            // create an array of indices from 0 to length - 1
            var indexArray = Enumerable.Range(0, length).ToList();

            // calculate time vector by dividing each index by the sampling frequency
            return indexArray.Select(i => i / samplingFrequency).ToList();
        }

        /**
         * 
         * how to use (allegedly)::
         * 
         *  - read in force and motion data
         *  
         *  - call CalculateTimeVector on both, passing their lengths and sample rates as parameters
         *  - call CalculateCrossCorrelation passing in the signals (the signals are the time columns of each file!!)
         *  
         *  - Find maximum value in cross-correlation
         *  - Get the index of that value
         *  - Calculate the time offset by taking the difference of the force and motion time vectors using the max value's index
         *  
         *  - adjust time column in motion data by adding time offset to all cells
         * 
         * 
         * JS Psuedo-code::
         *      
                const forceData = readCsv('force_data.csv');
                const motionData = readCsv('motion_data.csv');

                // calculate time vectors
                const forceTimeVector = calculateTimeVector(forceData.length, forceData.sampleRate);
                const motionTimeVector = calculateTimeVector(motionData.length, motionData.sampleRate);

                // calculate cross-correlation between force and motion data
                const crossCorrelation = calculateCrossCorrelation(forceData.signal, motionData.signal);

                // find maximum value in cross-correlation
                const maxCrossCorrelation = Math.max(...crossCorrelation);

                // find index of maximum value in cross-correlation
                const maxCrossCorrelationIndex = crossCorrelation.indexOf(maxCrossCorrelation);

                // calculate time offset between force and motion data
                const timeOffset = motionTimeVector[maxCrossCorrelationIndex] - forceTimeVector[maxCrossCorrelationIndex];

                // adjust time column in motion data by adding time offset
                const adjustedMotionTimeVector = motionTimeVector.map(t => t + timeOffset);

                // update time column in motion data with adjusted time values
                motionData.time = adjustedMotionTimeVector;

                // save synchronized data to new CSV files
                writeCsv('synchronized_force_data.csv', forceData);
                writeCsv('synchronized_motion_data.csv', motionData);
         */

        //static List<double> Fft(List<double> signal)
        //{
        //    var length = signal.Count;
        //    var fft = signal;

        //    var omega = -2 * System.Math.PI / length;

        //    // iterate over each stage of the FFT
        //    for (int stage = 1; stage < length; stage *= 2)
        //    {
        //        // split FFT into even and odd components
        //        var even = fft.Where((_, i) => i % (2 * stage) < stage);
        //        var odd = fft.Where((_, i) => i % (2 * stage) >= stage);

        //        // combine even and odd components
        //        fft = Enumerable.Repeat(0.0, length).ToList(); ;
        //        for (int i = 0; i < length / 2; i++)
        //        {
        //            var oddComponent = odd.ElementAt(i) * System.Math.Exp(omega * i * i);
        //            fft[i] = even.ElementAt(i) + oddComponent;
        //            fft[i + length / 2] = even.ElementAt(i) - oddComponent;
        //        }
        //    }

        //    return fft;

        //}
        //static List<double> Ifft(List<double> signal)
        //{
        //    var length = signal.Count;
        //    var ifft = Enumerable.Repeat(0.0, length).ToList();
        //    for (int i = 0; i < length / 2; i++)
        //    {
        //        var value = signal[i] / length;
        //        ifft[i] = value;
        //        ifft[length - i - 1] = value;
        //    }

        //    return ifft;
        //}

        //public static double[] CalculateCrossCorrelation(double[] signal1, double[] signal2)
        //{
        //    int signal1Length = signal1.Length;
        //    int signal2Length = signal2.Length;
        //    int length = signal1Length + signal2Length - 1;

        //    // pad signal1 and signal2 with zeros to make them the same length
        //    double[] signal1Padded = new double[length];
        //    signal1.CopyTo(signal1Padded, 0);
        //    double[] signal2Padded = new double[length];
        //    signal2.CopyTo(signal2Padded, 0);

        //    // calculate Fourier transforms of signal1 and signal2
        //    List<double> signal1Fft = Fft(signal1Padded.ToList());
        //    List<double> signal2Fft = Fft(signal2Padded.ToList());

        //    // calculate cross-correlation by taking the inverse Fourier transform of the product of the Fourier transforms of signal1 and signal2
        //    List<double> crossCorrelation = Ifft(
        //        (List<double>)signal1Fft.Select((s, i) => new Complex(s).Mult(new Complex(signal2Fft[i]).Conj()).Real)
        //    );

        //    // return real part of cross-correlation
        //    return crossCorrelation.ToArray();
        //}

    }
}
