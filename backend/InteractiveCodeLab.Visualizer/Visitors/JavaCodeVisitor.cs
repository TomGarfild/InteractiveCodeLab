using InteractiveCodeLab.Domain.Models;

namespace InteractiveCodeLab.Visualizer.Visitors;

public class JavaCodeVisitor : JavaParserBaseVisitor<Step>
{
    private int _idCounter = 0;
    public List<Step> Steps = new List<Step>();

    private int GetNextId() => _idCounter++;

    public override Step VisitLocalVariableDeclaration(JavaParser.LocalVariableDeclarationContext context)
    {
        foreach (var variableDeclarator in context.variableDeclarators().variableDeclarator())
        {
            var variableName = variableDeclarator.variableDeclaratorId().GetText();
            var value = variableDeclarator.variableInitializer()?.GetText();

            var step = new VariableAssignmentStep(GetNextId())
            {
                VariableName = variableName,
                Value = value
            };

            Steps.Add(step);
        }

        return base.VisitLocalVariableDeclaration(context);
    }

    public override Step VisitStatement(JavaParser.StatementContext context)
    {
        if (context.IF() != null)
        {
            var condition = context.parExpression().expression().GetText();

            var conditionStep = new ConditionStep(GetNextId())
            {
                Condition = condition
            };
            
            var thenBlock = context.statement(0);
            var step = Visit(thenBlock);
            if (step != null)
            {
                conditionStep.Steps.Add(step.Id);
            }

            if (context.ELSE() != null)
            {
                var elseBlock = context.statement(1);
                step = Visit(elseBlock);
                if (step != null)
                {
                    conditionStep.Steps.Add(step.Id);
                }
            }

            Steps.Add(conditionStep);
            return conditionStep;
        }
        else if (context.FOR() != null)
        {
            string loopVariable = null;
            string iterable = null;

            var forControl = context.forControl();
            if (forControl.enhancedForControl() != null)
            {
                var enhancedForControl = forControl.enhancedForControl();
                loopVariable = enhancedForControl.variableDeclaratorId().GetText();
                iterable = enhancedForControl.expression().GetText();
            }
            else
            {
                var forInit = forControl.forInit();
                loopVariable = forInit?.GetText();
                var expression = forControl.expression();
                iterable = expression?.GetText();
            }

            var loopStep = new LoopStep(GetNextId())
            {
                LoopVariable = loopVariable,
                Iterable = iterable
            };

            var block = context.statement(0);
            var step = Visit(block);
            if (step != null)
            {
                loopStep.Steps.Add(step.Id);
            }

            Steps.Add(loopStep);
            return loopStep;
        }
        
        return base.VisitStatement(context);
    }
}
