using ICSharpCode.AvalonEdit.Document;
using System.Text.Json.Serialization;
using System.Windows.Shapes;
using static System.Windows.Forms.LinkLabel;

namespace GeoWall_E
{
    public static class IntersectFigures
    {
        internal static Sequence IntersectPointAndRay(Type f1, Type f2)
        {
            (Point point, Ray ray) = Utils.OrderByType<Point, Ray>(f1, f2);

            (var m, var b) = Utils.SlopeAndEquation(ray.Start, ray.End);

            // Calcular la coordenada Y del punto en la recta
            var y = m * point.X + b;

            // Calcular la coordenada X del punto en la recta
            var x = (point.Y - b) / m;

            // Si la coordenada Y del punto es igual a la coordenada Y del punto en la recta, hay interseccion
            if (y == point.Y)
            {
                // Comprobar si el punto esta en el rayo
                if ((ray.End.X <= ray.Start.X || x >= ray.Start.X) &&
                    (ray.End.X >= ray.Start.X || x <= ray.Start.X) &&
                    (ray.End.Y <= ray.Start.Y || y >= ray.Start.Y) &&
                    (ray.End.Y >= ray.Start.Y || y <= ray.Start.Y)
                )
                {
                    if (point.X >= Math.Min(ray.Start.X, int.MaxValue) &&
                    point.X <= Math.Max(ray.Start.X, int.MaxValue) &&
                    point.Y >= Math.Min(ray.Start.Y, int.MaxValue) &&
                    point.Y <= Math.Max(ray.Start.Y, int.MaxValue)) return new Sequence(new List<Type>() { point });
                    else return new Sequence(new List<Type>() { new Undefined() });
                }
                else return new Sequence(new List<Type>() { new Undefined() });
            }
            else return new Sequence(new List<Type>() { new Undefined() });
        }

        internal static Sequence IntersectTwoCircles(Type f1, Type f2)
        {
            Circle c1 = (Circle)f1;
            Circle c2 = (Circle)f2;

            double d = Math.Sqrt(Math.Pow(c1.Center.X - c2.Center.X, 2) + Math.Pow(c1.Center.Y - c2.Center.Y, 2));

            // Si la distancia es mayor que la suma de los radios, no hay interseccion, se retorna null
            if (d > c1.Radius.Value + c2.Radius.Value) return new Sequence(new List<Type>() { new Undefined() });

            // Si la distancia es menor que la diferencia absoluta de los radios, un circulo esta dentro del otro
            if (d < Math.Abs(c1.Radius.Value - c2.Radius.Value)) return new Sequence(new List<Type>() { new Undefined() });
            // Si la distancia es igual a la suma de los radios, los círculos son tangentes y tienen un solo punto de intersección
            if (d == c1.Radius.Value + c2.Radius.Value)
            {
                // Calcula las coordenadas del punto de intersección
                double x = c1.Center.X + (c1.Radius.Value / d) * (c2.Center.X - c1.Center.X);
                double y = c1.Center.Y + (c1.Radius.Value / d) * (c2.Center.Y - c1.Center.Y);

                Point point = new();
                point.AsignX(x);
                point.AsignY(y);

                return new Sequence(new List<Type>() { point });
            }

            double r1 = c1.Radius.Value;
            double r2 = c2.Radius.Value;
            
            // Calcula la variable 'a', que es la distancia desde el centro del primer círculo hasta la línea que pasa por los puntos de intersección
            double a = (r1 * r1 - r2 * r2 + d * d) / (2 * d);
            // Calcula la variable 'h', que es la distancia desde la línea que pasa por los puntos de intersección hasta los puntos de intersección
            double h = Math.Sqrt(r1 * r1 - a * a);
            // Calcula las coordenadas de un punto en la línea que pasa por los centros de los círculos
            double x2 = c1.Center.X + a * (c2.Center.X - c1.Center.X) / d;
            double y2 = c1.Center.Y + a * (c2.Center.Y - c1.Center.Y) / d;
            // Calcula las coordenadas del primer punto de intersección
            double x3 = x2 + h * (c2.Center.Y - c1.Center.Y) / d;
            double y3 = y2 - h * (c2.Center.X - c1.Center.X) / d;
            // Calcula las coordenadas del segundo punto de intersección
            double x4 = x2 - h * (c2.Center.Y - c1.Center.Y) / d;
            double y4 = y2 + h * (c2.Center.X - c1.Center.X) / d;

            Point point1 = new();
            point1.AsignX(x3);
            point1.AsignY(y3);

            Point point2 = new();
            point2.AsignX(x4);
            point2.AsignY(y4);

            return new Sequence(new List<Type>() { point1, point2 });
        }

