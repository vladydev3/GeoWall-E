namespace GeoWall_E
{
    public class LineStatement : Statement
    {
        public override TokenType Type => TokenType.Line;
        private Token Name_ { get; set; }
        private bool IsSequence_ { get; set; }
        private Color Color_ { get; set; }

        public LineStatement(Token name, Color color, bool sequence = false)
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