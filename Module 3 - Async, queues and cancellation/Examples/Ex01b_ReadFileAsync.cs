using System;
using System.IO;
using System.Threading.Tasks;

namespace Module3
{
    // Ex01 with real I/O instead of Task.Delay: reading a .txt file.
    // File.ReadAllText: the thread waits for the disk, doing nothing.
    // await File.ReadAllTextAsync: the method waits - the thread goes back to the pool.
    class Ex01b_ReadFileAsync : IExample
    {
        public async Task RunAsync()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "data.txt");

            Console.WriteLine($"before ReadAllText      : thread {Environment.CurrentManagedThreadId}");
            string text = File.ReadAllText(path);
            Console.WriteLine($"after ReadAllText       : thread {Environment.CurrentManagedThreadId}");

            Console.WriteLine();

            Console.WriteLine($"before ReadAllTextAsync : thread {Environment.CurrentManagedThreadId}");
            text = await File.ReadAllTextAsync(path);
            Console.WriteLine($"after ReadAllTextAsync  : thread {Environment.CurrentManagedThreadId}");

            Console.WriteLine(text);
        }
    }
}
