using System;
using System.Numerics;

namespace QueueingTheoryLibrary
{
    public static class QueueingTheoryCalculator
    {
        public static decimal DivideNumberByFactorial(decimal number, uint index)
        {
            if (Math.Pow((double)number, index) < 0)
                return (decimal)Math.Exp(BigInteger.Log(new BigInteger(Math.Pow((double)number, index))) - BigInteger.Log(ComputingFactorial(index)));
            else if (ComputingFactorial(index) > 0)
                return DivideByBigInteger(Math.Pow((double)number, index), ComputingFactorial(index));
            else
                throw new ArithmeticException("Fail dividing Alpha by Factorial");
        }
        public static decimal DivideByBigInteger(double number, BigInteger divisor)
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
        public static BigInteger ComputingFactorial(uint n)
        {
            return n > 1 ? n * ComputingFactorial(n - 1) : 1;
        }
    }
}
