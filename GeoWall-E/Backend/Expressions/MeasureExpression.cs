namespace GeoWall_E
{
    public class MeasureExpression : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.Measure;
        private Token P1_ { get; set; }
        private Token P2_ { get; set; }

        public MeasureExpression(Token p1, Token p2)
        {
            P1_ = p1;
            P2_ = p2;
        }

        public Token P1 => P1_;
        public Token P2 => P2_;

        public Type Evaluate(SymbolTable symbolTable, Error error)
        {
            var p1 = symbolTable.Resolve(P1.Text);
            var p2 = symbolTable.Resolve(P2.Text);

            if (p1 is ErrorType)
            {
                error.AddError($"Undefined variable {P1.Text}, Line: {P1.Line}, Column: {P1.Column}");
                return p1;
            }

            if (p2 is ErrorType)
            {
                error.AddError($"Undefined variable {P2.Text}, Line: {P2.Line}, Column: {P2.Column}");
                return p2;
            }

            if (p1 is not Point)
            {
                error.AddError($"Expected point, got {p1.ObjectType}, Line: {P1.Line}, Column: {P1.Column}");
                return new ErrorType();
            }

            if (p2 is not Point)
            {
                error.AddError($"Expected point, got {p2.ObjectType}, Line: {P2.Line}, Column: {P2.Column}");
                return new ErrorType();
            }

            return new Measure((Point)p1, (Point)p2);
        }
    }
}