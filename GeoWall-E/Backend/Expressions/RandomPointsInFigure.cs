using static System.Runtime.InteropServices.JavaScript.JSType;

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
            if (Figure as IEvaluable != null)
            {
                var figure = ((IEvaluable)Figure).Evaluate(symbolTable, error);
                if (figure is Line line)
                {
                    IEnumerable<Point> points = GenerateRandomPointsOnLine(line);
                    return new Sequence(points);
                }
                else
                {
                    error.AddError($"SEMANTIC ERROR: Invalid expression in points()");
                    return new ErrorType();
                }
            }
            else
            {
                error.AddError($"SEMANTIC ERROR: Invalid expression in points()");
                return new ErrorType();
            }
        }
        public List<Point> GenerateRandomPointsOnLine(Line line)
        {
            List<Point> points = new List<Point>();
            Random random = new Random();

            int numPoints = random.Next(1, 51); // Genera un n�mero aleatorio entre 1 y 50

            for (int i = 0; i < numPoints; i++)
            {
                double t;
                if (i < numPoints / 2)
                {
                    // Para la primera mitad de los puntos, genera t entre 0 y 0.1
                    t = random.NextDouble() * 0.1;
                }
                else
                {
                    // Para la segunda mitad de los puntos, genera t entre 0.9 y 1
                    t = 0.9 + random.NextDouble() * 0.1;
                }

                double x = line.P1.X + t * (line.P2.X - line.P1.X);
                double y = line.P1.Y + t * (line.P1.Y - line.P1.Y);
                Point point = new Point();
                point.AsignX(x);
                point.AsignY(y);
                points.Add(point);
            }

            return points;
        }
    }
}