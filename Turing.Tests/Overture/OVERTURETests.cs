using Turing.Core.Overture;
using Turing.Core.Electricity;

namespace Turing.Tests.Overture;

[TestFixture]
internal class OVERTURETests
{
    private OVERTURE _overture;

    [SetUp]
    public void Setup()
    {
        _overture = new OVERTURE();
    }

    // Helper to build instruction byte: source (bits 0‑2), dest (bits 3‑5)
    private Byte MakeInstruction(int src, int dst)
    {
        int value = src | (dst << 3);
        return new Byte(value);
    }

    [Test]
    public void Move_RegisterToRegister_Works()
    {
        // Write 0xAA into REG0
        _overture.EVal(MakeInstruction(6, 0), new Byte(0xAA), new Bit(true));
        // Move REG0 -> REG1
        _overture.EVal(MakeInstruction(0, 1), new Byte(0x00), new Bit(true));
        // Read REG1 by moving to output
        _overture.EVal(MakeInstruction(1, 6), new Byte(0x00), new Bit(true));
        Assert.That((Byte)_overture, Is.EqualTo(new Byte(0xAA)));
    }

    [Test]
    public void Move_InputToRegister_Works()
    {
        // Input -> REG2
        _overture.EVal(MakeInstruction(6, 2), new Byte(0x55), new Bit(true));
        // REG2 -> output
        _overture.EVal(MakeInstruction(2, 6), new Byte(0x00), new Bit(true));
        Assert.That((Byte)_overture, Is.EqualTo(new Byte(0x55)));
    }

    [Test]
    public void Move_RegisterToOutput_Works()
    {
        // Write 0xCC into REG3
        _overture.EVal(MakeInstruction(6, 3), new Byte(0xCC), new Bit(true));
        // REG3 -> output
        _overture.EVal(MakeInstruction(3, 6), new Byte(0x00), new Bit(true));
        Assert.That((Byte)_overture, Is.EqualTo(new Byte(0xCC)));
    }

    [Test]
    public void Move_InputToOutput_Works()
    {
        // Input -> output directly
        _overture.EVal(MakeInstruction(6, 6), new Byte(0x77), new Bit(true));
        Assert.That((Byte)_overture, Is.EqualTo(new Byte(0x77)));
    }

    [Test]
    public void WriteOnlyOnTick_Works()
    {
        // Try to write with tick=0: should not change
        _overture.EVal(MakeInstruction(6, 0), new Byte(0xAA), new Bit(false));
        // Read REG0 to output
        _overture.EVal(MakeInstruction(0, 6), new Byte(0x00), new Bit(true));
        Assert.That((Byte)_overture, Is.EqualTo(new Byte(0x00))); // Still zero

        // Now write with tick=1
        _overture.EVal(MakeInstruction(6, 0), new Byte(0xAA), new Bit(true));
        _overture.EVal(MakeInstruction(0, 6), new Byte(0x00), new Bit(true));
        Assert.That((Byte)_overture, Is.EqualTo(new Byte(0xAA)));
    }

    [Test]
    public void Source7_Unused_ReturnsZero()
    {
        // Write something to REG4
        _overture.EVal(MakeInstruction(6, 4), new Byte(0xDD), new Bit(true));
        // Source=7 (unused) -> should produce 0, move to output
        _overture.EVal(MakeInstruction(7, 6), new Byte(0x00), new Bit(true));
        Assert.That((Byte)_overture, Is.EqualTo(new Byte(0x00)));
    }

    [Test]
    public void Destination7_Unused_DoesNothing()
    {
        // Write to REG5
        _overture.EVal(MakeInstruction(6, 5), new Byte(0xEE), new Bit(true));
        // Destination=7 (unused) should not write
        _overture.EVal(MakeInstruction(5, 7), new Byte(0x00), new Bit(true));
        // Read REG5 to output
        _overture.EVal(MakeInstruction(5, 6), new Byte(0x00), new Bit(true));
        Assert.That((Byte)_overture, Is.EqualTo(new Byte(0xEE))); // Still unchanged
    }

    [Test]
    public void MultipleMoves_Work()
    {
        // Input -> REG0
        _overture.EVal(MakeInstruction(6, 0), new Byte(0x01), new Bit(true));
        // REG0 -> REG1
        _overture.EVal(MakeInstruction(0, 1), new Byte(0x00), new Bit(true));
        // REG1 -> REG2
        _overture.EVal(MakeInstruction(1, 2), new Byte(0x00), new Bit(true));
        // REG2 -> output
        _overture.EVal(MakeInstruction(2, 6), new Byte(0x00), new Bit(true));
        Assert.That((Byte)_overture, Is.EqualTo(new Byte(0x01)));
    }
}