using QueueingTheoryLibrary;

using System;
using System.Collections.Generic;
using System.Linq;

namespace AntiAircraftComplex
{
    class Task : ITask
    {
        public const byte COUNT_OF_COMPLEX = 2;
        public readonly uint averageArrivaleRate;
        public readonly decimal[] averageServiceTimes;
        public readonly decimal[] destroyProbabilities;
        public decimal[] alphas;
        public (decimal, string)[] flightProbabilities;

        public bool IsSolved { get; set; }
        public Task(uint averageArrivaleRate, IList<decimal> averageServiceTimes, IList<decimal> destroyProbabilities)
        {            
            this.averageArrivaleRate = averageArrivaleRate;
            this.averageServiceTimes = averageServiceTimes.ToArray();
            this.destroyProbabilities = destroyProbabilities.ToArray();
            flightProbabilities = new (decimal, string)[3];
        }

        public string GetResult()
        {
            return GetBestResult().ToString();
        }

        public void Solve()
        {
            alphas = CalculateAlphas();

        }

        private decimal[] CalculateAlphas()
        {
            decimal[] array = new decimal[COUNT_OF_COMPLEX];
            for(int i = 0; i < COUNT_OF_COMPLEX; ++i)
            {
                array[i] = averageArrivaleRate * averageServiceTimes[i];
            }
            return array;
        }

        private void CalcucateProbabilityOfFlightWhen1Row()
        {
            var result = 0;

            flightProbabilities[0] = (result, String.Empty);
        }
        private decimal GetProbabilityOfFlightThrough()
        {
            return 1 - GetProbabilityOfNotFlightThrough();
        }
        private decimal GetProbabilityOfNotFlightThrough()
        {
            throw new NotImplementedException();
        }
        private decimal CalculateProbabilityOfDamage()
        {
            throw new NotImplementedException();
        }
        private decimal CalculateProbabilityOfCancel()
        {
            throw new NotImplementedException();
        }
        private decimal GetAverageArrivalRateAtSecondRow()
        {
            throw new NotImplementedException();
        }
        private decimal CalculateTotalProbabilityForTwoRows()
        {
            throw new NotImplementedException();
        }
        private (decimal, string) GetBestResult()
        {
            var minimumFlightProbability = flightProbabilities[0];
            for(int i = 1; i < flightProbabilities.Length; ++i)
            {
                if (flightProbabilities[i].Item1 < minimumFlightProbability.Item1)
                    minimumFlightProbability = flightProbabilities[i];
            }
            return minimumFlightProbability;
        }
    }
}
