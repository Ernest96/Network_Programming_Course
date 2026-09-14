using System;
using System.Diagnostics;
using System.Threading;

namespace Module1
{
    // Creates N threads that do nothing but sleep, and measures what that cost.
    //
    // .NET reserves 1 MB of ADDRESS SPACE per thread stack, but only the pages
    // actually touched are committed - so the working set grows by far less
    // than N megabytes.

    // Think of a hotel. Each new thread books a 1 MB room (its stack), but the
    // OS only cleans and furnishes the part of the room the guest actually walks
    // into. Our guests take two steps and fall asleep, so ~1 MB is booked and
    // only ~30 KB is used.
    class Ex09_ManyThreadsCost : IExample
    {
        const int COUNT = 1000;

        public void Run()
        {
            Process me = Process.GetCurrentProcess();
            me.Refresh();
            long before = me.WorkingSet64;

            Console.WriteLine($"threads to create : {COUNT:N0}");
            Console.WriteLine($"working set before: {before / 1024:N0} KB\n");

            Thread[] threads = new Thread[COUNT];

            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < COUNT; i++)
            {
                threads[i] = new Thread(() => Thread.Sleep(Timeout.Infinite))
                {
                    IsBackground = true
                };
                threads[i].Start();
            }

            sw.Stop();

            double perThread = (double)sw.ElapsedMilliseconds / COUNT;
            Console.WriteLine($"created and started in {sw.ElapsedMilliseconds} ms  ({perThread:F3} ms each)");

            Thread.Sleep(500); // let them all actually get going

            me.Refresh();
            long after = me.WorkingSet64;

            Console.WriteLine($"working set after : {after / 1024:N0} KB");
            Console.WriteLine($"difference        : {(after - before) / 1024:N0} KB");

            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
    }
}