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
        var tokens = Tokenize("");
        Assert.That(tokens, Is.Empty);
    }

    [Test]
    public void Tokenize_WhitespaceOnly_ReturnsEmpty()
    {
        var tokens = Tokenize("   \t\n\r   ");
        Assert.That(tokens, Is.Empty);
    }

    [Test]
    public void Tokenize_Identifier_ReturnsIdentifier()
    {
        var tokens = Tokenize("main");
        Assert.That(tokens.Count, Is.EqualTo(1));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
    }

    [Test]
    public void Tokenize_Keywords_ReturnsCorrectTokenTypes()
    {
        var source = "return for while if else";
        var tokens = Tokenize(source);

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
        var source = ";:[](){},&|^~!<>=";
        var tokens = Tokenize(source);

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

        Assert.That(tokens.Count, Is.EqualTo(expectedTypes.Length));
        for (int i = 0; i < tokens.Count; i++)
        {
            Assert.That(tokens[i].Type, Is.EqualTo(expectedTypes[i]));
        }
    }

    [Test]
    public void Tokenize_MultiCharOperators_ReturnsCorrectTypes()
    {
        var source = "&& || << >> >>>";
        var tokens = Tokenize(source);

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
        var source = "main()";
        var tokens = Tokenize(source);

        Assert.That(tokens.Count, Is.EqualTo(3));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[1].Type, Is.EqualTo(TokenType.OpenParen));
        Assert.That(tokens[2].Type, Is.EqualTo(TokenType.CloseParen));
    }

    [Test]
    public void Tokenize_FunctionDeclaration_ReturnsCorrectTokens()
    {
        var source = "int main() { return 0; }";
        var tokens = Tokenize(source);

        Assert.That(tokens.Count, Is.EqualTo(9));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier)); // int
        Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Identifier)); // main
        Assert.That(tokens[2].Type, Is.EqualTo(TokenType.OpenParen));
        Assert.That(tokens[3].Type, Is.EqualTo(TokenType.CloseParen));
        Assert.That(tokens[4].Type, Is.EqualTo(TokenType.OpenCurlyBracket));
        Assert.That(tokens[5].Type, Is.EqualTo(TokenType.Return));
        Assert.That(tokens[6].Type, Is.EqualTo(TokenType.Identifier)); // 0
        Assert.That(tokens[7].Type, Is.EqualTo(TokenType.Semicolon));
        Assert.That(tokens[8].Type, Is.EqualTo(TokenType.CloseCurlyBracket));
    }

    [Test]
    public void Tokenize_IfStatement_ReturnsCorrectTokens()
    {
        var source = "if (x > 0) { y = 1; }";
        var tokens = Tokenize(source);

        Assert.That(tokens.Count, Is.EqualTo(13));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.If));
        Assert.That(tokens[1].Type, Is.EqualTo(TokenType.OpenParen));
        Assert.That(tokens[2].Type, Is.EqualTo(TokenType.Identifier)); // x
        Assert.That(tokens[3].Type, Is.EqualTo(TokenType.GreaterThan));
        Assert.That(tokens[4].Type, Is.EqualTo(TokenType.Identifier)); // 0
        Assert.That(tokens[5].Type, Is.EqualTo(TokenType.CloseParen));
        Assert.That(tokens[6].Type, Is.EqualTo(TokenType.OpenCurlyBracket));
        Assert.That(tokens[7].Type, Is.EqualTo(TokenType.Identifier)); // y
        Assert.That(tokens[8].Type, Is.EqualTo(TokenType.Assign));
        Assert.That(tokens[9].Type, Is.EqualTo(TokenType.Identifier)); // 1
        Assert.That(tokens[10].Type, Is.EqualTo(TokenType.Semicolon));
        Assert.That(tokens[11].Type, Is.EqualTo(TokenType.CloseCurlyBracket));
    }

    [Test]
    public void Tokenize_WhileLoop_ReturnsCorrectTokens()
    {
        var source = "while (i < 10) { i = i + 1; }";
        var tokens = Tokenize(source);

        Assert.That(tokens.Count, Is.EqualTo(15));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.While));
        Assert.That(tokens[1].Type, Is.EqualTo(TokenType.OpenParen));
        Assert.That(tokens[2].Type, Is.EqualTo(TokenType.Identifier)); // i
        Assert.That(tokens[3].Type, Is.EqualTo(TokenType.LessThan));
        Assert.That(tokens[4].Type, Is.EqualTo(TokenType.Identifier)); // 10
        Assert.That(tokens[5].Type, Is.EqualTo(TokenType.CloseParen));
        Assert.That(tokens[6].Type, Is.EqualTo(TokenType.OpenCurlyBracket));
        Assert.That(tokens[7].Type, Is.EqualTo(TokenType.Identifier)); // i
        Assert.That(tokens[8].Type, Is.EqualTo(TokenType.Assign));
        Assert.That(tokens[9].Type, Is.EqualTo(TokenType.Identifier)); // i
        Assert.That(tokens[10].Type, Is.EqualTo(TokenType.BitOr)); // +
        Assert.That(tokens[11].Type, Is.EqualTo(TokenType.Identifier)); // 1
        Assert.That(tokens[12].Type, Is.EqualTo(TokenType.Semicolon));
        Assert.That(tokens[13].Type, Is.EqualTo(TokenType.CloseCurlyBracket));
    }

    [Test]
    public void Tokenize_ForLoop_ReturnsCorrectTokens()
    {
        var source = "for (i = 0; i < 10; i = i + 1) { }";
        var tokens = Tokenize(source);

        Assert.That(tokens.Count, Is.EqualTo(23));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.For));
        Assert.That(tokens[1].Type, Is.EqualTo(TokenType.OpenParen));
        Assert.That(tokens[2].Type, Is.EqualTo(TokenType.Identifier)); // i
        Assert.That(tokens[3].Type, Is.EqualTo(TokenType.Assign));
        Assert.That(tokens[4].Type, Is.EqualTo(TokenType.Identifier)); // 0
        Assert.That(tokens[5].Type, Is.EqualTo(TokenType.Semicolon));
        Assert.That(tokens[6].Type, Is.EqualTo(TokenType.Identifier)); // i
        Assert.That(tokens[7].Type, Is.EqualTo(TokenType.LessThan));
        Assert.That(tokens[8].Type, Is.EqualTo(TokenType.Identifier)); // 10
        Assert.That(tokens[9].Type, Is.EqualTo(TokenType.Semicolon));
        Assert.That(tokens[10].Type, Is.EqualTo(TokenType.Identifier)); // i
        Assert.That(tokens[11].Type, Is.EqualTo(TokenType.Assign));
        Assert.That(tokens[12].Type, Is.EqualTo(TokenType.Identifier)); // i
        Assert.That(tokens[13].Type, Is.EqualTo(TokenType.BitOr)); // +
        Assert.That(tokens[14].Type, Is.EqualTo(TokenType.Identifier)); // 1
        Assert.That(tokens[15].Type, Is.EqualTo(TokenType.CloseParen));
        Assert.That(tokens[16].Type, Is.EqualTo(TokenType.OpenCurlyBracket));
        Assert.That(tokens[17].Type, Is.EqualTo(TokenType.CloseCurlyBracket));
    }

    [Test]
    public void Tokenize_ComplexExpression_ReturnsCorrectTokens()
    {
        var source = "result = (a & b) | (c ^ d)";
        var tokens = Tokenize(source);

        var expectedTypes = new[]
        {
            TokenType.Identifier, // result
            TokenType.Assign,
            TokenType.OpenParen,
            TokenType.Identifier, // a
            TokenType.BitAnd,
            TokenType.Identifier, // b
            TokenType.CloseParen,
            TokenType.BitOr,
            TokenType.OpenParen,
            TokenType.Identifier, // c
            TokenType.BitXor,
            TokenType.Identifier, // d
            TokenType.CloseParen,
        };

        Assert.That(tokens.Count, Is.EqualTo(expectedTypes.Length));
        for (int i = 0; i < tokens.Count; i++)
        {
            Assert.That(tokens[i].Type, Is.EqualTo(expectedTypes[i]));
        }
    }

    [Test]
    public void Tokenize_BitwiseOperations_ReturnsCorrectTokens()
    {
        var source = "x = ~a & b | c ^ d << e >> f >>> g";
        var tokens = Tokenize(source);

        var expectedTypes = new[]
        {
            TokenType.Identifier, // x
            TokenType.Assign,
            TokenType.BitNot, // ~
            TokenType.Identifier, // a
            TokenType.BitAnd,
            TokenType.Identifier, // b
            TokenType.BitOr,
            TokenType.Identifier, // c
            TokenType.BitXor,
            TokenType.Identifier, // d
            TokenType.BitLShift,
            TokenType.Identifier, // e
            TokenType.BitRShift,
            TokenType.Identifier, // f
            TokenType.BitASRShift,
            TokenType.Identifier, // g
        };

        Assert.That(tokens.Count, Is.EqualTo(expectedTypes.Length));

        for (int i = 0; i < tokens.Count; i++)
        {
            Assert.That(tokens[i].Type, Is.EqualTo(expectedTypes[i]));
        }
    }

    [Test]
    public void Tokenize_MultipleLines_ReturnsCorrectTokens()
    {
        var source = @"
int main()
{
    int x = 10;
    return x;
}";
        var tokens = Tokenize(source);

        Assert.That(tokens.Count, Is.EqualTo(13));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier)); // int
        Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Identifier)); // main
        Assert.That(tokens[2].Type, Is.EqualTo(TokenType.OpenParen));
        Assert.That(tokens[3].Type, Is.EqualTo(TokenType.CloseParen));
        Assert.That(tokens[4].Type, Is.EqualTo(TokenType.OpenCurlyBracket));
        Assert.That(tokens[5].Type, Is.EqualTo(TokenType.Identifier)); // int
        Assert.That(tokens[6].Type, Is.EqualTo(TokenType.Identifier)); // x
        Assert.That(tokens[7].Type, Is.EqualTo(TokenType.Assign));
        Assert.That(tokens[8].Type, Is.EqualTo(TokenType.Identifier)); // 10
        Assert.That(tokens[9].Type, Is.EqualTo(TokenType.Semicolon));
        Assert.That(tokens[10].Type, Is.EqualTo(TokenType.Return));
        Assert.That(tokens[11].Type, Is.EqualTo(TokenType.Identifier)); // x
        Assert.That(tokens[12].Type, Is.EqualTo(TokenType.Semicolon));
        Assert.That(tokens[13].Type, Is.EqualTo(TokenType.CloseCurlyBracket));
    }

    [Test]
    public void Tokenize_NestedBlocks_ReturnsCorrectTokens()
    {
        var source = "if (x) { if (y) { z = 1; } }";
        var tokens = Tokenize(source);

        var expectedTypes = new[]
        {
            TokenType.If,
            TokenType.OpenParen,
            TokenType.Identifier, // x
            TokenType.CloseParen,
            TokenType.OpenCurlyBracket,
            TokenType.If,
            TokenType.OpenParen,
            TokenType.Identifier, // y
            TokenType.CloseParen,
            TokenType.OpenCurlyBracket,
            TokenType.Identifier, // z
            TokenType.Assign,
            TokenType.Identifier, // 1
            TokenType.Semicolon,
            TokenType.CloseCurlyBracket,
            TokenType.CloseCurlyBracket,
        };

        Assert.That(tokens.Count, Is.EqualTo(expectedTypes.Length));
        for (int i = 0; i < tokens.Count; i++)
        {
            Assert.That(tokens[i].Type, Is.EqualTo(expectedTypes[i]));
        }
    }

    [Test]
    public void Tokenize_ArrayDeclaration_ReturnsCorrectTokens()
    {
        var source = "int arr[10] = {1, 2, 3};";
        var tokens = Tokenize(source);

        var expectedTypes = new[]
        {
            TokenType.Identifier, // int
            TokenType.Identifier, // arr
            TokenType.OpenBracket,
            TokenType.Identifier, // 10
            TokenType.CloseBracket,
            TokenType.Assign,
            TokenType.OpenCurlyBracket,
            TokenType.Identifier, // 1
            TokenType.Comma,
            TokenType.Identifier, // 2
            TokenType.Comma,
            TokenType.Identifier, // 3
            TokenType.CloseCurlyBracket,
            TokenType.Semicolon,
        };

        Assert.That(tokens.Count, Is.EqualTo(expectedTypes.Length));
        for (int i = 0; i < tokens.Count; i++)
        {
            Assert.That(tokens[i].Type, Is.EqualTo(expectedTypes[i]));
        }
    }

    [Test]
    public void Tokenize_LogicalOperations_ReturnsCorrectTokens()
    {
        var source = "if (a && b || !c)";
        var tokens = Tokenize(source);

        var expectedTypes = new[]
        {
            TokenType.If,
            TokenType.OpenParen,
            TokenType.Identifier, // a
            TokenType.LogicalAnd,
            TokenType.Identifier, // b
            TokenType.LogicalOr,
            TokenType.LogicalNot,
            TokenType.Identifier, // c
            TokenType.CloseParen,
        };

        Assert.That(tokens.Count, Is.EqualTo(expectedTypes.Length));
        for (int i = 0; i < tokens.Count; i++)
        {
            Assert.That(tokens[i].Type, Is.EqualTo(expectedTypes[i]));
        }
    }

    [Test]
    public void Tokenize_FunctionCall_ReturnsCorrectTokens()
    {
        var source = "printf(\"hello\");";
        var tokens = Tokenize(source);

        var expectedTypes = new[]
        {
            TokenType.Identifier, // printf
            TokenType.OpenParen,
            TokenType.Identifier, // "hello"
            TokenType.CloseParen,
            TokenType.Semicolon,
        };

        Assert.That(tokens.Count, Is.EqualTo(expectedTypes.Length));
        for (int i = 0; i < tokens.Count; i++)
        {
            Assert.That(tokens[i].Type, Is.EqualTo(expectedTypes[i]));
        }
    }

    [Test]
    public void Tokenize_StructDeclaration_ReturnsCorrectTokens()
    {
        var source = "struct Point { int x; int y; };";
        var tokens = Tokenize(source);

        var expectedTypes = new[]
        {
            TokenType.Identifier, // struct
            TokenType.Identifier, // Point
            TokenType.OpenCurlyBracket,
            TokenType.Identifier, // int
            TokenType.Identifier, // x
            TokenType.Semicolon,
            TokenType.Identifier, // int
            TokenType.Identifier, // y
            TokenType.Semicolon,
            TokenType.CloseCurlyBracket,
            TokenType.Semicolon,
        };

        Assert.That(tokens.Count, Is.EqualTo(expectedTypes.Length));
        for (int i = 0; i < tokens.Count; i++)
        {
            Assert.That(tokens[i].Type, Is.EqualTo(expectedTypes[i]));
        }
    }
}