namespace GeoWall_E
{
    public static class IntersectFigures
    {
        internal static Sequence IntersectPointAndRay(Type f1, Type f2)
        {
            (Point point, Ray ray) = Utils.PointAndRayOrdered(f1, f2);

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

            double r1 = c1.Radius.Value;
            double r2 = c2.Radius.Value;

            double a = (r1 * r1 - r2 * r2 + d * d) / (2 * d);
            double h = Math.Sqrt(r1 * r1 - a * a);

            double x2 = c1.Center.X + a * (c2.Center.X - c1.Center.X) / d;
            double y2 = c1.Center.Y + a * (c2.Center.Y - c1.Center.Y) / d;

            double x3 = x2 + h * (c2.Center.Y - c1.Center.Y) / d;
            double y3 = y2 - h * (c2.Center.X - c1.Center.X) / d;

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
            (Circle circle, Point point) = Utils.CircleAndPointOrdered(f1, f2);

            double d = Math.Sqrt(Math.Pow(point.X - circle.Center.X, 2) + Math.Pow(point.Y - circle.Center.Y, 2));

            if (d == circle.Radius.Value) return new Sequence(new List<Type>() { point });

            return new Sequence(new List<Type>() { new Undefined() });
        }

        internal static Sequence IntersectSegmentAndPoint(Type f1, Type f2)
        {
            (Segment segment, Point point) = Utils.SegmentAndPointOrdered(f1, f2);

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
            Point point;
            Line line;

            (line, point) = Utils.LineAndPointOrdered(f1, f2);

            // Si la pendiente de la recta es infinita
            if (line.P1.X == line.P2.X) line.P2.AsignX(line.P2.X + 1);

            var m = (line.P2.Y - line.P1.Y) / (line.P2.X - line.P1.X);
            var b = line.P1.Y - m * line.P1.X;

            // Calcular la coordenada Y del punto en la recta
            var y = m * point.X + b;

            // Si la coordenada Y del punto es igual a la coordenada Y del punto en la recta, hay interseccion
            if (y == point.Y) return new Sequence(new List<Type>() { point });
            else return new Sequence(new List<Type>() { new Undefined() });
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
            Arc arc;
            Circle circle;

            (circle, arc) = Utils.CircleAndArcOrdered(f1, f2);

            // Calcular la distancia entre el centro del circulo y el centro del arco
            var distance = Math.Sqrt(Math.Pow(arc.Center.X - circle.Center.X, 2) + Math.Pow(arc.Center.Y - circle.Center.Y, 2));

            // Si la distancia es mayor que la suma de los radios, no hay interseccion
            if (distance > circle.Radius.Value + arc.Measure.Value) return new Sequence(new List<Type>() { new Undefined() });


            // Si la distancia es menor que la diferencia absoluta de los radios, un circulo esta dentro del otro
            if (distance < Math.Abs(circle.Radius.Value - arc.Measure.Value)) return new Sequence(new List<Type>() { new Undefined() });

            // Calcular los puntos de interseccion
            var a = (arc.Measure.Value * arc.Measure.Value - circle.Radius.Value * circle.Radius.Value + distance * distance) / (2 * distance);
            var h = Math.Sqrt(arc.Measure.Value * arc.Measure.Value - a * a);

            var x1 = arc.Center.X + a * (circle.Center.X - arc.Center.X) / distance;
            var y1 = arc.Center.Y + a * (circle.Center.Y - arc.Center.Y) / distance;

            var x2 = x1 + h * (circle.Center.Y - arc.Center.Y) / distance;
            var y2 = y1 - h * (circle.Center.X - arc.Center.X) / distance;

            var x3 = x1 - h * (circle.Center.Y - arc.Center.Y) / distance;
            var y3 = y1 + h * (circle.Center.X - arc.Center.X) / distance;

            var distanceStartToEnd = Math.Sqrt(Math.Pow(arc.Start.X - arc.End.X, 2) + Math.Pow(arc.Start.Y - arc.End.Y, 2));
            var distanceStartToX2Y2 = Math.Sqrt(Math.Pow(arc.Start.X - x2, 2) + Math.Pow(arc.Start.Y - y2, 2));
            var distanceEndToX2Y2 = Math.Sqrt(Math.Pow(arc.End.X - x2, 2) + Math.Pow(arc.End.Y - y2, 2));
            var distanceStartToX3Y3 = Math.Sqrt(Math.Pow(arc.Start.X - x3, 2) + Math.Pow(arc.Start.Y - y3, 2));
            var distanceEndToX3Y3 = Math.Sqrt(Math.Pow(arc.End.X - x3, 2) + Math.Pow(arc.End.Y - y3, 2));
            double cosTheta = (arc.Measure.Value * arc.Measure.Value + arc.Measure.Value * arc.Measure.Value - distanceStartToEnd * distanceStartToEnd) / (2 * arc.Measure.Value * arc.Measure.Value);
            // Usa la función arccos para obtener el ángulo en radianes
            double theta = Math.Acos(cosTheta);
            double arcLength = arc.Measure.Value * theta;
            double tolerance = 15;

            // If they are, the intersection points are on the arc
            if (Math.Abs(distanceEndToX2Y2 + distanceStartToX2Y2 - arcLength) <= tolerance && Math.Abs(distanceEndToX3Y3 + distanceStartToX3Y3 - arcLength) <= tolerance)
            {
                Point point1 = new();
                point1.AsignX(x2);
                point1.AsignY(y2);

                Point point2 = new();
                point2.AsignX(x3);
                point2.AsignY(y3);

                return new Sequence(new List<Type>() { point1, point2 });
            }
            else if (Math.Abs(distanceEndToX2Y2 + distanceStartToX2Y2 - arcLength) <= tolerance)
            {
                Point point1 = new();
                point1.AsignX(x2);
                point1.AsignY(y2);

                return new Sequence(new List<Type>() { point1 });
            }
            else if (Math.Abs(distanceEndToX3Y3 + distanceStartToX3Y3 - arcLength) <= tolerance)
            {
                Point point2 = new();
                point2.AsignX(x3);
                point2.AsignY(y3);

                return new Sequence(new List<Type>() { point2 });
            }
            else return new Sequence(new List<Type>() { new Undefined() });
        }

