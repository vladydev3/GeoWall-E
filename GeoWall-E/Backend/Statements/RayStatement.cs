namespace GeoWall_E
{
    public class RayStatement : Statement
    {
        public override TokenType Type => TokenType.Ray;
        private Token Name_ { get; set; }
        private bool IsSequence_ { get; set; }
        private Color Color_ { get; set; }

        public RayStatement(Token name, Color color, bool sequence = false)
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