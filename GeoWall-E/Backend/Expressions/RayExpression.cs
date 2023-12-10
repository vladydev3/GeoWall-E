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

        public Type Evaluate(SymbolTable symbolTable, Error error, List<Tuple<Type, Color>> toDraw)
        {
            if (Start as IEvaluable != null && End as IEvaluable != null)
            {
                var start = ((IEvaluable)Start).Evaluate(symbolTable, error, toDraw);
                var end = ((IEvaluable)End).Evaluate(symbolTable, error, toDraw);
                if (start is not ErrorType && end is not ErrorType)
                {
                    if (start.ObjectType == ObjectTypes.Point && end.ObjectType == ObjectTypes.Point) return new Ray((Point)start, (Point)end);
                    return new ErrorType();
                }
                else return new ErrorType();
            }
            else
            {
                return new ErrorType();
            }
        }

        public void HandleRayExpression(List<Tuple<Type, Color>> toDraw, Error errors, SymbolTable symbolTable, Color color, string name)
        {
            var ray = Evaluate(symbolTable, errors, toDraw);
            if (ray is not ErrorType)
            {
                ((Ray)ray).SetName(name);
                toDraw.Add(new Tuple<Type, Color>(ray, color));
            }
        }

        public void HandleRayAsignationExpression(SymbolTable symbolTable, Error errors, AsignationStatement asignation)
        {
            var ray = Evaluate(symbolTable, errors, toDraw: new List<Tuple<Type, Color>>());
            if (ray is not ErrorType)
            {
                symbolTable.Define(asignation.Name.Text, (Ray)ray);
            }
        }
    }
}