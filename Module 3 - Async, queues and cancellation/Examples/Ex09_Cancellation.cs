using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Module3
{
    // You ask a Task to stop, with a CancellationToken
    class Ex09_Cancellation : IExample
    {
        public async Task RunAsync()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            Task slowTask = SlowOperationAsync(token);

            await Task.Delay(1000);
            Console.WriteLine("main: Cancel()");
            cts.Cancel(); // ask - do not kill

            try
            {
                await slowTask;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"the task was cancelled");
            }
            Console.WriteLine($"task status: {slowTask.Status}");

            cts.Dispose();
        }

        // Heavy computation: counts the primes below 20 000 000.
        // Before each number it checks the token.
        async Task SlowOperationAsync(CancellationToken token)
        {
            await Task.Run(() =>
            {
                int count = 0;
                for (int n = 2; n < 20_000_000; n++)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine($"worker: asked to stop at n = {n}, {count} primes so far");
                        token.ThrowIfCancellationRequested();
                    }

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
