using System.Diagnostics.CodeAnalysis;

namespace RingBuffer.Lib;

/// <summary>
/// Represents a single item in the buffer.
/// </summary>
internal class Node<T>
{
    /// <summary>
    /// Next node in the buffer.
    /// </summary>
    public Node<T>? Next { get; set; }

    /// <summary>
    /// Value of the node.
    /// </summary>
    public T Value { get; set; } = default!;

    [ExcludeFromCodeCoverage]
    public override string ToString() => $"{nameof(Value)}: {Value}";
}