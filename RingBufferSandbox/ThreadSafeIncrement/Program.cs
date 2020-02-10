using System;
using System.Linq;
using System.Threading;

namespace ThreadatomicIncrement
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Unatomic increment");

            int counter = 0;

            var unatomic_thread1 = new Thread(() => Unatomic_Inc(ref counter));
            var unatomic_thread2 = new Thread(() => Unatomic_Inc(ref counter));
            var unatomic_thread3 = new Thread(() => Unatomic_Inc(ref counter));

            unatomic_thread1.Start();
            unatomic_thread2.Start();
            unatomic_thread3.Start();

            Thread.Sleep(500);

            Console.WriteLine($"Counter: {counter}");

            Console.WriteLine();
            Console.WriteLine("atomic increment");

            counter = 0;

            var atomic_thread1 = new Thread(() => Atomic_Inc(ref counter));
            var atomic_thread2 = new Thread(() => Atomic_Inc(ref counter));
            var atomic_thread3 = new Thread(() => Atomic_Inc(ref counter));

            atomic_thread1.Start();
            atomic_thread2.Start();
            atomic_thread3.Start();

            Thread.Sleep(500);

            Console.WriteLine($"Counter: {counter}");

        }

        private static void Unatomic_Inc(ref int counter)
        {
            foreach (var _ in Enumerable.Range(0, 1000000))
            {
                counter++;
            }
        }

        private static void Atomic_Inc(ref int counter)
        {
            foreach (var _ in Enumerable.Range(0, 1000000))
            {
                Interlocked.Increment(ref counter);
            }
        }
    }
}