        internal static Sequence IntersectPointAndCircle(Type f1, Type f2)
        {
            (Circle circle, Point point) = Utils.OrderByType<Circle, Point>(f1, f2);

            double d = Math.Sqrt(Math.Pow(point.X - circle.Center.X, 2) + Math.Pow(point.Y - circle.Center.Y, 2));

            if (d == circle.Radius.Value) return new Sequence(new List<Type>() { point });

            return new Sequence(new List<Type>() { new Undefined() });
        }

        internal static Sequence IntersectSegmentAndPoint(Type f1, Type f2)
        {
            (Segment segment, Point point) = Utils.OrderByType<Segment, Point>(f1, f2);

            // Si la pendiente de la recta es infinita
            if (segment.Start.X == segment.End.X) segment.End.AsignX(segment.End.X + 1);

            (var m, var b) = Utils.SlopeAndEquation(segment.Start, segment.End);

            // Calcular la coordenada Y del punto en la recta
            var y = m * point.X + b;

            // Si la coordenada Y del punto es igual a la coordenada Y del punto en la recta, hay interseccion
            if (y == point.Y)
            {
                // Comprobar si el punto esta dentro del segmento
                if (point.X >= Math.Min(segment.Start.X, segment.End.X) &&
                    point.X <= Math.Max(segment.Start.X, segment.End.X) &&
                    point.Y >= Math.Min(segment.Start.Y, segment.End.Y) &&
                    point.Y <= Math.Max(segment.Start.Y, segment.End.Y))
                    return new Sequence(new List<Type>() { point });
                else return new Sequence(new List<Type>() { new Undefined() });
            }
            else return new Sequence(new List<Type>() { new Undefined() });
        }

        internal static Sequence IntersectLineAndPoint(Type f1, Type f2)
        {
            (Line line, Point point) = Utils.OrderByType<Line, Point>(f1, f2);

            // Si la pendiente de la recta es infinita
            if (line.P1.X == line.P2.X) line.P2.AsignX(line.P2.X + 1);

            (var m, var b) = Utils.SlopeAndEquation(line.P1, line.P2);

            // Calcular la coordenada Y del punto en la recta
            var y = m * point.X + b;

            // Si la coordenada Y del punto es igual a la coordenada Y del punto en la recta, hay interseccion
            if (y == point.Y) return new Sequence(new List<Type>() { point });
            return new Sequence(new List<Type>() { new Undefined() });
        }

