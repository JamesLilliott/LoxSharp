using LoxSharp;

namespace Tests.Scanner;

[TestFixture]
public class CanParseSingleToken
{
    [Test]
    public void Test()
    {
        // Assign - Set up scanner and input
        var input = "(){}-+;,";
        var expectedTokens = new List<Token>()
        {
            new Token(TokenType.LEFT_PAREN, "(", null, 1),
            new Token(TokenType.RIGHT_PAREN, ")", null, 1),
            new Token(TokenType.LEFT_BRACE, "{", null, 1),
            new Token(TokenType.RIGHT_BRACE, "}", null, 1),
            new Token(TokenType.MINUIS, "-", null, 1),
            new Token(TokenType.PLUS, "+", null, 1),
            new Token(TokenType.SEMICOLON, ";", null, 1),
            new Token(TokenType.COMMA, ",", null, 1),
        };
        
        // Act 
        var actualTokens = new LoxSharp.Scanner(input).ScanTokens();
        
        // Assert
        TokenHelper.TestEqual(actualTokens, expectedTokens);
    }
}