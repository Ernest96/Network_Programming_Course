using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Module2
{
    // lock - one task at a time.
    //
    // lock marks a section of code that only ONE task may be inside at a time.
    // The others wait at the door, so counter++ (read, add, write) can no longer
    // be interrupted by another task in the middle.
    class Ex07_Step2_Lock : IExample
    {
        const int TASKS = 8;
        const int N = 1_000_000;

        readonly object _gate = new object();     // private, readonly, used only for locking

        int _counter;

        public void Run()
        {
            int expected = TASKS * N;
            Console.WriteLine($"expected : {expected}");

            Stopwatch sw = Stopwatch.StartNew();
            Count();
            sw.Stop();

            Console.WriteLine($"Counter is {_counter}");
            Console.WriteLine($"Time is {sw.ElapsedMilliseconds} ms");
        }

        void Count()
        {
            _counter = 0;

            Task[] tasks = new Task[TASKS];
            for (int i = 0; i < TASKS; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    for (int k = 0; k < N; k++)
                    {
                        lock (_gate)
                        {
                            _counter++;
                        }
                    }
                });
            }

            Task.WaitAll(tasks);
        }
    }
}