        internal static Sequence IntersectTwoSegments(Type f1, Type f2)
        {
            Segment segment1 = (Segment)f1;
            Segment segment2 = (Segment)f2;

            (var m1, var b1) = Utils.SlopeAndEquation(segment1.Start, segment1.End);

            (var m2, var b2) = Utils.SlopeAndEquation(segment2.Start, segment2.End);

            if (m1 == m2) return new Sequence(new List<Type>() { new Undefined() });

            var x = (b2 - b1) / (m1 - m2);
            var y = m1 * x + b1;

            // redondear a enteros las coordenadas
            x = Math.Round(x);
            y = Math.Round(y);

            if (x >= Math.Min(segment1.Start.X, segment1.End.X) &&
                x <= Math.Max(segment1.Start.X, segment1.End.X) &&
                y >= Math.Min(segment1.Start.Y, segment1.End.Y) &&
                y <= Math.Max(segment1.Start.Y, segment1.End.Y) &&
                x >= Math.Min(segment2.Start.X, segment2.End.X) &&
                x <= Math.Max(segment2.Start.X, segment2.End.X) &&
                y >= Math.Min(segment2.Start.Y, segment2.End.Y) &&
                y <= Math.Max(segment2.Start.Y, segment2.End.Y))
            {
                Point point = new();
                point.AsignX(x);
                point.AsignY(y);
                return new Sequence(new List<Type>() { point });
            }
            else return new Sequence(new List<Type>() { new Undefined() });
        }

        internal static Sequence IntersectCircleAndArc(Type f1, Type f2)
        {
            (Circle circle, Arc arc) = Utils.OrderByType<Circle, Arc>(f1, f2);

            // Calcular la distancia entre el centro del circulo y el centro del arco
            var distance = Math.Sqrt(Math.Pow(arc.Center.X - circle.Center.X, 2) + Math.Pow(arc.Center.Y - circle.Center.Y, 2));

            // Si la distancia es mayor que la suma de los radios, no hay interseccion
            if (distance > circle.Radius.Value + arc.Measure.Value) return new Sequence(new List<Type>() { new Undefined() });

            // Si la distancia es menor que la diferencia absoluta de los radios, un circulo esta dentro del otro
            if (distance < Math.Abs(circle.Radius.Value - arc.Measure.Value)) return new Sequence(new List<Type>() { new Undefined() });

            // Calcula la variable 'a', que es la distancia desde el centro del arco hasta la línea que pasa por los puntos de intersección
            var a = (arc.Measure.Value * arc.Measure.Value - circle.Radius.Value * circle.Radius.Value + distance * distance) / (2 * distance);
            // Calcula la variable 'h', que es la distancia desde la línea que pasa por los puntos de intersección hasta los puntos de intersección
            var h = Math.Sqrt(arc.Measure.Value * arc.Measure.Value - a * a);
            // Calcula las coordenadas de un punto en la línea que pasa por los centros del arco y del círculo
            var x1 = arc.Center.X + a * (circle.Center.X - arc.Center.X) / distance;
            var y1 = arc.Center.Y + a * (circle.Center.Y - arc.Center.Y) / distance;
            // Calcula las coordenadas del primer punto de intersección
            var x2 = x1 + h * (circle.Center.Y - arc.Center.Y) / distance;
            var y2 = y1 - h * (circle.Center.X - arc.Center.X) / distance;
            // Calcula las coordenadas del segundo punto de intersección
            var x3 = x1 - h * (circle.Center.Y - arc.Center.Y) / distance;
            var y3 = y1 + h * (circle.Center.X - arc.Center.X) / distance;
            Point point1 = new();
            point1.AsignX(x2);
            point1.AsignY(y2);
            bool judge1 = PointOnArc(point1, arc);
            Point point2 = new();
            point2.AsignX(x3);
            point2.AsignY(y3);
            bool judge2 = PointOnArc(point2, arc);
            if (judge1 == true && judge2 == true) return new Sequence(new List<Type>() { point1, point2 });
            if (judge1 == true) return new Sequence(new List<Type>() { point1 });
            if (judge2 == true) return new Sequence(new List<Type>() { point2 });
            return new Sequence(new List<Type>() { new Undefined() });
        }

