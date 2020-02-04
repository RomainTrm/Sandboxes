namespace RingBufferSandbox
{
    /// <summary>
    /// Solution inspired from : https://www.snellman.net/blog/archive/2016-12-13-ring-buffers/
    /// </summary>
    public class RingBuffer
    {
        private uint _read;
        private uint _write;

        private readonly object[] _objects;

        public RingBuffer(uint ringSize)
        {
            _objects = new object[ringSize];
        }

        private uint Mask(uint val) => val & (uint)(_objects.Length - 1);
        
        private bool Empty() => _read == _write;

        private bool Full() => _write + 1 == _read;

        public uint Size() => Mask(_write - _read);

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
