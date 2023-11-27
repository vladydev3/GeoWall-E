namespace GeoWall_E
{
    public class FunctionDeclaration : Statement
    {
        public override TokenType Type => TokenType.FunctionDeclaration;
        private Token Name_ { get; }
        private List<Token> Arguments_ { get; }
        private Expression Body_ { get; }

        public FunctionDeclaration(Token name, List<Token> arguments, Expression body)
        {
            Name_ = name;
            Arguments_ = arguments;
            Body_ = body;
        }

        public Token Name => Name_;

        public List<Token> Arguments => Arguments_;

        public Expression Body => Body_;
    }
}