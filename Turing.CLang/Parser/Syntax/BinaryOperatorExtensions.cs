namespace Turing.CLang.Parser.Syntax;

public static class BinaryOperatorExtensions
{
    public static string ToAssembly(this BinaryOperator op) => op switch
    {
        BinaryOperator.Plus => "add",
        BinaryOperator.Minus => "sub",
        BinaryOperator.Multiply => "mul",
        BinaryOperator.Divide => "div",
        BinaryOperator.Modulo => "mod",
        BinaryOperator.BitwiseAnd => "and",
        BinaryOperator.BitwiseOr => "or",
        BinaryOperator.BitwiseXor => "xor",
        BinaryOperator.BitwiseLeftShift => "lsl",
        BinaryOperator.BitwiseRightShift => "lsr",
        BinaryOperator.Equal => "cmp",
        BinaryOperator.NotEqual => "cmp",
        BinaryOperator.LessThan => "cmp",
        BinaryOperator.LessThanOrEqual => "cmp",
        BinaryOperator.GreaterThan => "cmp",
        BinaryOperator.GreaterThanOrEqual => "cmp",
        BinaryOperator.LogicalAnd => "and",
        BinaryOperator.LogicalOr => "or",
        BinaryOperator.Assign => "mov",
        _ => throw new NotImplementedException()
    };

    public static string GetConditionCode(this BinaryOperator op) => op switch
    {
        BinaryOperator.Equal => "je",
        BinaryOperator.NotEqual => "jne",
        BinaryOperator.LessThan => "jl",
        BinaryOperator.LessThanOrEqual => "jle",
        BinaryOperator.GreaterThan => "jg",
        BinaryOperator.GreaterThanOrEqual => "jge",
        _ => throw new NotImplementedException()
    };
}