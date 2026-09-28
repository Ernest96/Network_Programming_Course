using System;
using System.Threading.Tasks;

namespace Module2
{
    // counter++ is not one operation. It is three:
    //     READ  counter        -> 41
    //     ADD   41 + 1         -> 42
    //     WRITE 42             -> counter
    // The operating system can stop a task between any two of them.
    class Ex04_LostUpdate : IExample
    {
        const int TASKS_COUNT = 8;
        const int N = 1_000_000;

        int _counter; // race condition

        public void Run()
        {
            _counter = 0;

            Task[] tasks = new Task[TASKS_COUNT];
            for (int i = 0; i < TASKS_COUNT; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    for (int k = 0; k < N; k++)
                        _counter++;
                });
            }

            Task.WaitAll(tasks);

            int expected = TASKS_COUNT * N;
            int lost = expected - _counter;

            Console.WriteLine($"tasks     : {TASKS_COUNT}");
            Console.WriteLine($"expected  : {expected}");
            Console.WriteLine($"actual    : {_counter}");
            Console.WriteLine($"lost      : {lost}");
            Console.WriteLine();
            Console.WriteLine("Run it again. A different answer every time is the definition of a race.");
        }
    }
}
