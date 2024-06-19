using InteractiveCodeLab.Domain.Models;

namespace InteractiveCodeLab.Visualizer.Visitors;

public class CSharpCodeVisitor : CSharpParserBaseVisitor<Step>
{
    private int _idCounter = 0;
    public List<Step> Steps = new List<Step>();

    private int GetNextId() => _idCounter++;

    public override Step VisitLocal_variable_declaration(CSharpParser.Local_variable_declarationContext context)
    {
        foreach (var variableDeclarator in context.local_variable_declarator())
        {
            var variableName = variableDeclarator.identifier().GetText();
            var value = variableDeclarator.local_variable_initializer().GetText();

            var step = new VariableAssignmentStep(GetNextId())
            {
                VariableName = variableName,
                Value = value
            };

            Steps.Add(step);
        }

        return base.VisitLocal_variable_declaration(context);
    }

    public override Step VisitIfStatement(CSharpParser.IfStatementContext context)
    {
        var condition = context.expression().GetText();

        var conditionStep = new ConditionStep(GetNextId())
        {
            Condition = condition
        };

        foreach (var stmt in context.if_body())
        {
            var step = Visit(stmt);
            if (step != null)
            {
                conditionStep.Steps.Add(step.Id);
            }
        }

        if (context.ELSE() != null)
        {
            var step = Visit(context.ELSE());
            if (step != null)
            {
                conditionStep.Steps.Add(step.Id);
            }
        }

        Steps.Add(conditionStep);
        return conditionStep;
    }

    public override Step VisitForStatement(CSharpParser.ForStatementContext context)
    {
        var loopInitializer = context.for_initializer().GetText();
        var loopCondition = context.expression().GetText();
        var loopIterator = context.for_iterator().GetText();

        var loopStep = new LoopStep(GetNextId())
        {
            LoopVariable = loopInitializer,
            Iterable = loopCondition
        };

        foreach (var stmt in context.embedded_statement().children)
        {
            var step = Visit(stmt);
            if (step != null)
            {
                loopStep.Steps.Add(step.Id);
            }
        }

        Steps.Add(loopStep);
        return loopStep;
    }
}
