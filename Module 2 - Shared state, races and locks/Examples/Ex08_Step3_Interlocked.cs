using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Module2
{
    // STEP 3 of the ladder - when a lock is more than you need.
    //
    // A lock protects a whole section of code. If all you are changing is
    // ONE variable, the processor can do read-modify-write in a single step.
    // No task can be stopped in the middle of it, because there is no middle.
    class Ex08_Step3_Interlocked : IExample
    {
        const int TASKS = 8;
        const int N = 1_000_000;

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
                        Interlocked.Increment(ref _counter);
                    }
                });
            }

            Task.WaitAll(tasks);
        }
    }
}