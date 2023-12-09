namespace GeoWall_E{
    public interface IEvaluable{
        public Type Evaluate(SymbolTable symbolTable, Error error);
    }
}