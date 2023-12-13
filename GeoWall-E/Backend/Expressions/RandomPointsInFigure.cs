using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GeoWall_E
{
    public class RandomPointsInFigure : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.Points;
        Token Figure_ { get; set; }

        public RandomPointsInFigure(Token figure)
        {
            Figure_ = figure;
        }

        public Token Figure => Figure_;

        public Type Evaluate(SymbolTable symbolTable, Error error, List<Tuple<Type, Color>> toDraw)
        {
            // Buscar la figura en el scope
            var figureResolved = symbolTable.Resolve(Figure.Text);
            if (figureResolved is IDraw figure)
            {
                if (figure is Line line)
                {
                    IEnumerable<Point> points = GenerateRandomPointsOnLine(line);
                    return new Sequence(points);
                }
                if (figure is Segment segment)
                {
                    IEnumerable<Point> points = GenerateRandomPointsOnSegment(segment);
                    return new Sequence(points);
                }
                if (figure is Ray ray)
                {
                    IEnumerable<Point> points = GenerateRandomPointsOnRay(ray);
                    return new Sequence(points);
                }
                if (figure is Circle circle)
                {
                    IEnumerable<Point> points = GenerateRandomPointsOnCircle(circle);
                    return new Sequence(points);
                }
                if (figure is Arc arc)
                {
                    Circle circle1 = new Circle(arc.Center, arc.Measure);
                    double startAngle = Math.Atan2(arc.Extremo1.Y - circle1.Center.Y, arc.Extremo1.X - circle1.Center.X);
                    double endAngle = Math.Atan2(arc.Extremo2.Y - circle1.Center.Y, arc.Extremo2.X - circle1.Center.X);
                    IEnumerable<Point> points = GenerateRandomPointsOnArc(circle1, startAngle, endAngle, arc);
                    return new Sequence(points);
                }
            }
            error.AddError($"RUNTIME ERROR: Can't generate random points in '{Figure.Text}'.");
            return new Sequence(new List<Type>());
        }
        public IEnumerable<Point> GenerateRandomPointsOnLine(Line line)
        {
            Random random = new Random();

            while (true)
            {
                double t;
                // Genera t entre 0 y 1
                t = random.NextDouble();
                double x = line.P1.X + t * (line.P2.X - line.P1.X);
                double y = line.P1.Y + t * (line.P2.Y - line.P1.Y);
                Point point = new Point();
                point.AsignX(x);
                point.AsignY(y);
                yield return point;
            }
        }
        public IEnumerable<Point> GenerateRandomPointsOnSegment(Segment segment)
        {
            Random random = new Random();

            while (true)
            {
                double t = random.NextDouble();

                double x = segment.Start.X + t * (segment.End.X - segment.Start.X);
                double y = segment.Start.Y + t * (segment.End.Y - segment.Start.Y);

                Point point = new Point();
                point.AsignX(x);
                point.AsignY(y);

                yield return point;
            }
        }
        public IEnumerable<Point> GenerateRandomPointsOnRay(Ray ray)
        {
            Random random = new Random();

            while (true)
            {
                double t;
                // Genera t entre 0 y infinito
                t = random.NextDouble() * 10;

                // Verifica si el punto generado está dentro del rayo 
                if (t >= 0)
                {
                    double x = ray.Start.X + t * (ray.End.X - ray.Start.X);
                    double y = ray.Start.Y + t * (ray.End.Y - ray.Start.Y);
                    Point point = new Point();
                    point.AsignX(x);
                    point.AsignY(y);
                    yield return point;
                }
            }
        }
        public IEnumerable<Point> GenerateRandomPointsOnCircle(Circle circle)
        {
            Random random = new Random();

            while (true)
            {
                // Genera un ángulo aleatorio
                double angle = 2 * Math.PI * random.NextDouble();

                // Genera una distancia aleatoria desde el centro
                double r = circle.Radius.Value * Math.Sqrt(random.NextDouble());

                // Calcula las coordenadas del punto
                double x = r * Math.Cos(angle) + circle.Center.X;
                double y = r * Math.Sin(angle) + circle.Center.Y;

                Point point = new Point();
                point.AsignX(x);
                point.AsignY(y);

                yield return point;
            }
        }
        public IEnumerable<Point> GenerateRandomPointsOnArc(Circle circle, double startAngle, double endAngle, Arc arc)
        {
            Random random = new Random();

            // Asegúrate de que startAngle es menor que endAngle
            if (startAngle > endAngle)
            {
                double temp = startAngle;
                startAngle = endAngle;
                endAngle = temp;
            }

            while (true)
            {
                // Genera un ángulo aleatorio dentro del rango especificado
                double angle = startAngle + random.NextDouble() * (endAngle - startAngle);

                // Calcula las coordenadas del punto
                double x = circle.Radius.Value * Math.Cos(angle) + circle.Center.X;
                double y = circle.Radius.Value * Math.Sin(angle) + circle.Center.Y;

                Point point = new Point();
                point.AsignX(x);
                point.AsignY(y);
                if (IntersectFigures.PointOnArc(point, arc))
                {
                    yield return point;
                }

            }
        }
    }
}

