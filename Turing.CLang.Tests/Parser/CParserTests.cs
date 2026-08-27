using Turing.CLang.Lexer.Token;
using Turing.CLang.Parser;
using Turing.CLang.Parser.Syntax;
using Turing.Core.Components.Arithmetic;

namespace Turing.CLang.Tests.Parser;

internal class CParserTests
{
    private List<Token> Tokenize(string source)
    {
        var tokenizer = new Tokenizer(source.AsSpan());
        return [.. tokenizer.Tokenize()];
    }

    private List<string> Parse(string source)
    {
        var tokens = Tokenize(source);
        var parser = new CParser(source, tokens);
        return parser.Parse();
    }

    [Test]
    public void Parse_EmptyInput_ReturnsEmpty()
    {
        // Arrange
        var source = "";

        // Act
        var result = Parse(source);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void Parse_WhitespaceOnly_ReturnsEmpty()
    {
        // Arrange
        var source = "   \t\n\r   ";

        // Act
        var result = Parse(source);
        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void Parse_VariableDeclaration_ReturnsCorrectAssembly()
    {
        // Arrange
        var source = "int x;";

        // Act
        var result = Parse(source);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void Parse_VariableDeclarationWithInitializer_ReturnsCorrectAssembly()
    {
        // Arrange
        var source = "int x = 10;";

        // Act
        var result = Parse(source);

        // Assert
        var expected = new[]
        {
            "mov r1, 10",
            "store_32 [sp - 0], r1"
        };
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_MultipleVariableDeclarations_ReturnsCorrectAssembly()
    {
        // Arrange
        var source = "int x = 5; int y = 10;";

        // Act
        var result = Parse(source);

        // Assert
        var expected = new[]
        {
            "mov r1, 5",
            "store_32 [sp - 0], r1",
            "mov r1, 10",
            "store_32 [sp - 4], r1"
        };
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_ReturnStatement_ReturnsCorrectAssembly()
    {
        // Arrange
        var source = "return;";

        // Act
        var result = Parse(source);

        // Assert
        var expected = new[] { "ret" };
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_ReturnStatementWithValue_ReturnsCorrectAssembly()
    {
        // Arrange
        var source = "return 42;";

        // Act
        var result = Parse(source);

        // Assert
        var expected = new[]
        {
            "mov r1, 42",
            "ret"
        };
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_ReturnStatementWithVariable_ReturnsCorrectAssembly()
    {
        // Arrange
        var source = "int x = 5; return x;";

        // Act
        var result = Parse(source);

        // Assert
        var expected = new[]
        {
            "mov r1, 5",
            "store_32 [sp - 0], r1",
            "load_32 r1, [sp - 0]",
            "ret"
        };
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_IfStatement_ReturnsCorrectAssembly()
    {
        // Arrange
        var source = "if (x) { return 1; }";

        // Act
        var result = Parse(source);

        // Assert
        var expected = new[]
        {
            "mov r1, x",
            "cmp r1, 0",
            "je L0",
            "mov r1, 1",
            "ret",
            "L0:"
        };
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_IfElseStatement_ReturnsCorrectAssembly()
    {
        // Arrange
        var source = "if (x) { return 1; } else { return 2; }";

        // Act
        var result = Parse(source);

        // Assert
        var expected = new[]
        {
            "mov r1, x",
            "cmp r1, 0",
            "je L0",
            "mov r1, 1",
            "ret",
            "jmp L1",
            "L0:",
            "mov r1, 2",
            "ret",
            "L1:"
        };
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_WhileLoop_ReturnsCorrectAssembly()
    {
        // Arrange
        var source = "int x = 0; while (x < 10) { x = x + 1; }";

        // Act
        var result = Parse(source);

        // Assert
        var expected = new[]
        {
            "mov r1, 0",
            "store_32 [sp - 0], r1",
            "L0:",
            "load_32 r1, [sp - 0]",
            "push r1",
            "mov r1, 10",
            "pop r2",
            "cmp r2, r1",
            "mov r1, 0",
            "mov r1, 1",
            "cmp r1, 0",
            "je L1",
            "load_32 r1, [sp - 0]",
            "push r1",
            "mov r1, 1",
            "pop r2",
            "add r1, r2, r1",
            "store_32 [sp - 0], r1",
            "jmp L0",
            "L1:",          
        };
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_ForLoop_ReturnsCorrectAssembly()
    {
        // Arrange
        var source = "int i = 10; for (i = 0; i < 10; i = i + 1) { }";

        // Act
        var result = Parse(source);

        // Assert
        var expected = new[] 
        {
            "mov r1, 10",
            "store_32 [sp - 0], r1",
            "mov r1, 0",
            "store_32 [sp - 0], r1",
            "jmp L0",
            "L2:",
            "load_32 r1, [sp - 0]",
            "push r1",
            "mov r1, 1",
            "pop r2",
            "add r1, r2, r1",
            "store_32[sp - 0], r1",
            "L0:",
            "load_32 r1, [sp - 0]",
            "push r1",
            "mov r1, 10",
            "pop r2",
            "cmp r2, r1",
            "mov r1, 0",
            "mov r1, 1",
            "cmp r1, 0",
            "je L1",
            "jmp L2",
            "L1:"
        } ;

        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    public void Parse_BreakStatement_ReturnsCorrectAssembly()
    {
        // Arrange
        var source = "while (x) { break; }";

        // Act
        var result = Parse(source);
        
        // Assert
        Assert.That(result, Has.Some.Matches<string>(s => s.StartsWith("jmp L")));
    }

    [Test]
    public void Parse_ContinueStatement_ReturnsCorrectAssembly()
    {
        // Arrange
        var source = "while (x) { continue; }";

        // Act
        var result = Parse(source);

        // Assert
        Assert.That(result, Has.Some.Matches<string>(s => s.StartsWith("jmp L")));
    }

    [Test]
    public void Parse_BlockStatement_ReturnsCorrectAssembly()
    {
        // Arrange
        var source = "{ int x = 5; return x; }";

        // Act
        var result = Parse(source);

        // Assert
        var expected = new[]
        {
            "mov r1, 5",
            "store_32 [sp - 0], r1",
            "load_32 r1, [sp - 0]",
            "ret"
        };
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_NestedBlocks_ReturnsCorrectAssembly()
    {
        // Arrange
        var source = "{ int x = 5; { int y = 10; } return x; }";

        // Act
        var result = Parse(source);

        // Assert
        var expected = new[]
        {
            "mov r1, 5",
            "store_32 [sp - 0], r1",
            "mov r1, 10",
            "store_32 [sp - 4], r1",
            "load_32 r1, [sp - 0]",
            "ret"
        };
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_AssignmentExpression_ReturnsCorrectAssembly()
    {
        // Arrange
        var source = "int x; x = 42;";

        // Act
        var result = Parse(source);

        // Assert
        var expected = new[]
        {
            "mov r1, 42",
            "store_32 [sp - 0], r1"
        };
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_BinaryExpression_ReturnsCorrectAssembly()
    {
        // Arrange
        var source = "int x = 5 + 3;";

        // Act
        var result = Parse(source);

        // Assert
        var expected = new[]
        {
            "mov r1, 5",
            "push r1",
            "mov r1, 3",
            "pop r2",
            "add r1, r2, r1",
            "store_32 [sp - 0], r1"
        };
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_ComplexExpression_ReturnsCorrectAssembly()
    {
        // Arrange
        var source = "int x = 5 + 3 * 2;";

        // Act
        var result = Parse(source);

        // Assert
        var mulIndex = result.FindIndex(s => s.Contains("mul"));
        var addIndex = result.FindIndex(s => s.Contains("add"));
        Assert.That(mulIndex, Is.LessThan(addIndex));
    }

    [Test]
    public void Parse_ParenthesizedExpression_ReturnsCorrectAssembly()
    {
        // Arrange
        var source = "int x = (5 + 3) * 2;";

        // Act
        var result = Parse(source);

        // Assert
        var addIndex = result.FindIndex(s => s.Contains("add"));
        var mulIndex = result.FindIndex(s => s.Contains("mul"));
        Assert.That(addIndex, Is.LessThan(mulIndex));
    }

    [Test]
    public void Parse_ComparisonExpression_ReturnsCorrectAssembly()
    {
        // Arrange
        var source = "int x = 5 > 3;";

        // Act
        var result = Parse(source);

        // Assert 
        Assert.That(result, Has.Some.Matches<string>(s => s.Contains("cmp")));
        Assert.That(result, Has.Some.Matches<string>(s => s.Contains("mov r1, 1")));
        Assert.That(result, Has.Some.Matches<string>(s => s.Contains("mov r1, 0")));

        var cmpIndex = result.FindIndex(s => s.Contains("cmp"));
        var zeroIndex = result.FindIndex(s => s.Contains("mov r1, 0"));
        var oneIndex = result.FindIndex(s => s.Contains("mov r1, 1"));

        Assert.That(cmpIndex, Is.LessThan(zeroIndex));
        Assert.That(zeroIndex, Is.LessThan(oneIndex));
    }

    [Test]
    public void Parse_UnaryExpression_ReturnsCorrectAssembly()
    {
        // Arrange
        var source = "int x = -(1 + 1);";

        // Act
        var result = Parse(source);

        // Assert
        var expected = new[]
        {
            "mov r1, 1",
            "push r1",
            "mov r1, 1",
            "pop r2",
            "add r1, r2, r1",
            "neg r1, r1",
            "store_32 [sp - 0], r1",
        };
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_LogicalNot_ReturnsCorrectAssembly()
    {
        // Arrange
        var source = "int x = !5;";

        // Act
        var result = Parse(source);

        // Assert
        Assert.That(result, Has.Some.Matches<string>(s => s.Contains("cmp r1, 0")));
        Assert.That(result, Has.Some.Matches<string>(s => s.Contains("mov r1, 1")));
        Assert.That(result, Has.Some.Matches<string>(s => s.Contains("mov r1, 0")));

        var cmpIndex = result.FindIndex(s => s.Contains("cmp r1, 0"));
        var oneIndex = result.FindIndex(s => s.Contains("mov r1, 1"));
        var zeroIndex = result.FindIndex(s => s.Contains("mov r1, 0"));
        Assert.That(cmpIndex, Is.LessThan(oneIndex));
        Assert.That(oneIndex, Is.LessThan(zeroIndex));
    }

    [Test]
    public void Parse_MultipleStatements_ReturnsCorrectAssembly()
    {
        // Arrange
        var source = "int x = 5; int y = 10; int z = x + y;";

        // Act
        var result = Parse(source);

        // Assert
        var expected = new[]
        {
            "mov r1, 5",
            "store_32 [sp - 0], r1",
            "mov r1, 10",
            "store_32 [sp - 4], r1",
            "load_32 r1, [sp - 0]",
            "push r1",
            "load_32 r1, [sp - 4]",
            "pop r2",
            "add r1, r2, r1",
            "store_32 [sp - 8], r1"
        };
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_EmptyStatement_ReturnsEmptyAssembly()
    {
        // Arrange
        var source = ";";

        // Act
        var result = Parse(source);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void Parse_VariableReuse_ReturnsCorrectAssembly()
    {
        // Arrange
        var source = "int x = 5; x = x + 1;";

        // Act
        var result = Parse(source);

        // Assert
        var expected = new[]
        {
            "mov r1, 5",
            "store_32 [sp - 0], r1",
            "load_32 r1, [sp - 0]",
            "push r1",
            "mov r1, 1",
            "pop r2",
            "add r1, r2, r1",
            "store_32 [sp - 0], r1"
        };
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_MultipleTypes_ReturnsCorrectAssembly()
    {
        // Arrange
        var source = "int x = 5; char y = 'a'; long z = 100;";

        // Act
        var result = Parse(source);

        // Assert
        Assert.That(result.Count(x => x.StartsWith("mov r1")), Is.EqualTo(3));
        Assert.That(result.Count(x => x.StartsWith("store_32")), Is.EqualTo(3));
        Assert.That(result[0], Is.EqualTo("mov r1, 5"));
        Assert.That(result[1], Is.EqualTo("store_32 [sp - 0], r1"));
        Assert.That(result[2], Is.EqualTo("mov r1, 'a'"));
        Assert.That(result[3], Is.EqualTo("store_32 [sp - 4], r1"));
        Assert.That(result[4], Is.EqualTo("mov r1, 100"));
        Assert.That(result[5], Is.EqualTo("store_32 [sp - 8], r1"));
    }

    [Test, Ignore("Function declarations not yet implemented")]
    public void Parse_FunctionDefinition_ReturnsCorrectAssembly()
    {
        // Arrange
        var source = "int main() { return 0; }";

        // Act
        var result = Parse(source);

        // Assert
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Has.Some.Matches<string>(s => s.Contains("ret")));
    }
}