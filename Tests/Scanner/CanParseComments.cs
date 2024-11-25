using LoxSharp;

namespace Tests.Scanner;

public class CanParseComments
{
    [Test]
    [Ignore("Need proper error handling before testing")]
    public void TestCanParseComments()
    {
        // TODO: Add test for testing comments - can't do this due to error handling calling a static method
        // Assign - Set up scanner and input
        var input = "// This is a comment";
        var expectedTokens = new List<Token>()
        {
            
        };
        
        // Act 
        var actualTokens = new LoxSharp.Scanner(input).ScanTokens();
        
        // Assert
        TokenHelper.TestEqual(actualTokens, expectedTokens);
    }
    
    [Test]
    public void Test()
    {
        // Assign - Set up scanner and input
        var input = "/";
        var expectedTokens = new List<Token>()
        {
            new Token(TokenType.SLASH, "/", null, 1),
        };
        
        // Act 
        var actualTokens = new LoxSharp.Scanner(input).ScanTokens();
        
        // Assert
        TokenHelper.TestEqual(actualTokens, expectedTokens);
    }
}