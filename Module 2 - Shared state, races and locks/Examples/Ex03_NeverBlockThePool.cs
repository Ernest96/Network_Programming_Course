using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Module2
{
    // The pool has only a few threads - about one per core.
    // Block them and everything queued behind you waits.
    //
    // Watch the start times: the first tasks start at once, the rest have to
    // wait for a thread to come free. The pool does add more, but slowly and
    // on purpose - about one a second.
    class Ex03_NeverBlockThePool : IExample
    {
        const int JOBS = 100;
        const int BLOCK_MS = 20000;

        readonly Stopwatch _clock = Stopwatch.StartNew();

        public void Run()
        {
            Console.WriteLine($"cores : {Environment.ProcessorCount}");
            Console.WriteLine($"{JOBS} tasks, each blocking for {BLOCK_MS} ms");
            Console.WriteLine();

            ThreadPool.SetMinThreads(100, 100);
            
            Task[] tasks = new Task[JOBS];
            for (int i = 0; i < JOBS; i++)
            {
                int job = i;
                tasks[i] = Task.Run(() =>
                {
                    Console.WriteLine($"task {job} started at {_clock.ElapsedMilliseconds} ms");
                    Thread.Sleep(BLOCK_MS);     // never do this inside a Task.Run
                });
            }

            Task.WaitAll(tasks);

            Console.WriteLine();
            Console.WriteLine("Nothing here is a race. The program is correct.");
            Console.WriteLine("It just cannot answer anybody while its threads are asleep.");
            Console.WriteLine("Lesson 3 replaces Thread.Sleep with await - then the thread");
            Console.WriteLine("goes back to the pool instead of sitting there doing nothing.");
        }
    }
}
