using QueueingTheoryLibrary;

using System;
using System.Collections.Generic;
using System.Linq;

namespace AntiAircraftComplex
{
    class Task : ITask
    {
        public readonly uint countOfComples;
        public readonly uint averageArrivaleRate;
        public readonly decimal[] averageServiceTimes;
        public readonly decimal[] destroyProbabilities;
        public decimal[] alphas;

        public bool IsSolved { get; set; }
        public Task(uint countOfComples, uint averageArrivaleRate, IList<decimal> averageServiceTimes, IList<decimal> destroyProbabilities)
        {
            this.countOfComples = countOfComples;
            this.averageArrivaleRate = averageArrivaleRate;
            this.averageServiceTimes = averageServiceTimes.Select(val => 60 / val).ToArray();
            this.destroyProbabilities = destroyProbabilities.ToArray();
        }

        public string GetResult()
        {
            return "IDK";
        }


        public void Solve()
        {
            alphas = CalculateAlphas();
        }

        private decimal[] CalculateAlphas()
        {
            decimal[] array = new decimal[countOfComples];
            for(int i = 0; i < countOfComples; ++i)
            {
                array[i] = averageArrivaleRate / averageServiceTimes[i];
            }
            return array;
        }

        private decimal GetProbabilityOfFlightThrough()
        {
            return 1 - GetProbabilityOfNotFlightThrough();
        }
        private decimal GetProbabilityOfNotFlightThrough()
        {
            throw new NotImplementedException();
        }
    }
}
