using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyExample
{
    #region GenerateNumbers
    abstract class  GenerateNumbers
    {
        abstract public List<double> Generate();
    }
    #endregion

    #region CalculatePercentile
    abstract class CalculatePercentile
    {
        abstract public double Calculate(List<double> myList);
    }
    #endregion

    #region StrategyGenerateNumbers1
    class StrategyGenerateNumbers1 : GenerateNumbers
    {
        private double lowerLimit;
        private double upperLimit;
        private double step;
        private List<double> myList;

        public StrategyGenerateNumbers1(double lower, double upper, double stp)
        {
            this.lowerLimit = lower;
            this.upperLimit = upper;
            this.step = stp;
            this.myList = new List<double>();
        }

        override public List<double> Generate()
        {

            for (double i = lowerLimit; i <= upperLimit; i += step)
            {
                myList.Add(i);
            }

            myList.Sort();
            return myList;
        }
    }
#endregion

    #region StrategyGenerateNumbers2
    class StrategyGenerateNumbers2 : GenerateNumbers
    {
        private double mean;
        private double stdDev;
        private int n;
        private List<double> myList;

        public StrategyGenerateNumbers2(double Mean, double Dev, int c)
        {
            this.mean = Mean;
            this.stdDev = Dev;
            this.n = num;
            this.myList = new List<double>();
        }

        override public List<double> Generate()
        {
            Random rand = new Random();
            for (int i = 0; i < n; i++)
            {
                double u1 = rand.NextDouble();
                double u2 = rand.NextDouble();
                double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
                double randNormal = mean + stdDev * randStdNormal;
                myList.Add(randNormal);
            }
            myList.Sort();
            return myList;
        }
    }
    #endregion

    #region StrategyGenerateNumbers3
    class StrategyGenerateNumbers3 : GenerateNumbers
    {
        private int n;
        List<double> myList;

        public StrategyGenerateNumbers3(int number)
        {
            this.n = number;
            this.myList = new List<double>();
        }

        override public List<double> Generate()
        {
            if (n == 0)
                return myList;
            else if (n == 1)
            {
                myList.Add(1);
                return myList;
            }
            else if (n == 2)
            {
                myList.Add(1);
                myList.Add(1);
                return myList;
            }
            else
            {
                int a = 1, b = 1;
                myList.Add(1);
                myList.Add(1);
                for (int i = 3; i <= n; i++)
                {
                    int c = a + b;
                    myList.Add(c);
                    a = b;
                    b = c;
                }
                myList.Sort();
                return myList;
            }

        }
    }
    #endregion

    #region StrategyCalculatePercentile1
    class StrategyCalculatePercentile1 : CalculatePercentile
    {
        private double percentile;

        public StrategyCalculatePercentile1(double perc)
        {
            this.percentile = perc;
        }
        override public double Calculate(List<double> myList)
        {
            double n = percentile / 100 * myList.Count + 0.5;
            int index = Convert.ToInt32(Math.Round(n,MidpointRounding.AwayFromZero));
            return myList[index-1];
        }
    }
    #endregion

    #region StrategyCalculatePercentile2
    class StrategyCalculatePercentile2 : CalculatePercentile
    {
        private double percentile;

        public StrategyCalculatePercentile2(double perc)
        {
            this.percentile = perc;
        }
        override public double Calculate(List<double> myList)
        {
            int count = myList.Count;
            if (percentile <= ((double)100 / count * (1 - 0.5)))
                return myList[0];
            else if (percentile >= ((double)100 / count * (count - 0.5)))
                return myList[count - 1];
            else
            {
                for (int i = 0; i < count; i++)
                {
                    double current = (double)100 / count * (i + 1 - 0.5);
                    double next = (double)100 / count * (i + 2 - 0.5);
                    if (percentile == current)
                    {
                        return myList[i];
                    }
                    else if (percentile > current && percentile < next)
                    {
                        return (myList[i] + count * (double)(percentile - current) / 100 * (myList[i + 1] - myList[i]));
                    }
                }
                return 0;
            }
        }
    }
#endregion

	class StrategyGeneratePercentile3 : CalculatePercentile
    {
        private CalculatePercentile _gen1, _gen2;
        public StrategyGeneratePercentile3(CalculatePercentile gen1, CalculatePercentile gen2)
        {
            this._gen1 = gen1;
            this._gen2 = gen2;
        }
        override public double Calculate(List<double> myList)
        {
            double percentile = (_gen1.Calculate(myList) + _gen2.Calculate(myList))/2;
            return percentile;
        }
        
    }

    #region DistributionTester
    class DistributionTester
    {
        private GenerateNumbers stratGen;
        private CalculatePercentile stratCalc;
        private List<double> generatedNumbers;

        double percentilResult;

        public void SetGenerator(GenerateNumbers gen)
        {
            this.stratGen = gen;
        }
        public void SetCalculator(CalculatePercentile calc)
        {
            this.stratCalc = calc;
        }
        public void Generate()
        {
            this.generatedNumbers = this.stratGen.Generate();
        }
        public void Calculate()
        {
            percentilResult = this.stratCalc.Calculate(generatedNumbers);
        }
        public void doWrite()
        {
            Console.WriteLine("Generated numbers are:");
            foreach (double i in generatedNumbers)
            {
                Console.WriteLine(i.ToString());
            }
            Console.WriteLine("Percentile is: " + percentilResult.ToString());
        }
    }
    #endregion

    class Program
    {
        static void Main(string[] args)
        {
            #region Setup
            GenerateNumbers stratGen = null;
            CalculatePercentile stratCalc = null;

            Console.WriteLine("Number generation method:");
            int genMethod = Convert.ToInt32(Console.ReadLine());
            switch (genMethod)
            {
                case 1:
                    Console.WriteLine("Enter lower limit, upper limit and step");
                    double lower = Convert.ToDouble(Console.ReadLine());
                    double upper = Convert.ToDouble(Console.ReadLine());
                    double step = Convert.ToDouble(Console.ReadLine());
                    stratGen = new StrategyGenerateNumbers1(lower, upper, step);
                    break;
                case 2:
                    Console.WriteLine("Enter standard deviation, mean and number of elements");
                    double stdDev = Convert.ToDouble(Console.ReadLine());
                    double mean = Convert.ToDouble(Console.ReadLine());
                    int n = Convert.ToInt32(Console.ReadLine());
                    stratGen = new StrategyGenerateNumbers2(mean, stdDev, n);
                    break;
                case 3:
                    Console.WriteLine("Broj elemenata:");
                    int number = Convert.ToInt32(Console.ReadLine());
                    stratGen = new StrategyGenerateNumbers3(number);
                    break;
            }

            Console.WriteLine("Percentile calculation method:");
            int percMethod = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Wanted percentile:");
            double percentile = Convert.ToDouble(Console.ReadLine());
            switch (percMethod)
            {
                case 1:
                    stratCalc = new StrategyCalculatePercentile1(percentile);
                    break;
                case 2:
                    stratCalc = new StrategyCalculatePercentile2(percentile);
                    break;
				case 3:
                    stratCalc = new StrategyGeneratePercentile3(new StrategyCalculatePercentile1(percentile),
					new StrategyCalculatePercentile2(percentile));
                    break;
            }
            #endregion

            DistributionTester tester = new DistributionTester();
            tester.SetGenerator(stratGen);
            tester.SetCalculator(stratCalc);
            tester.Generate();
            tester.Calculate();
            tester.doWrite();

            Console.ReadKey();
        }
    }
}
