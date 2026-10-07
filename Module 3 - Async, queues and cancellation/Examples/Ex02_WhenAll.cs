using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Module3
{
    // Three downloads of 1 second each.
    // Await them one by one: 3 seconds. Start all three, then await: 1 second.
    class Ex02_WhenAll : IExample
    {
        public async Task RunAsync()
        {
            Stopwatch sw = Stopwatch.StartNew();

            string a = await DownloadAsync("a");
            string b = await DownloadAsync("b");
            string c = await DownloadAsync("c");

            Console.WriteLine($"one after another: {sw.ElapsedMilliseconds} ms  ({a}, {b}, {c})");

            sw.Restart();

            Task<string> ta = DownloadAsync("a");
            Task<string> tb = DownloadAsync("b");
            Task<string> tc = DownloadAsync("c");
            string[] all = await Task.WhenAll(ta, tb, tc);

            Console.WriteLine($"all together: {sw.ElapsedMilliseconds} ms  ({string.Join(", ", all)})");
        }

        async Task<string> DownloadAsync(string name)
        {
            await Task.Delay(1000);
            return name + ".txt";
        }
    }
}
