using System;
using System.Diagnostics;
using System.Threading;

namespace Module2
{
    // OK: the same two threads, but the greedy one keeps only the shared
    // update inside the lock and does its slow work outside.
    class Ex15b_Starvation_Ok : IExample
    {
        const int SECONDS = 2;

        readonly object gate = new object();
        readonly Stopwatch clock = new Stopwatch();

        long greedyTurns = 0;
        long otherTurns = 0;
        long longestWaitMs = 0;

        public void Run()
        {
            Thread greedy = new Thread(() =>
            {
                while (clock.ElapsedMilliseconds < SECONDS * 1000)
                {
                    lock (gate)
                    {
                        greedyTurns++;
                    }
                    Thread.Sleep(1);
                }
            });

            Thread other = new Thread(() =>
            {
                while (clock.ElapsedMilliseconds < SECONDS * 1000)
                {
                    Stopwatch waited = Stopwatch.StartNew();
                    lock (gate)
                    {
                        otherTurns++;
                    }
                    Thread.Sleep(1);
                }
            });

            clock.Start();
            greedy.Start();
            other.Start();
            greedy.Join();
            other.Join();

            Console.WriteLine($"in {SECONDS} s:");
            Console.WriteLine($"  greedy thread : {greedyTurns,5} turns");
            Console.WriteLine($"  other thread  : {otherTurns,5} turns");
        }

        
    }
}
