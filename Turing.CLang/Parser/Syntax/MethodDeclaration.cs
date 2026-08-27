namespace Turing.CLang.Parser.Syntax;

public record struct MethodDeclaration(
    IdentifierExpression Name,
    IReadOnlyList<Parameter> Parameters,
    BlockStatement Body,
    string? ReturnType = null
);

public record struct Parameter(string Type, string Name);