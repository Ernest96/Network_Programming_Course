using System;
using System.Threading;
using System.Threading.Tasks;

namespace Module3
{
    // What happens when the work of a thread or a task throws?
    //   Thread: nobody can catch it - the whole program dies.
    //   Task:   the exception is stored in the task and thrown again
    //   where you await it (or Wait / Result).
    class Ex11_Exceptions : IExample
    {
        public async Task RunAsync()
        {
            await TaskThenAwait();

            //ThreadThatThrows();
            //TaskThatThrows();
            //await TaskThatThrows();


            Thread.Sleep(5000);
        }

        // 1. Task, then await: the original exception - catch it like in normal code
        async Task TaskThenAwait()
        {
            Task task = Task.Run(() => DoWork("task 1"));
            try
            {
                await task;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine($"await caught        : {e.Message}");
            }
        }

        // 2. Task that throws exception
        async Task TaskThatThrows()
        {
            Task task = Task.Run(() => DoWork("task 2"));
            await task;
            Console.WriteLine("task 2 finished");
        }

        // 3. A thread: nobody can catch it - the whole process dies
        void ThreadThatThrows()
        {
            Thread thread = new Thread(() => DoWork("a thread"));
            thread.Start();
            thread.Join();
            Console.WriteLine("never printed - the process is already gone");
        }

        void DoWork(string who)
        {
            throw new InvalidOperationException($"boom in {who}");
        }
    }
}