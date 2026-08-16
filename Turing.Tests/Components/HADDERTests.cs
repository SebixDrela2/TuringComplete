using Turing.Core.Components.Logic;

namespace Turing.Tests.Components;

[TestFixture]
internal class HADDERTests
{
    [TestCase(0, 0, 0, 0)]
    [TestCase(0, 1, 1, 0)]
    [TestCase(1, 0, 1, 0)]
    [TestCase(1, 1, 0, 1)]
    public void HADDER_ImplicitConversion_WithBitInputs_ReturnsCorrectOutput(
        int inputA, int inputB, int expectedSum, int expectedCarry)
    {
        // Arrange
        var a = new Bit(inputA);
        var b = new Bit(inputB);
        var expectedSumBit = new Bit(expectedSum);
        var expectedCarryBit = new Bit(expectedCarry);

        // Act
        (Bit Sum, Bit Carry) actual = new HADDER(a, b);

        // Assert
        Assert.That(actual.Sum, Is.EqualTo(expectedSumBit));
        Assert.That(actual.Carry, Is.EqualTo(expectedCarryBit));
    }
}