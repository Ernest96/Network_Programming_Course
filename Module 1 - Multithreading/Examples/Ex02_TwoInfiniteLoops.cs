using System;
using System.Threading;

namespace Module1
{
    // Two threads print A and B forever, at the same time.
    // The pattern is never a clean ABABAB - the OS decides who runs and when.
    class Ex02_TwoInfiniteLoops : IExample
    {
        const int DELAY = 200;

        public void Run()
        {
            Console.WriteLine($"Two threads print A and B every {DELAY} ms.");
            Console.WriteLine("Press Enter to stop.\n");

            Thread a = new Thread(() => PrintForever('A')) { IsBackground = true };
            Thread b = new Thread(() => PrintForever('B')) { IsBackground = true };

            a.Start();
            b.Start();

            Console.ReadLine();   // main thread waits here; A and B keep going

            Console.WriteLine("\n");
            Console.WriteLine("Stopped.");
        }

        void PrintForever(char c)
        {
            while (true)
            {
                Console.Write(c);
                Thread.Sleep(DELAY);
            }
        }
    }
}
