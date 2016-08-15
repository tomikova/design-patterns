using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SingletonExample
{
    sealed class Singleton
    {
        private static volatile Singleton instance;
        private static object sync = new Object();

        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    Console.WriteLine(Thread.CurrentThread.Name + ": Instance is null");
                    Thread.Sleep(2000);
                    lock (sync)
                    {
                        if (instance == null)
                        {
                            Console.WriteLine(Thread.CurrentThread.Name + ": Creating new instance");
                            instance = new Singleton();
                            Thread.Sleep(2000);
                        }
                        else
                        {
                            Console.WriteLine(Thread.CurrentThread.Name + ": Instance is created in other thread");
                            Thread.Sleep(2000);
                        }
                    }
                }
                Console.WriteLine(Thread.CurrentThread.Name + ": Returning instance");
                return instance;
            }
        }
        private Singleton() { }
        public void Test()
        {
            Console.WriteLine(Thread.CurrentThread.Name + ": Im done");
            Thread.Sleep(2000);
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Thread t1 = new Thread(TestSingleton);
            t1.Name = "Thread 1";
            Thread t2 = new Thread(TestSingleton);
            t2.Name = "Thread 2";

            t1.Start();
            t2.Start();

            while (t1.IsAlive && t2.IsAlive)
            {
                }

            Console.ReadKey();
        }

         static void TestSingleton()
        {
            (Singleton.Instance).Test();
        }
    }
}
