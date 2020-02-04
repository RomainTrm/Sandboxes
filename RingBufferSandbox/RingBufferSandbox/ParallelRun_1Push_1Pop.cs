using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace RingBufferSandbox
{
    class ParallelRun_1Push_1Pop
    {
        private const int NbOfPush = 1000000;

        public static void Parallel()
        {
            var ringBuffer = new RingBuffer(1048576);

            var threadPushing = new Thread(() => Pushing(ringBuffer));
            var threadPoping = new Thread(() => Poping(ringBuffer));

            threadPushing.Start();
            threadPoping.Start();

            Thread.Sleep(200);
        }

        private static void Pushing(RingBuffer ringBuffer)
        {
            Console.WriteLine("Start pushing");
            Console.WriteLine($"Nb of elements to push: {NbOfPush}");
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            foreach (var i in Enumerable.Range(0, NbOfPush))
            {
                ringBuffer.Push(i);
            }

            stopwatch.Stop();
            Console.WriteLine($"Pushing done in {stopwatch.ElapsedMilliseconds} milliseconds");
        }

        private static void Poping(RingBuffer ringBuffer)
        {
            Console.WriteLine("Start popping");

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
                Console.WriteLine($"Popping done in {stopwatch.ElapsedMilliseconds} milliseconds");
                Console.WriteLine($"Nb of element popped : {nbOfElementsPopped}");
            }
        }
    }
}