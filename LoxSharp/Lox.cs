using LoxSharp.Parse;
using Tools;

namespace LoxSharp;

public class Lox
{
    public static bool _hadError = true;

    public Lox()
    {
    }

    public static void Main(string[] args)
    {
        if (args.Length > 1)
        {
            Console.WriteLine("Usage: lox-sharp [script]"); // TODO: Swap with the actual command and args
            Environment.Exit(64);
        }
        else if (args.Length == 1)
        {
            RunFile(args[0]);
        }
        else
        {
            RunPrompt();
        }
    }

    private static void RunFile(string path)
    {
        // Get all the bytes from the file at the path
        var bytes = File.ReadAllBytes(Path.GetFullPath(path)); // TODO: Check path is correct
        
        // Convert to string
        var content = bytes.ToString();
        if (string.IsNullOrWhiteSpace(content))
        {
            return; // TODO: Handle error better
        }
        
        // Pass to run function
        Run(content);
        if (_hadError)
        {
            Environment.Exit(65);
        }
    }
    
    private static void RunPrompt()
    {
        do
        {
            // Prompt user for input
            Console.WriteLine("> ");
            
            // Gather input
            var input = Console.ReadLine();
            if (input == null)
            {
                break;
            }
            
            // Run input
            Run(input);
            _hadError = false;
        } while (true);
    }
    
    private static void Run(string source)
    {
        var scanner = new Scanner(source);
        List<Token> tokens = scanner.ScanTokens();

        var parser = new Parser(tokens);
        var expression = parser.Parse();

        if (_hadError)
        {
            return;
        }

        Console.WriteLine(new AstPrinter().Print(expression));
    }

    public static void Error(int line, string message)
    {
        Report(line, "", message);
    }
    
    public static void Error(Token token, string message)
    {
        if (token.Type == TokenType.EOF)
        {
            Report(token.Line, "at end", message);
        }
        else
        {
            Report(token.Line, $"at {token.Lexeme}", message);   
        }
    }

    private static void Report(int line, string where, string message)
    {
        Console.WriteLine($"[line: {line}] {where}: {message}");
        _hadError = true;
    }
}
