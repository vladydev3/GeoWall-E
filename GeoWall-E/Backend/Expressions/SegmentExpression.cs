namespace GeoWall_E
{
    public class SegmentExpression : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.Segment;
        Expression Start_ { get; set; }
        Expression End_ { get; set; }
        readonly Dictionary<string, Tuple<int, int>> Positions_;

        public SegmentExpression(Expression start, Expression end, Dictionary<string, Tuple<int, int>> positions)
        {
            Start_ = start;
            End_ = end;
            Positions_ = positions;
        }

        public Expression Start => Start_;
        public Expression End => End_;
        public Dictionary<string, Tuple<int, int>> Positions => Positions_;

        public Type Evaluate(SymbolTable symbolTable, Error error, List<Tuple<Type, Color>> toDraw)
        {
            if (Start as IEvaluable != null && End as IEvaluable != null)
            {
                var start = ((IEvaluable)Start).Evaluate(symbolTable, error, toDraw);
                var end = ((IEvaluable)End).Evaluate(symbolTable, error, toDraw);
                if (start is not ErrorType && end is not ErrorType)
                {
                    if (start.ObjectType == ObjectTypes.Point && end.ObjectType == ObjectTypes.Point) return new Segment((Point)start, (Point)end);
                    if (start.ObjectType != ObjectTypes.Point)
                    {
                        return new ErrorType();
                    }
                    return new ErrorType();
                }
                else return new ErrorType();
            }
            else
            {
                return new ErrorType();
            }
        }

        public void HandleSegmentExpression(List<Tuple<Type, Color>> toDraw, Error errors, SymbolTable symbolTable, Color color, string name)
        {
            var segment = Evaluate(symbolTable, errors, toDraw);
            if (segment is not ErrorType)
            {
                ((Segment)segment).SetName(name);
                toDraw.Add(new Tuple<Type, Color>(segment, color));
            }
        }

        public void HandleSegmentAsignationExpression(SymbolTable symbolTable, Error errors, AsignationStatement asignation)
        {
            var segment = Evaluate(symbolTable, errors, toDraw: new List<Tuple<Type, Color>>());
            if (segment is not ErrorType)
            {
                symbolTable.Define(asignation.Name.Text, (Segment)segment);
            }
        }
    }
}