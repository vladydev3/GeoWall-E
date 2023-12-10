namespace GeoWall_E
{
    public class UndefinedExpression : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.Undefined;

        public Type Evaluate(SymbolTable symbolTable, Error error, List<Tuple<Type, Color>> toDraw)
        {
            return new Undefined();
        }
    }

}