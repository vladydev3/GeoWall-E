namespace GeoWall_E
{
    public class SegmentStatement : Statement
    {
        public override TokenType Type => TokenType.Segment;
        private Token Name_ { get; set; }
        private bool IsSequence_ { get; set; }

        public SegmentStatement(Token name, bool sequence = false)
        {
            Name_ = name;
            IsSequence_ = sequence;
        }

        public Token Name => Name_;

        public bool IsSequence => IsSequence_;

    }
}