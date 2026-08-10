using Turing.Core.Components.Logic;

namespace Turing.Tests.Components;

[TestFixture]
internal class BIT_DECODER_THREETests
{
    [TestCase(0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0)]
    [TestCase(0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0)]
    [TestCase(0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0)]
    [TestCase(0, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0)]
    [TestCase(1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0)]
    [TestCase(1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0)]
    [TestCase(1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0)]
    [TestCase(1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1)]
    [TestCase(0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0)]
    [TestCase(0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0)]
    [TestCase(0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0)]
    [TestCase(0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0)]
    [TestCase(1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0)]
    [TestCase(1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0)]
    [TestCase(1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0)]
    [TestCase(1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0)]
    public void Decoder3_ImplicitConversion_ReturnsCorrectOutput(
        int inputA, int inputB, int inputC, int disable,
        int expectedY0, int expectedY1, int expectedY2, int expectedY3,
        int expectedY4, int expectedY5, int expectedY6, int expectedY7)
    {
        // Arrange
        var a = new Bit(inputA);
        var b = new Bit(inputB);
        var c = new Bit(inputC);
        var d = new Bit(disable);

        // Act
        Byte @byte = new BIT_DECODER_THREE(a, b, c, d);

        var actualY0 = @byte.GetBit(0);
        var actualY1 = @byte.GetBit(1);
        var actualY2 = @byte.GetBit(2);
        var actualY3 = @byte.GetBit(3);
        var actualY4 = @byte.GetBit(4);
        var actualY5 = @byte.GetBit(5);
        var actualY6 = @byte.GetBit(6);
        var actualY7 = @byte.GetBit(7);

        // Assert
        Assert.That(actualY0, Is.EqualTo(new Bit(expectedY0)));
        Assert.That(actualY1, Is.EqualTo(new Bit(expectedY1)));
        Assert.That(actualY2, Is.EqualTo(new Bit(expectedY2)));
        Assert.That(actualY3, Is.EqualTo(new Bit(expectedY3)));
        Assert.That(actualY4, Is.EqualTo(new Bit(expectedY4)));
        Assert.That(actualY5, Is.EqualTo(new Bit(expectedY5)));
        Assert.That(actualY6, Is.EqualTo(new Bit(expectedY6)));
        Assert.That(actualY7, Is.EqualTo(new Bit(expectedY7)));
    }
}