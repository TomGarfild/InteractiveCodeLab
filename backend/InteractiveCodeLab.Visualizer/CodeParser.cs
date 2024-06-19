using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using InteractiveCodeLab.Domain;
using InteractiveCodeLab.Domain.Models;
using InteractiveCodeLab.Visualizer.Visitors;

namespace InteractiveCodeLab.Visualizer;

public static class CodeParser
{
    public static Step[] Parse(string language, string? code)
    {
        var inputStream = new AntlrInputStream(code);
        var lexer = GetLexer(language, inputStream);
        var commonTokenStream = new CommonTokenStream(lexer);
        var parseTree = GetParseTree(language,commonTokenStream);

        return GetSteps(language, parseTree);
    }

    private static Lexer GetLexer(string language, AntlrInputStream inputStream)
    {
        return language.ToLower() switch
        {
            Languages.Python3 => new Python3Lexer(inputStream),
            Languages.CSharp => new CSharpLexer(inputStream),
            Languages.Java => new JavaLexer(inputStream),
            _ => throw new ArgumentException($"Unsupported language = {language}")
        };
    }

    private static IParseTree GetParseTree(string language, CommonTokenStream commonTokenStream)
    {
        return language.ToLower() switch
        {
            Languages.Python3 =>new Python3Parser(commonTokenStream).file_input(),
            Languages.CSharp =>new CSharpParser(commonTokenStream).compilation_unit(),
            Languages.Java =>new JavaParser(commonTokenStream).compilationUnit(),
            _ => throw new ArgumentException($"Unsupported language = {language}")
        };
    }

    private static Step[] GetSteps(string language, IParseTree parseTree)
    {
        switch (language.ToLower())
        {
            case Languages.Python3:
                var python3Visitor = new Python3CodeVisitor();
                python3Visitor.Visit(parseTree);
                return python3Visitor.Steps.ToArray();
            case Languages.CSharp:
                var csharpVisitor = new CSharpCodeVisitor();
                csharpVisitor.Visit(parseTree);
                return csharpVisitor.Steps.ToArray();
            case Languages.Java:
                var javaVisitor = new JavaCodeVisitor();
                javaVisitor.Visit(parseTree);
                return javaVisitor.Steps.ToArray();
            default:
                throw new ArgumentException($"Unsupported language = {language}");
        }
    }
}