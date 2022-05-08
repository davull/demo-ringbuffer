namespace RingBuffer.Lib;

/// <summary>
/// Uses a ring buffer to implement a fifo queue.
/// </summary>
public class RingBuffer<T>
{
    private Node<T> _head;
    private Node<T> _tail;

    private readonly int _capacity;
    private int _size;

    public RingBuffer(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(
                paramName: nameof(capacity),
                actualValue: 0,
                message: "Capacity must be greater than 0.");

        _capacity = capacity;

        _head
            = _tail
                = CreateRing(capacity: capacity);
    }

    /// <summary>
    /// Adds a new element to the buffer. If the buffer is already full, the
    /// oldest element shall be overwritten.
    /// </summary>
    public void Put(T value)
    {
        _head.Value = value;
        _head = _head.Next!;

        // If the buffer is already full, the tail also move to the next node.
        if (_size == _capacity)
            _tail = _tail.Next!;
        else
            _size++;
    }

    /// <summary>
    /// Get and remove the oldest element. If the buffer is empty, an
    /// exception will be raised.
    /// </summary>
    public T Get()
    {
        if (_size == 0)
            throw new InvalidOperationException("Buffer is empty.");

        _size--;

        var value = _tail.Value;
        _tail = _tail.Next!;

        return value;
    }

    /// <summary>
    /// Gets the current size of the buffer (i.e., the actual number of
    /// elements contained in the buffer).
    /// </summary>
    public int Size() => _size;

    /// <summary>
    /// Gets the capacity which was set when the buffer was instantiated.
    /// </summary>
    public int Capacity() => _capacity;


    // Create ring and return head element
    private static Node<T> CreateRing(int capacity)
    {
        var head = new Node<T>();
        var prev = head;

        for (var i = 0; i < capacity - 1; i++)
        {
            prev.Next = new Node<T>();
            prev = prev.Next;
        }

        prev.Next = head;

        return head;
    }
}