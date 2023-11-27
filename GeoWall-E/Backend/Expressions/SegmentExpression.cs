namespace GeoWall_E
{
    public class SegmentExpression : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.Segment;
        private Token Start_ { get; set; }
        private Token End_ { get; set; }
        private Color Color_ { get; set; }

        public SegmentExpression(Token start, Token end, Color color)
        {
            Start_ = start;
            End_ = end;
            Color_ = color;
        }

        public Token Start => Start_;
        public Token End => End_;
        public Color Color => Color_;

        public Type Evaluate(SymbolTable symbolTable, Error error)
        {
            var start = symbolTable.Resolve(Start.Text);
            var end = symbolTable.Resolve(End.Text);
            if (start.ObjectType == ObjectTypes.Error || end.ObjectType == ObjectTypes.Error)
            {
                error.AddError($"SEMANTIC ERROR: Can't evaluate segment expression");
                return new ErrorType();
            }
            if (start.ObjectType != ObjectTypes.Point || end.ObjectType != ObjectTypes.Point)
            {
                error.AddError($"SEMANTIC ERROR: Invalid type in segment expression");
                return new ErrorType();
            }
            return new Segment((Point)start, (Point)end, Color);
        }
    }
}