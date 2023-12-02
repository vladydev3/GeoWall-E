namespace GeoWall_E
{
    public class CircleStatement : Statement
    {
        public override TokenType Type => TokenType.Circle;

        private Token Name_ { get; set; }
        private bool IsSequence_ { get; set; }

        public CircleStatement(Token name, bool sequence = false)
        {
            Name_ = name;
            IsSequence_ = sequence;
        }

        public Token Name => Name_;

        public bool IsSequence => IsSequence_;

    }
}