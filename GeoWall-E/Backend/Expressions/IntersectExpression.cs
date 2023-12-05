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

        internal void HandleIntersectExpression(SymbolTable symbolTable, Error errors, AsignationStatement asignation)
        {
            if (F1 as IEvaluable != null && F2 as IEvaluable != null)
            {
                var f1 = ((IEvaluable)F1).Evaluate(symbolTable, errors);
                var f2 = ((IEvaluable)F2).Evaluate(symbolTable, errors);
                if (f1 is not ErrorType && f2 is not ErrorType)
                {
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

                        if (x >= Math.Min(line1.P1.X, line1.P2.X) && x <= Math.Max(line1.P1.X, line1.P2.X) &&
                            x >= Math.Min(line2.P1.X, line2.P2.X) && x <= Math.Max(line2.P1.X, line2.P2.X) &&
                            y >= Math.Min(line1.P1.Y, line1.P2.Y) && y <= Math.Max(line1.P1.Y, line1.P2.Y) &&
                            y >= Math.Min(line2.P1.Y, line2.P2.Y) && y <= Math.Max(line2.P1.Y, line2.P2.Y))
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
                    else
                    {
                        errors.AddError("SEMANTIC ERROR: Can't intersect"); // TODO: Improve this message error
                    }
                }
            }
        }
    }
}