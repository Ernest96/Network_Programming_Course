using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Module3
{
    // 1000 operations that each wait 1 second - think 1000 clients of a server.
    // With threads: 1000 threads, each blocked. With await: a handful of threads.
    class Ex03_ThousandWaits : IExample
    {
        const int COUNT = 1000;

        public async Task RunAsync()
        {
            Stopwatch sw = Stopwatch.StartNew();

            Thread[] threads = new Thread[COUNT];
            for (int i = 0; i < COUNT; i++)
            {
                threads[i] = new Thread(() => Thread.Sleep(1000));
                threads[i].Start();
            }
            int maxThreadsUsed = Process.GetCurrentProcess().Threads.Count;
            foreach (Thread t in threads)
                t.Join();

            Console.WriteLine($"{COUNT} threads : {sw.ElapsedMilliseconds} ms");
            Console.WriteLine($"{maxThreadsUsed} OS threads in the process");

            sw.Restart();

            Task[] tasks = new Task[COUNT];
            
            for (int i = 0; i < COUNT; i++)
                tasks[i] = Task.Delay(1000);
            
            maxThreadsUsed = Process.GetCurrentProcess().Threads.Count;
            
            await Task.WhenAll(tasks);

            Console.WriteLine($"{COUNT} awaits  : {sw.ElapsedMilliseconds} ms, " +
                              $"{maxThreadsUsed} OS threads in the process");
        }
    }
}
