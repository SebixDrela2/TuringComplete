using Turing.Core.ComplexComponents;

namespace Turing.Tests.ComplexComponents;

[TestFixture]
internal class SINDEXERTests
{
    [Test]
    public void SINDEXER_ByteExhaustive_Returns_Correct_Output()
    {
        for (var i = 0; i < 256; i++)
        {
            for (var j = -8; j < 8; j++)
            {
                Byte actual = new SINDEXER<Byte>(i, j);
                Byte expected = j < 0 ? i << -j : i >> j;

                Assert.That(actual, Is.EqualTo(expected), $"{i}, {j}");
            }
        }
    }
}
