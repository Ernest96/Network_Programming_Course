using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Module1
{
    // The same experiment twice: eight jobs, sequentially and then on eight
    // threads. Once with work that BURNS the processor, once with work that WAITS.
    //
    //   CPU-bound -> speed-up stops at roughly the number of cores.
    //   I/O-bound -> speed-up is about 8x whatever your core count is.
    //
    // For waiting work the limit is not cores. It is how many threads you are
    // willing to waste - and in lecture 3 the answer becomes "none".
    class Ex10_CpuBoundVsIoBound : IExample
    {
        const int JOB_COUNT = 8;
        const int PRIME_LIMIT = 300_000;
        const int WAIT_MS = 500;

        public void Run()
        {
            Console.WriteLine($"Environment.ProcessorCount = {Environment.ProcessorCount}");
            Console.WriteLine($"{JOB_COUNT} jobs in each experiment.\n");

            Console.WriteLine("=== CPU-BOUND: each job counts primes ===");
            Compare(() => CountPrimes(PRIME_LIMIT));

            Console.WriteLine($"\n=== I/O-BOUND: each job waits {WAIT_MS} ms ===");
            Compare(() => Thread.Sleep(WAIT_MS));

            Console.WriteLine("\nSame jobs, same threads, different rules.");
            Console.WriteLine("CPU-bound work competes for cores. Waiting work does not.");
        }

        void Compare(Action job)
        {
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < JOB_COUNT; i++)
                job();
            sw.Stop();
            long sequential = sw.ElapsedMilliseconds;

            sw.Restart();
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < JOB_COUNT; i++)
            {
                Thread t = new Thread(() => job());
                threads.Add(t);
                t.Start();
            }
            foreach (Thread t in threads)
                t.Join();
            sw.Stop();
            long parallel = sw.ElapsedMilliseconds;

            double speedup = parallel == 0 ? 0 : (double)sequential / parallel;

            Console.WriteLine($"  sequential      : {sequential,6} ms");
            Console.WriteLine($"  {JOB_COUNT} threads       : {parallel,6} ms");
        }

        int CountPrimes(int limit)
        {
            int count = 0;
            for (int n = 2; n < limit; n++)
                if (IsPrime(n))
                    count++;
            return count;
        }

        bool IsPrime(int n)
        {
            if (n < 2) return false;
            if (n % 2 == 0) return n == 2;

            for (int d = 3; (long)d * d <= n; d += 2)
                if (n % d == 0)
                    return false;

            return true;
        }
    }
}
