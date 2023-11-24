namespace GeoWall_E;

public class IfExpression : Expression
{
    public override TokenType Type => TokenType.If;
    public Expression If { get; set; }
    public Expression Then { get; set; }
    public Expression Else { get; set; }

    public IfExpression(Expression ifExpression, Expression thenExpression, Expression elseExpression)
    {
        If = ifExpression;
        Then = thenExpression;
        Else = elseExpression;
    }
}