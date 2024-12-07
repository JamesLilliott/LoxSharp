// See https://aka.ms/new-console-template for more information

using Tools;

var expressionOutputDir = string.Empty;
var visitorOutputDir = string.Empty;

expressionOutputDir = "D:\\Code\\Lox\\LoxSharp\\LoxSharp\\LoxSharp\\Expressions";
visitorOutputDir = "D:\\Code\\Lox\\LoxSharp\\LoxSharp\\LoxSharp\\Visitors";


if (string.IsNullOrWhiteSpace(expressionOutputDir) && string.IsNullOrWhiteSpace(visitorOutputDir))
{
    Console.WriteLine("No path provided.");
    Environment.Exit(64);
}

var astGenerator = new GenerateAst(expressionOutputDir, visitorOutputDir);


var types = new List<string>()
{
    "Binary:Expression left,Token operator,Expression right",
    "Grouping:Expression expression",
    "Literal:object value",
    "Unary:Token operator,Expression right",
};

astGenerator.DefineAst("Expression", types);