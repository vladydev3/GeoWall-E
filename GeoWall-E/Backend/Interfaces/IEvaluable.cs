namespace GeoWall_E
{
    public interface IEvaluable
    {
        public Type Evaluate(SymbolTable symbolTable, Error error, List<Tuple<Type, Color>> toDraw);
    }
}