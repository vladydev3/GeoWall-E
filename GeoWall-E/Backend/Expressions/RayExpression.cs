namespace GeoWall_E
{
    public class RayExpression : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.Ray;
        private Token Start_ { get; set; }
        private Token End_ { get; set; }

        public RayExpression(Token start, Token end)
        {
            Start_ = start;
            End_ = end;
        }

        public Token Start => Start_;
        public Token End => End_;

        public Type Evaluate(SymbolTable symbolTable, Error error)
        {
            var start = symbolTable.Resolve(Start.Text);
            var end = symbolTable.Resolve(End.Text);

            if (start.ObjectType == ObjectTypes.Error || end.ObjectType == ObjectTypes.Error)
            {
                error.AddError($"SEMANTIC ERROR: Can't evaluate ray expression");
                return new ErrorType();
            }

            if (start.ObjectType != ObjectTypes.Point || end.ObjectType != ObjectTypes.Point)
            {
                error.AddError($"SEMANTIC ERROR: Ray start and end must be points");
                return new ErrorType();
            }

            return new Ray((Point)start, (Point)end);
        }

        public void HandleRayExpression(List<Tuple<Type, Color>> toDraw, Error errors, SymbolTable symbolTable, Color color)
        {
            var start = symbolTable.Resolve(Start.Text);
            var end = symbolTable.Resolve(End.Text);
            if (start is not ErrorType && end is not ErrorType)
            {
                if (start.ObjectType == ObjectTypes.Point && end.ObjectType == ObjectTypes.Point)
                {
                    toDraw.Add(new Tuple<Type, Color>(new Ray((Point)start, (Point)end), color));
                }
                else
                {
                    errors.AddError($"Invalid type for {Start.Text} or {End.Text}, Line: {Start.Line}, Column: {Start.Column}");
                }
            }
            else
            {
                errors.AddError($"Variable {Start.Text} or {End.Text} not declared, Line: {Start.Line}, Column: {Start.Column}");
            }
        }
    }
}