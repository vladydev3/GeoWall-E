namespace GeoWall_E
{
    public class CircleExpression : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.Circle;
        Expression Center_ { get; set; }
        Expression Radius_ { get; set; }
        readonly Dictionary<string, Tuple<int, int>> Positions_;

        public CircleExpression(Expression center, Expression radius, Dictionary<string, Tuple<int, int>> positions)
        {
            Center_ = center;
            Radius_ = radius;
            Positions_ = positions;
        }

        public Expression Center => Center_;
        public Expression Radius => Radius_;
        public Dictionary<string, Tuple<int, int>> Positions => Positions_;

        public Type Evaluate(SymbolTable symbolTable, Error errors)
        {
            if (Center as IEvaluable != null && Radius as IEvaluable != null)
            {
                var center = ((IEvaluable)Center).Evaluate(symbolTable, errors);
                var radius = ((IEvaluable)Radius).Evaluate(symbolTable, errors);
                if (center is not ErrorType && radius is not ErrorType) return new Circle((Point)center, (Measure)radius);
            }
            return new ErrorType();
        }

        public void HandleCircleExpression(List<Tuple<Type, Color>> toDraw, Error errors, SymbolTable symbolTable, Color color, string name)
        {
            var circle = Evaluate(symbolTable, errors);
            if (circle is not ErrorType)
            {
                ((Circle)circle).SetName(name);
                toDraw.Add(new Tuple<Type, Color>(circle, color));
            }
        }

        public void HandleCircleAsignationExpression(SymbolTable symbolTable, Error errors, AsignationStatement asignation)
        {
            var circle = Evaluate(symbolTable, errors);
            if (circle is not ErrorType)
            {
                symbolTable.Define(asignation.Name.Text, (Circle)circle);
            }
        }
    }
}