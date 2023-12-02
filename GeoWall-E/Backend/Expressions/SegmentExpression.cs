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

        public Type Evaluate(SymbolTable symbolTable, Error error)
        {
            if (Start as IEvaluable != null && End as IEvaluable != null)
            {
                var start = ((IEvaluable)Start).Evaluate(symbolTable, error);
                var end = ((IEvaluable)End).Evaluate(symbolTable, error);
                if (start is not ErrorType && end is not ErrorType)
                {
                    if (start.ObjectType == ObjectTypes.Point && end.ObjectType == ObjectTypes.Point) return new Segment((Point)start, (Point)end);
                    if (start.ObjectType != ObjectTypes.Point)
                    {
                        error.AddError($"Expected Point type but got {start.ObjectType} Line: {Positions["start"].Item1}, Column: {Positions["start"].Item2}");
                        return new ErrorType();
                    }
                    error.AddError($"Expected Point type but got {end.ObjectType} Line: {Positions["end"].Item1}, Column: {Positions["end"].Item2}");
                    return new ErrorType();
                }
                else return new ErrorType();
            }
            else
            {
                error.AddError($"Invalid expression in segment(), Line: {Positions["segment"].Item1}, Column: {Positions["segment"].Item2}");
                return new ErrorType();
            }
        }

        public void HandleSegmentExpression(List<Tuple<Type, Color>> toDraw, Error errors, SymbolTable symbolTable, Color color)
        {
            if (Start as IEvaluable != null && End as IEvaluable != null)
            {
                var start = ((IEvaluable)Start).Evaluate(symbolTable, errors);
                var end = ((IEvaluable)End).Evaluate(symbolTable, errors);
                if (start is not ErrorType && end is not ErrorType)
                {
                    if (start.ObjectType == ObjectTypes.Point && end.ObjectType == ObjectTypes.Point) toDraw.Add(new Tuple<Type, Color>(new Segment((Point)start, (Point)end), color));

                    else if (start.ObjectType != ObjectTypes.Point) errors.AddError($"Expected Point type but got {start.ObjectType} Line: {Positions["start"].Item1}, Column: {Positions["start"].Item2}");

                    else errors.AddError($"Expected Point type but got {end.ObjectType} Line: {Positions["end"].Item1}, Column: {Positions["end"].Item2}");
                }
            }
            else
            {
                errors.AddError($"Invalid expression in segment(), Line: {Positions["segment"].Item1}, Column: {Positions["segment"].Item2}");
            }
        }

        public void HandleSegmentAsignationExpression(SymbolTable symbolTable, Error errors, AsignationStatement asignation)
        {
            if (Start as IEvaluable != null && End as IEvaluable != null)
            {
                var start = ((IEvaluable)Start).Evaluate(symbolTable, errors);
                var end = ((IEvaluable)End).Evaluate(symbolTable, errors);
                if (start is not ErrorType && end is not ErrorType)
                {
                    if (start.ObjectType == ObjectTypes.Point && end.ObjectType == ObjectTypes.Point) symbolTable.Define(asignation.Name.Text, new Segment((Point)start, (Point)end));

                    else if (start.ObjectType != ObjectTypes.Point) errors.AddError($"Expected Point type but got {start.ObjectType} Line: {Positions["start"].Item1}, Column: {Positions["start"].Item2}");

                    else errors.AddError($"Expected Point type but got {end.ObjectType} Line: {Positions["end"].Item1}, Column: {Positions["end"].Item2}");
                }
            }
            else
            {
                errors.AddError($"Invalid expression in segment(), Line: {Positions["segment"].Item1}, Column: {Positions["segment"].Item2}");
            }
        }
    }
}