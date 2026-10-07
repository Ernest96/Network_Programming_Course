using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Module3
{
    // Every single call on a ConcurrentDictionary is atomic.
    // Two calls in a row are NOT - another task can run in between.
    // So do the whole step in ONE call: the Try... methods and AddOrUpdate.
    class Ex05_ConcurrentDictionary : IExample
    {
        const int TASKS = 4;
        const int N = 100_000;

        public async Task RunAsync()
        {
            ShowMethods();
        }

        void ShowMethods()
        {
            ConcurrentDictionary<string, int> cache = new ConcurrentDictionary<string, int>();
            int value;

            // Add only if the key is absent; returns false if it was already there
            bool added = cache.TryAdd("a", 1);
            Console.WriteLine($"TryAdd(a, 1)       : {added}");
            added = cache.TryAdd("a", 100);
            Console.WriteLine($"TryAdd(a, 100)     : {added}");

            // Read safely
            if (cache.TryGetValue("a", out value))
                Console.WriteLine($"TryGetValue(a)     : True, value {value}");

            // Compare-and-swap: update only if the current value equals the expected one
            bool updated = cache.TryUpdate("a", newValue: 2, comparisonValue: 1);
            Console.WriteLine($"TryUpdate(a, 2, 1) : {updated} - a was 1, now it is {cache["a"]}");
            updated = cache.TryUpdate("a", newValue: 3, comparisonValue: 1);
            Console.WriteLine($"TryUpdate(a, 3, 1) : {updated} - a is {cache["a"]}");

            // Remove and get the old value
            bool removedOk = cache.TryRemove("a", out int removed);
            Console.WriteLine($"TryRemove(a)       : {removedOk}, the old value was {removed}");

            bool found = cache.TryGetValue("a", out value);
            Console.WriteLine($"TryGetValue(a)     : {found} - a is gone");
        }
    }
}
