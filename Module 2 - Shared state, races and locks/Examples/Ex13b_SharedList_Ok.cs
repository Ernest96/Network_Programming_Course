using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module2
{
    // OK: the same 4 tasks and the same List<int>, but every Add is inside
    class Ex13b_SharedList_Ok : IExample
    {
        const int TASKS = 4;
        const int N = 25_000;
        const int RUNS = 5;

        const int EXPECTED = TASKS * N;

        readonly object _gate = new object();

        public void Run()
        {
            Console.WriteLine($"{TASKS} tasks, {N} Add calls each. Expect {EXPECTED}.");
            Console.WriteLine();

            for (int run = 1; run <= RUNS; run++)
                Console.WriteLine($"  run {run} : {Fill()}");

            Console.WriteLine();
            Console.WriteLine("There is another fix built for exactly this: ConcurrentBag,");
            Console.WriteLine("ConcurrentDictionary and the rest. That is the next lecture.");
        }

        string Fill()
        {
            List<int> list = new List<int>();

            Task[] tasks = new Task[TASKS];
            for (int i = 0; i < TASKS; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    for (int k = 0; k < N; k++)
                    {
                        lock (_gate)
                        {
                            list.Add(k);
                        }
                    }
                });
            }

            Task.WaitAll(tasks);

            if (list.Count == EXPECTED)
                return $"{list.Count}   correct";
            else
                return $"{list.Count}   WRONG";
        }
    }
}
