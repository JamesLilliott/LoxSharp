using System.Linq.Expressions;
using LoxSharp.Expressions;
using Expression = LoxSharp.Expressions.Expression;

namespace LoxSharp.Parse;

public class Parser
{
    private readonly List<Token> _tokens;
    private int _current = 0;

    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
    }

    public Expression Parse()
    {
        try
        {
            return Expression();
        }
        catch (ParseError e)
        {
            return null;
        }
    }

    private Expression Expression()
    {
        return Equality();
    }

    private Expression Equality()
    {
        Expression expression = Comparison();
        while (Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
        {
            Token @operator = Previous();
            Expression right = Comparison();
            expression = new Binary(expression, @operator, right);
        }

        return expression;
    }

    /**
     * Check if the current token matches any of types
     */
    private bool Match(params TokenType[] types)
    {
        foreach (var type in types)
        {
            if (Check(type))
            {
                Advance();
                return true;
            }
        }

        return false;
    }

    private Expression Comparison()
    {
        Expression expression = Term();
        while (Match(TokenType.LESS, TokenType.LESS_EQUAL, TokenType.GREATER, TokenType.GREATER_EQUAL))
        {
            Token @operator = Previous();
            Expression right = Term();
            expression = new Binary(expression, @operator, right);
        }

        return expression;
    }

    private Expression Term()
    {
        Expression expression = Factor();
        while (Match(TokenType.MINUIS, TokenType.PLUS))
        {
            Token @operator = Previous();
            Expression right = Factor();
            expression = new Binary(expression, @operator, right);
        }

        return expression;
    }

    private Expression Factor()
    {
        Expression expression = Unary();
        while (Match(TokenType.SLASH, TokenType.STAR))
        {
            Token @operator = Previous();
            Expression right = Unary();
            expression = new Binary(expression, @operator, right);
        }

        return expression;
    }
    
    private Expression Unary()
    {
        if (Match(TokenType.BANG, TokenType.MINUIS))
        {
            Token @operator = Previous();
            Expression right = Primary();
            return new Unary(@operator, right);
        }

        return Primary();
    }

    private Expression Primary()
    {
        if (Match(TokenType.FALSE))
        {
            return new Literal(false);
        }
        if (Match(TokenType.TRUE))
        {
            return new Literal(true);
        }
        if (Match(TokenType.NIL))
        {
            return new Literal(null);
        }

        if (Match(TokenType.NUMBER, TokenType.STRING))
        {
            return new Literal(Previous().Literal);
        }

        if (Match(TokenType.LEFT_PAREN))
        {
            Expression expression = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
            return new Grouping(expression);
        }

        throw Error(Peak(), "Expected expression.");
    }

    private Token Consume(TokenType type, string message)
    {
        if (Check(type))
        {
            return Advance();
        }

        throw Error(Peak(), message);
    }

    private ParseError Error(Token token, string message)
    {
        Lox.Error(token, message);
        return new ParseError();
    }

    private void Synchronise()
    {
        Advance();
        while (!IsAtEnd())
        {
            if (Previous().Type == TokenType.SEMICOLON)
            {
                return;
            }

            switch (Peak().Type)
            {
                case TokenType.CLASS:
                case TokenType.FOR:
                case TokenType.FUN:
                case TokenType.IF:
                case TokenType.PRINT:
                case TokenType.RETURN:
                case TokenType.VAR:
                case TokenType.WHILE:
                    return;
            }
        }

        Advance();
    }

    /**
     * Check if the current token is type
     */
    private bool Check(TokenType type)
    {
        if (IsAtEnd())
        {
            return false;
        }

        return Peak().Type == type;
    }

    private Token Peak()
    {
        return _tokens[_current];
    }

    private bool IsAtEnd()
    {
        return Peak().Type == TokenType.EOF;
    }

    /*
     * Consume current token and return it;
     */
    private Token Advance()
    {
        if (!IsAtEnd())
        {
            _current++;
        }

        return Previous();
    }

    private Token Previous()
    {
        return _tokens[_current - 1];
    }

}