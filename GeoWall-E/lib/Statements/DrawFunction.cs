namespace GeoWallE;

public class Draw : Expression
{
    public override TokenType Type => TokenType.Draw;
    public string Name { get; set; }
    public Expression? Expression { get; set; }
    public List<VariableExpression>? Sequence { get; set; }
    
    public Draw(Expression expression, string name = "")
    {
        Expression = expression;
        Name = name;
    }

    public Draw(List<VariableExpression> ids, string name = "")
    {
        Sequence = ids;
        Name = name;
    }
}