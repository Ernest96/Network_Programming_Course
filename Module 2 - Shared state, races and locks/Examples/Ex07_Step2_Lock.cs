using System;
using System.Threading;
using System.Threading.Tasks;

namespace Module2
{
    // lock marks a section of code that only ONE task may be inside at a time

    // The four rules for lock:
    //   1. lock on a private, readonly object - never lock(this), never a string
    //   2. hold it as briefly as possible - it is a queue
    //   3. never do I/O inside a lock
    //   4. every access takes the SAME lock
    class Ex07_Step2_Lock : IExample
    {
        const int TASKS = 8;
        const int N = 200_000;
        const int START = 2_000_000;

        readonly object _gate = new object();     // rule 1

        int _balance;
        int _withdrawals;

        public void Run()
        {
            Console.WriteLine($"start : {START}");
            Console.WriteLine($"{TASKS} tasks withdraw 1, {N} times each");
            Console.WriteLine();

            Withdraw(false);
            Report("without lock");

            Withdraw(true);
            Report("with lock   ");
        }

        void Withdraw(bool useLock)
        {
            _balance = START;
            _withdrawals = 0;

            Task[] tasks = new Task[TASKS];
            for (int i = 0; i < TASKS; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    for (int k = 0; k < N; k++)
                    {
                        if (useLock)
                        {
                            lock (_gate)          // one task in here at a time
                            {
                                _balance -= 1;         // rule 2: only the two
                                Thread.SpinWait(1);    // lines that must agree
                                _withdrawals += 1;
                            }
                        }
                        else
                        {
                            _balance -= 1;
                            Thread.SpinWait(1);    // in real code these two are
                            _withdrawals += 1;     // never adjacent instructions
                        }
                    }
                });
            }

            Task.WaitAll(tasks);
        }

        void Report(string label)
        {
            int sum = _balance + _withdrawals;

            Console.WriteLine($"{label} : balance {_balance} + withdrawals {_withdrawals} = {sum}");
            Console.WriteLine(sum == START
                ? "               the account adds up"
                : $"               it should be {START}. The account does not add up.");
            Console.WriteLine();
        }
    }
}
