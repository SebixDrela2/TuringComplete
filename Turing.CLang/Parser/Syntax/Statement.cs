namespace Turing.CLang.Parser.Syntax;

public union Statement(
    ReturnStatement,
    ExpressionStatement,
    BlockStatement,
    IfStatement,
    WhileStatement,
    ForStatement,
    DeclarationStatement,
    BreakStatement,
    ContinueStatement,
    EmptyStatement
);

public record struct ReturnStatement(Expression? Expression);
public record struct ExpressionStatement(Expression Expression);
public record struct BlockStatement(IReadOnlyList<Statement> Statements);
public record struct IfStatement(Expression Condition, Statement ThenStatement, Statement? ElseStatement);
public record struct WhileStatement(Expression Condition, Statement Body);
public record struct ForStatement(Statement? Initialization, Expression? Condition, Expression? Increment, Statement Body);
public record struct DeclarationStatement(string Type, string Name, Expression? Initializer);
public record struct BreakStatement();
public record struct ContinueStatement();
public record struct EmptyStatement();