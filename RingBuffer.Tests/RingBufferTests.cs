using FluentAssertions;
using RingBuffer.Lib;
using Xunit;

namespace RingBuffer.Tests;

public class RingBufferTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void WhenCreated_WithInvalidCapacity_ThrowException(int capacity)
    {
        // Act
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var _ = new RingBuffer<int>(capacity);
        });
    }

    [Fact]
    public void WhenCreated_CapacityIsAsDefined()
    {
        // Arrange
        var sut = new RingBuffer<int>(4);

        // Act
        var actual = sut.Capacity();

        // Assert
        actual.Should().Be(4);
    }

    [Fact]
    public void WhenCreated_IsEmpty()
    {
        // Arrange
        var sut = new RingBuffer<int>(4);

        // Act
        var actual = sut.Size();

        // Assert
        actual.Should().Be(0);
    }


    [Fact]
    public void Put_WhenEmpty_IncreaseSize()
    {
        // Arrange
        var sut = new RingBuffer<int>(4);

        var oldSize = sut.Size();

        // Act
        sut.Put(5);

        // Assert
        var newSize = sut.Size();
        newSize.Should().Be(oldSize + 1);
    }

    [Fact]
    public void Put_WhenNotEmptyAndNotFull_IncreaseSize()
    {
        // Arrange
        var sut = new RingBuffer<int>(4);
        sut.Put(5);

        var oldSize = sut.Size();

        // Act
        sut.Put(6);

        // Assert
        var newSize = sut.Size();
        newSize.Should().Be(oldSize + 1);
    }

    [Fact]
    public void Put_WhenFull_DontIncreaseSize()
    {
        // Arrange
        var sut = new RingBuffer<int>(4);
        sut.Put(5);
        sut.Put(6);
        sut.Put(7);
        sut.Put(8);

        var oldSize = sut.Size();

        // Act
        sut.Put(9);

        // Assert
        var newSize = sut.Size();
        newSize.Should().Be(oldSize);
    }

    [Fact]
    public void Put_CapacityDoesntChange()
    {
        // Arrange
        var sut = new RingBuffer<int>(4);

        // Act
        sut.Put(9);

        // Assert
        var actual = sut.Capacity();
        actual.Should().Be(4);
    }

    [Fact]
    public void Put_WhenCapacityExceeded_OverwriteOldestValue()
    {
        // Arrange
        var sut = new RingBuffer<int>(4);
        sut.Put(5);
        sut.Put(6);
        sut.Put(7);
        sut.Put(8);
        sut.Put(9);

        // Act/Assert
        sut.Get().Should().Be(6);
        sut.Get().Should().Be(7);
        sut.Get().Should().Be(8);
        sut.Get().Should().Be(9);

        sut.Size().Should().Be(0);
    }
    
    [Fact]
    public void Put_WhenCapacityExceeded_SizeEqualsCapacity()
    {
        // Arrange
        var sut = new RingBuffer<int>(4);
        sut.Put(5);
        sut.Put(6);
        sut.Put(7);
        sut.Put(8);
        sut.Put(9);

        // Act/Assert
        sut.Size().Should().Be(4);
    }


    [Fact]
    public void Get_WhenEmpty_ThrowException()
    {
        // Arrange
        var sut = new RingBuffer<int>(4);

        // Act/Assert
        Assert.Throws<InvalidOperationException>(() => { sut.Get(); });
    }

    [Fact]
    public void Get_CapacityDoesntChange()
    {
        // Arrange
        var sut = new RingBuffer<int>(4);
        sut.Put(9);

        // Act
        sut.Get();

        // Assert
        var actual = sut.Capacity();
        actual.Should().Be(4);
    }

    [Fact]
    public void Get_WhenNotEmpty_DecreaseSize()
    {
        // Arrange
        var sut = new RingBuffer<int>(4);
        sut.Put(5);
        sut.Put(6);

        var oldSize = sut.Size();

        // Act
        sut.Get();

        // Assert
        var newSize = sut.Size();
        newSize.Should().Be(oldSize - 1);
    }

    [Fact]
    public void Get_ReturnsOldestValue()
    {
        // Arrange
        var sut = new RingBuffer<int>(4);
        sut.Put(5);
        sut.Put(6);

        // Act/Assert
        sut.Get().Should().Be(5);
        sut.Size().Should().Be(1);
    }

    [Fact]
    public void Get_PutMultipleValues_GetMultipleValues()
    {
        // Arrange
        var sut = new RingBuffer<int>(4);
        sut.Put(5);
        sut.Put(6);
        sut.Put(7);
        sut.Put(8);

        // Act/Assert
        sut.Get().Should().Be(5);
        sut.Get().Should().Be(6);
        sut.Get().Should().Be(7);
        sut.Get().Should().Be(8);
        sut.Size().Should().Be(0);
    }

    
    [Fact]
    public void Test_SequenceOfAction()
    {
        // +5, +6, +7, -5, -6, +8, +9, -7, -8, -9

        // Arrange
        var sut = new RingBuffer<int>(4);

        // Act
        sut.Put(5);
        sut.Put(6);
        sut.Put(7);
        sut.Get().Should().Be(5);
        sut.Get().Should().Be(6);
        sut.Put(8);
        sut.Put(9);
        
        sut.Get().Should().Be(7);
        sut.Get().Should().Be(8);
        sut.Get().Should().Be(9);

        sut.Size().Should().Be(0);
    }


    [Fact]
    public void Test_RingWithOneElement()
    {
        // Arrange
        var sut = new RingBuffer<int>(1);

        // Act
        sut.Put(1);
        sut.Put(2);
        
        // Assert
        sut.Size().Should().Be(1);
        sut.Get().Should().Be(2);
    }
}