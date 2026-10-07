using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Module3
{
    // WRONG: a thread waits for a flag by checking it again and again.
    //It burns a whole core while it waits - look at the CPU time below.
    class Ex08a_BusyWait_Wrong : IExample
    {
        bool ready = false;                     // a plain bool

        public Task RunAsync()
        {
            TimeSpan cpuBefore = Process.GetCurrentProcess().TotalProcessorTime;

            Thread waiter = new Thread(() => WaitForDataAndProcess());
            waiter.IsBackground = true;
            waiter.Start();

            Thread.Sleep(2000);
            ready = true;

            bool finished = waiter.Join(3000);
            TimeSpan cpu = Process.GetCurrentProcess().TotalProcessorTime - cpuBefore;

            if (finished)
                Console.WriteLine("waiter finished");
            else
                Console.WriteLine("waiter never saw ready = true - it is still spinning");

            Console.WriteLine($"CPU time used while waiting: {cpu.TotalMilliseconds} ms");
            return Task.CompletedTask;
        }

        private void WaitForDataAndProcess()
        {
            while (!ready)
            {
                // still a busy wait: check, check, check for data to arrive
                // a bit better
                //Thread.Sleep(100); 
            }
            Console.WriteLine("waiter: saw ready");
            Console.WriteLine("Processing data which arrived!");
        }
    }
}
