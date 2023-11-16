namespace GeoWallE;

public class UnaryExpression : Expression
{
    public override TokenType Type => TokenType.UnaryExpression;
    public Expression Operand { get; }
    public Token Operator { get; }

    public UnaryExpression(Token @operator, Expression operand)
    {
        Operator = @operator;
        Operand = operand;
    }

    public override string ToString()
    {
        return $"({Operator} {Operand})";
    }
}