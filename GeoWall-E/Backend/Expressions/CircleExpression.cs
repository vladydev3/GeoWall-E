namespace GeoWall_E
{
    public class CircleExpression : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.Circle;
        private Token Center_ { get; set; }
        private Token Radius_ { get; set; }
        private Color Color_ { get; set; }

        public CircleExpression(Token center, Token radius, Color color)
        {
            Center_ = center;
            Radius_ = radius;
            Color_ = color;
        }

        public Token Center => Center_;
        public Token Radius => Radius_;
        public Color Color => Color_;

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
            return new Circle((Point)center, (Measure)radius, Color);
        }
    }
}