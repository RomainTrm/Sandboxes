namespace RingBufferSandbox
{
    /// <summary>
    /// Solution inspired from : https://www.snellman.net/blog/archive/2016-12-13-ring-buffers/
    /// </summary>
    public class RingBuffer_NoConcurrency : IRingBuffer
    {
        private long _read;
        private long _write;

        private readonly object[] _objects;

        public RingBuffer_NoConcurrency(long ringSize)
        {
            _objects = new object[ringSize];
        }

        private long Mask(long val) => val & _objects.Length - 1;
        
        private bool Empty() => _read == _write;

        private bool Full() => _write + 1 == _read;

        public long Size() => Mask(_write - _read);

        public void Push(object o)
        {
            if (Full()) throw new FullRingException();
            _objects[Mask(_write++)] = o;
        }

        public object Pop()
        {
            if (Empty()) throw new EmptyRingException();
            return _objects[Mask(_read++)];
        }
    }
}
