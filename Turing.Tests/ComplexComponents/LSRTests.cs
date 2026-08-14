using Turing.Core.ComplexComponents;

namespace Turing.Tests.ComplexComponents;

[TestFixture]
internal class LSRTests
{
    [Test]
    public void LSR_ByteExhaustive_Returns_Correct_Output()
    {
        for (var i = 0; i < 256; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                Byte actual = new LSR<Byte>(i, j);
                Byte expected = i >> j;

                Assert.That(actual, Is.EqualTo(expected));
            }
        }
    }
}
