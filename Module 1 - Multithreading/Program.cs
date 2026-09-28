using System;
using System.Threading;

int counter = 0;
Thread[] threads = new Thread[8];

for (int i = 0; i < 8; i++)
{
    threads[i] = new Thread(() =>
    {
        for (int k = 0; k < 1_000_000; k++)
            counter++;      // three operations, not one
    });
    threads[i].Start();
}
foreach (Thread t in threads) t.Join();

Console.WriteLine(counter);   // expected: 8 000 000

