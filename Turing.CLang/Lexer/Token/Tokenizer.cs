using System.Buffers;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Turing.CLang.Lexer.Token;

internal ref struct Tokenizer(ReadOnlySpan<char> content)
{
    private static readonly SearchValues<char> _wsSearchValues = SearchValues.Create("\n\r\t ");

    private ReadOnlySpan<char> _rest = content;
    private ReadOnlySpan<char> _content = content;

    public IReadOnlyList<Token> Tokenize()
    {
        var tokens = new List<Token>();

        while (_content is not [])
        {
            tokens.Add(GetToken());
        }

        return tokens;
    }

    private Token GetToken()
    {
        var chunk = GetChunk();
        var range = GetRange(_content, chunk);

        if (chunk.Length is 1)
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
            '&' => TokenType.BitAnd,
            '|' => TokenType.BitOr,
            '^' => TokenType.BitXor,
            '~' => TokenType.BitNot,
            '!' => TokenType.LogicalNot,
             _ => throw new NotImplementedException($"Not valid single char. {c}")
        };

        return new Token(type, range);
    }

    private ReadOnlySpan<char> GetChunk()
    {
        ReadOnlySpan<char> chunk;
        var whitespaceIndex = _rest.IndexOfAny(_wsSearchValues);

        if (whitespaceIndex < 0)
        {
            chunk = _rest;
            _rest = [];
            return chunk;
        }

        chunk = _rest[..whitespaceIndex];

        var afterWhitespace = _rest[whitespaceIndex..];
        var nonWhitespaceIndex = afterWhitespace.IndexOfAnyExcept(_wsSearchValues);

        if (nonWhitespaceIndex < 0)
        {
            _rest = [];
            return chunk;
        }

        _rest =  afterWhitespace[nonWhitespaceIndex..];
        return chunk;
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

        return new(elementOffset, elementOffset + slice.Length);
    }
}
