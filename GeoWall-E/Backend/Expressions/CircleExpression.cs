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

        public Type Evaluate(SymbolTable symbolTable, Error error)
        {
            if (Center as IEvaluable != null && Radius as IEvaluable != null)
            {
                var center = ((IEvaluable)Center).Evaluate(symbolTable, error);
                var radius = ((IEvaluable)Radius).Evaluate(symbolTable, error);
                if (center is not ErrorType && radius is not ErrorType)
                {
                    if (center.ObjectType == ObjectTypes.Point && radius.ObjectType == ObjectTypes.Measure) return new Circle((Point)center, (Measure)radius);
                    if (center.ObjectType != ObjectTypes.Point)
                    {
                        error.AddError($"Expected Point type but got {center.ObjectType} Line: {Positions["center"].Item1}, Column: {Positions["center"].Item2}");
                        return new ErrorType();
                    }
                    error.AddError($"Expected Measure type but got {radius.ObjectType} Line: {Positions["radius"].Item1}, Column: {Positions["radius"].Item2}");
                    return new ErrorType();
                }
                else return new ErrorType();
            }
            else
            {
                error.AddError($"Invalid expression in circle(), Line: {Positions["circle"].Item1}, Column: {Positions["circle"].Item2}");
                return new ErrorType();
            }
        }

        public void HandleCircleExpression(List<Tuple<Type, Color>> toDraw, Error errors, SymbolTable symbolTable, Color color, string name)
        {
            if (Center as IEvaluable != null && Radius as IEvaluable != null)
            {
                var center = ((IEvaluable)Center).Evaluate(symbolTable, errors);
                var radius = ((IEvaluable)Radius).Evaluate(symbolTable, errors);
                if (center is not ErrorType && radius is not ErrorType)
                {
                    if (center.ObjectType == ObjectTypes.Point && radius.ObjectType == ObjectTypes.Measure) toDraw.Add(new Tuple<Type, Color>(new Circle((Point)center, (Measure)radius, name), color));

                    else if (center.ObjectType != ObjectTypes.Point) errors.AddError($"Expected Point type but got {center.ObjectType} Line: {Positions["center"].Item1}, Column: {Positions["center"].Item2}");

                    else errors.AddError($"Expected Measure type but got {radius.ObjectType} Line: {Positions["radius"].Item1}, Column: {Positions["radius"].Item2}");
                }
            }
            else errors.AddError($"Invalid expression in circle(), Line: {Positions["circle"].Item1}, Column: {Positions["circle"].Item2}");
        }

        public void HandleCircleAsignationExpression(SymbolTable symbolTable, Error errors, AsignationStatement asignation)
        {
            if (Center as IEvaluable != null && Radius as IEvaluable != null)
            {
                var center = ((IEvaluable)Center).Evaluate(symbolTable, errors);
                var radius = ((IEvaluable)Radius).Evaluate(symbolTable, errors);
                if (center is not ErrorType && radius is not ErrorType)
                {
                    if (center.ObjectType == ObjectTypes.Point && radius.ObjectType == ObjectTypes.Measure) symbolTable.Define(asignation.Name.Text, new Circle((Point)center, (Measure)radius));

                    else if (center.ObjectType != ObjectTypes.Point) errors.AddError($"Expected Point type but got {center.ObjectType} Line: {Positions["center"].Item1}, Column: {Positions["center"].Item2}");

                    else errors.AddError($"Expected Measure type but got {radius.ObjectType} Line: {Positions["radius"].Item1}, Column: {Positions["radius"].Item2}");
                }
            }
            else errors.AddError($"Invalid expression in circle(), Line: {Positions["circle"].Item1}, Column: {Positions["circle"].Item2}");
        }
    }
}