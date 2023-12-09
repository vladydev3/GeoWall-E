namespace GeoWall_E
{
    public class NumberExpression : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.Number;
        private Token Number_ { get; set; }
        public NumberExpression(Token number)
        {
            Number_ = number;
        }
        public Token Number => Number_;

        public Type Evaluate(SymbolTable table, Error error)
        {
            return new NumberLiteral(double.Parse(Number_.Text));
        }
    }
}