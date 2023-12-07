namespace GeoWall_E
{
    public class Randoms : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.Randoms;

        public Type Evaluate(SymbolTable symbolTable, Error error)
        {
            var randoms = CreateRandoms();  //IEnumerable<NumberLiteral>
            return new Sequence(randoms);
        }

        static IEnumerable<NumberLiteral> CreateRandoms()
        {
            var random = new Random();
            // Se crean numeros de 0 a 1, la cantidad de numeros es aleatoria
            // Cantidad de numeros
            var count = random.Next(10, 100);
            // Crear numeros
            for (int i = 0; i <= count; i++)
            {
                yield return new NumberLiteral(random.NextDouble());
            }
        }
    }
}