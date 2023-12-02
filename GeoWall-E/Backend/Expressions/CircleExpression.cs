namespace GeoWall_E
{
    public class CircleExpression : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.Circle;
        private Token Center_ { get; set; }
        private Token Radius_ { get; set; }

        public CircleExpression(Token center, Token radius)
        {
            Center_ = center;
            Radius_ = radius;
        }

        public Token Center => Center_;
        public Token Radius => Radius_;

        public Type Evaluate(SymbolTable symbolTable, Error error)
        {
            var center = symbolTable.Resolve(Center.Text);
            var radius = symbolTable.Resolve(Radius.Text);
            if (center.ObjectType == ObjectTypes.Error || radius.ObjectType == ObjectTypes.Error)
            {
                error.AddError($"SEMANTIC ERROR: Can't evaluate circle expression");
                return new ErrorType();
            }
            if (center.ObjectType != ObjectTypes.Point || radius.ObjectType != ObjectTypes.Measure)
            {
                error.AddError($"SEMANTIC ERROR: Invalid type in circle expression");
                return new ErrorType();
            }
            return new Circle((Point)center, (Measure)radius);
        }

        public void HandleCircleExpression(List<Tuple<Type, Color>> toDraw, Error errors, SymbolTable symbolTable, Color color)
        {
            var center = symbolTable.Resolve(Center.Text);
            var radius = symbolTable.Resolve(Radius.Text);
            if (center is not ErrorType && radius is not ErrorType)
            {
                if (center.ObjectType == ObjectTypes.Point && radius.ObjectType == ObjectTypes.Measure)
                {
                    toDraw.Add(new Tuple<Type, Color>(new Circle((Point)center, (Measure)radius), color));
                }
                else
                {
                    errors.AddError($"Invalid type for {Center.Text} or {Radius.Text}, Line: {Center.Line}, Column: {Center.Column}");
                }
            }
            else
            {
                errors.AddError($"Variable {Center.Text} or {Radius.Text} not declared, Line: {Center.Line}, Column: {Center.Column}");
            }

        }
    }
}