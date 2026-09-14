using System;
using System.Diagnostics;
using System.Threading;

namespace Module1
{
    // Real CPU work. The range [2, LIMIT) is split longo N chunks, one per thread.
    class Ex08_PrimeCounter : IExample
    {
        const long LIMIT = 20_000_000;

        long _lastTotal;

        public void Run()
        {
            Console.WriteLine($"Counting primes below {LIMIT:N0}");
            Console.WriteLine($"Environment.ProcessorCount = {Environment.ProcessorCount}\n");

            Console.WriteLine(" threads |     time   |");
            Console.WriteLine("---------|------------|");

            long baseline = 0;

            for (long t = 1; t <= 128; t *= 2)
            {
                long ms = Measure(t);
                if (t == 1)
                    baseline = ms;


                Console.WriteLine($"{t,8} | {ms,6} ms |");
            }

            Console.WriteLine($"\nprimes found: {_lastTotal:N0}");
        }

        long Measure(long threadCount)
        {
            long[] counts = new long[threadCount];       // one slot per thread
            Thread[] threads = new Thread[threadCount];
            long chunk = LIMIT / threadCount;

            Stopwatch sw = Stopwatch.StartNew();

            for (long i = 0; i < threadCount; i++)
            {
                long index = i;
                long from = 2 + index * chunk;
                long to = index == threadCount - 1 ? LIMIT : 2 + (index + 1) * chunk;

                threads[i] = new Thread(() => counts[index] = CountPrimes(from, to));
                threads[i].Start();
            }

            foreach (Thread t in threads)
                t.Join();

            sw.Stop();

            // Safe to add up now: every thread has finished writing.
            long total = 0;
            foreach (long c in counts)
                total += c;
            _lastTotal = total;

            return sw.ElapsedMilliseconds;
        }

        long CountPrimes(long from, long to)
        {
            long count = 0;
            for (long n = from; n < to; n++)
                if (IsPrime(n))
                    count++;
            return count;
        }

        bool IsPrime(long n)
        {
            if (n < 2) return false;
            if (n % 2 == 0) return n == 2;

            for (long d = 3; (long)d * d <= n; d += 2)
                if (n % d == 0)
                    return false;

            return true;
        }
    }
}