        internal static Sequence IntersectCircleAndSegment(Type f1, Type f2)
        {
            (Segment segment, Circle circle) = Utils.OrderByType<Segment, Circle>(f1, f2);
            // Calcula la pendiente y el intercepto de la línea que representa el segmento
            (var m, var b) = Utils.SlopeAndEquation(segment.Start, segment.End);
            // Calcula los coeficientes de la ecuación cuadrática que se utilizará para encontrar los puntos de intersección
            var a = 1 + m * m;
            var b1 = -2 * circle.Center.X + 2 * m * b - 2 * m * circle.Center.Y;
            var c = circle.Center.X * circle.Center.X + b * b + circle.Center.Y * circle.Center.Y - 2 * b * circle.Center.Y - circle.Radius.Value * circle.Radius.Value;
            // Calcula el discriminante de la ecuación cuadrática
            var discriminant = b1 * b1 - 4 * a * c;
            // Si el discriminante es menor que 0, entonces la ecuación cuadrática no tiene soluciones reales, lo que significa que el segmento de línea y el círculo no se intersectan
            if (discriminant < 0) return new Sequence(new List<Type>() { new Undefined() });
            else
            {
                // Calcula las coordenadas x de los puntos de intersección
                var x1 = (-b1 + Math.Sqrt(discriminant)) / (2 * a);
                var x2 = (-b1 - Math.Sqrt(discriminant)) / (2 * a);
                // Calcula las coordenadas y de los puntos de intersección
                var y1 = m * x1 + b;
                var y2 = m * x2 + b;

                Point point1 = new();
                point1.AsignX(x1);
                point1.AsignY(y1);

                Point point2 = new();
                point2.AsignX(x2);
                point2.AsignY(y2);
                // Verifica si los puntos de intersección están en el segmento de línea
                // Si ambos puntos están en el segmento de línea, devuelve una secuencia que contiene ambos puntos
                // Si solo uno de los puntos está en el segmento de línea, devuelve una secuencia que contiene ese punto
                // Si ninguno de los puntos está en el segmento de línea, devuelve una secuencia que contiene un objeto Undefined
                if (x1 >= Math.Min(segment.Start.X, segment.End.X) &&
                    x1 <= Math.Max(segment.Start.X, segment.End.X) &&
                    y1 >= Math.Min(segment.Start.Y, segment.End.Y) &&
                    y1 <= Math.Max(segment.Start.Y, segment.End.Y) &&
                    x2 >= Math.Min(segment.Start.X, segment.End.X) &&
                    x2 <= Math.Max(segment.Start.X, segment.End.X) &&
                    y2 >= Math.Min(segment.Start.Y, segment.End.Y) &&
                    y2 <= Math.Max(segment.Start.Y, segment.End.Y))
                {
                    return new Sequence(new List<Type>() { point1, point2 });
                }
                if (x1 >= Math.Min(segment.Start.X, segment.End.X) &&
                    x1 <= Math.Max(segment.Start.X, segment.End.X) &&
                    y1 >= Math.Min(segment.Start.Y, segment.End.Y) &&
                    y1 <= Math.Max(segment.Start.Y, segment.End.Y))
                {
                    return new Sequence(new List<Type>() { point1 });
                }
                if (x2 >= Math.Min(segment.Start.X, segment.End.X) &&
                    x2 <= Math.Max(segment.Start.X, segment.End.X) &&
                    y2 >= Math.Min(segment.Start.Y, segment.End.Y) &&
                    y2 <= Math.Max(segment.Start.Y, segment.End.Y))
                {
                    return new Sequence(new List<Type>() { point2 });
                }
                return new Sequence(new List<Type>() { new Undefined() });
            }
        }

