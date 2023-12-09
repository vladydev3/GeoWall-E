namespace GeoWall_E
{
    public class StringExpression : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.String;
        private Token String_ { get; set; }
        public StringExpression(Token String)
        {
            String_ = String;
        }
        public Token String => String_;

        public Type Evaluate(SymbolTable table, Error error, List<Tuple<Type, Color>> toDraw)
        {
            return new StringLiteral(String_.Text);
        }
    }
}