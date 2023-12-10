namespace GeoWall_E
{
    public class ArcExpression : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.Arc;
        Expression Center_ { get; set; }
        Expression Start_ { get; set; }
        Expression End_ { get; set; }
        Expression Measure_ { get; set; }
        readonly Dictionary<string, Tuple<int, int>> Positions_;

        public ArcExpression(Expression center, Expression start, Expression end, Expression measure, Dictionary<string, Tuple<int, int>> positions)
        {
            Center_ = center;
            Start_ = start;
            End_ = end;
            Measure_ = measure;
            Positions_ = positions;
        }

        public Expression Center => Center_;
        public Expression Start => Start_;
        public Expression End => End_;
        public Expression Measure => Measure_;
        public Dictionary<string, Tuple<int, int>> Positions => Positions_;

        public Type Evaluate(SymbolTable symbolTable, Error error, List<Tuple<Type, Color>> toDraw)
        {
            if (Center is IEvaluable centerEvaluable && Start is IEvaluable startEvaluable && End is IEvaluable endEvaluable && Measure is IEvaluable measureEvaluable)
            {
                var center = centerEvaluable.Evaluate(symbolTable, error, toDraw);
                var start = startEvaluable.Evaluate(symbolTable, error, toDraw);
                var end = endEvaluable.Evaluate(symbolTable, error, toDraw);
                var measure = measureEvaluable.Evaluate(symbolTable, error, toDraw);
                if (center is not ErrorType && start is not ErrorType && end is not ErrorType && measure is not ErrorType) return new Arc((Point)center, (Point)start, (Point)end, (Measure)measure);
                else return new ErrorType();
            }
            else return new ErrorType();
        }
        public void HandleArcExpression(List<Tuple<Type, Color>> toDraw, Error errors, SymbolTable symbolTable, Color color, string name)
        {
            var arc = Evaluate(symbolTable, errors, toDraw);
            if (arc is not ErrorType)
            {
                ((Arc)arc).SetName(name);
                toDraw.Add(new Tuple<Type, Color>(arc, color));
            }
        }

        public void HandleArcAsignationExpression(SymbolTable symbolTable, Error errors, AsignationStatement asignation)
        {
            var arc = Evaluate(symbolTable, errors, toDraw: new List<Tuple<Type, Color>>());
            if (arc is not ErrorType)
            {
                symbolTable.Define(asignation.Name.Text, (Arc)arc);
            }
        }
    }
}