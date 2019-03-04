using System;
using System.Linq;

namespace TelephoneCommunicator
{
    internal class Task
    {
        public decimal AverageArrivalRate;
        public decimal AverageDepartureRate;
        public decimal Alpha;
        public decimal[] Probabilities;
        public readonly decimal ServiceTimeByMinutes;
        public readonly uint NumberOfThreads;

        public Task(uint numberOfThreads, decimal serviceTimeByMinutes, decimal averageArrivalRate, decimal averageDepartureRate)
        {
            NumberOfThreads = numberOfThreads;
            Probabilities = new decimal[NumberOfThreads + 1];
            ServiceTimeByMinutes = serviceTimeByMinutes;
            AverageArrivalRate = averageArrivalRate;
            AverageDepartureRate = averageDepartureRate;
            Alpha = AverageArrivalRate / AverageDepartureRate;
            Probabilities = GetProbabilities();
        }

        private decimal[] GetProbabilities()
        {
            decimal[] probabilities = new decimal[Probabilities.Length];
            probabilities[0] = ComputingZeroProbability();
            for (uint i = 1; i < Probabilities.Length; ++i)
            {
                probabilities[i] = ComputingProbability(i, probabilities[0]);
            }
            return (double)Math.Abs(probabilities.Sum() - 1) < 0.02 ? probabilities : throw new ArithmeticException("Sum of p not 1!");
        }
        public decimal ComputingZeroProbability()
        {
            decimal sum = 0;
            for (uint k = 0; k < Probabilities.Length; ++k)
            {
                sum += (decimal)(Math.Pow((double)Alpha, k) / ComputingFactorial(k));
            }
            return 1 / sum;
        }
        public decimal ComputingProbability(uint index, decimal zeroProbability)
        {
            return DivideAlphaByFactorial(index) * zeroProbability;
        }
        private decimal DivideAlphaByFactorial(uint index)
        {
            return (decimal)(Math.Pow((double)Alpha, index) / ComputingFactorial(index));
        }
        public decimal GetNProbability()
        {
            return Probabilities.Last();
        }
        public decimal GetZeroProbability()
        {
            return Probabilities[0];
        }
        public long ComputingFactorial(uint n)
        {
            return n > 1 ? n * ComputingFactorial(n - 1) : 1;
        }
    }
}