        internal static Sequence IntersectLineAndSegment(Type f1, Type f2)
        {
            (Line line, Segment segment) = Utils.OrderByType<Line, Segment>(f1, f2);
            // Si los puntos de inicio y fin de la línea tienen la misma coordenada x, ajusta la coordenada x del punto final para evitar una división por cero al calcular la pendiente
            if (line.P1.X == line.P2.X) line.P2.AsignX(line.P2.X + 1);
            // Calcula la pendiente y el intercepto de la línea
            (var m1, var b1) = Utils.SlopeAndEquation(line.P1, line.P2);
            //Calcula la pendiente y el intercepto del segmento
           (var m2, var b2) = Utils.SlopeAndEquation(segment.Start, segment.End);
            // Si las pendientes de la línea y el segmento de línea son iguales, entonces son paralelos y no se intersectan
            if (m1 == m2) return new Sequence(new List<Type>() { new Undefined() });
            // Calcula las coordenadas x e y del punto de intersección
            var x = (b2 - b1) / (m1 - m2);
            var y = m1 * x + b1;

            // redondear a enteros las coordenadas
            x = Math.Round(x);
            y = Math.Round(y);

            if (x >= Math.Min(segment.Start.X, segment.End.X) &&
                x <= Math.Max(segment.Start.X, segment.End.X) &&
                y >= Math.Min(segment.Start.Y, segment.End.Y) &&
                y <= Math.Max(segment.Start.Y, segment.End.Y))
            {
                Point point = new();
                point.AsignX(x);
                point.AsignY(y);
                return new Sequence(new List<Type>() { point });
            }
            else return new Sequence(new List<Type>() { new Undefined() });
        }

        internal static Sequence IntersectLineAndCircle(Type f1, Type f2)
        {
            (Line line, Circle circle) = Utils.OrderByType<Line, Circle>(f1, f2);
            // Calcula la pendiente y el intercepto de la línea
            (var m, var b) = Utils.SlopeAndEquation(line.P1, line.P2);
            // Calcula los coeficientes de la ecuación cuadrática que se utilizará para encontrar los puntos de intersección
            var a = 1 + m * m;
            var b1 = -2 * circle.Center.X + 2 * m * b - 2 * m * circle.Center.Y;
            var c = circle.Center.X * circle.Center.X + b * b + circle.Center.Y * circle.Center.Y - 2 * b * circle.Center.Y - circle.Radius.Value * circle.Radius.Value;
            // Calcula el discriminante de la ecuación cuadrática
            var discriminant = b1 * b1 - 4 * a * c;
            // Si el discriminante es menor que 0, entonces la ecuación cuadrática no tiene soluciones reales, lo que significa que la línea y el círculo no se intersectan
            if (discriminant < 0) return new Sequence(new List<Type>() { new Undefined() });
            else
            {
                // Calcula las coordenadas x de los puntos de intersección
                var x1 = (-b1 + Math.Sqrt(discriminant)) / (2 * a);
                var x2 = (-b1 - Math.Sqrt(discriminant)) / (2 * a);
                // Calcula las coordenadas y de los puntos de intersección
                var y1 = m * x1 + b;
                var y2 = m * x2 + b;

                Point point1 = new();
                point1.AsignX(x1);
                point1.AsignY(y1);

                Point point2 = new();
                point2.AsignX(x2);
                point2.AsignY(y2);
                return new Sequence(new List<Type>() { point1, point2 });
            }
        }

