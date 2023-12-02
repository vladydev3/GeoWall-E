namespace GeoWall_E
{
    public class SequenceExpression : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.Sequence;
        private List<Expression>? Elements_ { get; set; }
        private Token? Range_ {  get; set; }
        public SequenceExpression(List<Expression> elements)
        {
            Elements_ = elements;
        }

        public SequenceExpression(Token range)
        {
            Range_ = range;
        }

        public Token? Range => Range_;

        public List<Expression>? Elements => Elements_;

        public Type Evaluate(SymbolTable symbolTable, Error error)
        {
            
        }
    }
}