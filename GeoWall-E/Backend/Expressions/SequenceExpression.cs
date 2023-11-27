namespace GeoWall_E
{
    public class SequenceExpression : Expression // TODO: Make this implement IEvaluable
    {
        public override TokenType Type => TokenType.Sequence;
        private List<Token> Elements_ { get; set; }
        public SequenceExpression(List<Token> elements)
        {
            Elements_ = elements;
        }

        public List<Token> Elements => Elements_;
    }
}