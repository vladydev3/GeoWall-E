namespace GeoWall_E
{
    public class RayExpression : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.Ray;
        Expression Start_ { get; set; }
        Expression End_ { get; set; }
        readonly Dictionary<string, Tuple<int, int>> Positions_;

        public RayExpression(Expression start, Expression end, Dictionary<string, Tuple<int, int>> positions)
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
                    if (start.ObjectType == ObjectTypes.Point && end.ObjectType == ObjectTypes.Point) return new Ray((Point)start, (Point)end);
                    if (start.ObjectType != ObjectTypes.Point)
                    {
                        error.AddError($"Expected Point type but got {start.ObjectType}, Line: {Positions["p1"].Item1}, Column: {Positions["p1"].Item2}");
                        return new ErrorType();
                    }
                    error.AddError($"Expected Point type but got {end.ObjectType}, Line: {Positions["p2"].Item1}, Column: {Positions["p2"].Item2}");
                    return new ErrorType();
                }
                else return new ErrorType();
            }
            else
            {
                error.AddError($"Invalid expression in ray(), Line: {Positions["ray"].Item1}, Column: {Positions["ray"].Item2}");
                return new ErrorType();
            }
        }

        public void HandleRayExpression(List<Tuple<Type, Color>> toDraw, Error errors, SymbolTable symbolTable, Color color, string name)
        {
            if (Start as IEvaluable != null && End as IEvaluable != null)
            {
                var start = ((IEvaluable)Start).Evaluate(symbolTable, errors);
                var end = ((IEvaluable)End).Evaluate(symbolTable, errors);
                if (start is not ErrorType && end is not ErrorType)
                {
                    if (start.ObjectType == ObjectTypes.Point && end.ObjectType == ObjectTypes.Point) toDraw.Add(new Tuple<Type, Color>(new Ray((Point)start, (Point)end, name), color));

                    else if (start.ObjectType != ObjectTypes.Point) errors.AddError($"Expected Point type but got {start.ObjectType}, Line {Positions["p1"].Item1}, Column: {Positions["p1"].Item2}");

                    else errors.AddError($"Expected Point type but got {end.ObjectType}, Line: {Positions["p2"].Item1}, Column: {Positions["p2"].Item2}");
                }
            }
            else errors.AddError($"Invalid expression in ray(), Line: {Positions["ray"].Item1}, Column: {Positions["ray"].Item2}");
        }

        public void HandleRayAsignationExpression(SymbolTable symbolTable, Error errors, AsignationStatement asignation)
        {
            if (Start as IEvaluable != null && End as IEvaluable != null)
            {
                var start = ((IEvaluable)Start).Evaluate(symbolTable, errors);
                var end = ((IEvaluable)End).Evaluate(symbolTable, errors);
                if (start is not ErrorType && end is not ErrorType)
                {
                    if (start.ObjectType == ObjectTypes.Point && end.ObjectType == ObjectTypes.Point) symbolTable.Define(asignation.Name.Text, new Ray((Point)start, (Point)end));

                    else if (start.ObjectType != ObjectTypes.Point) errors.AddError($"Expected Point type but got {start.ObjectType}, Line: {Positions["p1"].Item1}, Column: {Positions["p1"].Item2}");

                    else errors.AddError($"Expected Point type but got {end.ObjectType}, Line: {Positions["p2"].Item1}, Column: {Positions["p2"].Item2}");
                }
            }
            else errors.AddError($"Invalid expression in ray(), Line: {Positions["ray"].Item1}, Column: {Positions["ray"].Item2}");
        }
    }
}