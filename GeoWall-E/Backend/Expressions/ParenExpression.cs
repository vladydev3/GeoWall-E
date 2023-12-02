namespace GeoWall_E;

public class ParenExpression : Expression, IEvaluable
{
    public override TokenType Type => TokenType.ParenExpression;
    public Expression Expression { get; set; }

    public ParenExpression(Expression expression)
    {
        Expression = expression;
    }

    public Type Evaluate(SymbolTable symbolTable, Error error)
    {
        var exp = (IEvaluable)Expression;
        return exp.Evaluate(symbolTable, error);
    }
}