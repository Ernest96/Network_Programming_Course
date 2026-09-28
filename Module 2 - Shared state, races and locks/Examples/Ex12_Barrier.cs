using System;
using System.Threading;

namespace Module2
{
    //  lock               one at a time
    //  SemaphoreSlim      at most N at a time
    //  Barrier            nobody moves on until everyone has arrived
    class Ex12_Barrier : IExample
    {
        const int WORKERS = 4;
        const int PHASES = 3;

        public void Run()
        {
            Console.WriteLine($"{WORKERS} workers, {PHASES} phases");
            Console.WriteLine("Each worker takes a different amount of time - on purpose.");
            Console.WriteLine();

            // The action runs once, after everyone has signalled.
            Barrier barrier = new Barrier(WORKERS, b =>
                Console.WriteLine($"  --- phase {b.CurrentPhaseNumber + 1} done ---\n"));

            Thread[] threads = new Thread[WORKERS];
            for (int i = 0; i < WORKERS; i++)
            {
                int id = i;
                threads[i] = new Thread(() =>
                {
                    Thread.Sleep(200 * (id + 1));
                    Console.WriteLine($"Worker {id} finished his work");

                    barrier.SignalAndWait(); // blocks until all 4 arrive
                    Console.WriteLine($"Worker {id} report his job to boss {id}");
                });
                threads[i].Start();
            }

            foreach (Thread t in threads)
                t.Join();

            Console.WriteLine("The fast workers waited for the slow one, every phase.");
            Console.WriteLine("A barrier is only as quick as its slowest participant.");

            barrier.Dispose();
        }
    }
}