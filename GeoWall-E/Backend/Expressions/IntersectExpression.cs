namespace GeoWall_E
{
    public class IntersectExpression : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.Intersect;
        private Expression F1_ { get; set; }
        private Expression F2_ { get; set; }

        public IntersectExpression(Expression f1, Expression f2)
        {
            F1_ = f1;
            F2_ = f2;
        }

        public Expression F1 => F1_;
        public Expression F2 => F2_;

        public Type Evaluate(SymbolTable symbolTable, Error error)
        {
            if (F1 is not IEvaluable f1 || F2 is not IEvaluable f2)
            {
                error.AddError("SEMANTIC ERROR: Can't intersect");
                return new ErrorType();
            }
            var f1Evaluated = f1.Evaluate(symbolTable, error);
            var f2Evaluated = f2.Evaluate(symbolTable, error);
            // Calcular la interseccion
            return new ErrorType();
        }
    }
}