        internal static Sequence IntersectTwoLines(Type f1, Type f2)
        {
            var line1 = (Line)f1;
            var line2 = (Line)f2;

            var m1 = (line1.P2.Y - line1.P1.Y) / (line1.P2.X - line1.P1.X);
            var m2 = (line2.P2.Y - line2.P1.Y) / (line2.P2.X - line2.P1.X);

            var b1 = line1.P1.Y - m1 * line1.P1.X;
            var b2 = line2.P1.Y - m2 * line2.P1.X;

            if (m1 == m2) return new Sequence(new List<Type>() { new Undefined() });

            var x = (b2 - b1) / (m1 - m2);
            var y = m1 * x + b1;

            Point point = new();
            point.AsignX(x);
            point.AsignY(y);
            return new Sequence(new List<Type>() { point });
        }
        internal static Sequence IntersectLineAndArc(Type f1, Type f2)
        {
            (Line line, Arc arc) = Utils.OrderByType<Line, Arc>(f1, f2);
            Circle circle = new(arc.Center, arc.Measure);
            List<Type> intersection = IntersectLineAndCircle(line, circle).Elements.ToList();
            if (intersection.Count == 0) return new Sequence(new List<Type>() { new Undefined() });
            if (intersection.Count == 1 && intersection[0] is not Undefined)
            {
                Point point = (Point)intersection[0];
                bool judge = PointOnArc(point, arc);
                if (judge == true) return new Sequence(new List<Type>() { point });
                return new Sequence(new List<Type>() { new Undefined() });
            }
            if (intersection.Count == 2)
            {
                Point point1 = (Point)intersection[0];
                Point point2 = (Point)intersection[1];
                bool judge1 = PointOnArc(point1, arc);
                bool judge2 = PointOnArc(point2, arc);
                if (judge1 == true && judge2 == true) return new Sequence(new List<Type>() { point1, point2 });
                if (judge1 == true) return new Sequence(new List<Type>() { point1 });
                if (judge2 == true) return new Sequence(new List<Type>() { point2 });
                return new Sequence(new List<Type>() { new Undefined() });
            }
            return new Sequence(new List<Type>() { new Undefined() });
        }
        internal static Sequence IntersectSegmentAndArc(Type f1, Type f2)
        {
            (Segment segment, Arc arc) = Utils.OrderByType<Segment, Arc>(f1, f2);
            Circle circle = new(arc.Center, arc.Measure);
            List<Type> intersection = IntersectCircleAndSegment(segment, circle).Elements.ToList();
            if (intersection.Count == 0) return new Sequence(new List<Type>() { new Undefined() });
            if (intersection.Count == 1 && intersection[0] is not Undefined)
            {
                Point point = (Point)intersection[0];
                bool judge = PointOnArc(point, arc);
                if (judge == true) return new Sequence(new List<Type>() { point });
                else return new Sequence(new List<Type>() { new Undefined() });
            }
            if (intersection.Count == 2)
            {
                Point point1 = (Point)intersection[0];
                Point point2 = (Point)intersection[1];
                bool judge1 = PointOnArc(point1, arc);
                bool judge2 = PointOnArc(point2, arc);
                if (judge1 == true && judge2 == true) return new Sequence(new List<Type>() { point1, point2 });
                if (judge1 == true) return new Sequence(new List<Type>() { point1 });
                if (judge2 == true) return new Sequence(new List<Type>() { point2 });
                return new Sequence(new List<Type>() { new Undefined() });
            }
            return new Sequence(new List<Type>() { new Undefined() });
        }
        internal static Sequence IntersectArcAndArc(Type f1, Type f2)
        {
            Arc arc1 = (Arc)f1;
            Arc arc2 = (Arc)f2;
            Circle circle1 = new(arc1.Center, arc1.Measure);
            Circle circle2 = new(arc2.Center, arc2.Measure);
            List<Type> intersection = IntersectTwoCircles(circle1, circle2).Elements.ToList();
            if (intersection[0] is Undefined) return new Sequence(new List<Type>() { new Undefined() });
            if (intersection.Count == 1)
            {
                Point point = (Point)intersection[0];
                bool judge = PointOnArc(point, arc1);
                bool judge1 = PointOnArc(point, arc2);
                if (judge == true && judge1 == true) return new Sequence(new List<Type>() { point });
                else return new Sequence(new List<Type>() { new Undefined() });
            }
            else if (intersection.Count == 2)
            {
                Point point1 = (Point)intersection[0];
                Point point2 = (Point)intersection[1];
                bool judge1 = PointOnArc(point1, arc1);
                bool judge2 = PointOnArc(point2, arc1);
                bool judge3 = PointOnArc(point1, arc2);
                bool judge4 = PointOnArc(point2, arc2);
                if (judge1 == true && judge3 == true) return new Sequence(new List<Type>() { point1 });
                else if (judge2 == true && judge4 == true) return new Sequence(new List<Type>() { point2 });
                else return new Sequence(new List<Type>() { new Undefined() });
            }
            else return new Sequence(new List<Type>() { new Undefined() });
        }
        internal static Sequence IntersectRayAndLine(Type f1, Type f2)
        {
            (Line line, Ray ray) = Utils.OrderByType<Line, Ray>(f1, f2);
            Line line1 = new(ray.Start, ray.End);
            List<Type> intersection = IntersectTwoLines(line, line1).Elements.ToList();
            if (intersection[0] is Undefined) return new Sequence(new List<Type>() { new Undefined() });
            else
            {
                Point intersection1 = (Point)intersection[0];
                if (ray.Start.X < ray.End.X && intersection1.X >= ray.Start.X) return new Sequence(new List<Type>() { intersection1 });
                if (ray.Start.X > ray.End.X && intersection1.X <= ray.Start.X) return new Sequence(new List<Type>() { intersection1 });
                else return new Sequence(new List<Type>() { new Undefined() });
            }
        }
        internal static Sequence IntersectRayAndSegment(Type f1, Type f2)
        {
            (Segment segment, Ray ray) = Utils.OrderByType<Segment, Ray>(f1, f2);
            Line line1 = new(ray.Start, ray.End);
            Line line2 = new(segment.Start, segment.End);
            List<Type> intersection = IntersectTwoLines(line2, line1).Elements.ToList();
            if (intersection[0] is Undefined) return new Sequence(new List<Type>() { new Undefined() });
            else
            {
                Point intersection1 = (Point)intersection[0];
                if (ray.Start.X < ray.End.X && intersection1.X >= ray.Start.X && IsPointOnSegment(intersection1, segment)) return new Sequence(new List<Type>() { intersection1 });
                else if (ray.Start.X > ray.End.X && intersection1.X <= ray.Start.X && IsPointOnSegment(intersection1, segment)) return new Sequence(new List<Type>() { intersection1 });
                else return new Sequence(new List<Type>() { new Undefined() });
            }
        }
        internal static Sequence IntersectRayAndCircle(Type f1, Type f2)
        {
            (Circle circle, Ray ray) = Utils.OrderByType<Circle, Ray>(f1, f2);
            Line line = new(ray.Start, ray.End);
            List<Type> intersection = IntersectLineAndCircle(line, circle).Elements.ToList();
            if (intersection[0] is Undefined) return new Sequence(new List<Type>() { new Undefined() });
            if (intersection.Count == 1)
            {
                Point intersect = (Point)intersection[0];
                if (IsPointOnRay(intersect, ray)) return new Sequence(new List<Type>() { intersect });
                else return new Sequence(new List<Type>() { new Undefined() });
            }
            if (intersection.Count == 2)
            {
                Point intersect1 = (Point)intersection[0];
                Point intersect2 = (Point)intersection[1];
                if (IsPointOnRay(intersect1, ray) && IsPointOnRay(intersect2, ray)) return new Sequence(new List<Type>() { intersect1, intersect2 });
                if (IsPointOnRay(intersect1, ray)) return new Sequence(new List<Type>() { intersect1 });
                if (IsPointOnRay(intersect2, ray)) return new Sequence(new List<Type>() { intersect2 });
                return new Sequence(new List<Type>() { new Undefined() });
            }
            return new Sequence(new List<Type>() { new Undefined() });
        }
        internal static Sequence IntersectRayAndArc(Type f1, Type f2)
        {
            (Arc arc, Ray ray) = Utils.OrderByType<Arc, Ray>(f1, f2);
            Line line = new(ray.Start, ray.End);
            List<Type> intersection = IntersectLineAndArc(arc, line).Elements.ToList();
            if (intersection[0] is Undefined) return new Sequence(new List<Type>() { new Undefined() });
            if (intersection.Count == 1)
            {
                Point intersect = (Point)intersection[0];
                if (IsPointOnRay(intersect, ray) && PointOnArc(intersect, arc)) return new Sequence(new List<Type>() { intersect });
                else return new Sequence(new List<Type>() { new Undefined() });
            }
            if (intersection.Count == 2)
            {
                Point intersect1 = (Point)intersection[0];
                Point intersect2 = (Point)intersection[1];
                if (IsPointOnRay(intersect1, ray) && IsPointOnRay(intersect2, ray) && PointOnArc(intersect1, arc) && PointOnArc(intersect2, arc)) return new Sequence(new List<Type>() { intersect1, intersect2 });
                if (IsPointOnRay(intersect1, ray) && PointOnArc(intersect1, arc)) return new Sequence(new List<Type>() { intersect1 });
                if (IsPointOnRay(intersect2, ray) && PointOnArc(intersect2, arc)) return new Sequence(new List<Type>() { intersect2 });
                else return new Sequence(new List<Type>() { new Undefined() });
            }
            else return new Sequence(new List<Type>() { new Undefined() });
        }
        internal static Sequence IntersectRayAndRay(Type f1, Type f2)
        {
            Ray ray1 = (Ray)f1;
            Ray ray2 = (Ray)f2;
            Line line1 = new(ray1.Start, ray1.End);
            Line line2 = new(ray2.Start, ray2.End);
            List<Type> intersection = IntersectTwoLines(line1, line2).Elements.ToList();
            if (intersection[0] is Undefined) return new Sequence(new List<Type>() { new Undefined() });
            else
            {
                Point intersection1 = (Point)intersection[0];
                if (IsPointOnRay(intersection1, ray1) && IsPointOnRay(intersection1, ray2)) return new Sequence(new List<Type>() { intersection1 });
                else return new Sequence(new List<Type>() { new Undefined() });
            }
        }
        internal static bool PointOnArc(Point point, Arc arc)
        {
            // Convierte los puntos a vectores
            var vectorStart = new { X = arc.Extremo1.X - arc.Center.X, Y = arc.Extremo1.Y - arc.Center.Y };
            var vectorEnd = new { X = arc.Extremo2.X - arc.Center.X, Y = arc.Extremo2.Y - arc.Center.Y };
            var vectorPoint = new { X = point.X - arc.Center.X, Y = point.Y - arc.Center.Y };

            // Calcula los productos cruzados
            var crossProductStart = vectorStart.X * vectorPoint.Y - vectorPoint.X * vectorStart.Y;
            var crossProductEnd = vectorEnd.X * vectorPoint.Y - vectorPoint.X * vectorEnd.Y;

            // Comprueba si el arco se define en sentido horario o antihorario
            bool isClockwise = vectorStart.X * vectorEnd.Y - vectorEnd.X * vectorStart.Y < 0;

            // Comprueba si el punto está en el arco
            // Si el arco está en sentido horario, el producto cruzado de inicio debe ser menor o igual a cero y el producto cruzado de fin mayor o igual a cero
            // Si el arco está en sentido antihorario, el producto cruzado de inicio debe ser mayor o igual a cero y el producto cruzado de fin menor o igual a cero
            if (isClockwise) return crossProductStart <= 0 && crossProductEnd >= 0;
            return crossProductStart >= 0 && crossProductEnd <= 0;
        }
        public static double Distance(this Point point1, Point point2) => Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2));
        public static bool IsPointOnSegment(Point point, Segment segment)
        {
            double minX = Math.Min(segment.Start.X, segment.End.X);
            double maxX = Math.Max(segment.Start.X, segment.End.X);
            double minY = Math.Min(segment.Start.Y, segment.End.Y);
            double maxY = Math.Max(segment.Start.Y, segment.End.Y);

            return point.X >= minX && point.X <= maxX && point.Y >= minY && point.Y <= maxY;
        }
        public static bool IsPointOnRay(Point point, Ray ray)
        {
            return ray.Start.X < ray.End.X && point.X >= ray.Start.X || ray.Start.X > ray.End.X && point.X <= ray.Start.X;
        }
    }
}
