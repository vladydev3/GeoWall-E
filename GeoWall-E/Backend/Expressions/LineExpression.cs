namespace GeoWall_E
{
    public class LineExpression : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.Line;
        Expression P1_ { get; set; }
        Expression P2_ { get; set; }
        readonly Dictionary<string, Tuple<int, int>> Positions_;

        public LineExpression(Expression p1, Expression p2, Dictionary<string, Tuple<int, int>> positions)
        {
            P1_ = p1;
            P2_ = p2;
            Positions_ = positions;
        }

        public Expression P1 => P1_;
        public Expression P2 => P2_;
        public Dictionary<string, Tuple<int, int>> Positions => Positions_;

        public Type Evaluate(SymbolTable symbolTable, Error error, List<Tuple<Type, Color>> toDraw)
        {
            if (P1 as IEvaluable != null && P2 as IEvaluable != null)
            {
                var p1 = ((IEvaluable)P1).Evaluate(symbolTable, error, toDraw);
                var p2 = ((IEvaluable)P2).Evaluate(symbolTable, error, toDraw);
                if (p1 is not ErrorType && p2 is not ErrorType)
                {
                    if (p1.ObjectType == ObjectTypes.Point && p2.ObjectType == ObjectTypes.Point) return new Line((Point)p1, (Point)p2);
                    return new ErrorType();
                }
                else return new ErrorType();
            }
            else
            {
                return new ErrorType();
            }
        }

        public void HandleLineExpression(List<Tuple<Type, Color>> toDraw, Error error, SymbolTable symbolTable, Color colors, string name = "")
        {
            var line = Evaluate(symbolTable, error, toDraw);
            if (line is not ErrorType)
            {
                ((Line)line).SetName(name);
                toDraw.Add(new Tuple<Type, Color>(line, colors));
            }
        }

        public void HandleLineAsignationExpression(SymbolTable symbolTable, Error errors, AsignationStatement asignation)
        {
            var line = Evaluate(symbolTable, errors, toDraw: new List<Tuple<Type, Color>>());
            if (line is not ErrorType)
            {
                symbolTable.Define(asignation.Name.Text, (Line)line);
            }
        }
    }

}