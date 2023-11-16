namespace GeoWallE;

public class LetInExpression : Expression
{
    public override TokenType Type => TokenType.LetInExpression;
    public List<Node> Let { get; set; }
    public Expression In { get; set; }

    public LetInExpression(List<Node> letStatement, Expression inExpression)
    {
        Let = letStatement;
        In = inExpression;
    }
}