using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;

namespace AnalysystemTakeTwo
{
    internal static class Math
    { 
        /// <summary> Courtesy of ChatGPT:
        ///     This code defines a helper function named BitReversal that performs bit 
        ///     reversal on an integer. Bit reversal is a mathematical operation that reverses 
        ///     the order of the bits in a binary number. For example, the number 1010 would 
        ///     become 0101 after bit reversal. This function is used in some implementations 
        ///     of the FFT algorithm to rearrange the elements of a signal in a specific 
        ///     order that is required for the calculation of the DFT. This can be an efficient 
        ///     way to calculate the DFT of a signal because it takes advantage of the symmetry 
        ///     and regularity of the transform.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        static int BitReverse(int n, int length)
        {
            int reversed = 0;
            for (int i = 0; i < length; i++)
            {
                reversed = (reversed << 1) | (n & 1);
                n >>= 1;
            }
            return reversed;
        }

        public static Complex[] BitReversal(Complex[] x)
        {
            int N = x.Length;
            Complex[] y = new Complex[N];

            // Pre-compute the bit-reversed indices
            int[] indices = Enumerable.Range(0, N).Select(i => BitReverse(i, N)).ToArray();

            for (int i = 0; i < N; i++)
            {
                // Permute the elements of the input array using the pre-computed indices
                y[indices[i]] = x[i];
            }

            return y;
        }

        private static Complex[] PadInput(Complex[] inputSignal)
        {
            // Compute the length of the input signal
            int inputLength = inputSignal.Length;

            // Edge case: 1-point FFT
            if (inputLength == 1)
            {
                return inputSignal;
            }

            // Compute the next power of 2 that is greater than or equal to N
            int paddedLength = (int)System.Math.Ceiling(System.Math.Log(inputLength, 2));
            paddedLength = (int)System.Math.Pow(2, paddedLength);

            // Zero-pad the input signal to length 2 * N - 1
            Complex[] paddedInput = new Complex[paddedLength];
            for (int i = 0; i < paddedLength; i++)
            {
                if (i < inputLength)
                {
                    paddedInput[i] = inputSignal[i];
                }
                else
                {
                    paddedInput[i] = 0;
                }
            }

            return paddedInput;
        }

        // Implementation of the Cooley-Tukey algorithm for computing the FFT of a small input signal
        private static Complex[] CooleyTukeyFft(Complex[] inputSignal)
        {
            inputSignal = BitReversal(inputSignal);
            Complex[] paddedInput = PadInput(inputSignal);
            int paddedLength = paddedInput.Length;
            int inputLength = inputSignal.Length;

            // Compute the FFT of the zero-padded input signal using the Cooley-Tukey algorithm
            Complex[] result = new Complex[paddedLength];
            for (int k = 0; k < paddedLength; k++)
            {
                // Compute the k-th output sample
                result[k] = 0;
                for (int n = 0; n < paddedLength; n++)
                {
                    result[k] += paddedInput[n] * Complex.Exp(-2 * System.Math.PI * k * n / paddedLength);
                }
            }

            // Extract the first N samples of the result
            Complex[] output = new Complex[inputLength];
            for (int k = 0; k < inputLength; k++)
            {
                output[k] = result[k];
            }

            return BitReversal(output);
        }

        public static Complex[] Radix2Fft(Complex[] inputSignal)
        {
            // Compute the length of the input signal
            int inputLength = inputSignal.Length;

            // Edge case: 1-point FFT
            if (inputLength == 1)
            {
                return inputSignal;
            }

            // Check if the input length is a power of two
            if ((inputLength & (inputLength - 1)) == 0)
            {
                return CooleyTukeyFft(inputSignal);
            }
            else
            {
                // Compute the FFT of the even and odd samples of the input signal
                Complex[] evenSamples = new Complex[inputLength / 2];
                Complex[] oddSamples = new Complex[inputLength / 2];
                for (int i = 0; i < inputLength / 2; i++)
                {
                    evenSamples[i] = inputSignal[2 * i];
                    oddSamples[i] = inputSignal[2 * i + 1];
                }
                evenSamples = Radix2Fft(evenSamples);
                oddSamples = Radix2Fft(oddSamples);

                // Combine the results of the FFTs of the even and odd samples
                Complex[] output = new Complex[inputLength];
                for (int k = 0; k < inputLength / 2; k++)
                {
                    output[k] = evenSamples[k] + Complex.Exp(-2 * System.Math.PI * k / inputLength) * oddSamples[k];
                    output[k + inputLength / 2] = evenSamples[k] - Complex.Exp(-2 * System.Math.PI * k / inputLength) * oddSamples[k];
                }

                return output;
            }
        }

