using System;

namespace RingBufferSandbox
{
    public class EmptyRingException : Exception
    {
        public EmptyRingException() : base("Ring is empty") { }
    }
}