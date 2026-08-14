using System.Buffers.Binary;
using Turing.Core.ComplexComponents;

namespace Turing.Tests.ComplexComponents;

[TestFixture]
internal class CLZTests
{
    [Test]
    public void CLZ_ByteExhaustive_Returns_Correct_Output()
    {
        for (var i = 0; i < 256; i++)
        {
            Byte expected = (int)uint.LeadingZeroCount((uint)i);
            Byte actual = (int)(Int)new CLZ<Int>(i);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
