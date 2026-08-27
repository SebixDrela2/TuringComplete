namespace Turing.CLang.Lexer.Syntax;

public union Statement(ReturnStatement, ExpressionStatement, BlockStatement);

public record struct ReturnStatement(Expression? expression);
public record struct ExpressionStatement(Expression Expression);

public record struct BlockStatement(IReadOnlyList<Statement> Statements);
