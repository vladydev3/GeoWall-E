namespace GeoWall_E
{
    public class PointStatement : Statement
    {
        public override TokenType Type => TokenType.Point;
        private Token Name_ { get; }
        private bool IsSequence_ { get; }
        private Color Color_ { get; }

        public PointStatement(Token name, Color color, bool sequence = false)
        {
            Name_ = name;
            IsSequence_ = sequence;
            Color_ = color;
        }

        public Token Name => Name_;

        public bool IsSequence => IsSequence_;

        public Color Color => Color_;
    }
}