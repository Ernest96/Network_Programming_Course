using System;
using System.Threading;
using System.Threading.Tasks;

namespace Module2
{
    // In a shop this is selling the last ticket twice. In a bank it is an
    // overdraft. In a file server it is two writers creating the same file.
    //
    // Note what is NOT wrong here. The guard is correct. The subtraction is
    // correct. Each line, read on its own, is right. Only the gap between
    // them is the multithreading bug - which is why you will never find it by reading.
    class Ex05_CheckThenAct : IExample
    {
        const int CASHIERS = 4;
        const int TICKETS = 100000;

        int _remaining;
        int _sold;
        readonly object _gate = new object();

        public void Run()
        {
            _remaining = TICKETS;
            _sold = 0;

            Console.WriteLine($"tickets on sale : {TICKETS}");
            Console.WriteLine($"cashiers        : {CASHIERS}");
            Console.WriteLine("Every cashier checks the counter before selling.\n");

            Task[] tasks = new Task[CASHIERS];
            for (int i = 0; i < CASHIERS; i++)
                tasks[i] = Task.Run(() => Sell());

            Task.WaitAll(tasks);

            Console.WriteLine($"tickets available : {TICKETS}");
            Console.WriteLine($"tickets sold      : {_sold}");
            Console.WriteLine($"counter says      : {_remaining} left");
            Console.WriteLine();

            if (_sold > TICKETS)
                Console.WriteLine($"OVERSOLD by {_sold - TICKETS}. ");
            else
                Console.WriteLine("Not oversold this run - run it again, or raise CASHIERS.");

            if (_remaining < 0)
                Console.WriteLine($"The counter went negative ({_remaining})");
        }

        void Sell()
        {
            while (true)
            {
                lock (_gate)
                {
                    if (_remaining <= 0)
                        return;

                    _remaining--;
                    _sold++;
                }
               
            }
        }
    }
}
