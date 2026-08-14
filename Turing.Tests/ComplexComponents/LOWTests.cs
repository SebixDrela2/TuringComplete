using Turing.Core.ComplexComponents;

namespace Turing.Tests.ComplexComponents;

[TestFixture]
internal class LOWTests
{
    [Test]
    public void LOW_ByteExhaustive_Returns_Correct_Output()
    {
        for (var i = -128; i < 128; i++)
        {
            for (var j = -128; j < 128; j++)
            {
                Bit expected = i < j;
                Bit actual = new LOW<Byte>(i, j);

                Assert.That(actual, Is.EqualTo(expected), $"Message: {i}, {j}");
            }
        }
    }
}
