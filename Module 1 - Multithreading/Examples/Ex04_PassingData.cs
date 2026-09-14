using System;
using System.Collections.Generic;
using System.Threading;

namespace Module1
{
    // The classic beginner bug: a lambda does not copy the variable it uses,
    // it remembers WHERE that variable lives.
    class Ex04_PassingData : IExample
    {
        const int Count = 5;

        public void Run()
        {
            // WRONG EXAMPLE
            
            Console.WriteLine("WRONG - every thread captures the same variable i: ");

            List<Thread> bad = new List<Thread>();
            for (int i = 0; i < Count; i++)
            {
                Thread t = new Thread(() => Console.Write($"{i} "));
                bad.Add(t);
                t.Start();
            }
            foreach (Thread t in bad) t.Join();
            
            // RIGHT EXAMPLE

            Console.WriteLine("\n");
            Console.WriteLine("RIGHT - each thread captures its own copy:");

            List<Thread> good = new List<Thread>();
            for (int i = 0; i < Count; i++)
            {
                int copy = i;   // a NEW variable on every iteration
                Thread t = new Thread(() => Console.Write($"{copy} "));
                good.Add(t);
                t.Start();
            }
            foreach (Thread t in good) t.Join();
        }
    }
}
