namespace GeoWall_E
{
    public class LineStatement : Statement
    {
        public override TokenType Type => TokenType.Line;
        private Token Name_ { get; set; }
        private bool IsSequence_ { get; set; }

        public LineStatement(Token name, bool sequence = false)
        {
            Name_ = name;
            IsSequence_ = sequence;
        }

        public Token Name => Name_;

        public bool IsSequence => IsSequence_;
    }
}