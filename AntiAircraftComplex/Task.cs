using QueueingTheoryLibrary;

using System;
using System.Collections.Generic;
using System.Linq;

namespace AntiAircraftComplex
{
    internal class Task : ITask
    {
        private const byte COUNT_OF_COMPLEX = 2;
        private readonly decimal averageArrivaleRate;
        private readonly decimal[] averageServiceTimes;
        private readonly decimal[] destroyProbabilities;
        private decimal[] alphas;
        private readonly decimal[,] probabilitiesOfFlightAndIndexesPair = new decimal[3, 3];

        public bool IsSolved { get; set; }
        public Task(decimal averageArrivaleRate, IList<decimal> averageServiceTimes, IList<decimal> destroyProbabilities)
        {
            this.averageArrivaleRate = averageArrivaleRate;
            this.averageServiceTimes = averageServiceTimes.ToArray();
            this.destroyProbabilities = destroyProbabilities.ToArray();
        }

        public string GetResult()
        {
            return GetBestResult();
        }
        public void Solve()
        {
            alphas = CalculateAlphas();
            probabilitiesOfFlightAndIndexesPair[0, 0] = CalcucateProbabilityOfFlightWhen1Row();
            var twoRowsArray = CalculateProbabilityOfFlightWhen2Row();
            for (int i = 0; i <= twoRowsArray.GetUpperBound(0); ++i)
            {
                for (int j = 0; j <= twoRowsArray.GetUpperBound(1); ++j)
                {
                    probabilitiesOfFlightAndIndexesPair[i + 1, j] = twoRowsArray[i, j];
                }
            }
        }

        private decimal[] CalculateAlphas()
        {
            decimal[] array = new decimal[COUNT_OF_COMPLEX];
            for (int i = 0; i < COUNT_OF_COMPLEX; ++i)
            {
                array[i] = averageArrivaleRate * averageServiceTimes[i];
            }
            return array;
        }

        #region When 1 row
        private decimal CalcucateProbabilityOfFlightWhen1Row()
        {
            return GetProbabilityOfFlightThrough();
        }
        private decimal CalculateProbabilityOfCancelByShahbazov()
        {
            var sum = 0m;
            for (byte i = 1; i < 3; ++i)
            {
                sum += (decimal)QueueingTheoryCalculator.ComputingFactorial(i) * CalculateSumOfExternalRow(i);
            }
            return 1 / (1 + sum);
        }
        private decimal CalculateSumOfExternalRow(byte i)
        {
            if (i == 1)
                return CalculateSumOfInnerRowWhen1();
            else
                return CalculateSumOfInnerRowWhen2();
        }
        private decimal CalculateSumOfInnerRowWhen2()
        {
            return 1 / (alphas[0] * alphas[1]);
        }
        private decimal CalculateSumOfInnerRowWhen1()
        {
            return 1 / (1 * alphas[1]) + 1 / (alphas[0] * 1);
        }
        #endregion
        #region When 2 rows
        private decimal[,] CalculateProbabilityOfFlightWhen2Row()
        {
            decimal[,] array = new decimal[2, 3];
            array[0, 0] = CalculateTotalProbabilityForTwoRows(0, 1);
            array[0, 1] = 0;
            array[0, 2] = 1;
            array[1, 0] = CalculateTotalProbabilityForTwoRows(1, 0);
            array[1, 1] = 1;
            array[1, 2] = 0;
            return array;
        }
        private decimal CalculateTotalProbabilityForTwoRows(byte firstIndex, byte secondIndex)
        {
            return GetProbabilityOfFlightThrough(firstIndex) * GetProbabilityOfFlightThrough(secondIndex, true);
        }
        private decimal CalculateProbabilityOfCancelByDefault(int index, bool isSecondRow = false)
        {
            var arivRate = averageArrivaleRate;
            if (isSecondRow)
                arivRate = GetAverageArrivalRateAtSecondRow(GetProbabilityOfFlightThrough(index == 0 ? 1 : 0));
            return arivRate / (arivRate + 1 / averageServiceTimes[index]);
        }
        private decimal GetAverageArrivalRateAtSecondRow(decimal probabilityOfCancelOfFirstRow)
        {
            return averageArrivaleRate * probabilityOfCancelOfFirstRow;
        }
        #endregion

        private decimal GetProbabilityOfFlightThrough(int index = -1, bool isSecondRow = false)
        {
            return 1 - GetProbabilityOfNotFlightThrough(index, isSecondRow);
        }
        private decimal GetProbabilityOfNotFlightThrough(int index, bool isSecondRow = false)
        {
            return CalculateProbabilityOfDamage(index, isSecondRow) * GetDestroyProbabilities(index);
        }
        private decimal GetDestroyProbabilities(int index)
        {
            return index == -1 ? destroyProbabilities.Average() : destroyProbabilities[index];
        }
        private decimal CalculateProbabilityOfDamage(int index, bool isSecondRow = false)
        {
            return 1 - (index == -1 ? CalculateProbabilityOfCancelByShahbazov() : CalculateProbabilityOfCancelByDefault(index, isSecondRow));
        }

        private string GetBestResult()
        {
            int indexOfMinimumProbability = 0;
            decimal minimumProbability = 1.01m;
            for (int i = 1; i <= probabilitiesOfFlightAndIndexesPair.GetUpperBound(0); ++i)
            {
                if (minimumProbability > probabilitiesOfFlightAndIndexesPair[i, 0])
                {
                    indexOfMinimumProbability = i;
                    minimumProbability = probabilitiesOfFlightAndIndexesPair[i, 0];
                }
            }

            return probabilitiesOfFlightAndIndexesPair[indexOfMinimumProbability, 0] == probabilitiesOfFlightAndIndexesPair[indexOfMinimumProbability, 1] ?
                GenerateAnswear(probabilitiesOfFlightAndIndexesPair[indexOfMinimumProbability, 0], 1) :
                GenerateAnswear(probabilitiesOfFlightAndIndexesPair[indexOfMinimumProbability, 0], 2,
                    (int)probabilitiesOfFlightAndIndexesPair[indexOfMinimumProbability, 1],
                    (int)probabilitiesOfFlightAndIndexesPair[indexOfMinimumProbability, 2]
                    );
        }
        private string GenerateAnswear(decimal minimalValue, int countOfRow, int firstIndex = -1, int secondIndex = -1)
        {
            String result = $"Найменше значення ймовірності прольоту: {minimalValue} - досягається, коли кількість рядів = {countOfRow}.";
            if (countOfRow == 2)
                result += $"При цьому в першій лінії необхідно розмістити комплекс із часом обслуговування {averageServiceTimes[firstIndex]} хв та ймовірністю влучання {destroyProbabilities[firstIndex] * 100}%," +
                    $" а на другій лінії з часом обслуговування {averageServiceTimes[secondIndex]} хв та ймовірністю влучання {destroyProbabilities[secondIndex] * 100}%";
            return result;
        }
    }
}
