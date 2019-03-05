using System;
using System.Linq;
using System.Text;

namespace TelephoneCommunicator
{
    internal class Task
    {
        public readonly uint NumberOfThreads;
        public decimal[] Probabilities;
        
        public decimal AverageArrivalRate;
        public decimal AverageDepartureRate;
        public readonly decimal ServiceTimeByMinutes;
        public decimal Alpha;
        
        private bool isSolved = false;
       
        public Task(uint numberOfThreads, decimal averageArrivalRate, decimal serviceTimeByMinutes)
        {
            NumberOfThreads = numberOfThreads > 0 ? numberOfThreads : throw new ArgumentException("Number of threads should be more than 0");
            AverageArrivalRate = averageArrivalRate > 0 ? averageArrivalRate : throw new ArgumentException("Average arrival rate should be more than 0");
            ServiceTimeByMinutes = serviceTimeByMinutes > 0 ? serviceTimeByMinutes : throw new ArgumentException("Service time should be more than 0");
        }
        public void Solve()
        {
            AverageDepartureRate = 1 / ServiceTimeByMinutes;
            Alpha = AverageArrivalRate * ServiceTimeByMinutes;
            Probabilities = CalculateProbabilities();
            isSolved = true;
        }
        public string GetResult()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Result:");
            for (int i = 0; i < Probabilities.Length; ++i)
            {
                sb.AppendLine($"p{i} = {Probabilities[i]}");
            }
            sb.AppendLine($"\nProbability of refusal = {Probabilities[Probabilities.Length - 1]}");
            sb.AppendLine($"Relative bandwidth = {CalculateRelativeBandwidth()}");
            sb.AppendLine($"Absolute bandwidth = {CalculateAbsoluteBandwidth()} commmand/minute");
            sb.AppendLine($"\nAverage number of busy channel = {GetNumberOfBusyChannel()} channels");
            sb.AppendLine($"Probab of busy = {CalucalteProbabilityOfBusyOfChannel()}");
            sb.AppendLine($"Busy time = {GetBusyTimeOfChannel()} minutes");
            sb.AppendLine($"Rest time = {GetRestTimeOfChannel()} minutes");
            return sb.ToString();
        }
        public decimal CalculateAbsoluteBandwidth()
        {
            return isSolved ? AverageArrivalRate * CalculateRelativeBandwidth() : throw new InvalidOperationException("Firstly solve this task!");
        }
        public decimal CalculateRelativeBandwidth()
        {
            return isSolved ? 1 - GetNProbability() : throw new InvalidOperationException("Firstly solve this task!");
        }

        public decimal GetBusyTimeOfChannel()
        {
            return 1 / AverageDepartureRate;
        }
        public decimal GetRestTimeOfChannel()
        {
            return isSolved ? (1 - CalucalteProbabilityOfBusyOfChannel()) / (AverageDepartureRate * CalucalteProbabilityOfBusyOfChannel()) : throw new InvalidOperationException("Firstly solve this task!");
        }
        public decimal GetNumberOfBusyChannel()
        {
            return isSolved ? Alpha * (1 - GetNProbability()) : throw new InvalidOperationException("Firstly solve this task!");
        }
        public decimal CalucalteProbabilityOfBusyOfChannel()
        {
            return isSolved ? (Alpha * (1 - GetNProbability())) / NumberOfThreads : throw new InvalidOperationException("Firstly solve this task!");
        }

        private decimal[] CalculateProbabilities()
        {
            decimal[] probabilities = new decimal[NumberOfThreads + 1];
            probabilities[0] = ComputingZeroProbability();
            for (uint i = 1; i < probabilities.Length; ++i)
            {
                probabilities[i] = ComputingProbability(i, probabilities[0]);
            }
            return (double)Math.Abs(probabilities.Sum() - 1) < 0.02 ? probabilities : throw new ArithmeticException("Sum of p not 1!");
        }
        private decimal ComputingZeroProbability()
        {
            decimal sum = 0;
            for (uint k = 0; k < NumberOfThreads + 1; ++k)
            {
                sum += (decimal)(Math.Pow((double)Alpha, k) / ComputingFactorial(k));
            }
            return 1 / sum;
        }
        private decimal ComputingProbability(uint index, decimal zeroProbability)
        {
            return DivideAlphaByFactorial(index) * zeroProbability;
        }

        private decimal DivideAlphaByFactorial(uint index)
        {
            return (decimal)(Math.Pow((double)Alpha, index) / ComputingFactorial(index));
        }
        public decimal GetNProbability()
        {
            return isSolved ? Probabilities[Probabilities.Length - 1] : throw new InvalidOperationException("Firstly solve this task!");
        }
        public decimal GetZeroProbability()
        {
            return isSolved ? Probabilities[0] : throw new InvalidOperationException("Firstly solve this task!");
        }
        private long ComputingFactorial(uint n)
        {
            return n > 1 ? n * ComputingFactorial(n - 1) : 1;
        }
    }
}
