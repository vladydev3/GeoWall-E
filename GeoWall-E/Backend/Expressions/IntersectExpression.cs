namespace GeoWall_E
{
    public class IntersectExpression : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.Intersect;
        Expression F1_ { get; set; }
        Expression F2_ { get; set; }
        readonly Dictionary<string, Tuple<int, int>> Positions_;

        public IntersectExpression(Expression f1, Expression f2, Dictionary<string, Tuple<int, int>> positions)
        {
            F1_ = f1;
            F2_ = f2;
            Positions_ = positions;
        }

        public Expression F1 => F1_;
        public Expression F2 => F2_;
        public Dictionary<string, Tuple<int, int>> Positions => Positions_;

        public Type Evaluate(SymbolTable symbolTable, Error error)
        {
            if (F1 is not IEvaluable f1 || F2 is not IEvaluable f2)
            {
                error.AddError("SEMANTIC ERROR: Can't intersect"); // TODO: Improve this message error
                return new ErrorType();
            }
            var fig1 = f1.Evaluate(symbolTable, error);
            var fig2 = f2.Evaluate(symbolTable, error);
            // Calcular la interseccion
            if (fig1 as Line != null && fig2 as Line != null)
            {
                var line1 = (Line)fig1;
                var line2 = (Line)fig2;

                try
                {
                    var m1 = (line1.P2.Y - line1.P1.Y) / (line1.P2.X - line1.P1.X);
                    var m2 = (line2.P2.Y - line2.P1.Y) / (line2.P2.X - line2.P1.X);
                    var b1 = line1.P1.Y - m1 * line1.P1.X;
                    var b2 = line2.P1.Y - m2 * line2.P1.X;

                    var x = (b2 - b1) / (m1 - m2);
                    var y = m1 * x + b1;
                    // comprobar si el punto esta dentro de los segmentos
                    if (x >= Math.Min(line1.P1.X, line1.P2.X) && x <= Math.Max(line1.P1.X, line1.P2.X) &&
                        x >= Math.Min(line2.P1.X, line2.P2.X) && x <= Math.Max(line2.P1.X, line2.P2.X) &&
                        y >= Math.Min(line1.P1.Y, line1.P2.Y) && y <= Math.Max(line1.P1.Y, line1.P2.Y) &&
                        y >= Math.Min(line2.P1.Y, line2.P2.Y) && y <= Math.Max(line2.P1.Y, line2.P2.Y))
                    {
                        Point point = new();
                        point.AsignX(x);
                        point.AsignY(y);
                        return new Sequence(new List<Type> { point });
                    }
                    else
                    {
                        return new Sequence(new List<Type> { new Undefined() });
                    }
                }
                // Si las pendientes son iguales o las X de los puntos coinciden de cada recta coinciden
                catch (DivideByZeroException)
                {
                    return new Sequence(new List<Type> { new Undefined() });
                }

            }

            return new ErrorType();
        }

        public void HandleIntersectExpression(SymbolTable symbolTable, Error errors, AsignationStatement asignation)
        {
            if (F1 as IEvaluable != null && F2 as IEvaluable != null)
            {
                var f1 = ((IEvaluable)F1).Evaluate(symbolTable, errors);
                var f2 = ((IEvaluable)F2).Evaluate(symbolTable, errors);
                if (f1 is not ErrorType && f2 is not ErrorType)
                {
                    // Intersect between two lines
                    if (f1.ObjectType == ObjectTypes.Line && f2.ObjectType == ObjectTypes.Line)
                    {
                        var line1 = (Line)f1;
                        var line2 = (Line)f2;

                        var m1 = (line1.P2.Y - line1.P1.Y) / (line1.P2.X - line1.P1.X);
                        var m2 = (line2.P2.Y - line2.P1.Y) / (line2.P2.X - line2.P1.X);

                        var b1 = line1.P1.Y - m1 * line1.P1.X;
                        var b2 = line2.P1.Y - m2 * line2.P1.X;

                        if (m1 == m2)
                        {
                            symbolTable.Define(asignation.Name.Text, new Undefined());
                            return;
                        }

                        var x = (b2 - b1) / (m1 - m2);
                        var y = m1 * x + b1;

                        Point point = new();
                        point.AsignX(x);
                        point.AsignY(y);
                        symbolTable.Define(asignation.Name.Text, point);
                        return;
                    }

                    // Intersect between a line and a circle
                    if (f1.ObjectType == ObjectTypes.Line && f2.ObjectType == ObjectTypes.Circle || f1.ObjectType == ObjectTypes.Circle && f2.ObjectType == ObjectTypes.Line)
                    {
                        Line line;
                        Circle circle;
                        (line, circle) = Utils.LineAndCircleOrdered(f1, f2);

                        var m = (line.P2.Y - line.P1.Y) / (line.P2.X - line.P1.X);
                        var b = line.P1.Y - m * line.P1.X;

                        var a = 1 + m * m;
                        var b1 = -2 * circle.Center.X + 2 * m * b - 2 * m * circle.Center.Y;
                        var c = circle.Center.X * circle.Center.X + b * b + circle.Center.Y * circle.Center.Y - 2 * b * circle.Center.Y - circle.Radius.Value * circle.Radius.Value;

                        var discriminant = b1 * b1 - 4 * a * c;

                        if (discriminant < 0)
                        {
                            symbolTable.Define(asignation.Name.Text, new Undefined());
                            return;
                        }
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
                            symbolTable.Define(asignation.Name.Text, new Sequence(new List<Type>() { point1, point2 }));
                            return;

                        }
                    }

                    // Intersect between a line and a segment
                    if (f1.ObjectType == ObjectTypes.Line && f2.ObjectType == ObjectTypes.Segment || f1.ObjectType == ObjectTypes.Segment && f2.ObjectType == ObjectTypes.Line)
                    {
                        Line line;
                        Segment segment;

                        (line, segment) = Utils.LineAndSegmentOrdered(f1, f2);

                        if (line.P1.X == line.P2.X)
                        {
                            line.P2.AsignX(line.P2.X + 1);
                        }

                        var m1 = (line.P2.Y - line.P1.Y) / (line.P2.X - line.P1.X);
                        var b1 = line.P1.Y - m1 * line.P1.X;

                        var m2 = (segment.End.Y - segment.Start.Y) / (segment.End.X - segment.Start.X);
                        var b2 = segment.Start.Y - m2 * segment.Start.X;

                        if (m1 == m2)
                        {
                            symbolTable.Define(asignation.Name.Text, new Undefined());
                            return;
                        }

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
                            symbolTable.Define(asignation.Name.Text, point);
                            return;
                        }
                        else
                        {
                            symbolTable.Define(asignation.Name.Text, new Undefined());
                            return;
                        }
                    }

                    // Intersect between a circle and a segment
                    if (f1.ObjectType == ObjectTypes.Circle && f2.ObjectType == ObjectTypes.Segment || f1.ObjectType == ObjectTypes.Segment && f2.ObjectType == ObjectTypes.Circle)
                    {
                        Circle circle;
                        Segment segment;

                        (segment, circle) = Utils.SegmentAndCircleOrdered(f1, f2);

                        var m = (segment.End.Y - segment.Start.Y) / (segment.End.X - segment.Start.X);
                        var b = segment.Start.Y - m * segment.Start.X;

                        var a = 1 + m * m;
                        var b1 = -2 * circle.Center.X + 2 * m * b - 2 * m * circle.Center.Y;
                        var c = circle.Center.X * circle.Center.X + b * b + circle.Center.Y * circle.Center.Y - 2 * b * circle.Center.Y - circle.Radius.Value * circle.Radius.Value;

                        var discriminant = b1 * b1 - 4 * a * c;

                        if (discriminant < 0)
                        {
                            symbolTable.Define(asignation.Name.Text, new Undefined());
                            return;
                        }
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
                                symbolTable.Define(asignation.Name.Text, new Sequence(new List<Type>() { point1, point2 }));
                                return;
                            }
                            else if (x1 >= Math.Min(segment.Start.X, segment.End.X) &&
                                x1 <= Math.Max(segment.Start.X, segment.End.X) &&
                                y1 >= Math.Min(segment.Start.Y, segment.End.Y) &&
                                y1 <= Math.Max(segment.Start.Y, segment.End.Y))
                            {
                                symbolTable.Define(asignation.Name.Text, new Sequence(new List<Type>() { point1 }));
                                return;
                            }
                            else if (x2 >= Math.Min(segment.Start.X, segment.End.X) &&
                                x2 <= Math.Max(segment.Start.X, segment.End.X) &&
                                y2 >= Math.Min(segment.Start.Y, segment.End.Y) &&
                                y2 <= Math.Max(segment.Start.Y, segment.End.Y))
                            {
                                symbolTable.Define(asignation.Name.Text, new Sequence(new List<Type>() { point2 }));
                                return;
                            }
                            else
                            {
                                symbolTable.Define(asignation.Name.Text, new Undefined());
                                return;
                            }
                        }
                    }

                    // Intersect between two segments
                    if (f1.ObjectType == ObjectTypes.Segment && f2.ObjectType == ObjectTypes.Segment)
                    {
                        Segment segment1 = (Segment)f1;
                        Segment segment2 = (Segment)f2;

                        var m1 = (segment1.End.Y - segment1.Start.Y) / (segment1.End.X - segment1.Start.X);
                        var b1 = segment1.Start.Y - m1 * segment1.Start.X;

                        var m2 = (segment2.End.Y - segment2.Start.Y) / (segment2.End.X - segment2.Start.X);
                        var b2 = segment2.Start.Y - m2 * segment2.Start.X;

                        if (m1 == m2)
                        {
                            symbolTable.Define(asignation.Name.Text, new Undefined());
                            return;
                        }

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
                            symbolTable.Define(asignation.Name.Text, point);
                            return;
                        }
                        else
                        {
                            symbolTable.Define(asignation.Name.Text, new Undefined());
                            return;
                        }
                    }

                    // Intersect between two circles
                    if (f1.ObjectType == ObjectTypes.Circle && f2.ObjectType == ObjectTypes.Circle)
                    {
                        Circle circle1 = (Circle)f1;
                        Circle circle2 = (Circle)f2;

                        // Calcular la distancia entre los centros de los circulos
                        var distance = Math.Sqrt(Math.Pow(circle2.Center.X - circle1.Center.X, 2) + Math.Pow(circle2.Center.Y - circle1.Center.Y, 2));

                        // Si la distancia es mayor que la suma de los radios, no hay interseccion
                        if (distance > circle1.Radius.Value + circle2.Radius.Value)
                        {
                            symbolTable.Define(asignation.Name.Text, new Undefined());
                            return;
                        }

                        // Si la distancia es igual a la suma de los radios, hay una interseccion en un solo punto
                        if (distance == circle1.Radius.Value + circle2.Radius.Value)
                        {
                            // Calcular el punto de interseccion
                            var x = (circle1.Center.X * circle2.Radius.Value + circle2.Center.X * circle1.Radius.Value) / (circle1.Radius.Value + circle2.Radius.Value);
                            var y = (circle1.Center.Y * circle2.Radius.Value + circle2.Center.Y * circle1.Radius.Value) / (circle1.Radius.Value + circle2.Radius.Value);

                            Point point = new();
                            point.AsignX(x);
                            point.AsignY(y);
                            symbolTable.Define(asignation.Name.Text, new Sequence(new List<Type>() { point }));
                            return;
                        }

                        // Si la distancia es menor que la diferencia absoluta de los radios, un circulo esta dentro del otro
                        if (distance < Math.Abs(circle1.Radius.Value - circle2.Radius.Value))
                        {
                            symbolTable.Define(asignation.Name.Text, new Undefined());
                            return;
                        }

                        // Calcular los puntos de interseccion
                        var a = (circle1.Radius.Value * circle1.Radius.Value - circle2.Radius.Value * circle2.Radius.Value + distance * distance) / (2 * distance);
                        var h = Math.Sqrt(circle1.Radius.Value * circle1.Radius.Value - a * a);

                        var x1 = circle1.Center.X + a * (circle2.Center.X - circle1.Center.X) / distance;
                        var y1 = circle1.Center.Y + a * (circle2.Center.Y - circle1.Center.Y) / distance;

                        var x2 = x1 + h * (circle2.Center.Y - circle1.Center.Y) / distance;
                        var y2 = y1 - h * (circle2.Center.X - circle1.Center.X) / distance;

                        var x3 = x1 - h * (circle2.Center.Y - circle1.Center.Y) / distance;
                        var y3 = y1 + h * (circle2.Center.X - circle1.Center.X) / distance;

                        Point point1 = new();
                        point1.AsignX(x2);
                        point1.AsignY(y2);

                        Point point2 = new();
                        point2.AsignX(x3);
                        point2.AsignY(y3);

                        symbolTable.Define(asignation.Name.Text, new Sequence(new List<Type>() { point1, point2 }));
                        return;
                    }

                    // Intersect between two points
                    if (f1.ObjectType == ObjectTypes.Point && f2.ObjectType == ObjectTypes.Point)
                    {
                        var point1 = (Point)f1;
                        var point2 = (Point)f2;

                        if (point1.X == point2.X && point1.Y == point2.Y)
                        {
                            symbolTable.Define(asignation.Name.Text, point1);
                            return;
                        }
                        else
                        {
                            symbolTable.Define(asignation.Name.Text, new Undefined());
                            return;
                        }
                    }

                    // Intersect between a point and a line
                    if (f1.ObjectType == ObjectTypes.Point && f2.ObjectType == ObjectTypes.Line || f1.ObjectType == ObjectTypes.Line && f2.ObjectType == ObjectTypes.Point)
                    {
                        Point point;
                        Line line;

                        (line, point) = Utils.LineAndPointOrdered(f1, f2);

                        // Si la pendiente de la recta es infinita
                        if (line.P1.X == line.P2.X)
                        {
                            line.P2.AsignX(line.P2.X + 1);
                        }

                        var m = (line.P2.Y - line.P1.Y) / (line.P2.X - line.P1.X);
                        var b = line.P1.Y - m * line.P1.X;

                        // Calcular la coordenada Y del punto en la recta
                        var y = m * point.X + b;

                        // Si la coordenada Y del punto es igual a la coordenada Y del punto en la recta, hay interseccion
                        if (y == point.Y)
                        {
                            symbolTable.Define(asignation.Name.Text, point);
                            return;
                        }
                        else
                        {
                            symbolTable.Define(asignation.Name.Text, new Undefined());
                            return;
                        }
                    }

                    // Intersect between a point and a segment
                    if (f1.ObjectType == ObjectTypes.Point && f2.ObjectType == ObjectTypes.Segment || f1.ObjectType == ObjectTypes.Segment && f2.ObjectType == ObjectTypes.Point)
                    {
                        Point point;
                        Segment segment;

                        (segment, point) = Utils.SegmentAndPointOrdered(f1, f2);

                        // Si la pendiente de la recta es infinita
                        if (segment.Start.X == segment.End.X)
                        {
                            segment.End.AsignX(segment.End.X + 1);
                        }

                        var m = (segment.End.Y - segment.Start.Y) / (segment.End.X - segment.Start.X);
                        var b = segment.Start.Y - m * segment.Start.X;

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
                            {
                                symbolTable.Define(asignation.Name.Text, point);
                                return;
                            }
                            else
                            {
                                symbolTable.Define(asignation.Name.Text, new Undefined());
                                return;
                            }
                        }
                        else
                        {
                            symbolTable.Define(asignation.Name.Text, new Undefined());
                            return;
                        }
                    }

                    // Intersect between a point and a circle
                    if (f1.ObjectType == ObjectTypes.Point && f2.ObjectType == ObjectTypes.Circle || f1.ObjectType == ObjectTypes.Circle && f2.ObjectType == ObjectTypes.Point)
                    {
                        Point point;
                        Circle circle;

                        (circle, point) = Utils.CircleAndPointOrdered(f1, f2);

                        // Calcular la distancia entre el punto y el centro del circulo
                        var distance = Math.Sqrt(Math.Pow(point.X - circle.Center.X, 2) + Math.Pow(point.Y - circle.Center.Y, 2));

                        // Si la distancia es igual al radio, hay interseccion
                        if (distance == circle.Radius.Value)
                        {
                            symbolTable.Define(asignation.Name.Text, point);
                            return;
                        }
                        else
                        {
                            symbolTable.Define(asignation.Name.Text, new Undefined());
                            return;
                        }
                    }

                    // Intersect between an arc and a circle
                    if (f1.ObjectType == ObjectTypes.Arc && f2.ObjectType == ObjectTypes.Circle || f1.ObjectType == ObjectTypes.Circle && f2.ObjectType == ObjectTypes.Arc)
                    {
                        Arc arc;
                        Circle circle;

                        (circle, arc) = Utils.CircleAndArcOrdered(f1, f2);

                        // Calcular la distancia entre el centro del circulo y el centro del arco
                        var distance = Math.Sqrt(Math.Pow(arc.Center.X - circle.Center.X, 2) + Math.Pow(arc.Center.Y - circle.Center.Y, 2));

                        // Si la distancia es mayor que la suma de los radios, no hay interseccion
                        if (distance > circle.Radius.Value + arc.Measure.Value)
                        {
                            symbolTable.Define(asignation.Name.Text, new Undefined());
                            return;
                        }


                        // Si la distancia es menor que la diferencia absoluta de los radios, un circulo esta dentro del otro
                        if (distance < Math.Abs(circle.Radius.Value - arc.Measure.Value))
                        {
                            symbolTable.Define(asignation.Name.Text, new Undefined());
                            return;
                        }

                        // Calcular los puntos de interseccion
                        var a = (arc.Measure.Value * arc.Measure.Value - circle.Radius.Value * circle.Radius.Value + distance * distance) / (2 * distance);
                        var h = Math.Sqrt(arc.Measure.Value * arc.Measure.Value - a * a);

                        var x1 = arc.Center.X + a * (circle.Center.X - arc.Center.X) / distance;
                        var y1 = arc.Center.Y + a * (circle.Center.Y - arc.Center.Y) / distance;

                        var x2 = x1 + h * (circle.Center.Y - arc.Center.Y) / distance;
                        var y2 = y1 - h * (circle.Center.X - arc.Center.X) / distance;

                        var x3 = x1 - h * (circle.Center.Y - arc.Center.Y) / distance;
                        var y3 = y1 + h * (circle.Center.X - arc.Center.X) / distance;

                        // Calcular el angulo que forman los vectores del centro a start y del centro a end
                        var angle1 = Math.Atan2(arc.Start.Y - arc.Center.Y, arc.Start.X - arc.Center.X);
                        var angle2 = Math.Atan2(arc.End.Y - arc.Center.Y, arc.End.X - arc.Center.X);

                        // Calculate the angles formed by the lines from the arc's center to the intersection points
                        var angleX2Y2 = Math.Atan2(y2 - arc.Center.Y, x2 - arc.Center.X);
                        var angleX3Y3 = Math.Atan2(y3 - arc.Center.Y, x3 - arc.Center.X);

                        // Check if these angles are within the range of the arc's start and end angles
                        bool isX2Y2OnArc = (angle1 <= angleX2Y2 && angleX2Y2 <= angle2) || (angle2 <= angleX2Y2 && angleX2Y2 <= angle1);
                        bool isX3Y3OnArc = (angle1 <= angleX3Y3 && angleX3Y3 <= angle2) || (angle2 <= angleX3Y3 && angleX3Y3 <= angle1);

                        // If they are, the intersection points are on the arc
                        if (isX2Y2OnArc && isX3Y3OnArc)
                        {
                            Point point1 = new();
                            point1.AsignX(x2);
                            point1.AsignY(y2);

                            Point point2 = new();
                            point2.AsignX(x3);
                            point2.AsignY(y3);

                            symbolTable.Define(asignation.Name.Text, new Sequence(new List<Type>() { point1, point2 }));
                            return;
                        }
                        else if (isX2Y2OnArc)
                        {
                            Point point1 = new();
                            point1.AsignX(x2);
                            point1.AsignY(y2);

                            symbolTable.Define(asignation.Name.Text, new Sequence(new List<Type>() { point1 }));
                            return;
                        }
                        else if (isX3Y3OnArc)
                        {
                            Point point2 = new();
                            point2.AsignX(x3);
                            point2.AsignY(y3);

                            symbolTable.Define(asignation.Name.Text, new Sequence(new List<Type>() { point2 }));
                            return;
                        }
                        else
                        {
                            symbolTable.Define(asignation.Name.Text, new Undefined());
                            return;
                        }
                    }


                    else
                    {
                        errors.AddError("SEMANTIC ERROR: Can't intersect"); // TODO: Improve this message error
                    }
                }
            }
        }
    }
}