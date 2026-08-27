using Turing.CLang.Lexer.Token;

namespace Turing.CLang.Tests.Lexer;

public class TokenizerTests
{
    private List<Token> Tokenize(string source)
    {
        var tokenizer = new Tokenizer(source.AsSpan());
        return [.. tokenizer.Tokenize()];
    }

    [Test]
    public void Tokenize_EmptyInput_ReturnsEmpty()
    {
        // Arrange
        var source = "";

        // Act
        var tokens = Tokenize(source);

        // Assert
        Assert.That(tokens, Is.Empty);
    }

    [Test]
    public void Tokenize_WhitespaceOnly_ReturnsEmpty()
    {
        // Arrange
        var source = "   \t\n\r   ";

        // Act
        var tokens = Tokenize(source);

        // Assert
        Assert.That(tokens, Is.Empty);
    }

    [Test]
    public void Tokenize_Identifier_ReturnsIdentifier()
    {
        // Arrange
        var source = "main";

        // Act
        var tokens = Tokenize(source);

        // Assert
        Assert.That(tokens.Count, Is.EqualTo(1));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
    }

    [Test]
    public void Tokenize_Keywords_ReturnsCorrectTokenTypes()
    {
        // Arrange
        var source = "return for while if else";

        // Act
        var tokens = Tokenize(source);

        // Assert
        Assert.That(tokens.Count, Is.EqualTo(5));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Return));
        Assert.That(tokens[1].Type, Is.EqualTo(TokenType.For));
        Assert.That(tokens[2].Type, Is.EqualTo(TokenType.While));
        Assert.That(tokens[3].Type, Is.EqualTo(TokenType.If));
        Assert.That(tokens[4].Type, Is.EqualTo(TokenType.Else));
    }

    [Test]
    public void Tokenize_SingleCharTokens_ReturnsCorrectTypes()
    {
        // Arrange
        var source = ";:[](){},&|^~!<>=";
        var expectedTypes = new[]
        {
            TokenType.Semicolon,
            TokenType.Colon,
            TokenType.OpenBracket,
            TokenType.CloseBracket,
            TokenType.OpenParen,
            TokenType.CloseParen,
            TokenType.OpenCurlyBracket,
            TokenType.CloseCurlyBracket,
            TokenType.Comma,
            TokenType.BitAnd,
            TokenType.BitOr,
            TokenType.BitXor,
            TokenType.BitNot,
            TokenType.LogicalNot,
            TokenType.LessThan,
            TokenType.GreaterThan,
            TokenType.Assign,
        };

        // Act
        var tokens = Tokenize(source);

        // Assert
        Assert.That(tokens.Count, Is.EqualTo(expectedTypes.Length));
        for (int i = 0; i < tokens.Count; i++)
        {
            Assert.That(tokens[i].Type, Is.EqualTo(expectedTypes[i]));
        }
    }

    [Test]
    public void Tokenize_MultiCharOperators_ReturnsCorrectTypes()
    {
        // Arrange
        var source = "&& || << >> >>>";

        // Act
        var tokens = Tokenize(source);

        // Assert
        Assert.That(tokens.Count, Is.EqualTo(5));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.LogicalAnd));
        Assert.That(tokens[1].Type, Is.EqualTo(TokenType.LogicalOr));
        Assert.That(tokens[2].Type, Is.EqualTo(TokenType.BitLShift));
        Assert.That(tokens[3].Type, Is.EqualTo(TokenType.BitRShift));
        Assert.That(tokens[4].Type, Is.EqualTo(TokenType.BitASRShift));
    }

    [Test]
    public void Tokenize_MainFunction_ReturnsCorrectTokens()
    {
        // Arrange
        var source = "main()";

        // Act
        var tokens = Tokenize(source);

        // Assert
        Assert.That(tokens.Count, Is.EqualTo(3));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[1].Type, Is.EqualTo(TokenType.OpenParen));
        Assert.That(tokens[2].Type, Is.EqualTo(TokenType.CloseParen));
    }

    [Test]
    public void Tokenize_FunctionDeclaration_ReturnsCorrectTokens()
    {
        // Arrange
        var source = "int main() { return 0; }";

        // Act
        var tokens = Tokenize(source);

        // Assert
        Assert.That(tokens.Count, Is.EqualTo(9));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[2].Type, Is.EqualTo(TokenType.OpenParen));
        Assert.That(tokens[3].Type, Is.EqualTo(TokenType.CloseParen));
        Assert.That(tokens[4].Type, Is.EqualTo(TokenType.OpenCurlyBracket));
        Assert.That(tokens[5].Type, Is.EqualTo(TokenType.Return));
        Assert.That(tokens[6].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[7].Type, Is.EqualTo(TokenType.Semicolon));
        Assert.That(tokens[8].Type, Is.EqualTo(TokenType.CloseCurlyBracket));
    }

    [Test]
    public void Tokenize_IfStatement_ReturnsCorrectTokens()
    {
        // Arrange
        var source = "if (x > 0) { y = 1; }";

        // Act
        var tokens = Tokenize(source);

        // Assert
        Assert.That(tokens.Count, Is.EqualTo(12));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.If));
        Assert.That(tokens[1].Type, Is.EqualTo(TokenType.OpenParen));
        Assert.That(tokens[2].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[3].Type, Is.EqualTo(TokenType.GreaterThan));
        Assert.That(tokens[4].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[5].Type, Is.EqualTo(TokenType.CloseParen));
        Assert.That(tokens[6].Type, Is.EqualTo(TokenType.OpenCurlyBracket));
        Assert.That(tokens[7].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[8].Type, Is.EqualTo(TokenType.Assign));
        Assert.That(tokens[9].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[10].Type, Is.EqualTo(TokenType.Semicolon));
        Assert.That(tokens[11].Type, Is.EqualTo(TokenType.CloseCurlyBracket));
    }

    [Test]
    public void Tokenize_WhileLoop_ReturnsCorrectTokens()
    {
        // Arrange
        var source = "while (i < 10) { i = i + 1; }";

        // Act
        var tokens = Tokenize(source);

        // Assert
        Assert.That(tokens.Count, Is.EqualTo(14));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.While));
        Assert.That(tokens[1].Type, Is.EqualTo(TokenType.OpenParen));
        Assert.That(tokens[2].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[3].Type, Is.EqualTo(TokenType.LessThan));
        Assert.That(tokens[4].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[5].Type, Is.EqualTo(TokenType.CloseParen));
        Assert.That(tokens[6].Type, Is.EqualTo(TokenType.OpenCurlyBracket));
        Assert.That(tokens[7].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[8].Type, Is.EqualTo(TokenType.Assign));
        Assert.That(tokens[9].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[10].Type, Is.EqualTo(TokenType.Plus));
        Assert.That(tokens[11].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[12].Type, Is.EqualTo(TokenType.Semicolon));
        Assert.That(tokens[13].Type, Is.EqualTo(TokenType.CloseCurlyBracket));
    }

    [Test]
    public void Tokenize_ForLoop_ReturnsCorrectTokens()
    {
        // Arrange
        var source = "for (i = 0; i < 10; i = i + 1) { }";

        // Act
        var tokens = Tokenize(source);

        // Assert
        Assert.That(tokens.Count, Is.EqualTo(18));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.For));
        Assert.That(tokens[1].Type, Is.EqualTo(TokenType.OpenParen));
        Assert.That(tokens[2].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[3].Type, Is.EqualTo(TokenType.Assign));
        Assert.That(tokens[4].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[5].Type, Is.EqualTo(TokenType.Semicolon));
        Assert.That(tokens[6].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[7].Type, Is.EqualTo(TokenType.LessThan));
        Assert.That(tokens[8].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[9].Type, Is.EqualTo(TokenType.Semicolon));
        Assert.That(tokens[10].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[11].Type, Is.EqualTo(TokenType.Assign));
        Assert.That(tokens[12].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[13].Type, Is.EqualTo(TokenType.Plus));
        Assert.That(tokens[14].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[15].Type, Is.EqualTo(TokenType.CloseParen));
        Assert.That(tokens[16].Type, Is.EqualTo(TokenType.OpenCurlyBracket));
        Assert.That(tokens[17].Type, Is.EqualTo(TokenType.CloseCurlyBracket));
    }

    [Test]
    public void Tokenize_ComplexExpression_ReturnsCorrectTokens()
    {
        // Arrange
        var source = "result = (a & b) | (c ^ d)";
        var expectedTypes = new[]
        {
            TokenType.Identifier,
            TokenType.Assign,
            TokenType.OpenParen,
            TokenType.Identifier,
            TokenType.BitAnd,
            TokenType.Identifier,
            TokenType.CloseParen,
            TokenType.BitOr,
            TokenType.OpenParen,
            TokenType.Identifier,
            TokenType.BitXor,
            TokenType.Identifier,
            TokenType.CloseParen,
        };

        // Act
        var tokens = Tokenize(source);

        // Assert
        Assert.That(tokens.Count, Is.EqualTo(expectedTypes.Length));
        for (int i = 0; i < tokens.Count; i++)
        {
            Assert.That(tokens[i].Type, Is.EqualTo(expectedTypes[i]));
        }
    }

    [Test]
    public void Tokenize_BitwiseOperations_ReturnsCorrectTokens()
    {
        // Arrange
        var source = "x = ~a & b | c ^ d << e >> f >>> g";
        var expectedTypes = new[]
        {
            TokenType.Identifier,
            TokenType.Assign,
            TokenType.BitNot,
            TokenType.Identifier,
            TokenType.BitAnd,
            TokenType.Identifier,
            TokenType.BitOr,
            TokenType.Identifier,
            TokenType.BitXor,
            TokenType.Identifier,
            TokenType.BitLShift,
            TokenType.Identifier,
            TokenType.BitRShift,
            TokenType.Identifier,
            TokenType.BitASRShift,
            TokenType.Identifier,
        };

        // Act
        var tokens = Tokenize(source);

        // Assert
        Assert.That(tokens.Count, Is.EqualTo(expectedTypes.Length));
        for (int i = 0; i < tokens.Count; i++)
        {
            Assert.That(tokens[i].Type, Is.EqualTo(expectedTypes[i]));
        }
    }

    [Test]
    public void Tokenize_MultipleLines_ReturnsCorrectTokens()
    {
        // Arrange
        var source = @"
int main()
{
    int x = 10;
    return x;
}";

        // Act
        var tokens = Tokenize(source);

        // Assert
        Assert.That(tokens.Count, Is.EqualTo(14));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[2].Type, Is.EqualTo(TokenType.OpenParen));
        Assert.That(tokens[3].Type, Is.EqualTo(TokenType.CloseParen));
        Assert.That(tokens[4].Type, Is.EqualTo(TokenType.OpenCurlyBracket));
        Assert.That(tokens[5].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[6].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[7].Type, Is.EqualTo(TokenType.Assign));
        Assert.That(tokens[8].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[9].Type, Is.EqualTo(TokenType.Semicolon));
        Assert.That(tokens[10].Type, Is.EqualTo(TokenType.Return));
        Assert.That(tokens[11].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[12].Type, Is.EqualTo(TokenType.Semicolon));
        Assert.That(tokens[13].Type, Is.EqualTo(TokenType.CloseCurlyBracket));
    }

    [Test]
    public void Tokenize_NestedBlocks_ReturnsCorrectTokens()
    {
        // Arrange
        var source = "if (x) { if (y) { z = 1; } }";
        var expectedTypes = new[]
        {
            TokenType.If,
            TokenType.OpenParen,
            TokenType.Identifier,
            TokenType.CloseParen,
            TokenType.OpenCurlyBracket,
            TokenType.If,
            TokenType.OpenParen,
            TokenType.Identifier,
            TokenType.CloseParen,
            TokenType.OpenCurlyBracket,
            TokenType.Identifier,
            TokenType.Assign,
            TokenType.Identifier,
            TokenType.Semicolon,
            TokenType.CloseCurlyBracket,
            TokenType.CloseCurlyBracket,
        };

        // Act
        var tokens = Tokenize(source);

        // Assert
        Assert.That(tokens.Count, Is.EqualTo(expectedTypes.Length));
        for (int i = 0; i < tokens.Count; i++)
        {
            Assert.That(tokens[i].Type, Is.EqualTo(expectedTypes[i]));
        }
    }

    [Test]
    public void Tokenize_ArrayDeclaration_ReturnsCorrectTokens()
    {
        // Arrange
        var source = "int arr[10] = {1, 2, 3};";
        var expectedTypes = new[]
        {
            TokenType.Identifier,
            TokenType.Identifier,
            TokenType.OpenBracket,
            TokenType.Identifier,
            TokenType.CloseBracket,
            TokenType.Assign,
            TokenType.OpenCurlyBracket,
            TokenType.Identifier,
            TokenType.Comma,
            TokenType.Identifier,
            TokenType.Comma,
            TokenType.Identifier,
            TokenType.CloseCurlyBracket,
            TokenType.Semicolon,
        };

        // Act
        var tokens = Tokenize(source);

        // Assert
        Assert.That(tokens.Count, Is.EqualTo(expectedTypes.Length));
        for (int i = 0; i < tokens.Count; i++)
        {
            Assert.That(tokens[i].Type, Is.EqualTo(expectedTypes[i]));
        }
    }

    [Test]
    public void Tokenize_LogicalOperations_ReturnsCorrectTokens()
    {
        // Arrange
        var source = "if (a && b || !c)";
        var expectedTypes = new[]
        {
            TokenType.If,
            TokenType.OpenParen,
            TokenType.Identifier,
            TokenType.LogicalAnd,
            TokenType.Identifier,
            TokenType.LogicalOr,
            TokenType.LogicalNot,
            TokenType.Identifier,
            TokenType.CloseParen,
        };

        // Act
        var tokens = Tokenize(source);

        // Assert
        Assert.That(tokens.Count, Is.EqualTo(expectedTypes.Length));
        for (int i = 0; i < tokens.Count; i++)
        {
            Assert.That(tokens[i].Type, Is.EqualTo(expectedTypes[i]));
        }
    }

    [Test]
    public void Tokenize_FunctionCall_ReturnsCorrectTokens()
    {
        // Arrange
        var source = "printf(\"hello\");";
        var expectedTypes = new[]
        {
            TokenType.Identifier,
            TokenType.OpenParen,
            TokenType.Identifier,
            TokenType.CloseParen,
            TokenType.Semicolon,
        };

        // Act
        var tokens = Tokenize(source);

        // Assert
        Assert.That(tokens.Count, Is.EqualTo(expectedTypes.Length));
        for (int i = 0; i < tokens.Count; i++)
        {
            Assert.That(tokens[i].Type, Is.EqualTo(expectedTypes[i]));
        }
    }

    [Test]
    public void Tokenize_StructDeclaration_ReturnsCorrectTokens()
    {
        // Arrange
        var source = "struct Point { int x; int y; };";
        var expectedTypes = new[]
        {
            TokenType.Identifier,
            TokenType.Identifier,
            TokenType.OpenCurlyBracket,
            TokenType.Identifier,
            TokenType.Identifier,
            TokenType.Semicolon,
            TokenType.Identifier,
            TokenType.Identifier,
            TokenType.Semicolon,
            TokenType.CloseCurlyBracket,
            TokenType.Semicolon,
        };

        // Act
        var tokens = Tokenize(source);

        // Assert
        Assert.That(tokens.Count, Is.EqualTo(expectedTypes.Length));
        for (int i = 0; i < tokens.Count; i++)
        {
            Assert.That(tokens[i].Type, Is.EqualTo(expectedTypes[i]));
        }
    }
}