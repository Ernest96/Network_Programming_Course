using System;
using System.Threading;

namespace Module2
{
    
    class Ex09a_Deadlock_Ok : IExample
    {
        const int TIMEOUT_MS = 3000;

        readonly object first = new object();
        readonly object second = new object();

        public void Run()
        {
            Thread a = new Thread(() =>
            {
                lock (second)
                {
                    Console.WriteLine("[A] took first, now wants second");
                    Thread.Sleep(200);              
                    lock (first)
                    {
                        Console.WriteLine("[A] took both");  
                    }
                }
            });

            Thread b = new Thread(() =>
            {
                lock (second)
                {
                    Console.WriteLine("[B] took second, now wants first");
                    Thread.Sleep(200);              
                    lock (first)
                    {
                        Console.WriteLine("[B] took both"); 
                    }
                }
            });

            a.IsBackground = true;
            a.IsBackground = true;
            
            a.Start();
            b.Start();

            // A plain Join() here would wait forever.
            bool finished = a.Join(TIMEOUT_MS) && b.Join(TIMEOUT_MS);

            if (finished)
                Console.WriteLine("Finished");
            else
                Console.WriteLine($"Timeout{TIMEOUT_MS} ms - DEADLOCK");
        }
    }
}