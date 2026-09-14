using System;
using System.Collections.Generic;
using System.Threading;

namespace Module1
{
    // Five threads are started in the order 0,1,2,3,4 
    // and almost never print in that order.
    class Ex03_StartOrder : IExample
    {
        const int THREADS_PER_ROUND = 5;

        public void Run()
        {
            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < THREADS_PER_ROUND; i++)
            {
                int id = i;
                Thread t = new Thread(() => Console.Write($"thread {id}  "));
                threads.Add(t);
                t.Start();
            }

            foreach (Thread t in threads)
                t.Join(); 

            Console.WriteLine("\n");
            Console.WriteLine("Nobody chose these orders - not you, not the compiler.");
            Console.WriteLine("You may assume nothing about order unless you enforce it.");
        }
    }
}