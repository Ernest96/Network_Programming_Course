using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Module1
{
    // Three jobs, one second each. One after another that is three seconds.
    // On three threads it is one second.
    class Ex07_SequentialVsParallel : IExample
    {
        const int JOB_COUNT = 3;
        const int JOB_MS = 1000;

        public void Run()
        {
            Console.WriteLine($"{JOB_COUNT} jobs, {JOB_MS} ms each.\n");

            // ---- one after another ----------------------------------
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 1; i <= JOB_COUNT; i++)
                Job(i);
            sw.Stop();
            long sequential = sw.ElapsedMilliseconds;
            Console.WriteLine($"\ntotal, one after another : {sequential} ms\n");

            // ---- all at once ----------------------------------------
            sw.Restart();
            List<Thread> threads = new List<Thread>();
            for (int i = 1; i <= JOB_COUNT; i++)
            {
                int id = i;
                Thread t = new Thread(() => Job(id));
                threads.Add(t);
                t.Start();
            }
            foreach (Thread t in threads)
                t.Join();
            sw.Stop();
            long parallel = sw.ElapsedMilliseconds;
            Console.WriteLine($"\ntotal, all at once       : {parallel} ms");

            Console.WriteLine("\nThis works even on a single-core machine.");
            Console.WriteLine("Threads are SLEEPING, not competing for the processor.");
        }

        void Job(int id)
        {
            Console.WriteLine($"  job {id} starting on thread {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(JOB_MS);
            Console.WriteLine($"  job {id} done");
        }
    }
}
