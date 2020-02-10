using System;
using System.Threading;

namespace RingBufferSandbox
{
    class Program
    {
        private const int RingSize = 1048576;

        static void Main(string[] args)
        {
            Console.WriteLine($"Console thread: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("--- No concurrency implementation ---");
            Console.WriteLine();

            Console.WriteLine("Sequential run");
            SequentialRun.Sequential(new RingBuffer_NoConcurrency(RingSize));

            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine();

            Console.WriteLine("Parallel run (1 push, 1 pop)");
            ParallelRun_1Push_1Pop.Parallel(new RingBuffer_NoConcurrency(RingSize));

            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine();

            Console.WriteLine("Parallel run (3 push, 1 pop)");
            ParallelRun_3Push_1Pop.Parallel(new RingBuffer_NoConcurrency(RingSize));

            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine();

            //Console.WriteLine("Parallel run (3 push, 3 pop)");
            //ParallelRun_3Push_3Pop.Parallel(new RingBuffer_NoConcurrency(RingSize));

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("--- Write concurrency implementation ---");
            Console.WriteLine();

            Console.WriteLine("Sequential run");
            SequentialRun.Sequential(new RingBuffer_WriteConcurrent(RingSize));

            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine();

            Console.WriteLine("Parallel run (1 push, 1 pop)");
            ParallelRun_1Push_1Pop.Parallel(new RingBuffer_WriteConcurrent(RingSize));

            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine();

            Console.WriteLine("Parallel run (3 push, 1 pop)");
            ParallelRun_3Push_1Pop.Parallel(new RingBuffer_WriteConcurrent(RingSize));

            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine();

            //Console.WriteLine("Parallel run (3 push, 3 pop)");
            //ParallelRun_3Push_3Pop.Parallel(new RingBuffer_WriteConcurrent(RingSize));

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("--- Read & Write concurrency implementation ---");
            Console.WriteLine();

            Console.WriteLine("Sequential run");
            SequentialRun.Sequential(new RingBuffer_FullConcurrent(RingSize));

            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine();

            Console.WriteLine("Parallel run (1 push, 1 pop)");
            ParallelRun_1Push_1Pop.Parallel(new RingBuffer_FullConcurrent(RingSize));

            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine();

            Console.WriteLine("Parallel run (3 push, 1 pop)");
            ParallelRun_3Push_1Pop.Parallel(new RingBuffer_FullConcurrent(RingSize));

            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine();

            //Console.WriteLine("Parallel run (3 push, 3 pop)");
            //ParallelRun_3Push_3Pop.Parallel(new RingBuffer_FullConcurrent(RingSize));
        }
    }
}
