namespace RingBufferSandbox
{
    public interface IRingBuffer
    {
        long Size();
        void Push(object o);
        object Pop();
    }
}