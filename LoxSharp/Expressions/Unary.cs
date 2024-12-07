using LoxSharp.Visitors;

namespace LoxSharp.Expressions;

/* DO NOT EDIT. This class is generated from the tools project. 
 Why not just hand write it? Because I spent over an hour converting the expression generator from the book so feel a need to use it even though hand writing the expressions would be 10* easier and quicker.*/
public class Unary : Expression
{
    public Token Operator { get; set;}
    public Expression Right { get; set;}

    public Unary(Token @operator,Expression right)
    {
        Operator = @operator;
        Right = right;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
