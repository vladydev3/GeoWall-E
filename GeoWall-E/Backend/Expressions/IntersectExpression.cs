namespace GeoWall_E
{
    public class IntersectExpression : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.Intersect;
        Expression F1_ { get; set; }
        Expression F2_ { get; set; }
        readonly Dictionary<string, Tuple<int, int>> Positions_;

        public IntersectExpression(Expression f1, Expression f2, Dictionary<string, Tuple<int, int>> positions)
        {
            F1_ = f1;
            F2_ = f2;
            Positions_ = positions;
        }

        public Expression F1 => F1_;
        public Expression F2 => F2_;
        public Dictionary<string, Tuple<int, int>> Positions => Positions_;

        public Type Evaluate(SymbolTable symbolTable, Error error)
        {
            if (F1 is not IEvaluable f1 || F2 is not IEvaluable f2)
            {
                error.AddError("SEMANTIC ERROR: Can't intersect {}"); // TODO: Improve this message error
                return new ErrorType();
            }
            var f1Evaluated = f1.Evaluate(symbolTable, error);
            var f2Evaluated = f2.Evaluate(symbolTable, error);
            // Calcular la interseccion
            return new ErrorType();
        }
    }
}