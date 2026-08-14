using Turing.Core.ComplexComponents;

namespace Turing.Tests.ComplexComponents;

[TestFixture]
internal class EQTests
{
    [Test]
    public void EQ_ByteExhaustive_Returns_Correct_Output()
    {
        for(var i = 0; i < 256; i++)
        {
            for (var j = 0; j < 256; j++)
            {
                Bit expected = i == j;
                Bit actual = new EQ<Byte>(i, j);

                Assert.That(actual, Is.EqualTo(expected));
            }
        }
    }
}
