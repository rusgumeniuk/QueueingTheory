using QueueingTheoryLibrary;

using System;
using System.Linq;
using System.Text;

namespace TelephoneCommunicator
{
    internal class Task : ITask
    {
        public readonly uint NumberOfThreads;
        public decimal[] Probabilities;

        public decimal AverageArrivalRate;
        public decimal AverageDepartureRate;
        public readonly decimal ServiceTimeByMinutes;
        public decimal Alpha;

        public bool IsSolved { get; set; } = false;

        public Task(uint numberOfThreads, decimal averageArrivalRate, decimal serviceTimeBySeconds)
        {
            NumberOfThreads = numberOfThreads > 0 ? numberOfThreads : throw new ArgumentException("Number of threads should be more than 0");
            AverageArrivalRate = averageArrivalRate > 0 ? averageArrivalRate : throw new ArgumentException("Average arrival rate should be more than 0");
            ServiceTimeByMinutes = serviceTimeBySeconds > 0 ? serviceTimeBySeconds / 60 : throw new ArgumentException("Service time should be more than 0");
        }
        public void Solve()
        {
            try
            {
                AverageDepartureRate = 1 / ServiceTimeByMinutes;
                Alpha = AverageArrivalRate * ServiceTimeByMinutes;
                Probabilities = CalculateProbabilities();
                IsSolved = true;
            }
            catch (Exception ex)
            {
                IsSolved = false;
                throw ex;
            }            
        }

        public string GetResult()
        {
            if (!IsSolved) return "Для отримання результату спершу розв'яжіть задачу!";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Ймовірність відмови = {Probabilities[Probabilities.Length - 1]}");
            sb.AppendLine($"Відносна пропускна здатність (Q) = {CalculateRelativeBandwidth()}");
            sb.AppendLine($"Абсолютна пропускна здатність (A) = {CalculateAbsoluteBandwidth()} команд/хвилину");
            sb.AppendLine($"\nСереднє число зайнятих каналів = {GetNumberOfBusyChannel()} каналів");
            sb.AppendLine($"Ймовірність зайнятості каналу = {CalucalteProbabilityOfBusyOfChannel()}");
            sb.AppendLine($"Час зайнятості каналу = {GetBusyTimeOfChannel()} хвилин");
            sb.AppendLine($"Час простою каналу = {GetRestTimeOfChannel()} хвилин");
            return sb.ToString();
        }
        public string GetProbabilities()
        {
            if (!IsSolved) return "Спочатку розв'яжіть задачу!";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Ймовірності:");
            for (uint i = 0; i < Probabilities.Length; ++i)
            {
                sb.AppendLine($"p{i} = {Probabilities[i]}");
            }
            return sb.ToString();
        }

        public decimal CalculateAbsoluteBandwidth()
        {
            return IsSolved ? AverageArrivalRate * CalculateRelativeBandwidth() : throw new InvalidOperationException("Firstly solve this task!");
        }
        public decimal CalculateRelativeBandwidth()
        {
            return IsSolved ? 1 - GetNProbability() : throw new InvalidOperationException("Firstly solve this task!");
        }

        public decimal GetBusyTimeOfChannel()
        {
            return 1 / AverageDepartureRate;
        }
        public decimal GetRestTimeOfChannel()
        {
            return IsSolved ? (1 - CalucalteProbabilityOfBusyOfChannel()) / (AverageDepartureRate * CalucalteProbabilityOfBusyOfChannel()) : throw new InvalidOperationException("Firstly solve this task!");
        }
        public decimal GetNumberOfBusyChannel()
        {
            return IsSolved ? Alpha * (1 - GetNProbability()) : throw new InvalidOperationException("Firstly solve this task!");
        }
        public decimal CalucalteProbabilityOfBusyOfChannel()
        {
            return IsSolved ? (Alpha * (1 - GetNProbability())) / NumberOfThreads : throw new InvalidOperationException("Firstly solve this task!");
        }

        private decimal[] CalculateProbabilities()
        {
            decimal[] probabilities = new decimal[NumberOfThreads + 1];
            probabilities[0] = ComputingZeroProbability();
            for (uint i = 1; i < probabilities.Length; ++i)
            {
                probabilities[i] = ComputingProbability(i, probabilities[0]);
            }
            return (double)Math.Abs(probabilities.Sum() - 1) < 0.00001 ? probabilities : throw new ArithmeticException("Sum of p not 1!");
        }
        private decimal ComputingZeroProbability()
        {
            decimal sum = 0;
            for (uint k = 0; k < NumberOfThreads + 1; ++k)
            {
                sum += QueueingTheoryCalculator.DivideNumberByFactorial(Alpha, k);
            }
            return 1 / sum;
        }
        private decimal ComputingProbability(uint index, decimal zeroProbability)
        {
            return QueueingTheoryCalculator.DivideNumberByFactorial(Alpha, index) * zeroProbability;
        }

        public decimal GetNProbability()
        {
            return IsSolved ? Probabilities[Probabilities.Length - 1] : throw new InvalidOperationException("Firstly solve this task!");
        }
        public decimal GetZeroProbability()
        {
            return IsSolved ? Probabilities[0] : throw new InvalidOperationException("Firstly solve this task!");
        }
    }
}
