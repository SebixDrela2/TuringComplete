using Turing.CLang.Parser.Syntax;
using Turing.CLang.Lexer.Token;

namespace Turing.CLang.Parser;

public ref struct CParser(string source, IReadOnlyList<Token> tokens)
{
    private int _position = 0;
    private readonly string _source = source;
    private readonly IReadOnlyList<Token> _tokens = tokens;
    private readonly List<string> _assembly = [];
    private readonly Dictionary<string, int> _labels = [];
    private readonly Dictionary<string, string> _variables = [];
    private readonly Dictionary<string, int> _variableOffsets = [];
    private int _stackOffset = 0;
    private int _labelCounter = 0;
    private int _tempCounter = 0;

    public List<string> Parse()
    {
        while (_position < _tokens.Count)
        {
            var statement = ParseStatement();

            if (statement.HasValue)
            {
                GenerateStatement(statement.Value);
            }
        }

        return _assembly;
    }

    private Statement? ParseStatement()
    {
        var token = Peek();

        if (!token.HasValue)
        {
            return null;
        }

        var value = GetTokenValue(token.Value);

        return (token.Value.Type, value) switch
        {
            (TokenType.Identifier, Keywords.Return) => ParseReturnStatement(),
            (TokenType.Identifier, Keywords.If) => ParseIfStatement(),
            (TokenType.Identifier, Keywords.While) => ParseWhileStatement(),
            (TokenType.Identifier, Keywords.For) => ParseForStatement(),
            (TokenType.Identifier, Keywords.Break) => ParseBreakStatement(),
            (TokenType.Identifier, Keywords.Continue) => ParseContinueStatement(),
            (TokenType.Identifier, var v) when Keywords.IsType(v) => ParseDeclarationStatement(),
            (TokenType.Identifier, var v) when IsVariable(v) => ParseExpressionStatement(),
            (TokenType.OpenCurlyBracket, _) => ParseBlockStatement(),
            (TokenType.Semicolon, _) => ParseEmptyStatement(),
            _ => ParseExpressionStatement()
        };
    }

    private Statement ParseDeclarationStatement()
    {
        var type = GetTokenValue(Consume());
        var name = GetTokenValue(Consume());

        _variables[name] = type;
        _variableOffsets[name] = _stackOffset;
        _stackOffset += 4;

        if (Peek().Value.Type == TokenType.Assign)
        {
            Consume();
            var expression = ParseExpression();
            Consume(TokenType.Semicolon);
            return new DeclarationStatement(type, name, expression);
        }

        Consume(TokenType.Semicolon);
        return new DeclarationStatement(type, name, null);
    }

    private Statement ParseReturnStatement()
    {
        Consume();
        Expression? expression = null;

        if (Peek().Value.Type != TokenType.Semicolon)
        {
            expression = ParseExpression();
        }

        Consume(TokenType.Semicolon);
        return new ReturnStatement(expression);
    }

    private Statement ParseIfStatement()
    {
        Consume();
        Consume(TokenType.OpenParen);
        var condition = ParseExpression();
        Consume(TokenType.CloseParen);

        var thenStatement = ParseStatement();
        Statement? elseStatement = null;

        if (Peek().Value.Type == TokenType.Identifier && GetTokenValue(Peek().Value) == Keywords.Else)
        {
            Consume();
            elseStatement = ParseStatement();
        }

        return new IfStatement(condition, thenStatement.Value, elseStatement);
    }

    private Statement ParseWhileStatement()
    {
        Consume();
        Consume(TokenType.OpenParen);
        var condition = ParseExpression();
        Consume(TokenType.CloseParen);

        var body = ParseStatement();
        return new WhileStatement(condition, body.Value);
    }

    private Statement ParseForStatement()
    {
        Consume();
        Consume(TokenType.OpenParen);

        Statement? initialization = null;
        if (Peek().Value.Type != TokenType.Semicolon)
        {
            initialization = ParseStatement();
        }
        Consume(TokenType.Semicolon);

        Expression? condition = null;
        if (Peek().Value.Type != TokenType.Semicolon)
        {
            condition = ParseExpression();
        }
        Consume(TokenType.Semicolon);

        Expression? increment = null;
        if (Peek().Value.Type != TokenType.CloseParen)
        {
            increment = ParseExpression();
        }
        Consume(TokenType.CloseParen);

        var body = ParseStatement();
        return new ForStatement(initialization, condition, increment, body.Value);
    }

    private Statement ParseBreakStatement()
    {
        Consume();
        Consume(TokenType.Semicolon);
        return new BreakStatement();
    }

    private Statement ParseContinueStatement()
    {
        Consume();
        Consume(TokenType.Semicolon);
        return new ContinueStatement();
    }

    private Statement ParseBlockStatement()
    {
        Consume(TokenType.OpenCurlyBracket);
        var statements = new List<Statement>();

        while (Peek().Value.Type != TokenType.CloseCurlyBracket && _position < _tokens.Count)
        {
            var statement = ParseStatement();
            if (statement.HasValue)
            {
                statements.Add(statement.Value);
            }
        }

        Consume(TokenType.CloseCurlyBracket);
        return new BlockStatement(statements);
    }

    private Statement ParseEmptyStatement()
    {
        Consume(TokenType.Semicolon);
        return new EmptyStatement();
    }

    private Statement ParseExpressionStatement()
    {
        var expression = ParseExpression();
        Consume(TokenType.Semicolon);
        return new ExpressionStatement(expression);
    }

    private Expression ParseExpression(int precedence = 0)
    {
        var left = ParsePrimary();

        while (_position < _tokens.Count)
        {
            var token = Peek();
            var op = GetBinaryOperator(token.Value);

            if (!op.HasValue || GetPrecedence(op.Value) <= precedence)
            {
                break;
            }

            Consume();
            var right = ParseExpression(GetPrecedence(op.Value));
            left = new BinaryExpression(left, right, op.Value);
        }

        return left;
    }

    private Expression ParsePrimary()
    {
        var token = Consume();
        var value = GetTokenValue(token);

        return token.Type switch
        {
            TokenType.Identifier when int.TryParse(value, out int num) =>
                new LiteralExpression(num),

            TokenType.Identifier when IsVariable(value) =>
                new IdentifierExpression(value),

            TokenType.Identifier =>
                new IdentifierExpression(value),

            TokenType.OpenParen =>
                ParseParenthesizedExpression(),

            TokenType.Plus or TokenType.Minus or TokenType.BitNot or TokenType.LogicalNot =>
                ParseUnaryExpression(token),

            _ => throw new NotImplementedException($"Unexpected token: {token.Type}")
        };
    }

    private Expression ParseParenthesizedExpression()
    {
        var expression = ParseExpression();
        Consume(TokenType.CloseParen);
        return new ParenthesizedExpression(expression);
    }

    private Expression ParseUnaryExpression(Token token)
    {
        var op = token.Type switch
        {
            TokenType.Plus => UnaryOperator.Plus,
            TokenType.Minus => UnaryOperator.Minus,
            TokenType.BitNot => UnaryOperator.BitwiseNot,
            TokenType.LogicalNot => UnaryOperator.LogicalNot,
            _ => throw new NotImplementedException()
        };

        var operand = ParseExpression(GetPrecedence(GetBinaryOperator(token) ?? BinaryOperator.Plus));
        return new UnaryExpression(op, operand);
    }

    private BinaryOperator? GetBinaryOperator(Token token)
    {
        return token.Type switch
        {
            TokenType.Plus => BinaryOperator.Plus,
            TokenType.Minus => BinaryOperator.Minus,
            TokenType.Star => BinaryOperator.Multiply,
            TokenType.Slash => BinaryOperator.Divide,
            TokenType.Percent => BinaryOperator.Modulo,
            TokenType.BitAnd => BinaryOperator.BitwiseAnd,
            TokenType.BitOr => BinaryOperator.BitwiseOr,
            TokenType.BitXor => BinaryOperator.BitwiseXor,
            TokenType.BitLShift => BinaryOperator.BitwiseLeftShift,
            TokenType.BitRShift => BinaryOperator.BitwiseRightShift,
            TokenType.BitASRShift => BinaryOperator.BitwiseRightShift,
            TokenType.LessThan => BinaryOperator.LessThan,
            TokenType.GreaterThan => BinaryOperator.GreaterThan,
            TokenType.Assign when Peek().Value.Type == TokenType.Assign => BinaryOperator.Equal,
            TokenType.LogicalAnd => BinaryOperator.LogicalAnd,
            TokenType.LogicalOr => BinaryOperator.LogicalOr,
            TokenType.Assign => BinaryOperator.Assign,
            _ => null
        };
    }

    private int GetPrecedence(BinaryOperator op) => op switch
    {
        BinaryOperator.Assign => 0,
        BinaryOperator.LogicalOr => 1,
        BinaryOperator.LogicalAnd => 2,
        BinaryOperator.BitwiseOr => 3,
        BinaryOperator.BitwiseXor => 4,
        BinaryOperator.BitwiseAnd => 5,
        BinaryOperator.Equal or BinaryOperator.NotEqual => 6,
        BinaryOperator.LessThan or BinaryOperator.LessThanOrEqual
            or BinaryOperator.GreaterThan or BinaryOperator.GreaterThanOrEqual => 7,
        BinaryOperator.BitwiseLeftShift or BinaryOperator.BitwiseRightShift => 8,
        BinaryOperator.Plus or BinaryOperator.Minus => 9,
        BinaryOperator.Multiply or BinaryOperator.Divide or BinaryOperator.Modulo => 10,
        _ => 0
    };

    private void GenerateStatement(Statement statement)
    {
        switch (statement)
        {
            case ReturnStatement returnStmt:
                GenerateReturn(returnStmt);
                break;
            case ExpressionStatement exprStmt:
                GenerateExpression(exprStmt.Expression);
                break;
            case BlockStatement blockStmt:
                foreach (var stmt in blockStmt.Statements)
                {
                    GenerateStatement(stmt);
                }
                break;
            case IfStatement ifStmt:
                GenerateIf(ifStmt);
                break;
            case WhileStatement whileStmt:
                GenerateWhile(whileStmt);
                break;
            case ForStatement forStmt:
                GenerateFor(forStmt);
                break;
            case DeclarationStatement declStmt:
                GenerateDeclaration(declStmt);
                break;
            case BreakStatement:
                _assembly.Add($"jmp {GetTempLabel()}");
                break;
            case ContinueStatement:
                _assembly.Add($"jmp {GetTempLabel()}");
                break;
            case EmptyStatement:
                break;
        }
    }

    private void GenerateReturn(ReturnStatement returnStmt)
    {
        if (returnStmt.Expression.HasValue)
        {
            GenerateExpression(returnStmt.Expression.Value);
            _assembly.Add($"mov r1, r1");
        }
        _assembly.Add($"ret");
    }

    private void GenerateIf(IfStatement ifStmt)
    {
        var elseLabel = GetTempLabel();
        var endLabel = GetTempLabel();

        GenerateExpression(ifStmt.Condition);
        _assembly.Add($"cmp r1, #0");
        _assembly.Add($"je {elseLabel}");

        GenerateStatement(ifStmt.ThenStatement);
        _assembly.Add($"jmp {endLabel}");
        _assembly.Add($"{elseLabel}:");

        if (ifStmt.ElseStatement.HasValue)
        {
            GenerateStatement(ifStmt.ElseStatement.Value);
        }

        _assembly.Add($"{endLabel}:");
    }

    private void GenerateWhile(WhileStatement whileStmt)
    {
        var startLabel = GetTempLabel();
        var endLabel = GetTempLabel();

        _assembly.Add($"{startLabel}:");
        GenerateExpression(whileStmt.Condition);
        _assembly.Add($"cmp r1, #0");
        _assembly.Add($"je {endLabel}");

        GenerateStatement(whileStmt.Body);
        _assembly.Add($"jmp {startLabel}");
        _assembly.Add($"{endLabel}:");
    }

    private void GenerateFor(ForStatement forStmt)
    {
        var startLabel = GetTempLabel();
        var endLabel = GetTempLabel();
        var incrementLabel = GetTempLabel();

        if (forStmt.Initialization.HasValue)
        {
            GenerateStatement(forStmt.Initialization.Value);
        }

        _assembly.Add($"jmp {startLabel}");
        _assembly.Add($"{incrementLabel}:");

        if (forStmt.Increment.HasValue)
        {
            GenerateExpression(forStmt.Increment.Value);
        }

        _assembly.Add($"{startLabel}:");
        if (forStmt.Condition.HasValue)
        {
            GenerateExpression(forStmt.Condition.Value);
            _assembly.Add($"cmp r1, #0");
            _assembly.Add($"je {endLabel}");
        }

        GenerateStatement(forStmt.Body);
        _assembly.Add($"jmp {incrementLabel}");
        _assembly.Add($"{endLabel}:");
    }

    private void GenerateDeclaration(DeclarationStatement declStmt)
    {
        if (declStmt.Initializer.HasValue)
        {
            GenerateExpression(declStmt.Initializer.Value);
            var offset = _variableOffsets[declStmt.Name];
            _assembly.Add($"store_32 [sp - {offset}], r1");
        }
    }

    private void GenerateExpression(Expression expression)
    {
        switch (expression)
        {
            case LiteralExpression literal:
                _assembly.Add($"mov r1, #{literal.Value}");
                break;

            case IdentifierExpression ident:
                if (_variableOffsets.TryGetValue(ident.Name, out var offset))
                {
                    _assembly.Add($"load_32 r1, [sp - {offset}]");
                }
                else
                {
                    _assembly.Add($"mov r1, {ident.Name}");
                }
                break;

            case BinaryExpression binary:
                GenerateBinary(binary);
                break;

            case AssignmentExpression assignment:
                GenerateAssignment(assignment);
                break;

            case UnaryExpression unary:
                GenerateUnary(unary);
                break;

            case ParenthesizedExpression paren:
                GenerateExpression(paren.Expression);
                break;
        }
    }

    private void GenerateBinary(BinaryExpression binary)
    {
        GenerateExpression(binary.Lhs);
        _assembly.Add($"push r1");

        GenerateExpression(binary.Rhs);
        _assembly.Add($"pop r2");

        var op = binary.Op;
        if (op == BinaryOperator.Assign)
        {
            _assembly.Add($"mov r1, r2");
            return;
        }

        var asmOp = op.ToAssembly();

        if (op is BinaryOperator.Equal or BinaryOperator.NotEqual or
            BinaryOperator.LessThan or BinaryOperator.LessThanOrEqual or
            BinaryOperator.GreaterThan or BinaryOperator.GreaterThanOrEqual)
        {
            _assembly.Add($"cmp r2, r1");
            _assembly.Add($"mov r1, #1");
            _assembly.Add($"mov r1, #0");
            return;
        }

        _assembly.Add($"{asmOp} r1, r2, r1");
    }

    private void GenerateAssignment(AssignmentExpression assignment)
    {
        GenerateExpression(assignment.Value);

        if (assignment.Target is IdentifierExpression ident)
        {
            if (_variableOffsets.TryGetValue(ident.Name, out var offset))
            {
                _assembly.Add($"store_32 [sp - {offset}], r1");
            }
        }
    }

    private void GenerateUnary(UnaryExpression unary)
    {
        GenerateExpression(unary.Operand);

        switch (unary.Op)
        {
            case UnaryOperator.Minus:
                _assembly.Add($"neg r1, r1");
                break;
            case UnaryOperator.BitwiseNot:
                _assembly.Add($"not r1, r1");
                break;
            case UnaryOperator.LogicalNot:
                _assembly.Add($"cmp r1, #0");
                _assembly.Add($"mov r1, #1");
                _assembly.Add($"mov r1, #0");
                break;
        }
    }

    private string GetTempLabel() => $"L{_labelCounter++}";
    private string GetTempRegister() => $"r{_tempCounter++ % 13 + 1}";

    private Token? Peek() => _position < _tokens.Count ? _tokens[_position] : null;
    private Token Consume() => _tokens[_position++];
    private void Consume(TokenType type)
    {
        if (Peek().HasValue && Peek()?.Value.Type == type)
            _position++;
    }
    private string GetTokenValue(Token token) => _source.Substring(token.Range.Start, token.Range.End - token.Range.Start);
    private bool IsType(string value) => Keywords.IsType(value);
    private bool IsVariable(string value) => _variables.ContainsKey(value);
}