        // implementation based on a hybrid Bluestein's algorithm.
        // small inputs get passed into the CooleyTukey method which performs better with that.
        // Computing FFT of the product of the chirp z-transform and the chirp sequence is handled
        //  by a hybrid Radix-2 algorithm which will pass the signal on to the CooleyTukey method
        //  if the length of the signal is a power of two.
        public static Complex[] Fft(Complex[] inputSignal)
        {
            // Compute the length of the input signal
            int inputLength = inputSignal.Length;

            // Use the Cooley-Tukey algorithm for computing the FFT of small inputs
            if (inputLength <= 32)
            {
                return CooleyTukeyFft(inputSignal);
            }

            Complex[] paddedInput = PadInput(inputSignal);
            int paddedLength = paddedInput.Length;

            // Compute the chirp z-transform of the zero-padded input signal
            Complex[] chirpZTransform = new Complex[paddedLength];
            Parallel.For(0, 2 * inputLength - 1, k =>
            {
                chirpZTransform[k] = paddedInput[k] * new Complex(System.Math.Pow(-1, k), 0);
            });

            // Compute the chirp sequence
            Complex[] chirpSequence = new Complex[paddedLength];
            chirpSequence[0] = 1;
            Parallel.For(1, paddedLength, k =>
            {
                chirpSequence[k] = Complex.Exp(-2 * System.Math.PI * k * k / inputLength);
            });

            // Multiply the chirp z-transform by the chirp sequence
            Parallel.For(0, paddedLength, k =>
            {
                chirpZTransform[k] = chirpZTransform[k] * chirpSequence[k];
            });

            // Compute the FFT of the product of the chirp z-transform and the chirp sequence
            chirpZTransform = Radix2Fft(chirpZTransform);

            // Multiply the FFT by the conjugate of the chirp sequence
            Parallel.For(0, paddedLength, k =>
            {
                chirpZTransform[k] = chirpZTransform[k] * Complex.Conjugate(chirpSequence[k]);
            });

            // Extract the first N samples of the result
            Complex[] result = new Complex[inputLength];
            Parallel.For(0, paddedLength, k =>
            {
                result[k] = chirpZTransform[k];
            });

            return result;
        }

        /// <summary>
        ///     This code defines a method named Ifft that performs an inverse fast Fourier 
        ///     transform (IFFT) on a given signal. The IFFT is the inverse of the FFT, and it 
        ///     can be used to transform a signal from the frequency domain back to the time 
        ///     domain. This implementation of the IFFT uses the BitReversal function defined in 
        ///     the previous code snippet to rearrange the elements of the signal, and then applies 
        ///     the conjugate of each element and divides the result by the length of the signal 
        ///     to calculate the IFFT.
        /// </summary>
        /// <param name="signal"></param>
        /// <returns></returns>
        static Complex[] Ifft(Complex[] signal)
        {
            int length = signal.Length;
            var ifft = new Complex[length];

            // compute the inverse FFT
            for (int i = 0; i < length; i++)
            {
                ifft[BitReverse(i, length)] = Complex.Multiply(Complex.Conjugate(signal[i]), (new Complex(1.0 / length, 0)));
            }

            return ifft;
        }

        /// <summary> Courtesy of ChatGPT:
        ///     This code defines a method named CalculateCrossCorrelation that calculates the 
        ///     cross-correlation of two signals. Cross-correlation is a mathematical operation 
        ///     that measures the similarity between two signals as a function of the time-lag 
        ///     between them. This implementation of cross-correlation first pads the two input 
        ///     signals with zeros to make them the same length, then calculates the Fourier 
        ///     transforms of both signals using the Fft method defined in the previous code 
        ///     snippet. Finally, it calculates the cross-correlation by taking the inverse 
        ///     Fourier transform of the product of the two Fourier transforms using the Ifft 
        ///     method. The result is an array of real numbers that represents the cross-correlation 
        ///     of the two input signals.
        /// </summary>
        /// <param name="signal1"></param>
        /// <param name="signal2"></param>
        /// <returns></returns>
        public static double[] CalculateCrossCorrelation(double[] signal1, double[] signal2)
        {
            int length = signal1.Length + signal2.Length - 1;

            // pad signal1 and signal2 with zeros to make them the same length
            Complex[] signal1Padded = new Complex[length];
            signal1.CopyTo(signal1Padded, 0);
            Complex[] signal2Padded = new Complex[length];
            signal2.CopyTo(signal2Padded, 0);

            // calculate Fourier transforms of signal1 and signal2
            Complex[] signal1Fft = Fft(signal1Padded);
            Complex[] signal2Fft = Fft(signal2Padded);

            // calculate cross-correlation by taking the inverse Fourier transform of the product of the Fourier transforms of signal1 and signal2
            Complex[] crossCorrelation = Ifft(
                (Complex[])signal1Fft.Select((s, i) => Complex.Multiply(s, Complex.Conjugate(signal2Fft[i])))
            );

            // return real part of cross-correlation
            return crossCorrelation.Select((cC) => cC.Real).ToArray();
        }

