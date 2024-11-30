namespace Tools;

public class GenerateAst(string expressionOutputDir, string visitorOutputDir)
{
    private string _expressionOutputDir = expressionOutputDir;
    private string _visitorOutputDir = visitorOutputDir;

    public void DefineAst(string baseName, List<string> types)
    {
        var path = $"{_expressionOutputDir}\\{baseName}.cs";
        var writer = new StreamWriter(path);
        var classNames = new List<string>();
        
        writer.WriteLine("using LoxSharp.Visitors;");
        writer.WriteLine("");
        writer.WriteLine("namespace LoxSharp.Expressions;");
        writer.WriteLine("");
        writer.WriteLine(GetGeneratedText());
        writer.WriteLine($"public abstract class {baseName}");
        writer.WriteLine("{");
        
        writer.WriteLine("    public abstract T Accept<T>(IVisitor<T> visitor);");
        
        writer.WriteLine("}");

        writer.Close();
        
        foreach (var type in types)
        {
            var className = type.Split(':')[0].Trim();
            var properties = type.Split(':')[1].Trim();
            
            path = $"{_expressionOutputDir}\\{className}.cs";
            writer = new StreamWriter(path);
            
            DefineType(writer, baseName, className, properties);
            classNames.Add(className);
            
            writer.Close();
        }

        path = $"{_visitorOutputDir}\\IVisitor.cs";
        writer = new StreamWriter(path);
        DefineVisitor(writer, classNames);
        writer.Close();
    }

    private void DefineType(StreamWriter writer, string baseName, string className, string propertyString)
    {
        var properties = propertyString.Split(',');
        
        writer.WriteLine("using LoxSharp.Visitors;");
        writer.WriteLine("");
        writer.WriteLine("namespace LoxSharp.Expressions;");
        writer.WriteLine("");
        writer.WriteLine(GetGeneratedText());
        writer.WriteLine($"public class {className} : {baseName}");
        writer.WriteLine("{");
        
        foreach (var property in properties)
        {
            var propertyParts = property.Split(' ');
            writer.WriteLine($"    public {propertyParts[0]} {ToTitleCase(propertyParts[1])} {{ get; set;}}");
        }
        
        writer.WriteLine("");
        writer.WriteLine($"    public {className}({EscapeKeyword(propertyString)})");
        writer.WriteLine("    {");
        
        foreach (var property in properties)
        {
            var propertyParts = property.Split(' ');
            writer.WriteLine($"        {ToTitleCase(propertyParts[1])} = {EscapeKeyword(propertyParts[1])};");
        }
        
        writer.WriteLine("    }");
        
        writer.WriteLine("");
        writer.WriteLine("    public override T Accept<T>(IVisitor<T> visitor)");
        writer.WriteLine("    {");
        writer.WriteLine("        return visitor.Visit(this);");
        writer.WriteLine("    }");
        
        writer.WriteLine("}");
    }

    private void DefineVisitor(StreamWriter writer, List<string> classNames)
    {
        writer.WriteLine("using LoxSharp.Expressions;");
        writer.WriteLine("");
        writer.WriteLine("namespace LoxSharp.Visitors;");
        writer.WriteLine("");
        writer.WriteLine(GetGeneratedText());
        writer.WriteLine($"public interface IVisitor<T>");
        writer.WriteLine("{");

        foreach (var className in classNames)
        {
            writer.WriteLine("");
            writer.WriteLine($"    public T Visit({className} expression);");
        }
        
        writer.WriteLine("}");
    }

    private string EscapeKeyword(string templateText)
    {
        var keywords = new List<string>() { "operator" };

        foreach (var keyword in keywords)
        {
            if (templateText.Contains(keyword))
            {
                templateText = templateText.Replace(keyword, $"@{keyword}");
            }            
        }

        return templateText;
    }

    private string ToTitleCase(string text)
    {
        if (String.IsNullOrEmpty(text))
            return "";
        
        return Char.ToUpper(text[0]) + text.Substring(1);
    }

    private string GetGeneratedText()
    {
        return "/* DO NOT EDIT. This class is generated from the tools project.";
    }
    
    //  TODO: Auto generate the IVisitor; 
}