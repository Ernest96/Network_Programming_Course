using System;
using System.Threading;

namespace Module1
{
    // How to give a thread its input - 2 ways.
    class Ex04_PassingParameters : IExample
    {
        public void Run()
        {
            // 1. Start(object): the method takes ONE parameter, of type object
            Thread t1 = new Thread(Greet);
            t1.Start("Ana");
            t1.Join();

            // 2. A lambda: call any method, with any parameters and real types
            Thread t2 = new Thread(() => Repeat("Ion", 3));
            t2.Start();
            t2.Join();
        }

        void Greet(object data)
        {
            string name = (string)data; // type cast
            Console.WriteLine($"Hello, {name}!");
        }

        void Repeat(string word, int times)
        {
            for (int i = 1; i <= times; i++)
                Console.WriteLine($"{word} {i}/{times}");
        }
    }
}
