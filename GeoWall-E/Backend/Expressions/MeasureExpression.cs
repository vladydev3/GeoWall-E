namespace GeoWall_E
{
    public class MeasureExpression : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.Measure;
        Expression P1_ { get; set; }
        Expression P2_ { get; set; }
        readonly Dictionary<string, Tuple<int, int>> Positions_;

        public MeasureExpression(Expression p1, Expression p2, Dictionary<string, Tuple<int, int>> positions)
        {
            P1_ = p1;
            P2_ = p2;
            Positions_ = positions;
        }

        public Expression P1 => P1_;
        public Expression P2 => P2_;
        public Dictionary<string, Tuple<int, int>> Positions => Positions_;

        public Type Evaluate(SymbolTable symbolTable, Error error)
        {
            if (P1 as IEvaluable != null && P2 as IEvaluable != null)
            {
                var p1 = ((IEvaluable)P1).Evaluate(symbolTable, error);
                var p2 = ((IEvaluable)P2).Evaluate(symbolTable, error);
                if (p1 is not ErrorType && p2 is not ErrorType)
                {
                    if (p1.ObjectType == ObjectTypes.Point && p2.ObjectType == ObjectTypes.Point) return new Measure((Point)p1, (Point)p2);
                    if (p1.ObjectType != ObjectTypes.Point)
                    {
                        error.AddError($"Expected Point type but got {p1.ObjectType} Line: {Positions["p1"].Item1}, Column: {Positions["p1"].Item2}");
                        return new ErrorType();
                    }
                    error.AddError($"Expected Point type but got {p2.ObjectType} Line: {Positions["p2"].Item1}, Column: {Positions["p2"].Item2}");
                    return new ErrorType();
                }
                else return new ErrorType();
            }
            else
            {
                error.AddError($"Invalid expression in measure(), Line: {Positions["measure"].Item1}, Column: {Positions["measure"].Item2}");
                return new ErrorType();
            }
        }

        public void HandleMeasureExpression(SymbolTable symbolTable, Error errors, string asignationName)
        {
            var measure = Evaluate(symbolTable, errors);
            if (measure is not ErrorType)
            {
                symbolTable.Define(asignationName, (Measure)measure);
            }
        }
    }
}