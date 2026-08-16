
using Turing.Core.ComplexComponents;

namespace Turing.Tests.ComplexComponents;

internal class LSLTests
{
    [Test]
    public void LSL_ByteExhaustive_Returns_Correct_Output()
    {
        for (var i = 0; i < 256; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                Byte actual = new LSL<Byte>(i, j);
                Byte expected = i << j;

                Assert.That(actual, Is.EqualTo(expected), $"{i}, {j}");
            }
        }
    }
}
