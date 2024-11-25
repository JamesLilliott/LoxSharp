using LoxSharp;

namespace Tests.Scanner;

public class CanParseSingleOrDoubleToken
{
    [Test]
    public void TestBangEquals()
    {
        // Assign - Set up scanner and input
        var input = "!=";
        var expectedTokens = new List<Token>()
        {
            new Token(TokenType.BANG_EQUAL, "!=", null, 1),
        };
        
        // Act 
        var actualTokens = new LoxSharp.Scanner(input).ScanTokens();
        
        // Assert
        TokenHelper.TestEqual(actualTokens, expectedTokens);
    }
    
    [Test]
    public void TestEqualsEquals()
    {
        // Assign - Set up scanner and input
        var input = "==";
        var expectedTokens = new List<Token>()
        {
            new Token(TokenType.EQUAL_EQUAL, "==", null, 1),
        };
        
        // Act 
        var actualTokens = new LoxSharp.Scanner(input).ScanTokens();
        
        // Assert
        TokenHelper.TestEqual(actualTokens, expectedTokens);
    }
    
    [Test]
    public void TestLessEquals()
    {
        // Assign - Set up scanner and input
        var input = "<=";
        var expectedTokens = new List<Token>()
        {
            new Token(TokenType.LESS_EQUAL, "<=", null, 1),
        };
        
        // Act 
        var actualTokens = new LoxSharp.Scanner(input).ScanTokens();
        
        // Assert
        TokenHelper.TestEqual(actualTokens, expectedTokens);
    }
    
    [Test]
    public void TestGreaterEquals()
    {
        // Assign - Set up scanner and input
        var input = ">=";
        var expectedTokens = new List<Token>()
        {
            new Token(TokenType.GREATER_EQUAL, ">=", null, 1),
        };
        
        // Act 
        var actualTokens = new LoxSharp.Scanner(input).ScanTokens();
        
        // Assert
        TokenHelper.TestEqual(actualTokens, expectedTokens);
    }
}