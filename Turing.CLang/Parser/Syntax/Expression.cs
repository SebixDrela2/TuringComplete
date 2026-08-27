namespace Turing.CLang.Parser.Syntax;

public union Expression(
    LiteralExpression,
    BinaryExpression,
    AssignmentExpression,
    IdentifierExpression,
    UnaryExpression,
    ParenthesizedExpression,
    CallExpression,
    ArrayAccessExpression
);

public record struct LiteralExpression(int Value);
public record struct BinaryExpression(Expression Lhs, Expression Rhs, BinaryOperator Op);
public record struct AssignmentExpression(Expression Target, Expression Value);
public record struct IdentifierExpression(string Name);
public record struct UnaryExpression(UnaryOperator Op, Expression Operand);
public record struct ParenthesizedExpression(Expression Expression);
public record struct CallExpression(Expression Target, IReadOnlyList<Expression> Arguments);
public record struct ArrayAccessExpression(Expression Array, Expression Index);