using System;
using System.Diagnostics;
using System.Linq;

namespace RingBufferSandbox
{
    static class SequentialRun
    {
        public static void Sequential()
        {
            var ringBuffer = new RingBuffer(1048576);

            Console.WriteLine("Start pushing");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            foreach (var i in Enumerable.Range(0, 1000000))
            {
                ringBuffer.Push(i);
            }

            stopwatch.Stop();
            Console.WriteLine($"Pushing done in {stopwatch.ElapsedMilliseconds} milliseconds");
            Console.WriteLine($"{ringBuffer.Size()} elements inside the buffer");

            Console.WriteLine("Press any key to read.");

            stopwatch.Reset();
            stopwatch.Start();

            try
            {
                while (ringBuffer.Size() > 0)
                {
                    var r = ringBuffer.Pop();
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
                Console.WriteLine($"{ringBuffer.Size()} elements inside the buffer");
            }
        }
    }
}