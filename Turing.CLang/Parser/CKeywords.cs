namespace Turing.CLang.Parser;

public static class Keywords
{
    public static readonly HashSet<string> Types = new()
    {
        "int",
        "char",
        "void",
        "long",
        "short",
        "float",
        "double",
        "struct",
        "enum",
        "union",
        "typedef",
        "unsigned",
        "signed",
        "const",
        "volatile"
    };

    public const string Return = "return";
    public const string If = "if";
    public const string Else = "else";
    public const string While = "while";
    public const string For = "for";
    public const string Break = "break";
    public const string Continue = "continue";
    public const string Switch = "switch";
    public const string Case = "case";
    public const string Default = "default";
    public const string Do = "do";
    public const string Goto = "goto";

    public static readonly HashSet<string> All = new()
    {
        Return, If, Else, While, For, Break, Continue,
        "int", "char", "void", "long", "short", "float", "double",
        "struct", "enum", "union", "typedef", "unsigned", "signed",
        "const", "volatile", Switch, Case, Default, Do, Goto
    };

    public static bool IsType(string value) => Types.Contains(value);
    public static bool IsKeyword(string value) => All.Contains(value);
}