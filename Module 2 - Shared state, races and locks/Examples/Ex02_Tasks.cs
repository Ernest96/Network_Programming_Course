using System;
using System.Threading;
using System.Threading.Tasks;

namespace Module2
{
    // A Task is NOT a thread. It is a piece of work plus a promise of its
    // result. Task.Run puts that work on the same thread pool as Ex01 and
    // hands you a handle you can wait on.
    class Ex02_Tasks : IExample
    {
        public void Run()
        {
            Console.WriteLine($"main thread : {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine();

            // 1 - run something and wait for it
            //
            Task task = Task.Run(() =>
            {
                Thread me = Thread.CurrentThread;
                Console.WriteLine($"1) task ran on thread {me.ManagedThreadId}, " +
                                  $"from the pool: {me.IsThreadPoolThread}");
            });

            // Task task = Task.Factory.StartNew(() => 
            //     {
            //         Thread me = Thread.CurrentThread;
            //         Console.WriteLine($"1) task ran on thread {me.ManagedThreadId}, " +
            //                           $"from the pool: {me.IsThreadPoolThread}");
            //     },
            //     TaskCreationOptions.LongRunning);

            task.Wait();

            // 2 - a task that gives an answer back
            Console.WriteLine();
            Task<long> sum = Task.Run(() => AddUp(1_000_000));
            Console.WriteLine($"2) the sum is {sum.Result}"); // Result waits, then returns

            // 3 - many at once
            Console.WriteLine();
            Console.WriteLine("3) four tasks at the same time:");

            Task<long>[] tasks = new Task<long>[4];
            for (int i = 0; i < 4; i++)
            {
                int n = (i + 1) * 1_000_000; // a copy, as always
                tasks[i] = Task.Run(() => AddUp(n));
            }

            Task.WaitAll(tasks); // Join on all of them

            foreach (Task<long> t in tasks)
                Console.WriteLine($"     {t.Result}");

            Console.WriteLine();
            Console.WriteLine("Fewer lines than Thread, and you get the result back.");
            Console.WriteLine("Same pool underneath, so the rule from Ex03 still applies:");
            Console.WriteLine("never block inside a Task.Run.");
        }

        long AddUp(int n)
        {
            long total = 0;
            for (int i = 1; i <= n; i++)
                total += i;
            return total;
        }
    }
}