namespace GeoWall_E
{
    public class Draw : Expression
    {
        public override TokenType Type => TokenType.Draw;

        private string Name_ { get; set; }
        private Expression Expression_ { get; set; }
        private Color Color_ { get; set; }

        public Draw(Expression expression, Color color, string name = "")
        {
            Expression_ = expression;
            Color_ = color;
            Name_ = name;
        }


        public string Name => Name_;

        public Expression Expression => Expression_;

        public Color Color => Color_;
    }
}