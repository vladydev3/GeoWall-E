namespace GeoWall_E
{
    public class PointStatement : Statement
    {
        public override TokenType Type => TokenType.Point;
        private Token Name_ { get; }
        private bool IsSequence_ { get; }

        public PointStatement(Token name, bool sequence = false)
        {
            Name_ = name;
            IsSequence_ = sequence;
        }

        public Token Name => Name_;

        public bool IsSequence => IsSequence_;

        public static Sequence CreateSequence()
        {
            // crear una secuencia de puntos aleatorios, una cantidad aleatoria 10 y 10000
            var random = new Random();
            var points = new List<Point>();
            var count = random.Next(10, 10000);
            for (var i = 0; i < count; i++)
            {
                var point = new Point();
                point.AsignX(random.Next(0, 10000));
                point.AsignY(random.Next(0, 10000));
                points.Add(point);
            }
            return new Sequence(points);
        }

    }
}