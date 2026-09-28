using System;
using System.Threading.Tasks;

namespace Module2
{
    // Give each task its own int and there is nothing left to protect. The
    // tasks never meet; the main thread adds the slots up AFTER WaitAll,
    // when it is the only one still running.
    class Ex06_Step1_DontShare : IExample
    {
        const int TASKS = 8;
        const int N = 1_000_000;

        public void Run()
        {
            int[] counts = new int[TASKS];       // one slot per task
            Task[] tasks = new Task[TASKS];

            for (int i = 0; i < TASKS; i++)
            {
                int index = i;                   // a copy
                tasks[i] = Task.Run(() =>
                {
                    for (int k = 0; k < N; k++)
                        counts[index]++;         // nobody else writes here
                });
            }

            Task.WaitAll(tasks);                 // the merge point

            int total = 0;
            for (int i = 0; i < TASKS; i++)
            {
                Console.WriteLine($"  task {i} counted {counts[i]}");
                total += counts[i];
            }

            int expected = TASKS * N;
            Console.WriteLine();
            Console.WriteLine($"expected : {expected}");
            Console.WriteLine($"actual   : {total}");
            Console.WriteLine();
            Console.WriteLine("Run it ten times. It is correct ten times.");
            Console.WriteLine("Different tasks, different memory. No race is possible.");
        }
    }
}
