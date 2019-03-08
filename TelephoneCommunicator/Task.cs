using System;
using System.Linq;
using System.Numerics;
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

        public Task(uint numberOfThreads, decimal averageArrivalRate, decimal serviceTimeBySeconds)
        {
            NumberOfThreads = numberOfThreads > 0 ? numberOfThreads : throw new ArgumentException("Number of threads should be more than 0");
            AverageArrivalRate = averageArrivalRate > 0 ? averageArrivalRate : throw new ArgumentException("Average arrival rate should be more than 0");
            ServiceTimeByMinutes = serviceTimeBySeconds > 0 ? serviceTimeBySeconds / 60 : throw new ArgumentException("Service time should be more than 0");
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
            if (!isSolved) return "Для отримання результату спершу розв'яжіть задачу!";
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
            if (!isSolved) return "Спочатку розв'яжіть задачу!";
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
            return (double)Math.Abs(probabilities.Sum() - 1) < 0.00001 ? probabilities : throw new ArithmeticException("Sum of p not 1!");
        }
        private decimal ComputingZeroProbability()
        {
            decimal sum = 0;
            for (uint k = 0; k < NumberOfThreads + 1; ++k)
            {
                sum += DivideAlphaByFactorial(k);
            }
            return 1 / sum;
        }
        private decimal ComputingProbability(uint index, decimal zeroProbability)
        {
            return DivideAlphaByFactorial(index) * zeroProbability;
        }

        private decimal DivideAlphaByFactorial(uint index)
        {
            if (Math.Pow((double)Alpha, index) < 0)
                return (decimal)Math.Exp(BigInteger.Log(new BigInteger(Math.Pow((double)Alpha, index))) - BigInteger.Log(ComputingFactorial(index)));
            else if (ComputingFactorial(index) > 0)
                return DivideByBigInteger(Math.Pow((double)Alpha, index), ComputingFactorial(index));
            else
                throw new ArithmeticException("Fail dividing Alpha by Factorial");
        }
        private decimal DivideByBigInteger(double number, BigInteger divisor)
        {
            while (true)
            {
                try
                {
                    return (decimal)(number /= (ulong)divisor);
                }
                catch (Exception)
                {
                    number /= ulong.MaxValue;
                    divisor /= ulong.MaxValue;
                }
            }
        }

        public decimal GetNProbability()
        {
            return isSolved ? Probabilities[Probabilities.Length - 1] : throw new InvalidOperationException("Firstly solve this task!");
        }
        public decimal GetZeroProbability()
        {
            return isSolved ? Probabilities[0] : throw new InvalidOperationException("Firstly solve this task!");
        }
        private BigInteger ComputingFactorial(uint n)
        {
            return n > 1 ? n * ComputingFactorial(n - 1) : 1;
        }
    }
}
