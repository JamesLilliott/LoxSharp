// See https://aka.ms/new-console-template for more information

using System.Linq.Expressions;
using LoxSharp;
using LoxSharp.Expressions;
using LoxSharp.Visitors;

Console.WriteLine("Printing the AST!");

var output = new AstPrinter().Print(
    new Binary(
        new Unary(
            new Token(TokenType.MINUIS, "-", null, 1),
            new Literal(123)
            ),
        new Token(TokenType.STAR, "*", null, 1),
        new Grouping(
            new Literal(45.67))
        ));

Console.WriteLine(output);