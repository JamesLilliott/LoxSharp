using System.Linq.Expressions;
using System.Text;
using LoxSharp.Expressions;
using Expression = LoxSharp.Expressions.Expression;

namespace LoxSharp.Visitors;

public class AstPrinter : IVisitor<string>
{
    public string Visit(Binary expression)
    {
        return Parenthesize(expression.Operator.Lexeme, expression.Left, expression.Right);
    }

    public string Visit(Grouping expression)
    {
        return Parenthesize("Group", expression.Expression);
    }

    public string Visit(Literal expression)
    {
        if (expression.Value == null)
        {
            return "nil";
        }

        return expression.Value.ToString();
    }

    public string Visit(Unary expression)
    {
        return Parenthesize(expression.Operator.Lexeme, expression.Right);
    }

    public string Print(Expression expression)
    {
        return expression.Accept(this);
    }

    private string Parenthesize(string name, params Expression[] expressions)
    {
        var builder = new StringBuilder();
        builder.Append('(').Append(name);
        
        foreach (var expression in expressions)
        {
            builder.Append(" ");
            builder.Append(expression.Accept(this));
        }

        builder.Append(')');

        return builder.ToString();
    }
}