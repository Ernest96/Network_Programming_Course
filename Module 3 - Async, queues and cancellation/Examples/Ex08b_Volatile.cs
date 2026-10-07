using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Module3
{
    // 1. It fixes the endless loop of Ex08a, also in Release.
    //    But the wait is still a busy wait: look at the CPU time.
    // 2. It does not make counter++ atomic: read, add, write are still three steps.
    class Ex08b_Volatile : IExample
    {
        const int THREADS = 4;
        const int N = 1_000_000;

        volatile bool ready = false;

        public Task RunAsync()
        {
            // 1. the flag from Ex08a, now volatile
            TimeSpan cpuBefore = Process.GetCurrentProcess().TotalProcessorTime;

            Thread waiter = new Thread(() => WaitForDataAndProcess());
            waiter.Start();

            Thread.Sleep(2000);
            ready = true;
            waiter.Join();

            TimeSpan cpu = Process.GetCurrentProcess().TotalProcessorTime - cpuBefore;
            Console.WriteLine($"CPU time used while waiting: {cpu.TotalMilliseconds} ms - still a busy wait");

            
            return Task.CompletedTask;
        }

        private void WaitForDataAndProcess()
        {
            while (!ready)
            {
                // still a busy wait: check, check, check for data to arrive
                // a bit better
                // //Thread.Sleep(100); 
            }
            Console.WriteLine("waiter: saw ready");
            Console.WriteLine("Processing data which arrived!");
        }
    }
}
