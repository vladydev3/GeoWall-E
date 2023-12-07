namespace GeoWall_E
{
    public class RayStatement : Statement
    {
        public override TokenType Type => TokenType.Ray;
        private Token Name_ { get; set; }
        private bool IsSequence_ { get; set; }

        public RayStatement(Token name, bool sequence = false)
        {
            Name_ = name;
            IsSequence_ = sequence;
        }

        public Token Name => Name_;

        public bool IsSequence => IsSequence_;

        public static Sequence CreateSequence()
        {
            // crear una secuencia de rayos aleatorios, una cantidad aleatoria entre 10 y 10000
            var random = new Random();
            var rays = new List<Ray>();
            var count = random.Next(10, 10000);
            for (var i = 0; i < count; i++)
            {
                var point1 = new Point();
                point1.AsignX(random.Next(0, 10000));
                point1.AsignY(random.Next(0, 10000));
                var point2 = new Point();
                point2.AsignX(random.Next(0, 10000));
                point2.AsignY(random.Next(0, 10000));
                rays.Add(new Ray(point1, point2));
            }
            return new Sequence(rays);
        }
    }
}