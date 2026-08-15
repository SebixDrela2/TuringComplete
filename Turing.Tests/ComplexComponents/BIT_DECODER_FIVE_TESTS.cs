using Turing.Core.Components.Logic;

namespace Turing.Tests.Components;

[TestFixture]
internal class BIT_DECODER_FIVETests
{
    // Test all 32 combinations without disable (disable = 0)
    [TestCase(0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)] // Input 0 → Y0=1
    [TestCase(0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)] // Input 1 → Y1=1 (E=1)
    [TestCase(0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)] // Input 2 → Y2=1 (D=1)
    [TestCase(0, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)] // Input 3 → Y3=1 (D=1,E=1)
    [TestCase(0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)] // Input 4 → Y4=1 (C=1)
    [TestCase(0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)] // Input 5 → Y5=1
    [TestCase(0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)] // Input 6 → Y6=1
    [TestCase(0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)] // Input 7 → Y7=1
    [TestCase(0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)] // Input 8 → Y8=1 (B=1)
    [TestCase(0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)] // Input 9 → Y9=1
    [TestCase(0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)] // Input 10 → Y10=1
    [TestCase(0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)] // Input 11 → Y11=1
    [TestCase(0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)] // Input 12 → Y12=1
    [TestCase(0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)] // Input 13 → Y13=1
    [TestCase(0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)] // Input 14 → Y14=1
    [TestCase(0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)] // Input 15 → Y15=1
    [TestCase(1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)] // Input 16 → Y16=1 (A=1)
    [TestCase(1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)] // Input 17 → Y17=1
    [TestCase(1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)] // Input 18 → Y18=1
    [TestCase(1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)] // Input 19 → Y19=1
    [TestCase(1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)] // Input 20 → Y20=1
    [TestCase(1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)] // Input 21 → Y21=1
    [TestCase(1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0)] // Input 22 → Y22=1
    [TestCase(1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0)] // Input 23 → Y23=1
    [TestCase(1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0)] // Input 24 → Y24=1 (A=1,B=1)
    [TestCase(1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0)] // Input 25 → Y25=1
    [TestCase(1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0)] // Input 26 → Y26=1
    [TestCase(1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0)] // Input 27 → Y27=1
    [TestCase(1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0)] // Input 28 → Y28=1
    [TestCase(1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0)] // Input 29 → Y29=1
    [TestCase(1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0)] // Input 30 → Y30=1
    [TestCase(1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)] // Input 31 → Y31=1
    public void Decoder5_ImplicitConversion_ReturnsCorrectOutput(
        int inputA, int inputB, int inputC, int inputD, int inputE,
        int expectedY0, int expectedY1, int expectedY2, int expectedY3,
        int expectedY4, int expectedY5, int expectedY6, int expectedY7,
        int expectedY8, int expectedY9, int expectedY10, int expectedY11,
        int expectedY12, int expectedY13, int expectedY14, int expectedY15,
        int expectedY16, int expectedY17, int expectedY18, int expectedY19,
        int expectedY20, int expectedY21, int expectedY22, int expectedY23,
        int expectedY24, int expectedY25, int expectedY26, int expectedY27,
        int expectedY28, int expectedY29, int expectedY30, int expectedY31)
    {
        // Arrange
        var a = new Bit(inputA);
        var b = new Bit(inputB);
        var c = new Bit(inputC);
        var d = new Bit(inputD);
        var e = new Bit(inputE);

        // Act
        Int result = new BIT_DECODER_FIVE(a, b, c, d, e, Bit.Zero);

        int inputValue = inputA + (inputB << 1) + (inputC << 2) + (inputD << 3) + (inputE << 4);

        // Assert - check each bit with detailed error messages
        Assert.That(result.GetBit(0).Value, Is.EqualTo(new Bit(expectedY0)),
            $"Input={inputValue} (A={inputA},B={inputB},C={inputC},D={inputD},E={inputE}): Y0 expected {expectedY0} but got {result.GetBit(0).Value}");
        Assert.That(result.GetBit(1).Value, Is.EqualTo(new Bit(expectedY1)),
            $"Input={inputValue}: Y1 expected {expectedY1} but got {result.GetBit(1).Value}");
        Assert.That(result.GetBit(2).Value, Is.EqualTo(new Bit(expectedY2)),
            $"Input={inputValue}: Y2 expected {expectedY2} but got {result.GetBit(2).Value}");
        Assert.That(result.GetBit(3).Value, Is.EqualTo(new Bit(expectedY3)),
            $"Input={inputValue}: Y3 expected {expectedY3} but got {result.GetBit(3).Value}");
        Assert.That(result.GetBit(4).Value, Is.EqualTo(new Bit(expectedY4)),
            $"Input={inputValue}: Y4 expected {expectedY4} but got {result.GetBit(4).Value}");
        Assert.That(result.GetBit(5).Value, Is.EqualTo(new Bit(expectedY5)),
            $"Input={inputValue}: Y5 expected {expectedY5} but got {result.GetBit(5).Value}");
        Assert.That(result.GetBit(6).Value, Is.EqualTo(new Bit(expectedY6)),
            $"Input={inputValue}: Y6 expected {expectedY6} but got {result.GetBit(6).Value}");
        Assert.That(result.GetBit(7).Value, Is.EqualTo(new Bit(expectedY7)),
            $"Input={inputValue}: Y7 expected {expectedY7} but got {result.GetBit(7).Value}");
        Assert.That(result.GetBit(8).Value, Is.EqualTo(new Bit(expectedY8)),
            $"Input={inputValue}: Y8 expected {expectedY8} but got {result.GetBit(8).Value}");
        Assert.That(result.GetBit(9).Value, Is.EqualTo(new Bit(expectedY9)),
            $"Input={inputValue}: Y9 expected {expectedY9} but got {result.GetBit(9).Value}");
        Assert.That(result.GetBit(10).Value, Is.EqualTo(new Bit(expectedY10)),
            $"Input={inputValue}: Y10 expected {expectedY10} but got {result.GetBit(10).Value}");
        Assert.That(result.GetBit(11).Value, Is.EqualTo(new Bit(expectedY11)),
            $"Input={inputValue}: Y11 expected {expectedY11} but got {result.GetBit(11).Value}");
        Assert.That(result.GetBit(12).Value, Is.EqualTo(new Bit(expectedY12)),
            $"Input={inputValue}: Y12 expected {expectedY12} but got {result.GetBit(12).Value}");
        Assert.That(result.GetBit(13).Value, Is.EqualTo(new Bit(expectedY13)),
            $"Input={inputValue}: Y13 expected {expectedY13} but got {result.GetBit(13).Value}");
        Assert.That(result.GetBit(14).Value, Is.EqualTo(new Bit(expectedY14)),
            $"Input={inputValue}: Y14 expected {expectedY14} but got {result.GetBit(14).Value}");
        Assert.That(result.GetBit(15).Value, Is.EqualTo(new Bit(expectedY15)),
            $"Input={inputValue}: Y15 expected {expectedY15} but got {result.GetBit(15).Value}");
        Assert.That(result.GetBit(16).Value, Is.EqualTo(new Bit(expectedY16)),
            $"Input={inputValue}: Y16 expected {expectedY16} but got {result.GetBit(16).Value}");
        Assert.That(result.GetBit(17).Value, Is.EqualTo(new Bit(expectedY17)),
            $"Input={inputValue}: Y17 expected {expectedY17} but got {result.GetBit(17).Value}");
        Assert.That(result.GetBit(18).Value, Is.EqualTo(new Bit(expectedY18)),
            $"Input={inputValue}: Y18 expected {expectedY18} but got {result.GetBit(18).Value}");
        Assert.That(result.GetBit(19).Value, Is.EqualTo(new Bit(expectedY19)),
            $"Input={inputValue}: Y19 expected {expectedY19} but got {result.GetBit(19).Value}");
        Assert.That(result.GetBit(20).Value, Is.EqualTo(new Bit(expectedY20)),
            $"Input={inputValue}: Y20 expected {expectedY20} but got {result.GetBit(20).Value}");
        Assert.That(result.GetBit(21).Value, Is.EqualTo(new Bit(expectedY21)),
            $"Input={inputValue}: Y21 expected {expectedY21} but got {result.GetBit(21).Value}");
        Assert.That(result.GetBit(22).Value, Is.EqualTo(new Bit(expectedY22)),
            $"Input={inputValue}: Y22 expected {expectedY22} but got {result.GetBit(22).Value}");
        Assert.That(result.GetBit(23).Value, Is.EqualTo(new Bit(expectedY23)),
            $"Input={inputValue}: Y23 expected {expectedY23} but got {result.GetBit(23).Value}");
        Assert.That(result.GetBit(24).Value, Is.EqualTo(new Bit(expectedY24)),
            $"Input={inputValue}: Y24 expected {expectedY24} but got {result.GetBit(24).Value}");
        Assert.That(result.GetBit(25).Value, Is.EqualTo(new Bit(expectedY25)),
            $"Input={inputValue}: Y25 expected {expectedY25} but got {result.GetBit(25).Value}");
        Assert.That(result.GetBit(26).Value, Is.EqualTo(new Bit(expectedY26)),
            $"Input={inputValue}: Y26 expected {expectedY26} but got {result.GetBit(26).Value}");
        Assert.That(result.GetBit(27).Value, Is.EqualTo(new Bit(expectedY27)),
            $"Input={inputValue}: Y27 expected {expectedY27} but got {result.GetBit(27).Value}");
        Assert.That(result.GetBit(28).Value, Is.EqualTo(new Bit(expectedY28)),
            $"Input={inputValue}: Y28 expected {expectedY28} but got {result.GetBit(28).Value}");
        Assert.That(result.GetBit(29).Value, Is.EqualTo(new Bit(expectedY29)),
            $"Input={inputValue}: Y29 expected {expectedY29} but got {result.GetBit(29).Value}");
        Assert.That(result.GetBit(30).Value, Is.EqualTo(new Bit(expectedY30)),
            $"Input={inputValue}: Y30 expected {expectedY30} but got {result.GetBit(30).Value}");
        Assert.That(result.GetBit(31).Value, Is.EqualTo(new Bit(expectedY31)),
            $"Input={inputValue}: Y31 expected {expectedY31} but got {result.GetBit(31).Value}");
    }

    // Test disable functionality (disable = 1 should output all zeros)
    [TestCase(0, 0, 0, 0, 0, 1)]
    [TestCase(1, 1, 1, 1, 1, 1)]
    [TestCase(0, 1, 0, 1, 0, 1)]
    [TestCase(1, 0, 1, 0, 1, 1)]
    public void Decoder5_WithDisable_ReturnsAllZeros(
        int inputA, int inputB, int inputC, int inputD, int inputE, int disable)
    {
        // Arrange
        var a = new Bit(inputA);
        var b = new Bit(inputB);
        var c = new Bit(inputC);
        var d = new Bit(inputD);
        var e = new Bit(inputE);
        var dis = new Bit(disable);

        // Act
        Int result = new BIT_DECODER_FIVE(a, b, c, d, e, dis);

        int inputValue = inputA + (inputB << 1) + (inputC << 2) + (inputD << 3) + (inputE << 4);

        // Assert - all 32 bits should be 0 with detailed error messages
        for (int i = 0; i < 32; i++)
        {
            Assert.That(result.GetBit(i).Value, Is.EqualTo(new Bit(0)),
                $"Input={inputValue}, Disable={disable}: Bit Y{i} expected 0 but got {result.GetBit(i).Value}");
        }
    }
}