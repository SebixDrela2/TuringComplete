namespace Turing.CLang.Lexer.Syntax;

public record struct MethodDeclaration(IdentifierExpression Name, BlockStatement Body);
