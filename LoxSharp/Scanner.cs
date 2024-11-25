using System.Text.RegularExpressions;

namespace LoxSharp;

public class Scanner(string source)
{
    private string _source = source;
    private List<Token> _tokens = new List<Token>();
    private int _start = 0;
    private int _current = 0;
    private int _line = 1;
    private Dictionary<string, TokenType> _keywords = new Dictionary<string, TokenType>()
    {
        {"and", TokenType.AND},
        {"class", TokenType.CLASS},
        {"else", TokenType.ELSE},
        {"false", TokenType.FALSE},
        {"for", TokenType.FOR},
        {"fun", TokenType.FUN},
        {"if", TokenType.IF},
        {"nil", TokenType.NIL},
        {"or", TokenType.OR},
        {"print", TokenType.PRINT},
        {"return", TokenType.RETURN},
        {"super", TokenType.SUPER},
        {"this", TokenType.THIS},
        {"true", TokenType.TRUE},
        {"var", TokenType.VAR},
        {"while", TokenType.WHILE},
    };

    public List<Token> ScanTokens()
    {
        while (!IsAtEnd())
        {
            _start = _current;
            ScanToken();
        }
        
        _tokens.Add(new Token(TokenType.EOF, string.Empty, null, _line));
        return _tokens;
    }
    
    private bool IsAtEnd()
    {
        return _current >= _source.Length;
    }
    
    private void ScanToken()
    {
        var c = Advance();
        switch (c)
        {
            // Match single characters
            case '(': AddToken(TokenType.LEFT_PAREN); break;
            case ')': AddToken(TokenType.RIGHT_PAREN); break;
            case '{': AddToken(TokenType.LEFT_BRACE); break;
            case '}': AddToken(TokenType.RIGHT_BRACE); break;
            case ',': AddToken(TokenType.COMMA); break;
            case '.': AddToken(TokenType.DOT); break;
            case '-': AddToken(TokenType.MINUIS); break;
            case '+': AddToken(TokenType.PLUS); break;
            case ';': AddToken(TokenType.SEMICOLON); break;
            case '*': AddToken(TokenType.STAR); break;
            
            // Match single or double characters
            case '!': AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
            case '=': AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
            case '<': AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
            case '>': AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;
            
            // Ignored characters
            case '/':
                //  TODO: can this be put into a function?
                if (Match('/'))
                {
                    if (Peek() != '\n' && !IsAtEnd())
                    {
                        Advance();
                    }
                }
                else
                {
                    AddToken(TokenType.SLASH);
                }
                break;
            case ' ':
            case '\r':
            case '\t':
                break;
            
            // New line
            case '\n':
                _line++;
                break;
            
            // Strings
            case '"': CreateString(); break;
            
            default:
                if (IsDigit(c))
                {
                    CreateNumber();
                }
                else if (IsAlpha(c))
                {
                    CreateIdentifier();
                }
                else
                {
                    Lox.Error(_line, $"Unexpected character '{c}'.");
                }
                
                break;
        }
    }

    private void CreateIdentifier()
    {
        while (IsAlphaNumeric(Peek()))
        {
            Advance();
        }

        var length = _current - _start;
        var identifierText = _source.Substring(_start, length);
        var identifierIsKeyword = _keywords.TryGetValue(identifierText, out var tokenType);
        if (!identifierIsKeyword)
        {
            tokenType = TokenType.IDENTIFIER;
        }
        
        AddToken(tokenType);
    }

    private bool IsAlphaNumeric(char c)
    {
        return IsAlpha(c) || IsDigit(c);
    }

    private bool IsAlpha(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';
    }

    private void CreateNumber()
    {
        while (IsDigit(Peek()))
        {
            Advance();
        }

        if (Peek() == '.' && IsDigit(PeekNext()))
        {
            Advance();

            while (IsDigit(Peek()))
            {
                Advance();
            }
        }

        var length = _current - _start;
        AddToken(TokenType.NUMBER, double.Parse(_source.Substring(_start, length)));
    }

    private char PeekNext()
    {
        if ((_current + 1) >= _source.Length)
        {
            return '\0';
        }

        return _source[_current + 1];
    }

    private bool IsDigit(char c)
    {
        return c >= '0' && c <= '9';
    }

    private void CreateString()
    {
        while (Peek() != '"' && !IsAtEnd())
        {
            if (Peek() == '\n')
            {
                _line++;
            }

            Advance();
        }

        if (IsAtEnd())
        {
            Lox.Error(_line, "Unterminated string.");
        }

        Advance();

        var length = (_current - 1) -(_start + 1);
        var stringLiteral = _source.Substring(_start + 1, length);
        
        AddToken(TokenType.STRING, stringLiteral);
    }

    private char Peek()
    {
        if (IsAtEnd())
        {
            return '\0';
        }

        return _source[_current];
    }

    private char Advance()
    {
        return _source[_current++];
    }

    private void AddToken(TokenType tokenType)
    {
        AddToken(tokenType, null);
    }

    private void AddToken(TokenType tokenType, object literal)
    {
        var length = _current - _start;
        var text = _source.Substring(_start, length);
        _tokens.Add(new Token(tokenType, text, literal, _line));
    }

    private bool Match(char expected)
    {
        if (IsAtEnd() || source[_current] != expected)
        {
            return false;
        }

        _current++;
        return true;
    }
}
