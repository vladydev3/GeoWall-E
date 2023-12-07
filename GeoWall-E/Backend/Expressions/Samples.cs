namespace GeoWall_E
{
    public class Samples : Expression, IEvaluable // random points
    {
        public override TokenType Type => TokenType.Samples;

        public Type Evaluate(SymbolTable symbolTable, Error error)
        {
            var points = CreatePoints();
            return new Sequence(points);
        }

        static IEnumerable<Type> CreatePoints()
        {
            // Cantidad de puntos a generar
            var random = new Random();
            var count = random.Next(10, 10000);

            for (var i = 0; i < count; i++)
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