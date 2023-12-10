namespace GeoWall_E;

public class LetInExpression : Expression, IEvaluable
{
    public override TokenType Type => TokenType.LetInExpression;
    private List<Statement> Let_ { get; set; }
    private Expression In_ { get; set; }

    public LetInExpression(List<Statement> letStatement, Expression inExpression)
    {
        Let_ = letStatement;
        In_ = inExpression;
    }

    public List<Statement> Let => Let_;
    public Expression In => In_;

    public Type Evaluate(SymbolTable symbolTable, Error error, List<Tuple<Type, Color>> toDraw)
    {
        symbolTable.EnterScope();

        var evaluator = new Evaluator(Let, error, symbolTable);
        evaluator.Evaluate();
        toDraw.AddRange(evaluator.ToDraw);
        var evaluatedIn = (IEvaluable)In;
        var result = evaluatedIn.Evaluate(symbolTable, error, toDraw);
        symbolTable.ExitScope();
        return result;
    }
}