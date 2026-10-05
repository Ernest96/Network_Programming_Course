using System;
using System.Threading;

namespace Module2
{
    // OK: the same two threads, but each one waits a RANDOM time before it
    // tries again. That breaks the tie: soon one thread tries while the other
    // is waiting, gets both locks and finishes - then the other one does.
    class Ex14b_Livelock_Ok : IExample
    {
        readonly Lock lockA = new();
        readonly Lock lockB = new();

        public void Run()
        {
            Thread t1 = new Thread(Thread1Work);
            Thread t2 = new Thread(Thread2Work);
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();
        }

        // Thread 1: takes A first, then tries B
        void Thread1Work()
        {
            while (true)
            {
                lock (lockA)
                {
                    Console.WriteLine("T1 has A, trying B...");
                    Thread.Sleep(50);

                    if (lockB.TryEnter())
                    {
                        try
                        {
                            Console.WriteLine("T1 got A and B, doing work!");
                            return;
                        }
                        finally
                        {
                            lockB.Exit();
                        }
                    }

                    Console.WriteLine("T1 couldn't get B, releasing A...");
                    Thread.Sleep(50);               // undo the work done with A
                } // A released here
                Thread.Sleep(Random.Shared.Next(0, 500));   // a RANDOM wait breaks the tie
            }
        }

        // Thread 2: takes B first, then tries A
        void Thread2Work()
        {
            while (true)
            {
                lock (lockB)
                {
                    Console.WriteLine("T2 has B, trying A...");
                    Thread.Sleep(50);

                    if (lockA.TryEnter())
                    {
                        try
                        {
                            Console.WriteLine("T2 got B and A, doing work!");
                            return;
                        }
                        finally
                        {
                            lockA.Exit();
                        }
                    }

                    Console.WriteLine("T2 couldn't get A, releasing B...");
                    Thread.Sleep(50);               // undo the work done with B
                } // B released here
                Thread.Sleep(Random.Shared.Next(0, 500));   // a RANDOM wait breaks the tie
            }
        }
    }
}
