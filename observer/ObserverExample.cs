using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace ObserverExample
{
    #region Source
    abstract class Source
    {
        abstract public int Read(ref List<double> myList);
    }
    #endregion

    #region Observer
    abstract class Observer
    {
        abstract public void Update();
    }
    #endregion

    #region KeyboardSource
    class KeyboardSource : Source
    {
        override public int Read(ref List<double> myList)
        {
            double number;
            bool result = Double.TryParse(Console.ReadLine() , out number);
            if (result && number > -1)
            {
                myList.Add(number);
                return 1;
            }
            else
                return -1;
        }
    }
    #endregion

    #region FileSource
    class FileSource : Source
    {
        private string fileSource;
        private StreamReader sr;
        double number;

        public FileSource(string source)
        {
            this.fileSource = source;
            this.sr = new StreamReader(File.OpenRead(source));

        }
        override public int Read(ref List<double> myList)
        {
            bool result = Double.TryParse(sr.ReadLine(), out number);
            if (result && number > -1)
            {
                myList.Add(number);
                return 1;
            }
            else
                return -1;
        }
    }
    #endregion

    #region FileObserver
    class FileObserver : Observer
    {
        NumberSequence _ns;
        string filePath;

        public FileObserver(NumberSequence ns, string path)
        {
            this._ns = ns;
            this.filePath = path;
            this._ns.addObservers(this);
        }

        public override void Update()
        {
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine();
                sw.Write("Numbers: ");
                foreach (double number in _ns.GetNumbers())
                {
                    sw.Write(number.ToString()+ " ");
                }
                sw.WriteLine();
                sw.WriteLine("Time and date " + DateTime.Now.ToString());
            }
        }
    }
    #endregion

    #region SumObserver
    class SumObserver : Observer
    {
        NumberSequence _ns;

        public SumObserver(NumberSequence ns)
        {
            this._ns = ns;
            this._ns.addObservers(this);
        }

        public override void Update()
        {
            double sum = 0;
            foreach (double number in _ns.GetNumbers())
            {
                sum += number;
            }
            Console.WriteLine("Sum: " + sum.ToString()); 
        }
    }
    #endregion

    #region AvgObserver
    class AvgObserver : Observer
    {
        NumberSequence _ns;

        public AvgObserver(NumberSequence ns)
        {
            this._ns = ns;
            this._ns.addObservers(this);
        }

        public override void Update()
        {
            double sum = 0;
            List<double> numbers = _ns.GetNumbers();
            foreach (double number in numbers)
            {
                sum += number;
            }
            Console.WriteLine("Avg: " + (sum/numbers.Count).ToString());
        }
    }
    #endregion

    #region MeanObserver
    class MeanObserver : Observer
    {
        NumberSequence _ns;

        public MeanObserver(NumberSequence ns)
        {
            this._ns = ns;
            this._ns.addObservers(this);
        }

        public override void Update()
        {
            List<double> numbers = _ns.GetNumbers();
            numbers.Sort();
            if (numbers.Count % 2 == 0)
            {
                int index = numbers.Count / 2;
                double mean = (numbers[index] + numbers[index - 1]) / 2;
                Console.WriteLine("Mean: " + mean.ToString());
            }
            else
            {
                double n = (double)numbers.Count / 2;
                int index = Convert.ToInt32(Math.Floor(n));
                Console.WriteLine("Mean: " + numbers[index].ToString());
            }
        }
    }
    #endregion

    #region NumberSequence
    class NumberSequence
    {
        List<Observer> observers;
        List<double> numbers;
        Source source;
        int stat = 0;

        public NumberSequence()
        {
            this.observers = new List<Observer>();
            this.numbers = new List<double>();
        }

        public void setSource(Source selectedSource)
        {
            this.source = selectedSource;
        }

        public void addObservers(Observer o)
        {
            this.observers.Add(o);
        }

        public List<double> GetNumbers()
        {
            return this.numbers;
        }
        public void start()
        {
            while(stat != -1)
            {
                stat = source.Read(ref numbers);

                if (stat == 1)
                {
                    foreach (Observer o in observers)
                    {
                        o.Update();
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
    #endregion

    class Program
    {
        static void Main(string[] args)
        {
            #region Setup
            Source source = null;
            string filePath = "file.txt";
            string observerFilePath = "observerFile.txt";
            Console.WriteLine("Select source:");
            int selectedSource = Convert.ToInt32(Console.ReadLine());
            switch (selectedSource)
            {
                case 1:
                    source = new KeyboardSource();
                    break;
                case 2:
                    source = new FileSource(filePath);
                    break;
            }

            NumberSequence ns = new NumberSequence();
            ns.setSource(source);

            FileObserver od = new FileObserver(ns, observerFilePath);
            SumObserver os = new SumObserver(ns);   
            AvgObserver oa = new AvgObserver(ns);       
            MeanObserver om = new MeanObserver(ns);
            

            //ns.addObservers(od);
            //ns.addObservers(os);
            //ns.addObservers(oa);
            //ns.addObservers(om);
            #endregion

            ns.start();

            Console.ReadKey();
        }
    }
}
