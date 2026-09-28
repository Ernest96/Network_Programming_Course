using System;
using System.Diagnostics;
using System.Threading;

namespace Module2
{
    // The same job, done many times, two ways:
    //   new Thread  - you create a thread, it runs once, it dies
    //   ThreadPool  - the runtime keeps a few threads and reuses them
    //
    // Creating a thread is expensive. Reusing one is not.
    class Ex01_ThreadVsThreadPool : IExample
    {
        const int JOBS = 200;

        public void Run()
        {
            Console.WriteLine($"cores : {Environment.ProcessorCount}");
            Console.WriteLine($"jobs  : {JOBS}");
            Console.WriteLine();

            Console.WriteLine($"new Thread  : {WithoutPool()} ms");
            Console.WriteLine($"ThreadPool  : {WithPool()} ms");

            Console.WriteLine();
        }

        long WithoutPool()
        {
            Stopwatch sw = Stopwatch.StartNew();

            Thread[] threads = new Thread[JOBS];
            for (int i = 0; i < JOBS; i++)
            {
                threads[i] = new Thread(Work);
                threads[i].Start();
            }

            foreach (Thread t in threads)
                t.Join();

            return sw.ElapsedMilliseconds;
        }

        long WithPool()
        {
            Stopwatch sw = Stopwatch.StartNew();

            // The pool gives us no Join(), so we count the jobs down ourselves.
            CountdownEvent done = new CountdownEvent(JOBS);

            for (int i = 0; i < JOBS; i++)
            {
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    Work();
                    done.Signal();
                });
            }

            done.Wait();
            return sw.ElapsedMilliseconds;
        }

        void Work()
        {
            int val;
            for (int i = 0; i < 100000; i++)
                val = (int)Math.Sqrt(i);
        }
    }
}
