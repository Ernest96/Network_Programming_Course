using System;
using System.Threading;

namespace Module1
{
    // A FOREGROUND thread keeps the process alive: the program does not exit
    // until it finishes, even though nobody called Join().
    // A BACKGROUND thread does not. The process exits and kills it.
    // new Thread() is FOREGROUND by default - the usual reason a console
    // program "hangs" and refuses to close.
    class Ex05_ForegroundBackground : IExample
    {

        public void Run()
        {
            Thread worker = new Thread(() =>
            {
                Thread.Sleep(3000);
                Console.WriteLine("[worker] finished");
            });

            worker.IsBackground = false;

            worker.Start();

        }
    }
}
