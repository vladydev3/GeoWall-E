namespace GeoWall_E
{
    public class Randoms : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.Randoms;

        public Type Evaluate(SymbolTable symbolTable, Error error, List<Tuple<Type, Color>> toDraw)
        {
            var randoms = CreateRandoms();  //IEnumerable<NumberLiteral>
            return new Sequence(randoms);
        }

        static IEnumerable<NumberLiteral> CreateRandoms()
        {
            var random = new Random();
            while (true)
            {
                yield return new NumberLiteral(random.NextDouble());
            }
        }
    }
}