using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Module3
{
    // A collection built for many threads needs no lock of yours.
    class Ex04_ConcurrentBag : IExample
    {
        const int TASKS = 4;
        const int N = 10;

        public async Task RunAsync()
        {
            ConcurrentBag<int> bag = new ConcurrentBag<int>();

            Task[] tasks = new Task[TASKS];
            for (int i = 0; i < TASKS; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    for (int k = 0; k < N; k++)
                        bag.Add(k); // safe from any number of threads
                });
            }
            await Task.WhenAll(tasks);

            Console.WriteLine();
            Console.WriteLine($"expected {TASKS * N}, got {bag.Count}");
            Console.WriteLine();

            int value = 0, idx = 0;
            while (bag.TryTake(out value))
            {
                Console.WriteLine($"{idx}: {value}");
                idx++;
            }
        }
    }
}
