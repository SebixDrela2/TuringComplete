using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Turing.CLang.Lexer.Token;

public ref struct Tokenizer(ReadOnlySpan<char> content)
{
    private static readonly SearchValues<char> _wsSearchValues = SearchValues.Create("\n\r\t ");
    private static readonly SearchValues<char> _delimiterSearchValues = SearchValues.Create(";:[]{}(),&|^~!<>=");

    private readonly ReadOnlySpan<char> _content = content;
    private ReadOnlySpan<char> _rest = content;

    public IReadOnlyList<Token> Tokenize()
    {
        var tokens = new List<Token>();

        while (_rest is not [])
        {
            var nonWhitespaceIndex = _rest.IndexOfAnyExcept(_wsSearchValues);
            if (nonWhitespaceIndex < 0)
            {
                break;
            }

            _rest = _rest[nonWhitespaceIndex..];
            tokens.Add(GetToken());
        }

        return tokens;
    }

    private Token GetToken()
    {
        ReadOnlySpan<char> chunk;
        Range range;

        // Check for multi-character operators FIRST
        if (_rest.Length >= 2)
        {
            var twoChar = _rest[..2];

            // Check for triple char operator (>>>)
            if (_rest.Length >= 3 && _rest[..3] is ['>', '>', '>'])
            {
                chunk = _rest[..3];
                _rest = _rest[3..];
                range = GetRange(_content, chunk);
                return GetMultiCharToken(chunk, range);
            }

            // Check for double char operators
            if (twoChar is ['&', '&'] or ['|', '|'] or ['<', '<'] or ['>', '>'])
            {
                chunk = _rest[..2];
                _rest = _rest[2..];
                range = GetRange(_content, chunk);
                return GetMultiCharToken(chunk, range);
            }
        }

        // Now handle single character
        var delimiterIndex = _rest.IndexOfAny(_delimiterSearchValues);

        if (delimiterIndex == 0)
        {
            chunk = _rest[..1];
            _rest = _rest[1..];
            range = GetRange(_content, chunk);
            return GetSingleCharToken(chunk[0], range);
        }

        var nextDelimiterIndex = _rest.IndexOfAny(_delimiterSearchValues);
        var nextWhitespaceIndex = _rest.IndexOfAny(_wsSearchValues);

        int endIndex = _rest.Length;

        if (nextDelimiterIndex >= 0)
        {
            endIndex = nextDelimiterIndex;
        }

        if (nextWhitespaceIndex >= 0 && nextWhitespaceIndex < endIndex)
        {
            endIndex = nextWhitespaceIndex;
        }

        chunk = _rest[..endIndex];
        _rest = _rest[endIndex..];
        range = GetRange(_content, chunk);

        if (chunk.Length == 1)
        {
            return GetSingleCharToken(chunk[0], range);
        }

        return GetMultiCharToken(chunk, range);
    }

    private static Token GetMultiCharToken(ReadOnlySpan<char> chunk, Range range)
    {
        TokenType type = chunk switch
        {
            ['&', '&'] => TokenType.LogicalAnd,
            ['|', '|'] => TokenType.LogicalOr,
            ['<', '<'] => TokenType.BitLShift,
            ['>', '>', '>'] => TokenType.BitASRShift,
            ['>', '>'] => TokenType.BitRShift,
            ['r', 'e', 't', 'u', 'r', 'n'] => TokenType.Return,
            ['f', 'o', 'r'] => TokenType.For,
            ['w', 'h', 'i', 'l', 'e'] => TokenType.While,
            ['i', 'f'] => TokenType.If,
            ['e', 'l', 's', 'e'] => TokenType.Else,
            _ => TokenType.Identifier
        };

        return new Token(type, range);
    }

    private static Token GetSingleCharToken(char c, Range range)
    {
        TokenType type = c switch
        {
            ';' => TokenType.Semicolon,
            ':' => TokenType.Colon,
            '[' => TokenType.OpenBracket,
            ']' => TokenType.CloseBracket,
            '{' => TokenType.OpenCurlyBracket,
            '}' => TokenType.CloseCurlyBracket,
            ',' => TokenType.Comma,
            '(' => TokenType.OpenParen,
            ')' => TokenType.CloseParen,
            '&' => TokenType.BitAnd,
            '|' => TokenType.BitOr,
            '^' => TokenType.BitXor,
            '~' => TokenType.BitNot,
            '!' => TokenType.LogicalNot,
            '<' => TokenType.LessThan,
            '>' => TokenType.GreaterThan,
            '=' => TokenType.Assign,
            '+' => TokenType.Plus,
            '-' => TokenType.Minus,
            '/' => TokenType.Slash,
            '*' => TokenType.Star,
            '%' => TokenType.Percent,
            _ => TokenType.Identifier
        };

        return new Token(type, range);
    }

    private static Range GetRange(ReadOnlySpan<char> source, ReadOnlySpan<char> slice)
    {
        if (source.IsEmpty || slice.IsEmpty)
        {
            return default;
        }

        ref var sourceRef = ref MemoryMarshal.GetReference(source);
        ref var sliceRef = ref MemoryMarshal.GetReference(slice);

        var byteOffset = Unsafe.ByteOffset(ref sourceRef, ref sliceRef);
        var elementOffset = (int)(byteOffset / Unsafe.SizeOf<char>());

        if (elementOffset < 0 || elementOffset >= source.Length)
        {
            return default;
        }

        return new Range(elementOffset, elementOffset + slice.Length);
    }
}