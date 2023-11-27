namespace GeoWall_E;

public class IfExpression : Expression, IEvaluable
{
    public override TokenType Type => TokenType.If;
    private Expression Condition_ { get; set; }
    private Expression Then_ { get; set; }
    private Expression Else_ { get; set; }

    public IfExpression(Expression ifExpression, Expression thenExpression, Expression elseExpression)
    {
        Condition_ = ifExpression;
        Then_ = thenExpression;
        Else_ = elseExpression;
    }

    public Expression Condition => Condition_;
    public Expression Then => Then_;
    public Expression Else => Else_;

    public Type Evaluate(SymbolTable symbolTable, Error error)
    {
        symbolTable.EnterScope();
        var evaluatedIf = (IEvaluable)Condition;
        var evaluatedThen = (IEvaluable)Then;
        var evaluatedElse = (IEvaluable)Else;

        var evaluatedIfResult = evaluatedIf.Evaluate(symbolTable, error);
        if (evaluatedIfResult.ObjectType == ObjectTypes.Error) return evaluatedIfResult;
        if (evaluatedIfResult.ObjectType == ObjectTypes.Number)
        {
            var number = (NumberLiteral)evaluatedIfResult;
            if (number.Value == 0)
            {
                return evaluatedElse.Evaluate(symbolTable, error);
            }
            else
            {
                return evaluatedThen.Evaluate(symbolTable, error);
            }
        }
        if (evaluatedIfResult.ObjectType == ObjectTypes.Undefined)
        {
            return evaluatedElse.Evaluate(symbolTable, error);
        }
        if (evaluatedIfResult.ObjectType == ObjectTypes.Sequence)
        {
            var sequence = (Sequence)evaluatedIfResult;
            if (sequence.Elements.Count == 0)
            {
                return evaluatedElse.Evaluate(symbolTable, error);
            }
            else
            {
                return evaluatedThen.Evaluate(symbolTable, error);
            }
        }
        else
        {
            return evaluatedThen.Evaluate(symbolTable, error);
        }
    }
}