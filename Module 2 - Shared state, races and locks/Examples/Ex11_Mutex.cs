using System;
using System.Threading;

namespace Module2
{
    // lock lives inside ONE process. Two copies of your program do not see
    // each other's locks at all.
    //
    // A NAMED Mutex is known to the operating system, so it works across
    // processes. The price is that every Wait and Release is a system call -
    // tens of times slower than lock. Inside one process, lock always wins.
    class Ex11_Mutex : IExample
    {
        const string NAME = @"Global\Module2-Demo";
        const int HOLD_MS = 20000;

        public void Run()
        {
            Console.WriteLine($"asking the operating system for the mutex {NAME}");

            using (Mutex mutex = new Mutex(false, NAME))
            {
                mutex.WaitOne();
                try
                {
                    Console.WriteLine("got it");
                    Console.WriteLine();

                    Console.WriteLine($"  holding it... {HOLD_MS} ");
                    Thread.Sleep(HOLD_MS);
                }
                finally
                {
                    mutex.ReleaseMutex();
                    Console.WriteLine();
                    Console.WriteLine("released");
                }
            }

            Console.WriteLine();
            Console.WriteLine("This is how \"only one copy of this app may run\" is done.");
        }
    }
}