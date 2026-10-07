using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Module3
{
    // OK: the same wait, but the waiter sleeps until it is told.
    // ManualResetEventSlim: Wait() blocks thread (no CPU burn)
    class Ex08c_BusyWait_Ok : IExample
    {
        private ManualResetEventSlim _signaler = new ManualResetEventSlim(false);
        
        public Task RunAsync()
        {
            TimeSpan cpuBefore = Process.GetCurrentProcess().TotalProcessorTime;

            Thread waiter = new Thread(() => WaitForDataAndProcess());
            waiter.Start();

            Thread.Sleep(2000);
            _signaler.Set();

            waiter.Join();
            TimeSpan cpu = Process.GetCurrentProcess().TotalProcessorTime - cpuBefore;

            Console.WriteLine("waiter finished");
            Console.WriteLine($"CPU time used while waiting: {cpu.TotalMilliseconds} ms");

            _signaler.Dispose();
            return Task.CompletedTask;
        }
        
        void WaitForDataAndProcess()
        {
            _signaler.Wait();
            Console.WriteLine("waiter: saw ready");
            Console.WriteLine("Processing data which arrived!");
        }
    }
}
