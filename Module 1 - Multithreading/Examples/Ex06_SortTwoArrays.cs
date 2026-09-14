using System;
using System.Diagnostics;
using System.Threading;

namespace Module1
{
    // Two threads, two arrays, one sort each. No locks are needed and none
    // are used - because the two threads never touch the same memory.
    // THE RULE: threads are only dangerous when they SHARE something.
    // Split the data first and the danger disappears.
    class Ex06_SortTwoArrays : IExample
    {
        const int SMALL_SIZE = 15;

        public void Run()
        {
            // ---- small arrays: look at the result --------------------
            int[] a = RandomArray(SMALL_SIZE, 1);
            int[] b = RandomArray(SMALL_SIZE, 2);

            Print("A before", a);
            Print("B before", b);

            Thread t1 = new Thread(() => Array.Sort(a));
            Thread t2 = new Thread(() => Array.Sort(b));

            t1.Start();
            t2.Start();

            t1.Join();   
            t2.Join();   

            Console.WriteLine();
            Print("A sorted", a);
            Print("B sorted", b);
        }

        int[] RandomArray(int size, int seed)
        {
            Random rnd = new Random(seed);
            int[] result = new int[size];
            for (int i = 0; i < size; i++)
                result[i] = rnd.Next(0, 1000);
            return result;
        }

        void Print(string label, int[] values)
        {
            Console.Write($"{label}: ");
            foreach (int v in values)
                Console.Write($"{v,5}");
            Console.WriteLine();
        }
    }
}
