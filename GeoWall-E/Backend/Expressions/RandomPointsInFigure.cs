namespace GeoWall_E
{
    public class RandomPointsInFigure : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.Points;
        Expression Figure_ { get; set; }

        public RandomPointsInFigure(Expression figure)
        {
            Figure_ = figure;
        }

        public Expression Figure => Figure_;

        public Type Evaluate(SymbolTable symbolTable, Error error)
        {
            throw new NotImplementedException();
        }
    }
}