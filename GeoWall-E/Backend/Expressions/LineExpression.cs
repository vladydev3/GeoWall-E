namespace GeoWall_E
{
    public class LineExpression : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.Line;
        private Token P1_ { get; set; }
        private Token P2_ { get; set; }

        public LineExpression(Token p1, Token p2)
        {
            P1_ = p1;
            P2_ = p2;
        }

        public Token P1 => P1_;
        public Token P2 => P2_;

        public Type Evaluate(SymbolTable symbolTable, Error error)
        {
            var p1 = symbolTable.Resolve(P1.Text);
            var p2 = symbolTable.Resolve(P2.Text);
            if (p1.ObjectType != ObjectTypes.Point || p1.ObjectType == ObjectTypes.Error)
            {
                error.AddError($"SEMANTIC ERROR: Point {P1.Text} not defined");
                return p1;
            }
            if (p2.ObjectType != ObjectTypes.Point || p2.ObjectType == ObjectTypes.Error)
            {
                error.AddError($"SEMANTIC ERROR: Point {P2.Text} not defined");
                return p2;
            }
            var p1Defined = (Point)p1;
            var p2Defined = (Point)p2;
            return new Line(p1Defined, p2Defined);
        }
        public void HandleLineExpression(List<Tuple<Type,Color>> toDraw, Error error, SymbolTable symbolTable, Color color)
        {
            var p1 = symbolTable.Resolve(P1.Text);
            var p2 = symbolTable.Resolve(P2.Text);
            if (p1 is not ErrorType && p2 is not ErrorType)
            {
                if (p1.ObjectType == ObjectTypes.Point && p2.ObjectType == ObjectTypes.Point)
                {
                    toDraw.Add(new Tuple<Type, Color>(new Line((Point)p1, (Point)p2), color));
                }
                else
                {
                    error.AddError($"Invalid type for {P1.Text} or {P2.Text}, Line: {P1.Line}, Column: {P1.Column}");
                }
            }
            else
            {
                error.AddError($"Variable {P1.Text} or {P2.Text} not declared, Line: {P1.Line}, Column: {P1.Column}");
            }
        }
    }

}