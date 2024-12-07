using LoxSharp.Expressions;

namespace LoxSharp.Visitors;

/* DO NOT EDIT. This class is generated from the tools project. 
 Why not just hand write it? Because I spent over an hour converting the expression generator from the book so feel a need to use it even though hand writing the expressions would be 10* easier and quicker.*/
public interface IVisitor<T>
{

    public T Visit(Binary expression);

    public T Visit(Grouping expression);

    public T Visit(Literal expression);

    public T Visit(Unary expression);
}
