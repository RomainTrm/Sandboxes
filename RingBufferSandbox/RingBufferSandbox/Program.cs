using System;

namespace RingBufferSandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sequential run");
            SequentialRun.Sequential();

            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine();

            Console.WriteLine("Parallel run (1 push, 1 pop)");
            ParallelRun_1Push_1Pop.Parallel();

            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine();

            Console.WriteLine("Parallel run (3 push, 1 pop)");
            ParallelRun_3Push_1Pop.Parallel();
        }
    }
}
