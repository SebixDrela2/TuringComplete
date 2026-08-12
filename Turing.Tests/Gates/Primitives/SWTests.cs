using Turing.Core.Electricity;
using Turing.Core.Gates.Primitives;

namespace Turing.Tests.Gates.Primitives;

[TestFixture]
internal class SWTests
{
    [TestCase(0, 0, 0)]
    [TestCase(0, 1, 0)]
    [TestCase(1, 0, 0)]
    [TestCase(1, 1, 1)]
    public void SW_Evaluate_When_Input_Provided_Returns_Transistor_Behaviour(int input, int condition, int expectedOutputInt)
    {
        // Arrange
        var source = new Bit(input);
        var gate = new Bit(condition);
        var expectedOutput = new Bit(expectedOutputInt);

        // Act
        Bit actualOutput = new SW<Bit>(gate, source);

        // Assert
        Assert.That(actualOutput, Is.EqualTo(expectedOutput));
    }

    [TestCase(0, 0, 0)]
    [TestCase(0, 1, 0)]
    [TestCase(1, 0, 0)]
    [TestCase(1, 1, 1)]
    public void SW_Evaluate_When_Input_Provided_With_Byte_Returns_Transistor_Behaviour(int input, int condition, int expectedOutputInt)
    {
        // Arrange
        var source = new Byte(input);
        var gate = new Bit(condition);
        var expectedOutput = new Byte(expectedOutputInt);

        // Act
        Byte actualOutput = new SW<Byte>(gate, source);

        // Assert
        Assert.That(actualOutput, Is.EqualTo(expectedOutput));
    }
}