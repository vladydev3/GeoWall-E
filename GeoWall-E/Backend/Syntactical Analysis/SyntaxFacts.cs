namespace GeoWall_E
{
    static class SyntaxFacts
    {
        public static int GetUnaryOperatorPrecedence(this TokenType kind)
        {
            return kind switch
            {
                TokenType.Plus or TokenType.Minus => 4,
                _ => 0,
            };
        }
        public static int GetBinaryOperatorPrecedence(this TokenType type)
        {
            return type switch
            {
                TokenType.Pow => 6,
                TokenType.Mult or TokenType.Slash => 5,
                TokenType.Plus or TokenType.Minus => 4,
                TokenType.Equal or TokenType.Greater or TokenType.GreaterOrEqual or TokenType.Less or TokenType.LessOrEqual or TokenType.Mod => 3,
                TokenType.And => 2,
                TokenType.Or => 1,
                _ => 0,
            };
        }
    }
}