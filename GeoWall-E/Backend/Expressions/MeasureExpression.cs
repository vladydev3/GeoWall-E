namespace GeoWall_E
{
    public class MeasureExpression : Expression
    {
        public override TokenType Type => TokenType.Measure;
        private Token P1_ { get; set; }
        private Token P2_ { get; set; }

        public MeasureExpression(Token p1, Token p2)
        {
            P1_ = p1;
            P2_ = p2;
        }

        public Token P1 => P1_;
        public Token P2 => P2_;
    }
}