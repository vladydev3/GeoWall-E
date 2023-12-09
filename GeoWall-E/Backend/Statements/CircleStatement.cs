namespace GeoWall_E
{
    public class CircleStatement : Statement, IFigureStatement
    {
        public override TokenType Type => TokenType.Circle;

        private Token Name_ { get; set; }
        private bool IsSequence_ { get; set; }

        public CircleStatement(Token name, bool sequence = false)
        {
            Name_ = name;
            IsSequence_ = sequence;
        }

        public Token Name => Name_;

        public bool IsSequence => IsSequence_;

        public Sequence CreateSequence()
        {
            // crear una secuencia de circulos aleatorios, una cantidad aleatoria entre 10 y 10000
            var random = new Random();
            var circles = new List<Circle>();
            var count = random.Next(10, 10000);
            for (var i = 0; i < count; i++)
            {
                var point = new Point();
                point.AsignX(random.Next(0, 10000));
                point.AsignY(random.Next(0, 10000));
                var circle = new Circle(point, new Measure(random.Next(0, 10000)));
                circles.Add(circle);
            }
            return new Sequence(circles);
        }
    }
}