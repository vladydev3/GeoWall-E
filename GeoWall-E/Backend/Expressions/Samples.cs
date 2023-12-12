namespace GeoWall_E
{
    public class Samples : Expression, IEvaluable // random points
    {
        public override TokenType Type => TokenType.Samples;

        public Type Evaluate(SymbolTable symbolTable, Error error, List<Tuple<Type, Color>> toDraw)
        {
            return new Sequence(CreatePoints());
        }

        static IEnumerable<Type> CreatePoints()
        {
            var random = new Random();

            while (true)
            {
                var point = new Point();
                // Asignar coordenadas random
                point.AsignX(random.Next(0, 10000));
                point.AsignY(random.Next(0, 10000));
                yield return point;
            }
        }
    }
}