        internal static Sequence IntersectCircleAndSegment(Type f1, Type f2)
        {
            Circle circle;
            Segment segment;

            (segment, circle) = Utils.SegmentAndCircleOrdered(f1, f2);

            (var m, var b) = Utils.SlopeAndEquation(segment.Start, segment.End);

            var a = 1 + m * m;
            var b1 = -2 * circle.Center.X + 2 * m * b - 2 * m * circle.Center.Y;
            var c = circle.Center.X * circle.Center.X + b * b + circle.Center.Y * circle.Center.Y - 2 * b * circle.Center.Y - circle.Radius.Value * circle.Radius.Value;

            var discriminant = b1 * b1 - 4 * a * c;

            if (discriminant < 0) return new Sequence(new List<Type>() { new Undefined() });
            else
            {
                var x1 = (-b1 + Math.Sqrt(discriminant)) / (2 * a);
                var x2 = (-b1 - Math.Sqrt(discriminant)) / (2 * a);

                var y1 = m * x1 + b;
                var y2 = m * x2 + b;

                Point point1 = new();
                point1.AsignX(x1);
                point1.AsignY(y1);

                Point point2 = new();
                point2.AsignX(x2);
                point2.AsignY(y2);

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
                else if (x1 >= Math.Min(segment.Start.X, segment.End.X) &&
                    x1 <= Math.Max(segment.Start.X, segment.End.X) &&
                    y1 >= Math.Min(segment.Start.Y, segment.End.Y) &&
                    y1 <= Math.Max(segment.Start.Y, segment.End.Y))
                {
                    return new Sequence(new List<Type>() { point1 });
                }
                else if (x2 >= Math.Min(segment.Start.X, segment.End.X) &&
                    x2 <= Math.Max(segment.Start.X, segment.End.X) &&
                    y2 >= Math.Min(segment.Start.Y, segment.End.Y) &&
                    y2 <= Math.Max(segment.Start.Y, segment.End.Y))
                {
                    return new Sequence(new List<Type>() { point2 });
                }
                else return new Sequence(new List<Type>() { new Undefined() });
            }
        }

        internal static Sequence IntersectLineAndSegment(Type f1, Type f2)
        {
            Line line;
            Segment segment;

            (line, segment) = Utils.LineAndSegmentOrdered(f1, f2);

            if (line.P1.X == line.P2.X) line.P2.AsignX(line.P2.X + 1);

            (var m1, var b1) = Utils.SlopeAndEquation(line.P1, line.P2);

            (var m2, var b2) = Utils.SlopeAndEquation(segment.Start, segment.End);

            if (m1 == m2) return new Sequence(new List<Type>() { new Undefined() });

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
            Line line;
            Circle circle;
            (line, circle) = Utils.LineAndCircleOrdered(f1, f2);

            (var m, var b) = Utils.SlopeAndEquation(line.P1, line.P2);

            var a = 1 + m * m;
            var b1 = -2 * circle.Center.X + 2 * m * b - 2 * m * circle.Center.Y;
            var c = circle.Center.X * circle.Center.X + b * b + circle.Center.Y * circle.Center.Y - 2 * b * circle.Center.Y - circle.Radius.Value * circle.Radius.Value;

            var discriminant = b1 * b1 - 4 * a * c;

            if (discriminant < 0) return new Sequence(new List<Type>() { new Undefined() });
            else
            {
                var x1 = (-b1 + Math.Sqrt(discriminant)) / (2 * a);
                var x2 = (-b1 - Math.Sqrt(discriminant)) / (2 * a);

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

            if (m1 == m2) return new Sequence(new List<Type>() { new Undefined });

            var x = (b2 - b1) / (m1 - m2);
            var y = m1 * x + b1;

            Point point = new();
            point.AsignX(x);
            point.AsignY(y);
            return new Sequence(new List<Type>() { point });
        }
    }
}