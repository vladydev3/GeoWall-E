namespace GeoWall_E
{
    public class IntersectExpression : Expression
    {
        public override TokenType Type => TokenType.Intersect;
        private Token F1_ { get; set; }
        private Token F2_ { get; set; }
        private Color Color_ { get; set; }

        public IntersectExpression(Token f1, Token f2, Color color)
        {
            F1_ = f1;
            F2_ = f2;
            Color_ = color;
        }

        public Token F1 => F1_;
        public Token F2 => F2_;
        public Color Color => Color_;
    }
}