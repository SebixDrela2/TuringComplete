using Turing.Core.ComplexComponents;

namespace Turing.Tests.ComplexComponents;

[TestFixture]
internal class ASRTests
{
    [Test]
    public void ASR_ByteExhaustive_Returns_Correct_Output()
    {
        for (var i = -128; i < 128; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                Byte iByte = i;
                Byte jByte = j;
                Byte actual = new ASR<Byte>(i, j);
                Byte expected = i >> jByte;

                Assert.That(actual, Is.EqualTo(expected), $"{i}, {j}");
            }
        }
    }
}
