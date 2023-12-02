namespace GeoWall_E
{
    public class RayStatement : Statement
    {
        public override TokenType Type => TokenType.Ray;
        private Token Name_ { get; set; }
        private bool IsSequence_ { get; set; }

        public RayStatement(Token name, bool sequence = false)
        {
            Name_ = name;
            IsSequence_ = sequence;
        }

        public Token Name => Name_;

        public bool IsSequence => IsSequence_;

    }
}