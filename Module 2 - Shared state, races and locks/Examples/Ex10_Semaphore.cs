using System;
using System.Threading;

namespace Module2
{
    // lock answers WHO: one at a time.
    // SemaphoreSlim answers HOW MANY: at most N at a time.
    class Ex10_Semaphore : IExample
    {
        const int WORKERS = 9;
        const int PERMITS = 3;

        public void Run()
        {
            Console.WriteLine($"{WORKERS} workers, {PERMITS} permits");
            Console.WriteLine();

            SemaphoreSlim gate = new SemaphoreSlim(PERMITS);

            Thread[] threads = new Thread[WORKERS];
            for (int i = 0; i < WORKERS; i++)
            {
                int id = i;
                threads[i] = new Thread(() =>
                {
                    gate.Wait();                 // take a permit, or queue
                    try
                    {
                        Console.WriteLine($"  worker {id} inside");
                        Thread.Sleep(500);
                        Console.WriteLine($"  worker {id} leaving");
                    }
                    finally
                    {
                        gate.Release();          // ALWAYS release, in finally
                    }
                });
                threads[i].Start();
            }

            foreach (Thread t in threads)
                t.Join();

            Console.WriteLine();
            Console.WriteLine($"Never more than {PERMITS} inside at once.");
            Console.WriteLine("Forget the Release and the program stops for good, with no error.");

            gate.Dispose();
        }
    }
}