        /// <summary> Courtesy of ChatGPT:
        ///     This code defines a method named CalculateTimeVector that calculates a time vector 
        ///     for a given length and sampling frequency. A time vector is an array of numbers 
        ///     that represents the time at each point of a signal. This implementation of 
        ///     CalculateTimeVector creates an array of the specified length, and then calculates
        ///     the time at each point of the signal by iterating over the array and multiplying 
        ///     the index by the time step, which is calculated as the inverse of the sampling 
        ///     frequency. The result is an array of doubles that represents the time at each 
        ///     point of the signal.
        /// </summary>
        /// <param name="length"></param>
        /// <param name="samplingFrequency"></param>
        /// <returns></returns>
        public static double[] CalculateTimeVector(int length, double samplingFrequency)
        {
            double[] timeVector = new double[length];
            double timeStep = 1 / samplingFrequency;

            for (int i = 0; i < length; i++)
            {
                timeVector[i] = i * timeStep;
            }

            return timeVector;
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




        /// <summary> Courtesy of ChatGPT:
        ///     This code defines a Complex class that represents complex 
        ///     numbers in C#. A complex number is a number of the form a + bi, 
        ///     where a is the real part and b is the imaginary part. This 
        ///     class has properties for the real and imaginary parts of the 
        ///     complex number, and several methods that allow you to add, 
        ///     subtract, multiply, and divide complex numbers, as well as 
        ///     calculate the conjugate of a complex number.
        /// </summary>
        //private static Complex[] StockhamAutocorrelation(Complex[] inputSignal)
        //{
        //    // Compute the length of the input signal
        //    int inputLength = inputSignal.Length;

        //    // Compute the next power of 2 that is greater than or equal to N
        //    int paddedLength = (int)System.Math.Ceiling(System.Math.Log(inputLength, 2));
        //    paddedLength = (int)System.Math.Pow(2, paddedLength);

        //    // Zero-pad the input signal to length 2 * N - 1
        //    Complex[] paddedInput = new Complex[paddedLength];
        //    for (int i = 0; i < paddedLength; i++)
        //    {
        //        if (i < inputLength)
        //        {
        //            paddedInput[i] = inputSignal[i];
        //        }
        //        else
        //        {
        //            paddedInput[i] = 0;
        //        }
        //    }

        //    // Compute the autocorrelation of the zero-padded input signal using the Stockham algorithm
        //    Complex[] result = new Complex[paddedLength];
        //    int halfLength = paddedLength / 2;
        //    for (int i = 0; i < halfLength; i++)
        //    {
        //        result[i] = 0;
        //        for (int j = 0; j < halfLength; j++)
        //        {
        //            result[i] += paddedInput[j] * paddedInput[j + halfLength];
        //        }
        //        result[i + halfLength] = 0;
        //        for (int j = 0; j < halfLength; j++)
        //        {
        //            result[i + halfLength] += paddedInput[j] * paddedInput[j + halfLength - i];
        //        }
        //    }

        //    return result;
        //}


        //public class Complex
        //{
        //    public double Real { get; set; }
        //    public double Imaginary { get; set; }

        //    public Complex(double real, double imaginary = 0)
        //    {
        //        Real = real;
        //        Imaginary = imaginary;
        //    }

        //    public Complex Conj()
        //    {
        //        return new Complex(Real - Imaginary, 0);
        //    }

        //    public Complex Mult(Complex a)
        //    {
        //        return new Complex(
        //            Real * a.Real - Imaginary * a.Imaginary,
        //            Real * a.Imaginary + Imaginary * a.Real
        //        );
        //    }

        //    public static Complex Add(Complex a, Complex b)
        //    {
        //        return new Complex(a.Real + b.Real, a.Imaginary + b.Imaginary);
        //    }

        //    public static Complex Subtract(Complex a, Complex b)
        //    {
        //        return new Complex(a.Real - b.Real, a.Imaginary - b.Imaginary);
        //    }

        //    public static Complex Multiply(Complex a, Complex b)
        //    {
        //        return new Complex(
        //            a.Real * b.Real - a.Imaginary * b.Imaginary,
        //            a.Real * b.Imaginary + a.Imaginary * b.Real
        //        );
        //    }

        //    public static Complex Divide(Complex a, Complex b)
        //    {
        //        double denominator = b.Real * b.Real + b.Imaginary * b.Imaginary;
        //        return new Complex(
        //            (a.Real * b.Real + a.Imaginary * b.Imaginary) / denominator,
        //            (a.Imaginary * b.Real - a.Real * b.Imaginary) / denominator
        //        );
        //    }

        //    public static Complex Conjugate(Complex a)
        //    {
        //        return new Complex(a.Real, -a.Imaginary);
        //    }
        //}

        /// <summary> Courtesy of ChatGPT:
        ///     This code defines a method named Fft that performs a fast Fourier 
        ///     transform (FFT) on a given signal. The FFT is an algorithm that is 
        ///     used to calculate the discrete Fourier transform (DFT) of a signal. 
        ///     The DFT is a mathematical representation of a signal in the frequency 
        ///     domain, which can be used to analyze and process the signal in various 
        ///     ways. This implementation of the FFT uses a divide-and-conquer approach 
        ///     to calculate the DFT of the given signal in an efficient manner.
        /// </summary>
        /// <param name="signal"></param>
        /// <returns></returns>
        // RADIX-2 FFT
        // POTENTIAL FOR INACCURACY DUE TO NEEDING PADDING
        //static Complex[] Fft(Complex[] signal)
        //{
        //    int length = signal.Length;
        //    Complex[] fft = new Complex[length];

        //    double omega = -2 * System.Math.PI / length;

        //    // iterate over each stage of the FFT
        //    for (int stage = 1; stage < length; stage *= 2)
        //    {
        //        // split FFT into even and odd components
        //        var even = new List<Complex>();
        //        var odd = new List<Complex>();
        //        for (int i = 0; i < length; i++)
        //        {
        //            if (i % (2 * stage) < stage)
        //            {
        //                even.Add(signal[i]);
        //            }
        //            else
        //            {
        //                odd.Add(signal[i]);
        //            }
        //        }

        //        // combine even and odd components
        //        for (int i = 0; i < length / 2; i++)
        //        {
        //            Complex oddComponent = odd[i].Mult(new Complex(System.Math.Exp(omega * i)));
        //            fft[i] = Complex.Add(even[i], oddComponent);
        //            fft[i + length / 2] = Complex.Subtract(even[i], oddComponent);
        //        }
        //    }

        //    return fft;
        //}

        //static Complex[] Ifft(Complex[] signal)
        //{
        //    int length = signal.Length;
        //    Complex[] ifft = new Complex[length];

        //    // use Parallel.ForEach to perform the calculation in parallel
        //    Parallel.ForEach(Partitioner.Create(0, length), range =>
        //    {
        //        for (int i = range.Item1; i < range.Item2; i++)
        //        {
        //            ifft[i] = Complex.Divide(signal[i], new Complex(length));
        //        }
        //    });

        //    return ifft;
        //}

        //static double[] Fft(double[] signal)
        //{
        //    int length = signal.Length;
        //    double[] fft = new double[length];

        //    double omega = -2 * System.Math.PI / length;

        //    // iterate over each stage of the FFT
        //    for (int stage = 1; stage < length; stage *= 2)
        //    {
        //        // split FFT into even and odd components
        //        var even = new List<double>();
        //        var odd = new List<double>();
        //        for (int i = 0; i < length; i++)
        //        {
        //            if (i % (2 * stage) < stage)
        //            {
        //                even.Add(fft[i]);
        //            }
        //            else
        //            {
        //                odd.Add(fft[i]);
        //            }
        //        }

        //        // combine even and odd components
        //        for (int i = 0; i < length / 2; i++)
        //        {
        //            double oddComponent = odd[i] * System.Math.Exp(omega * i * i);
        //            fft[i] = even[i] + oddComponent;
        //            fft[i + length / 2] = even[i] - oddComponent;
        //        }
        //    }

        //    return fft;

        //}
        //static double[] Ifft(double[] signal)
        //{
        //    int length = signal.Length;
        //    double[] ifft = new double[length];

        //    // use Parallel.ForEach to perform the calculation in parallel
        //    Parallel.ForEach(Partitioner.Create(0, length / 2), range =>
        //    {
        //        for (int i = range.Item1; i < range.Item2; i++)
        //        {
        //            double value = signal[i] / length;
        //            ifft[i] = value;
        //            ifft[length - i - 1] = value;
        //        }
        //    });

        //    return ifft;
        //}

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
