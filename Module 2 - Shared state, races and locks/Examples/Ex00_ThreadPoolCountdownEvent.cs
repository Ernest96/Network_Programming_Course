using System;
using System.Threading;

namespace Module2
{
    // With new Thread you own the thread, so you can Join() it.
    // With the pool you only hand over the WORK - the thread is not yours,
    // so there is no Join().
    class Ex00_ThreadPoolCountdownEvent : IExample
    {
        public void Run()
        {
            Console.WriteLine($"[main]   I am thread {Thread.CurrentThread.ManagedThreadId}");

            CountdownEvent done = new CountdownEvent(1);     // waiting for 1 job

            ThreadPool.QueueUserWorkItem(_ =>
            {
                Thread currentThread = Thread.CurrentThread;
                Console.WriteLine($"[worker] I am thread {currentThread.ManagedThreadId}, " +
                                  $"from the pool: {currentThread.IsThreadPoolThread}");

                Thread.Sleep(500);
                Console.WriteLine("[worker] finished");

                done.Signal(); // 1 -> 0
            });

            Console.WriteLine("[main]   job queued, waiting...");

            done.Wait(); // blocks until 0

            Console.WriteLine("[main]   all done");
        }
    }
}
