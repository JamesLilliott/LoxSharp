using LoxSharp.Visitors;

namespace LoxSharp.Expressions;

/* DO NOT EDIT. This class is generated from the tools project. 
 Why not just hand write it? Because I spent over an hour converting the expression generator from the book so feel a need to use it even though hand writing the expressions would be 10* easier and quicker.*/
public class Literal : Expression
{
    public object Value { get; set;}

    public Literal(object value)
    {
        Value = value;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
