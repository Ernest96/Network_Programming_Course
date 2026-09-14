using System;
using System.Diagnostics;
using System.Threading;

namespace Module1
{
    // Two threads, two arrays, one sort each. No locks are needed and none
    // are used - because the two threads never touch the same memory.
    // THE RULE: threads are only dangerous when they SHARE something.
    // Split the data first and the danger disappears.
    class Ex06_SortTwoArraysBenchmark : IExample
    {
        const int BIG_SIZE = 5_000_000;

        public void Run()
        {
            // ---- big arrays: is it actually faster? ------------------
            Console.WriteLine($"\nNow two arrays of {BIG_SIZE:N0} elements each.\n");

            int[] x = RandomArray(BIG_SIZE, 3);
            int[] y = RandomArray(BIG_SIZE, 4);

            Stopwatch sw = Stopwatch.StartNew();
            Array.Sort(x);
            Array.Sort(y);
            sw.Stop();
            long sequential = sw.ElapsedMilliseconds;
            Console.WriteLine($"one after another : {sequential,6} ms");

            x = RandomArray(BIG_SIZE, 3);
            y = RandomArray(BIG_SIZE, 4);

            sw.Restart();
            Thread s1 = new Thread(() => Array.Sort(x));
            Thread s2 = new Thread(() => Array.Sort(y));
            s1.Start();
            s2.Start();
            s1.Join();
            s2.Join();
            sw.Stop();
            long parallel = sw.ElapsedMilliseconds;
            Console.WriteLine($"both at once      : {parallel,6} ms");

            double speedup = parallel == 0 ? 0 : (double)sequential / parallel;
            Console.WriteLine($"\nspeed-up: {speedup:F2}x");
        }

        int[] RandomArray(int size, int seed)
        {
            Random rnd = new Random(seed);
            int[] result = new int[size];
            for (int i = 0; i < size; i++)
                result[i] = rnd.Next(0, 1000);
            return result;
        }
    }
}
