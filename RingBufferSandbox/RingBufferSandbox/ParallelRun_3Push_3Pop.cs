using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace RingBufferSandbox
{
    class ParallelRun_3Push_3Pop
    {
        private const int NbOfPush = 1000000;

        public static void Parallel(IRingBuffer ringBuffer)
        {
            var threadPushing1 = new Thread(() => Pushing(ringBuffer, 1));
            var threadPushing2 = new Thread(() => Pushing(ringBuffer, 2));
            var threadPushing3 = new Thread(() => Pushing(ringBuffer, 3));
            var threadPopping1 = new Thread(() => Popping(ringBuffer, 1));
            var threadPopping2 = new Thread(() => Popping(ringBuffer, 2));
            var threadPopping3 = new Thread(() => Popping(ringBuffer, 3));

            threadPopping1.Start();
            threadPopping2.Start();
            threadPopping3.Start();
            threadPushing1.Start();
            threadPushing2.Start();
            threadPushing3.Start();

            Thread.Sleep(500);
        }

        private static void Pushing(IRingBuffer ringBuffer, int threadNumber)
        {
            Console.WriteLine($"Pushing thread {threadNumber}: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Thread {threadNumber}: Start pushing");
            Console.WriteLine($"Thread {threadNumber}: Nb of elements to push: {NbOfPush}");
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                foreach (var i in Enumerable.Range(0, NbOfPush))
                {
                    ringBuffer.Push(i);
                }
            }
            catch (FullRingException e)
            {
                Console.WriteLine(e.Message);
            }

            stopwatch.Stop();
            Console.WriteLine($"Thread {threadNumber}: Pushing done in {stopwatch.ElapsedMilliseconds} milliseconds");
        }

        private static void Popping(IRingBuffer ringBuffer, int threadNumber)
        {
            Console.WriteLine($"Popping thread {threadNumber}: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Thread {threadNumber}: Start popping");

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            uint nbOfElementsPopped = 0;

            try
            {
                var spleepCount = 0;

                while (true)
                {
                    if (spleepCount == 5) break;
                    if (ringBuffer.Size() == 0)
                    {
                        Thread.Sleep(2);
                        spleepCount += 1;
                        continue;
                    }

                    spleepCount = 0;
                    var r = ringBuffer.Pop();
                    nbOfElementsPopped += 1;
                }
            }
            catch (EmptyRingException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                stopwatch.Stop();
                Console.WriteLine($"Thread {threadNumber}: Popping done in {stopwatch.ElapsedMilliseconds} milliseconds");
                Console.WriteLine($"Thread {threadNumber}: Nb of element popped : {nbOfElementsPopped}"); 
            }
        }
    }
}