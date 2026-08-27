namespace Turing.CLang.Lexer.Syntax;

public union Expression(LiteralExpression, BinaryExpression, AsignmentExpression, IdentifierExpression);

public record struct IdentifierExpression(string name);
public record struct LiteralExpression(int Value);
public record struct BinaryExpression(Expression Lhs, Expression Rhs, BinaryOperator Op);
public record struct AsignmentExpression(Expression Target, Expression Value);
