using LoxSharp;

namespace Tests.Scanner;

public static class TokenHelper
{
    public static void TestEqual(List<Token> actualTokens, List<Token> expectedTokens)
    {
        expectedTokens.Add(new Token(TokenType.EOF, string.Empty, null, 1));
        
        for (var i = 0; i < actualTokens.Count; i++)
        {
            Assert.That(actualTokens[i].ToString(), Is.EqualTo(expectedTokens[i].ToString()));   
        }
    }
}