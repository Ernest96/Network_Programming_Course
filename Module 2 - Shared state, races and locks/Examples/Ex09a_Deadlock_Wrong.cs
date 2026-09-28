using System;
using System.Threading;

namespace Module2
{
    // WRONG: two threads, two locks, OPPOSITE order.
    //
    //   A takes first,  then wants second
    //   B takes second, then wants first
    //
    // Each one holds what the other needs, and neither lets go.
    // A deadlocked program does not crash. No exception, no error, no log
    // line, CPU at zero. It just stops, forever, looking healthy.
    class Ex09a_Deadlock_Wrong : IExample
    {
        const int TIMEOUT_MS = 3000;

        readonly object first = new object();
        readonly object second = new object();

        public void Run()
        {
            Thread a = new Thread(() =>
            {
                lock (first)
                {
                    Console.WriteLine("[A] took first, now wants second");
                    Thread.Sleep(200);              
                    lock (second)
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
