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

    public Type Evaluate(SymbolTable symbolTable, Error error, List<Tuple<Type, Color>> toDraw)
    {
        symbolTable.EnterScope();
        var evaluatedIf = (IEvaluable)Condition;
        var evaluatedThen = (IEvaluable)Then;
        var evaluatedElse = (IEvaluable)Else;

        var evaluatedIfResult = evaluatedIf.Evaluate(symbolTable, error, toDraw);
        if (evaluatedIfResult.ObjectType == ObjectTypes.Error) return evaluatedIfResult;

        // Si el resultado de la condicion es un numero, se revisa si es 0 (true) u otro numero (false)
        if (evaluatedIfResult.ObjectType == ObjectTypes.Number)
        {
            var number = (NumberLiteral)evaluatedIfResult;
            if (number.Value == 0)
            {
                return evaluatedElse.Evaluate(symbolTable, error, toDraw);
            }
            else
            {
                return evaluatedThen.Evaluate(symbolTable, error, toDraw);
            }
        }
        // Si el resultado de la condicion es Undefined o una secuencia vacia, se evalua el else
        if (evaluatedIfResult.ObjectType == ObjectTypes.Undefined)
        {
            return evaluatedElse.Evaluate(symbolTable, error, toDraw);
        }
        if (evaluatedIfResult.ObjectType == ObjectTypes.Sequence)
        {
            var sequence = (Sequence)evaluatedIfResult;
            if (!sequence.Elements.Any())
            {
                return evaluatedElse.Evaluate(symbolTable, error, toDraw);
            }
            else
            {
                return evaluatedThen.Evaluate(symbolTable, error, toDraw);
            }
        }
        // Cualquier otro tipo de objeto se considera true
        else
        {
            return evaluatedThen.Evaluate(symbolTable, error, toDraw);
        }
    }
}