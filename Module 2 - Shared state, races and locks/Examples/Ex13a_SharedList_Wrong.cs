using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module2
{
    // WRONG: 4 tasks call Add on the same List<int>, with no lock.
    class Ex13a_SharedList_Wrong : IExample
    {
        const int TASKS = 4;
        const int N = 25_000;
        const int RUNS = 5;

        const int EXPECTED = TASKS * N;

        public void Run()
        {
            Console.WriteLine($"{TASKS} tasks, {N} Add calls each. Expect {EXPECTED}.");
            Console.WriteLine();

            for (int run = 1; run <= RUNS; run++)
                Console.WriteLine($"  run {run} : {Fill()}");
        }

        string Fill()
        {
            List<int> list = new List<int>();
            string error = null;

            Task[] tasks = new Task[TASKS];
            for (int i = 0; i < TASKS; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    try
                    {
                        for (int k = 0; k < N; k++)
                            list.Add(k); 
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                    }
                });
            }

            Task.WaitAll(tasks);

            if (error != null)
                return $"threw {error}";
            

            if (list.Count == EXPECTED)
                return $"{list.Count}   correct";
            else
                return $"{list.Count}   WRONG, and it said nothing";
        }
    }
}
