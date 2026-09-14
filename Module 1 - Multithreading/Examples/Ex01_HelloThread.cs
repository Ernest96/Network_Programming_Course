using System;
using System.Threading;

namespace Module1
{
    class Ex01_HelloThread : IExample
    {
        public void Run()
        {
            Console.WriteLine($"[main] ThreadId am thread {Thread.CurrentThread.ManagedThreadId}");
            Thread worker = new Thread(Work);

            worker.Start();
            Console.WriteLine("[main]   started the worker");

            // Join blocks the CALLING thread (main) until the worker ends.
            // Remove it and "all done" may print before the worker finishes.
            worker.Join();

            Console.WriteLine("[main]   all done");
        }

        void Work()
        {
            Console.WriteLine("[worker] I am thread {0}", Thread.CurrentThread.ManagedThreadId);

            int result = 40 + 2;
            Thread.Sleep(500);

            Console.WriteLine($"Result is: {result}");
            Console.WriteLine("[worker] finished");
        }
    }
}
