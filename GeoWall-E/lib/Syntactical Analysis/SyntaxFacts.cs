namespace GeoWallE;

static class SyntaxFacts
{
    public static int GetUnaryOperatorPrecedence(this TokenType kind)
    {
        return kind switch
        {
            TokenType.Plus or TokenType.Minus => 3,
            _ => 0,
        };
    }
    public static int GetBinaryOperatorPrecedence(this TokenType type)
    {
        return type switch
        {
            TokenType.Pow => 5,
            TokenType.Mult or TokenType.Slash => 4,
            TokenType.Plus or TokenType.Minus => 3,
            TokenType.Greater or TokenType.GreaterOrEqual or TokenType.Less or TokenType.LessOrEqual or TokenType.Mod => 2,
            TokenType.And or TokenType.Or => 1,
            _ => 0,
        };
    }
}