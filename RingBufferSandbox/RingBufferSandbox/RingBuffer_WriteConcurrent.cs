using System.Threading;

namespace RingBufferSandbox
{
    public class RingBuffer_WriteConcurrent : IRingBuffer
    {
        private long _read;
        private long _write;

        private readonly object[] _objects;

        public RingBuffer_WriteConcurrent(uint ringSize)
        {
            _objects = new object[ringSize];
        }

        private long Mask(long val) => val & _objects.Length - 1;

        private bool Empty() => _read == _write;

        private bool Full() => Mask(_write + 1) == _read;

        public long Size() => Mask(_write - _read);

        public void Push(object o)
        {
            while (Full()) { }
            var tmpWrite = Interlocked.Increment(ref _write);
            _objects[Mask(tmpWrite)] = o;
        }

        public object Pop()
        {
            while (Empty()) { }
            return _objects[Mask(_read++)];
        }
    }
}