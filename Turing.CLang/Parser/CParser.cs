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

    public List<string> Parse()
    {
        while (_position < _tokens.Count)
        {
            var statement = ParseStatement();
            if (statement.HasValue)
                GenerateStatement(statement.Value);
        }
        return _assembly;
    }

    private Statement? ParseStatement()
    {
        var token = Peek();
        if (!token.HasValue) return null;

        return token.Value.Type switch
        {
            TokenType.Return => ParseReturnStatement(),
            TokenType.If => ParseIfStatement(),
            TokenType.While => ParseWhileStatement(),
            TokenType.For => ParseForStatement(),
            TokenType.Break => ParseBreakStatement(),
            TokenType.Continue => ParseContinueStatement(),
            TokenType.Else => throw new InvalidOperationException("Unexpected 'else'"),
            TokenType.Identifier when Keywords.IsType(GetTokenValue(token.Value)) => ParseDeclarationStatement(),
            TokenType.Identifier when IsVariable(GetTokenValue(token.Value)) => ParseExpressionStatement(),
            TokenType.Identifier => ParseExpressionStatement(),
            TokenType.OpenCurlyBracket => ParseBlockStatement(),
            TokenType.Semicolon => ParseEmptyStatement(),
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

        if (Peek().HasValue && Peek().Value.Type == TokenType.Assign)
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
        if (Peek().HasValue && Peek().Value.Type != TokenType.Semicolon)
            expression = ParseExpression();
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

        if (Peek().HasValue && Peek().Value.Type == TokenType.Else)
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
        if (Peek().HasValue && Peek().Value.Type != TokenType.Semicolon)
            initialization = ParseStatement();
        Consume(TokenType.Semicolon);

        Expression? condition = null;
        if (Peek().HasValue && Peek().Value.Type != TokenType.Semicolon)
            condition = ParseExpression();
        Consume(TokenType.Semicolon);

        Expression? increment = null;
        if (Peek().HasValue && Peek().Value.Type != TokenType.CloseParen)
            increment = ParseExpression();
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
        while (_position < _tokens.Count && Peek().Value.Type != TokenType.CloseCurlyBracket)
        {
            var stmt = ParseStatement();
            if (stmt.HasValue) statements.Add(stmt.Value);
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
        var expr = ParseExpression();
        Consume(TokenType.Semicolon);
        return new ExpressionStatement(expr);
    }

    private Expression ParseExpression(int precedence = 0)
    {
        var left = ParsePrimary();

        while (_position < _tokens.Count)
        {
            var token = Peek();
            if (!token.HasValue) break;

            // Special handling for assignment (lowest precedence)
            if (token.Value.Type == TokenType.Assign && precedence <= 0)
            {
                Consume();
                var rhs = ParseExpression(0);
                return new AssignmentExpression(left, rhs);
            }

            var op = GetBinaryOperator(token.Value);
            if (!op.HasValue || GetPrecedence(op.Value) <= precedence)
                break;

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
            TokenType.Identifier when int.TryParse(value, out int num) => new LiteralExpression(num),
            TokenType.Identifier when IsVariable(value) => new IdentifierExpression(value),
            TokenType.Identifier => new IdentifierExpression(value),
            TokenType.OpenParen => ParseParenthesizedExpression(),
            TokenType.Plus or TokenType.Minus or TokenType.BitNot or TokenType.LogicalNot => ParseUnaryExpression(token),
            _ => throw new NotImplementedException($"Unexpected token: {token.Type}")
        };
    }

    private Expression ParseParenthesizedExpression()
    {
        var expr = ParseExpression();
        Consume(TokenType.CloseParen);
        return new ParenthesizedExpression(expr);
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
        var operand = ParseExpression(GetPrecedence(BinaryOperator.Plus));
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
            TokenType.Assign when Peek().HasValue && Peek().Value.Type == TokenType.Assign => BinaryOperator.Equal,
            TokenType.LogicalAnd => BinaryOperator.LogicalAnd,
            TokenType.LogicalOr => BinaryOperator.LogicalOr,
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

    // ------------------- Code Generation ----------------------

    private void GenerateStatement(Statement statement)
    {
        switch (statement)
        {
            case ReturnStatement rs: GenerateReturn(rs); break;
            case ExpressionStatement es: GenerateExpression(es.Expression); break;
            case BlockStatement bs: foreach (var s in bs.Statements) GenerateStatement(s); break;
            case IfStatement ifs: GenerateIf(ifs); break;
            case WhileStatement ws: GenerateWhile(ws); break;
            case ForStatement fs: GenerateFor(fs); break;
            case DeclarationStatement ds: GenerateDeclaration(ds); break;
            case BreakStatement: _assembly.Add($"jmp {GetTempLabel()}"); break;
            case ContinueStatement: _assembly.Add($"jmp {GetTempLabel()}"); break;
            case EmptyStatement: break;
        }
    }

    private void GenerateReturn(ReturnStatement rs)
    {
        if (rs.Expression.HasValue)
            GenerateExpression(rs.Expression.Value);
        _assembly.Add("ret");
    }

    private void GenerateIf(IfStatement ifStmt)
    {
        var elseLabel = GetTempLabel();
        var endLabel = GetTempLabel();

        GenerateExpression(ifStmt.Condition);
        _assembly.Add("cmp r1, #0");
        _assembly.Add($"je {elseLabel}");

        GenerateStatement(ifStmt.ThenStatement);

        // Only add jump to end if there is an else block or if the then block doesn't end with return/jump
        bool thenEndsWithReturn = ifStmt.ThenStatement is ReturnStatement ||
                                  (ifStmt.ThenStatement is BlockStatement bs && bs.Statements.LastOrDefault() is ReturnStatement);
        if (ifStmt.ElseStatement.HasValue || !thenEndsWithReturn)
        {
            _assembly.Add($"jmp {endLabel}");
        }

        _assembly.Add($"{elseLabel}:");
        if (ifStmt.ElseStatement.HasValue)
            GenerateStatement(ifStmt.ElseStatement.Value);

        // Add end label only if we used it
        if (ifStmt.ElseStatement.HasValue || !thenEndsWithReturn)
            _assembly.Add($"{endLabel}:");
    }

    private void GenerateWhile(WhileStatement whileStmt)
    {
        var startLabel = GetTempLabel();
        var endLabel = GetTempLabel();

        _assembly.Add($"{startLabel}:");
        GenerateExpression(whileStmt.Condition);
        _assembly.Add("cmp r1, #0");
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
            GenerateStatement(forStmt.Initialization.Value);

        _assembly.Add($"jmp {startLabel}");
        _assembly.Add($"{incrementLabel}:");
        if (forStmt.Increment.HasValue)
            GenerateExpression(forStmt.Increment.Value);

        _assembly.Add($"{startLabel}:");
        if (forStmt.Condition.HasValue)
        {
            GenerateExpression(forStmt.Condition.Value);
            _assembly.Add("cmp r1, #0");
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
            case LiteralExpression lit:
                _assembly.Add($"mov r1, #{lit.Value}");
                break;
            case IdentifierExpression ident:
                if (_variableOffsets.TryGetValue(ident.Name, out var offset))
                    _assembly.Add($"load_32 r1, [sp - {offset}]");
                else
                    _assembly.Add($"mov r1, {ident.Name}");
                break;
            case BinaryExpression bin:
                GenerateBinary(bin);
                break;
            case AssignmentExpression assign:
                GenerateAssignment(assign);
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
        _assembly.Add("push r1");
        GenerateExpression(binary.Rhs);
        _assembly.Add("pop r2");

        var op = binary.Op;
        var asmOp = op.ToAssembly();

        if (op is BinaryOperator.Equal or BinaryOperator.NotEqual or
            BinaryOperator.LessThan or BinaryOperator.LessThanOrEqual or
            BinaryOperator.GreaterThan or BinaryOperator.GreaterThanOrEqual)
        {
            _assembly.Add($"cmp r2, r1");
            _assembly.Add("mov r1, #0");
            _assembly.Add("mov r1, #1");
            return;
        }

        _assembly.Add($"{asmOp} r1, r2, r1");
    }

    private void GenerateAssignment(AssignmentExpression assignment)
    {
        GenerateExpression(assignment.Value);
        if (assignment.Target is IdentifierExpression ident &&
            _variableOffsets.TryGetValue(ident.Name, out var offset))
        {
            _assembly.Add($"store_32 [sp - {offset}], r1");
        }
    }

    private void GenerateUnary(UnaryExpression unary)
    {
        GenerateExpression(unary.Operand);
        switch (unary.Op)
        {
            case UnaryOperator.Minus:
                _assembly.Add("neg r1, r1");
                break;
            case UnaryOperator.BitwiseNot:
                _assembly.Add("not r1, r1");
                break;
            case UnaryOperator.LogicalNot:
                _assembly.Add("cmp r1, #0");
                _assembly.Add("mov r1, #1");
                _assembly.Add("mov r1, #0");
                break;
        }
    }

    private string GetTempLabel() => $"L{_labelCounter++}";

    private Token? Peek() => _position < _tokens.Count ? _tokens[_position] : null;
    private Token Consume() => _tokens[_position++];
    private void Consume(TokenType type)
    {
        if (Peek().HasValue && Peek().Value.Type == type)
            _position++;
    }
    private string GetTokenValue(Token token) => _source.Substring(token.Range.Start, token.Range.End - token.Range.Start);
    private bool IsType(string value) => Keywords.IsType(value);
    private bool IsVariable(string value) => _variables.ContainsKey(value);
}