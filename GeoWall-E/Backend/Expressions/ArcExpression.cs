namespace GeoWall_E
{
    public class ArcExpression : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.Arc;
        private Token Center_ { get; set; }
        private Token Start_ { get; set; }
        private Token End_ { get; set; }
        private Token Measure_ { get; set; }

        public ArcExpression(Token center, Token start, Token end, Token measure)
        {
            Center_ = center;
            Start_ = start;
            End_ = end;
            Measure_ = measure;
        }

        public Token Center => Center_;
        public Token Start => Start_;
        public Token End => End_;
        public Token Measure => Measure_;

        public Type Evaluate(SymbolTable symbolTable, Error error)
        {
            var center = (IEvaluable)Center;
            var evaluatedCenter = center.Evaluate(symbolTable, error);
            if (evaluatedCenter.ObjectType == ObjectTypes.Error) return evaluatedCenter;
            if (evaluatedCenter.ObjectType != ObjectTypes.Point)
            {
                error.AddError($"SEMANTIC ERROR: Center of arc must be a point");
                return new ErrorType();
            }

            var start = (IEvaluable)Start;
            var evaluatedStart = start.Evaluate(symbolTable, error);
            if (evaluatedStart.ObjectType == ObjectTypes.Error) return evaluatedStart;
            if (evaluatedStart.ObjectType != ObjectTypes.Point)
            {
                error.AddError($"SEMANTIC ERROR: Start of arc must be a point");
                return new ErrorType();
            }

            var end = (IEvaluable)End;
            var evaluatedEnd = end.Evaluate(symbolTable, error);
            if (evaluatedEnd.ObjectType == ObjectTypes.Error) return evaluatedEnd;
            if (evaluatedEnd.ObjectType != ObjectTypes.Point)
            {
                error.AddError($"SEMANTIC ERROR: End of arc must be a point");
                return new ErrorType();
            }

            var measure = (IEvaluable)Measure;
            var evaluatedMeasure = measure.Evaluate(symbolTable, error);
            if (evaluatedMeasure.ObjectType == ObjectTypes.Error) return evaluatedMeasure;
            if (evaluatedMeasure.ObjectType != ObjectTypes.Number)
            {
                error.AddError($"SEMANTIC ERROR: Measure of arc must be a number");
                return new ErrorType();
            }

            return new Arc((Point)evaluatedCenter, (Point)evaluatedStart, (Point)evaluatedEnd, (Measure)evaluatedMeasure);
        }

        public void HandleArcExpression(List<Tuple<Type, Color>> toDraw, Error errors, SymbolTable symbolTable, Color color)
        {
            var center = symbolTable.Resolve(Center.Text);
            var start = symbolTable.Resolve(Start.Text);
            var end = symbolTable.Resolve(End.Text);
            var measure = symbolTable.Resolve(Measure.Text);
            if (center is not ErrorType && start is not ErrorType && end is not ErrorType && measure is not ErrorType)
            {
                if (center.ObjectType == ObjectTypes.Point && start.ObjectType == ObjectTypes.Point && end.ObjectType == ObjectTypes.Point && measure.ObjectType == ObjectTypes.Measure)
                {
                    toDraw.Add(new Tuple<Type, Color>(new Arc((Point)center, (Point)start, (Point)end, (Measure)measure), color));
                }
                else
                {
                    errors.AddError($"Invalid type for {Center.Text} or {Start.Text} or {End.Text} or {Measure.Text}, Line: {Center.Line}, Column: {Center.Column}");
                }
            }
            else
            {
                errors.AddError($"Variable {Center.Text} or {Start.Text} or {End.Text} or {Measure.Text} not declared, Line: {Center.Line}, Column: {Center.Column}");
            }
        }
    }
}