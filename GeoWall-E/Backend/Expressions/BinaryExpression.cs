namespace GeoWall_E;

public class BinaryExpression : Expression
{
    public override TokenType Type => TokenType.BinaryExpression;
    public Expression Left { get; }
    public Token Operator { get; }
    public Expression Right { get; }

    public BinaryExpression(Expression left, Token @operator, Expression right)
    {
        Left = left;
        Operator = @operator;
        Right = right;
    }

}