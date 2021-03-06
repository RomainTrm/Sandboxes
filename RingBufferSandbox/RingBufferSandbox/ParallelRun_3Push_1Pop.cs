﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace RingBufferSandbox
{
    class ParallelRun_3Push_1Pop
    {
        private const int NbOfPush = 1000000;

        public static void Parallel(IRingBuffer ringBuffer)
        {
            var threadPushing1 = new Thread(() => Pushing(ringBuffer, 1));
            var threadPushing2 = new Thread(() => Pushing(ringBuffer, 2));
            var threadPushing3 = new Thread(() => Pushing(ringBuffer, 3));
            var threadPopping = new Thread(() => Popping(ringBuffer));

            threadPopping.Start();
            threadPushing1.Start();
            threadPushing2.Start();
            threadPushing3.Start();

            Thread.Sleep(800);

            threadPopping.Interrupt();
            threadPushing1.Interrupt();
            threadPushing2.Interrupt();
            threadPushing3.Interrupt();
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

        private static void Popping(IRingBuffer ringBuffer)
        {
            Console.WriteLine($"Popping thread: {Thread.CurrentThread.ManagedThreadId}");
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

                // Doesn't pop every elements due to push concurrency (NoConcurrency implementation), thread should write on the same slot
                // Pop 3 000 000 elements as expected with write concurrency implementation

                // Sometimes pop only 1 951 424 elements, correspond to 3 000 000 - 1048576 (ring size)
                // Issue : Read misses one complete tour of the ring
                
                Console.WriteLine($"Nb of element popped : {nbOfElementsPopped}"); 
            }
        }
    }
}