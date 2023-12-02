namespace GeoWall_E
{
    public class PointStatement : Statement
    {
        public override TokenType Type => TokenType.Point;
        private Token Name_ { get; }
        private bool IsSequence_ { get; }

        public PointStatement(Token name, bool sequence = false)
        {
            Name_ = name;
            IsSequence_ = sequence;
        }

        public Token Name => Name_;

        public bool IsSequence => IsSequence_;

    }
}