using System;

namespace RingBufferSandbox
{
    public class FullRingException : Exception
    {
        public FullRingException() : base("Ring is full") { }
    }
}