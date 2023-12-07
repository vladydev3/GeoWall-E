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
            Point point1 = new();
            point1.AsignX(x2);
            point1.AsignY(y2);
            bool judge1 = PointOnArc(point1, arc);
            Point point2 = new();
            point2.AsignX(x3);
            point2.AsignY(y3);
            bool judge2 = PointOnArc(point2, arc);
           // If they are, the intersection points are on the arc
            if (judge1==true&&judge2==true)
            {
                return new Sequence(new List<Type>() { point1, point2 });
            }
            else if (judge1==true)
            {
              
                return new Sequence(new List<Type>() { point1 });
            }
            else if (judge2==true)
            {
                

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
            Line line;
            Arc arc;
            (line, arc) = Utils.LineAndArcOrdered(f1, f2);
            Circle circle = new Circle(arc.Center, arc.Measure);
            IEnumerable<Type> intersection = IntersectLineAndCircle(line, circle).Elements;
            List<Type> intersectionList = intersection.ToList();
            if (intersectionList.Count() == 0)
            {
                return new Sequence(new List<Type>() { new Undefined() });
            }
            else if (intersectionList.Count() == 1 && intersectionList[0] is not Undefined)
            {
                Point point = (Point)intersectionList[0];
                bool judge = PointOnArc(point, arc);
                if (judge == true)
                {
                    return new Sequence(new List<Type>() { point });
                }
                else
                {
                    return new Sequence(new List<Type>() { new Undefined() });
                }
            }
            else if (intersectionList.Count() == 2)
            {
                Point point1 = (Point)intersectionList[0];
                Point point2 = (Point)intersectionList[1];
                bool judge1 = PointOnArc(point1, arc);
                bool judge2 = PointOnArc(point2, arc);
                if (judge1 == true && judge2 == true)
                {
                    return new Sequence(new List<Type>() { point1, point2 });
                }
                else if (judge1 == true)
                {
                    return new Sequence(new List<Type>() { point1 });
                }
                else if (judge2 == true)
                {
                    return new Sequence(new List<Type>() { point2 });
                }
                else
                {
                    return new Sequence(new List<Type>() { new Undefined() });
                }
            }
            else
            {
                return new Sequence(new List<Type>() { new Undefined() });
            }
        }
        internal static Sequence IntersectSegmentAndArc(Type f1, Type f2) 
        {
            Segment segment;
            Arc arc;
            (segment, arc) = Utils.SegmentAndArcOrdered(f1, f2);
            Circle circle = new Circle(arc.Center, arc.Measure);
            IEnumerable<Type> intersection = IntersectCircleAndSegment(segment, circle).Elements;
            List<Type> intersectionList = intersection.ToList();
            if (intersectionList.Count() == 0)
            {
                return new Sequence(new List<Type>() { new Undefined() });
            }
            else if (intersectionList.Count() == 1 && intersectionList[0] is not Undefined)
            {
                Point point = (Point)intersectionList[0];
                bool judge = PointOnArc(point, arc);
                if (judge == true)
                {
                    return new Sequence(new List<Type>() { point });
                }
                else
                {
                    return new Sequence(new List<Type>() { new Undefined() });
                }
            }
            else if (intersectionList.Count() == 2)
            {
                Point point1 = (Point)intersectionList[0];
                Point point2 = (Point)intersectionList[1];
                bool judge1 = PointOnArc(point1, arc);
                bool judge2 = PointOnArc(point2, arc);
                if (judge1 == true && judge2 == true)
                {
                    return new Sequence(new List<Type>() { point1, point2 });
                }
                else if (judge1 == true)
                {
                    return new Sequence(new List<Type>() { point1 });
                }
                else if (judge2 == true)
                {
                    return new Sequence(new List<Type>() { point2 });
                }
                else
                {
                    return new Sequence(new List<Type>() { new Undefined() });
                }
            }
            else
            {
                return new Sequence(new List<Type>() { new Undefined() });
            }
        }
        internal static Sequence IntersectArcAndArc(Type f1, Type f2) 
        {
            Arc arc1= (Arc)f1;
            Arc arc2= (Arc)f2;
            Circle circle1 = new Circle(arc1.Center, arc1.Measure);
            Circle circle2 = new Circle(arc2.Center, arc2.Measure);
            IEnumerable<Type> intersection = IntersectTwoCircles(circle1, circle2).Elements;
            List<Type> intersectionList = intersection.ToList();
            if (intersectionList[0] is  Undefined)
            {
                return new Sequence(new List<Type>() { new Undefined() });
            }
            else if (intersectionList.Count() == 1)
            {
                Point point = (Point)intersectionList[0];
                bool judge = PointOnArc(point, arc1);
                bool judge1=PointOnArc(point, arc2);
                if (judge == true&&judge1==true)
                {
                    return new Sequence(new List<Type>() { point });
                }
           
                else
                {
                    return new Sequence(new List<Type>() { new Undefined() });
                }
            }
            else if (intersectionList.Count() == 2)
            {
                Point point1 = (Point)intersectionList[0];
                Point point2 = (Point)intersectionList[1];
                bool judge1 = PointOnArc(point1, arc1);
                bool judge2 = PointOnArc(point2, arc1);
                bool judge3=PointOnArc(point1, arc2);
                bool judge4=PointOnArc(point2, arc2);
                if (judge1 == true&&judge3==true)
                {
                    return new Sequence(new List<Type>() { point1 });
                }
                else if (judge2 == true&&judge4==true)
                {
                    return new Sequence(new List<Type>() { point2 });
                }
                else
                {
                    return new Sequence(new List<Type>() { new Undefined() });
                }
            }
            else
            {
                return new Sequence(new List<Type>() { new Undefined() });
            }

        }
        internal static Sequence IntersectRayAndLine(Type f1, Type f2)
        {
            Line line;
            Ray ray;
            (line,ray ) = Utils.LineAndRayOrdered(f1, f2);
            Line line1= new Line(ray.Start,ray.End);
            IEnumerable<Type> intersection = IntersectTwoLines(line, line1).Elements;
            List<Type> intersectionList = intersection.ToList();
            if (intersectionList[0] is Undefined)
            {
                return new Sequence(new List<Type>() { new Undefined() });
            }
            else
            {
                Point intersection1 = (Point)intersectionList[0];
                if (ray.Start.X<ray.End.X && intersection1.X>=ray.Start.X)
                {
                    return new Sequence(new List<Type>() { intersection1 });
                }
                else if (ray.Start.X > ray.End.X && intersection1.X <= ray.Start.X)
                {
                    return new Sequence(new List<Type>() { intersection1 });
                }
                else
                {
                    return new Sequence(new List<Type>() { new Undefined() });
                }
               
            }
        }
        internal static Sequence IntersectRayAndSegment(Type f1, Type f2)
        {
            Segment segment;
            Ray ray;
            (segment, ray) = Utils.SegmentAndRayOrdered(f1, f2);
            Line line1 = new Line(ray.Start, ray.End);
            Line line2= new Line(segment.Start, segment.End);
            IEnumerable<Type> intersection = IntersectTwoLines(line2, line1).Elements;
            List<Type> intersectionList = intersection.ToList();
            if (intersectionList[0] is Undefined)
            {
                return new Sequence(new List<Type>() { new Undefined() });
            }
            else
            {
                Point intersection1 = (Point)intersectionList[0];
                if (ray.Start.X < ray.End.X && intersection1.X >= ray.Start.X&& IsPointOnSegment(intersection1,segment))
                {
                    return new Sequence(new List<Type>() { intersection1 });
                }
                else if (ray.Start.X > ray.End.X && intersection1.X <= ray.Start.X&& IsPointOnSegment(intersection1, segment))
                {
                    return new Sequence(new List<Type>() { intersection1 });
                }
                else
                {
                    return new Sequence(new List<Type>() { new Undefined() });
                }

            }         
        }
        internal static Sequence IntersectRayAndCircle(Type f1, Type f2)
        {
            Circle circle;
            Ray ray;
            (circle, ray) = Utils.CircleAndRayOrdered(f1, f2);
            Line line=new Line(ray.Start, ray.End);
            IEnumerable<Type> intersection = IntersectLineAndCircle(line,circle).Elements;
            List<Type> intersectionList = intersection.ToList();
            if (intersectionList[0] is Undefined)
            {
                return new Sequence(new List<Type>() { new Undefined() });
            }
            else if (intersectionList.Count==1)
            {
                Point intersect = (Point)intersectionList[0];
                if (IsPointOnRay(intersect,ray))
                {
                    return new Sequence(new List<Type>() { intersect });
                }
                else
                {
                    return new Sequence(new List<Type>() { new Undefined() });
                }
            }
            else if (intersectionList.Count == 2)
            {
                Point intersect1 = (Point)intersectionList[0];
                Point intersect2= (Point)intersectionList[1];
                if (IsPointOnRay(intersect1,ray) && IsPointOnRay(intersect2,ray))
                {
                    return new Sequence(new List<Type>() { intersect1,intersect2 });
                }
                if (IsPointOnRay(intersect1, ray))
                {
                    return new Sequence(new List<Type>() { intersect1 });
                }
                if (IsPointOnRay(intersect2, ray))
                {
                    return new Sequence(new List<Type>() { intersect2 });
                }
                else
                {
                    return new Sequence(new List<Type>() { new Undefined() });
                }
            }
            else
            {
                return new Sequence(new List<Type>() { new Undefined() });
            }
        }
        internal static Sequence IntersectRayAndArc(Type f1, Type f2)
        {
            Ray ray;
            Arc arc;
            (arc, ray) = Utils.ArcAndRayOrdered(f1, f2);
            Line line = new Line(ray.Start, ray.End);
            IEnumerable<Type> intersection = IntersectLineAndArc(arc, line).Elements;
            List<Type> intersectionList = intersection.ToList();
            if (intersectionList[0] is Undefined)
            {
                return new Sequence(new List<Type>() { new Undefined() });
            }
            else if (intersectionList.Count == 1)
            {
                Point intersect = (Point)intersectionList[0];
                if (IsPointOnRay(intersect, ray) && PointOnArc(intersect, arc))
                {
                    return new Sequence(new List<Type>() { intersect });
                }
                else
                {
                    return new Sequence(new List<Type>() { new Undefined() });
                }
            }
            else if (intersectionList.Count == 2)
            {
                Point intersect1 = (Point)intersectionList[0];
                Point intersect2 = (Point)intersectionList[1];
                if (IsPointOnRay(intersect1, ray) && IsPointOnRay(intersect2, ray)&&PointOnArc(intersect1,arc)&&PointOnArc(intersect2,arc))
                {
                    return new Sequence(new List<Type>() { intersect1, intersect2 });
                }
                if (IsPointOnRay(intersect1, ray) && PointOnArc(intersect1, arc))
                {
                    return new Sequence(new List<Type>() { intersect1 });
                }
                if (IsPointOnRay(intersect2, ray) && PointOnArc(intersect2, arc))
                {
                    return new Sequence(new List<Type>() { intersect2 });
                }
                else
                {
                    return new Sequence(new List<Type>() { new Undefined() });
                }
            }
            else
            {
                return new Sequence(new List<Type>() { new Undefined() });
            }

        }
        internal static Sequence IntersectRayAndRay(Type f1, Type f2)
        {

            Ray ray1=(Ray)f1;
            Ray ray2= (Ray)f2;
            Line line1 = new Line(ray1.Start, ray1.End);
            Line line2 = new Line(ray2.Start, ray2.End);
            IEnumerable<Type> intersection = IntersectTwoLines(line1, line2).Elements;
            List<Type> intersectionList = intersection.ToList();
            if (intersectionList[0] is Undefined)
            {
                return new Sequence(new List<Type>() { new Undefined() });
            }
            else
            {
                Point intersection1 = (Point)intersectionList[0];
                if (IsPointOnRay(intersection1,ray1)&&IsPointOnRay(intersection1,ray2))
                {
                    return new Sequence(new List<Type>() { intersection1 });
                }
                else
                {
                    return new Sequence(new List<Type>() { new Undefined() });
                }

            }
        }
        internal static bool PointOnArc(Point point, Arc arc)
        {// Convierte los puntos a vectores
            var vectorStart = new { X = arc.Extremo1.X - arc.Center.X, Y = arc.Extremo1.Y - arc.Center.Y };
            var vectorEnd = new { X = arc.Extremo2.X - arc.Center.X, Y = arc.Extremo2.Y - arc.Center.Y };
            var vectorPoint = new { X = point.X - arc.Center.X, Y = point.Y - arc.Center.Y };

            // Calcula los productos cruzados
            var crossProductStart = vectorStart.X * vectorPoint.Y - vectorPoint.X * vectorStart.Y;
            var crossProductEnd = vectorEnd.X * vectorPoint.Y - vectorPoint.X * vectorEnd.Y;

            // Comprueba si el arco se define en sentido horario o antihorario
            bool isClockwise = vectorStart.X * vectorEnd.Y - vectorEnd.X * vectorStart.Y < 0;

            // Comprueba si el punto está en el arco
            if (isClockwise)
            {
                return crossProductStart <= 0 && crossProductEnd >= 0;
            }
            else
            {
                return crossProductStart >= 0 && crossProductEnd <= 0;
            }
        }
        public static double Distance(this Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2));
        }
        public static  bool IsPointOnSegment(Point point, Segment segment)
        {
            double minX = Math.Min(segment.Start.X, segment.End.X);
            double maxX = Math.Max(segment.Start.X, segment.End.X);
            double minY = Math.Min(segment.Start.Y, segment.End.Y);
            double maxY = Math.Max(segment.Start.Y, segment.End.Y);

            if (point.X >= minX && point.X <= maxX && point.Y >= minY && point.Y <= maxY)
                return true;

            return false;
        }
        public static bool IsPointOnRay(Point point, Ray ray) 
        {
            if (ray.Start.X < ray.End.X && point.X >= ray.Start.X)
            {
                return true;
            }
            else if (ray.Start.X > ray.End.X && point.X <= ray.Start.X )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
 }
