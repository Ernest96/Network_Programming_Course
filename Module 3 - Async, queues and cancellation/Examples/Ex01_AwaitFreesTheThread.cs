using System;
using System.Threading;
using System.Threading.Tasks;

namespace Module3
{
    // Thread.Sleep: the thread waits, doing nothing, and nobody else can use it.
    // await: the method pauses, the thread goes back to the pool,
    // and the rest of the method runs later - maybe on another thread.
    class Ex01_AwaitFreesTheThread : IExample
    {
        public async Task RunAsync()
        {
            Console.WriteLine($"before Sleep : thread {Environment.CurrentManagedThreadId}");
            Thread.Sleep(500);
            Console.WriteLine($"after Sleep  : thread {Environment.CurrentManagedThreadId}");

            Console.WriteLine();

            Console.WriteLine($"before await : thread {Environment.CurrentManagedThreadId}");
            await Task.Delay(500);
            Console.WriteLine($"after await  : thread {Environment.CurrentManagedThreadId}");

            Console.WriteLine();
            Console.WriteLine("Same 500 ms. Sleep kept its thread; await gave it back,");
            Console.WriteLine("and a pool thread picked the method up when the time was over.");
        }
    }
}
