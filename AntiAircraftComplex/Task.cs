using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiAircraftComplex
{
    class Task
    {
        public readonly uint countOfComples;
        public readonly uint averageArrivaleRate;
        public readonly double[] averageServiceTimes;
        public readonly double[] destroyProbabilities;

        public Task(uint countOfComples, uint averageArrivaleRate, IList<double> averageServiceTimes, IList<double> destroyProbabilities)
        {
            this.countOfComples = countOfComples;
            this.averageArrivaleRate = averageArrivaleRate;
            this.averageServiceTimes = averageServiceTimes.ToArray();
            this.destroyProbabilities = destroyProbabilities.ToArray();
        }

        public void Solve()
        {
            throw new NotImplementedException();
        }
    }
}
