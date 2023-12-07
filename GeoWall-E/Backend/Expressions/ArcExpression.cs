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

        public Type Evaluate(SymbolTable symbolTable, Error error)
        {
            if (Center as IEvaluable != null && Start as IEvaluable != null && End as IEvaluable != null && Measure as IEvaluable != null)
            {
                var center = ((IEvaluable)Center).Evaluate(symbolTable, error);
                var start = ((IEvaluable)Start).Evaluate(symbolTable, error);
                var end = ((IEvaluable)End).Evaluate(symbolTable, error);
                var measure = ((IEvaluable)Measure).Evaluate(symbolTable, error);
                if (center is not ErrorType && start is not ErrorType && end is not ErrorType && measure is not ErrorType)
                {
                    if (center.ObjectType == ObjectTypes.Point && start.ObjectType == ObjectTypes.Point && end.ObjectType == ObjectTypes.Point && measure.ObjectType == ObjectTypes.Measure) return new Arc((Point)center, (Point)start, (Point)end, (Measure)measure);
                    if (center.ObjectType != ObjectTypes.Point)
                    {
                        error.AddError($"SEMANTIC ERROR: Expected Point type but got {center.ObjectType} Line: {Positions["center"].Item1}, Column: {Positions["center"].Item2}");
                        return new ErrorType();
                    }
                    if (start.ObjectType != ObjectTypes.Point)
                    {
                        error.AddError($"SEMANTIC ERROR: Expected Point type but got {start.ObjectType} Line: {Positions["start"].Item1}, Column: {Positions["start"].Item2}");
                        return new ErrorType();
                    }
                    if (end.ObjectType != ObjectTypes.Point)
                    {
                        error.AddError($"SEMANTIC ERROR: Expected Point type but got {end.ObjectType} Line: {Positions["end"].Item1}, Column: {Positions["end"].Item2}");
                        return new ErrorType();
                    }
                    error.AddError($"SEMANTIC ERROR: Expected Measure type but got {measure.ObjectType} Line: {Positions["measure"].Item1}, Column: {Positions["measure"].Item2}");
                    return new ErrorType();
                }
                else return new ErrorType();
            }
            else
            {
                error.AddError($"SEMANTIC ERROR: Invalid expression in arc(), Line: {Positions["arc"].Item1}, Column: {Positions["arc"].Item2}");
                return new ErrorType();
            }
        }

        public void HandleArcExpression(List<Tuple<Type, Color>> toDraw, Error errors, SymbolTable symbolTable, Color color, string name)
        {
            var arc = Evaluate(symbolTable, errors);
            if (arc is not ErrorType)
            {
                ((Arc)arc).SetName(name);
                toDraw.Add(new Tuple<Type, Color>(arc, color));
            }
        }

        public void HandleArcAsignationExpression(SymbolTable symbolTable, Error errors, AsignationStatement asignation)
        {
            var arc = Evaluate(symbolTable, errors);
            if (arc is not ErrorType)
            {
                symbolTable.Define(asignation.Name.Text, (Arc)arc);
            }
        }
    }
}