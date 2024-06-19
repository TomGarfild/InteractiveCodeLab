using InteractiveCodeLab.Domain.Models;

namespace InteractiveCodeLab.Visualizer.Visitors;

public class Python3CodeVisitor : Python3ParserBaseVisitor<List<Step>>
{
    public List<Step> Steps = new();

    public override List<Step> VisitFile_input(Python3Parser.File_inputContext context)
    {
        foreach (var stmt in context.stmt())
        {
            Visit(stmt);
        }
        return Steps;
    }

    public override List<Step> VisitExpr_stmt(Python3Parser.Expr_stmtContext context)
    {
        if (context.testlist_star_expr().Length > 0 && context.ASSIGN().Length > 0)
        {
            for (var i = 0; i < context.testlist_star_expr().Length - 1; i++)
            {
                var variableName = context.testlist_star_expr(i).GetText();
                var value = context.testlist_star_expr(i + 1).GetText();

                Steps.Add(new VariableAssignmentStep(0)
                {
                    VariableName = variableName,
                    Value = value
                });
            }
        }
        return base.VisitExpr_stmt(context);
    }
    
    public override List<Step> VisitIf_stmt(Python3Parser.If_stmtContext context)
    {
        for (var i = 0; i < context.test().Length; i++)
        {
            var condition = context.test(i).GetText();
            var conditionStep = new ConditionStep(0)
            {
                Condition = condition
            };

            var steps = new List<Step>();
            VisitBlock(context.block(i), steps);
            conditionStep.Steps = steps.Select(s => s.Id).ToList();

            Steps.Add(conditionStep);
        }

        if (context.ELSE() != null)
        {
            var elseStep = new ConditionStep(0)
            {
                Condition = null
            };
            var steps = new List<Step>();
            VisitBlock(context.block(context.block().Length - 1), steps);
            elseStep.Steps = steps.Select(s => s.Id).ToList();
            Steps.Add(elseStep);
        }

        return base.VisitIf_stmt(context);
    }

    public override List<Step> VisitFor_stmt(Python3Parser.For_stmtContext context)
    {
        var loopVariable = context.exprlist().GetText();
        var iterable = context.testlist().GetText();
        var forLoopStep = new LoopStep(0)
        {
            LoopVariable = loopVariable,
            Iterable = iterable
        };

        var steps = new List<Step>();
        VisitBlock(context.block(0), steps);
        forLoopStep.Steps = steps.Select(s => s.Id).ToList();

        Steps.Add(forLoopStep);
        return base.VisitFor_stmt(context);
    }
    
    public override List<Step> VisitFuncdef(Python3Parser.FuncdefContext context)
    {
        var functionName = context.children[1].GetText();
        var functionStep = new FunctionStep(0, functionName);

        var parameters = context.children[2] as Python3Parser.ParametersContext;
        if (parameters != null)
        {
            var paramList = parameters.typedargslist();
            if (paramList != null)
            {
                foreach (var param in paramList.children)
                {
                    if (param is Python3Parser.TfpdefContext)
                    {
                        functionStep.Arguments.Add(param.GetText());
                    }
                }
            }
        }

        Steps.Add(functionStep);
        return base.VisitFuncdef(context);
    }

    public override List<Step> VisitAtom_expr(Python3Parser.Atom_exprContext context)
    {
        if (context.trailer() != null && context.trailer().Length > 0)
        {
            string functionName = context.atom().GetText();
            List<string> arguments = new List<string>();

            foreach (var trailer in context.trailer())
            {
                if (trailer.arglist() != null)
                {
                    foreach (var argument in trailer.arglist().argument())
                    {
                        arguments.Add(argument.GetText());
                    }
                }
            }

            var callStep = new CallStep(0, functionName) {Arguments = new List<int>()};
            Steps.Add(callStep);
        }
        return base.VisitAtom_expr(context);
    }
    
    private void VisitBlock(Python3Parser.BlockContext block, List<Step> steps)
    {
        var previousSteps = Steps;
        Steps = steps;
        Visit(block);
        Steps = previousSteps;
    }
}
