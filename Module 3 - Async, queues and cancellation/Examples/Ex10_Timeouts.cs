using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Module3
{
    // A timeout: how long are you ready to wait?
    // task.WaitAsync(time) stops WAITING after that time - the operation itself keeps running.
    class Ex10_Timeouts : IExample
    {
        public async Task RunAsync()
        {
            Stopwatch sw = Stopwatch.StartNew();

            Task slowTask = SlowOperationAsync();
            try
            {
                await slowTask.WaitAsync(TimeSpan.FromSeconds(1));
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"gave up after {sw.ElapsedMilliseconds} ms");
            }

            await slowTask;
        }

        // Heavy computation: counts the primes below 20 000 000.
        async Task SlowOperationAsync()
        {
            await Task.Run(() =>
            {
                int count = 0;
                for (int n = 2; n < 20_000_000; n++)
                {
                    if (IsPrime(n))
                        count++;
                }
                Console.WriteLine($"worker: done, {count} primes");
            });
        }

        static bool IsPrime(int n)
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
