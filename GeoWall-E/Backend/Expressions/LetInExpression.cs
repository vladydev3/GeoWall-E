namespace GeoWall_E;

public class LetInExpression : Expression
{
    public override TokenType Type => TokenType.LetInExpression;
    public List<Statement> Let { get; set; }
    public Expression In { get; set; }

    public LetInExpression(List<Statement> letStatement, Expression inExpression)
    {
        Let = letStatement;
        In = inExpression;
    }
}