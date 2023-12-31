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

        public Type Evaluate(SymbolTable symbolTable, Error errors, List<Tuple<Type, Color>> toDraw)
        {
            if (F1 as IEvaluable != null && F2 as IEvaluable != null)
            {
                var f1 = ((IEvaluable)F1).Evaluate(symbolTable, errors, toDraw);
                var f2 = ((IEvaluable)F2).Evaluate(symbolTable, errors, toDraw);
                if (f1 is not ErrorType && f2 is not ErrorType)
                {
                    if (f1.ObjectType == ObjectTypes.Line && f2.ObjectType == ObjectTypes.Line) return IntersectFigures.IntersectTwoLines(f1, f2);

                    if (f1.ObjectType == ObjectTypes.Line && f2.ObjectType == ObjectTypes.Circle || f1.ObjectType == ObjectTypes.Circle && f2.ObjectType == ObjectTypes.Line) return IntersectFigures.IntersectLineAndCircle(f1, f2);

                    if (f1.ObjectType == ObjectTypes.Line && f2.ObjectType == ObjectTypes.Segment || f1.ObjectType == ObjectTypes.Segment && f2.ObjectType == ObjectTypes.Line) return IntersectFigures.IntersectLineAndSegment(f1, f2);

                    if (f1.ObjectType == ObjectTypes.Circle && f2.ObjectType == ObjectTypes.Segment || f1.ObjectType == ObjectTypes.Segment && f2.ObjectType == ObjectTypes.Circle) return IntersectFigures.IntersectCircleAndSegment(f1, f2);

                    if (f1.ObjectType == ObjectTypes.Segment && f2.ObjectType == ObjectTypes.Segment) return IntersectFigures.IntersectTwoSegments(f1, f2);

                    if (f1.ObjectType == ObjectTypes.Circle && f2.ObjectType == ObjectTypes.Circle) return IntersectFigures.IntersectTwoCircles(f1, f2);

                    if (f1.ObjectType == ObjectTypes.Point && f2.ObjectType == ObjectTypes.Point)
                    {
                        var point1 = (Point)f1;
                        var point2 = (Point)f2;

                        if (point1.X == point2.X && point1.Y == point2.Y) return new Sequence(new List<Type>() { point1 });

                        return new Sequence(new List<Type>() { new Undefined() });
                    }

                    if (f1.ObjectType == ObjectTypes.Point && f2.ObjectType == ObjectTypes.Line || f1.ObjectType == ObjectTypes.Line && f2.ObjectType == ObjectTypes.Point) return IntersectFigures.IntersectLineAndPoint(f1, f2);

                    if (f1.ObjectType == ObjectTypes.Point && f2.ObjectType == ObjectTypes.Segment || f1.ObjectType == ObjectTypes.Segment && f2.ObjectType == ObjectTypes.Point) return IntersectFigures.IntersectSegmentAndPoint(f1, f2);

                    if (f1.ObjectType == ObjectTypes.Point && f2.ObjectType == ObjectTypes.Circle || f1.ObjectType == ObjectTypes.Circle && f2.ObjectType == ObjectTypes.Point) return IntersectFigures.IntersectPointAndCircle(f1, f2);

                    if (f1.ObjectType == ObjectTypes.Arc && f2.ObjectType == ObjectTypes.Circle || f1.ObjectType == ObjectTypes.Circle && f2.ObjectType == ObjectTypes.Arc) return IntersectFigures.IntersectCircleAndArc(f1, f2);

                    if (f1.ObjectType == ObjectTypes.Arc && f2.ObjectType == ObjectTypes.Line || f1.ObjectType == ObjectTypes.Line && f2.ObjectType == ObjectTypes.Arc) return IntersectFigures.IntersectLineAndArc(f1, f2);

                    if (f1.ObjectType == ObjectTypes.Arc && f2.ObjectType == ObjectTypes.Segment || f1.ObjectType == ObjectTypes.Segment && f2.ObjectType == ObjectTypes.Arc) return IntersectFigures.IntersectSegmentAndArc(f1, f2);

                    if (f1.ObjectType == ObjectTypes.Arc && f2.ObjectType == ObjectTypes.Arc || f1.ObjectType == ObjectTypes.Arc && f2.ObjectType == ObjectTypes.Arc) return IntersectFigures.IntersectArcAndArc(f1, f2);

                    if (f1.ObjectType == ObjectTypes.Ray && f2.ObjectType == ObjectTypes.Line || f1.ObjectType == ObjectTypes.Line && f2.ObjectType == ObjectTypes.Ray) return IntersectFigures.IntersectRayAndLine(f1, f2);

                    if (f1.ObjectType == ObjectTypes.Ray && f2.ObjectType == ObjectTypes.Segment || f1.ObjectType == ObjectTypes.Segment && f2.ObjectType == ObjectTypes.Ray) return IntersectFigures.IntersectRayAndSegment(f1, f2);

                    if (f1.ObjectType == ObjectTypes.Ray && f2.ObjectType == ObjectTypes.Circle || f1.ObjectType == ObjectTypes.Circle && f2.ObjectType == ObjectTypes.Ray) return IntersectFigures.IntersectRayAndCircle(f1, f2);

                    if (f1.ObjectType == ObjectTypes.Ray && f2.ObjectType == ObjectTypes.Arc || f1.ObjectType == ObjectTypes.Arc && f2.ObjectType == ObjectTypes.Ray) return IntersectFigures.IntersectRayAndArc(f1, f2);

                    if (f1.ObjectType == ObjectTypes.Ray && f2.ObjectType == ObjectTypes.Ray || f1.ObjectType == ObjectTypes.Ray && f2.ObjectType == ObjectTypes.Ray) return IntersectFigures.IntersectRayAndRay(f1, f2);

                    if (f1.ObjectType == ObjectTypes.Point && f2.ObjectType == ObjectTypes.Ray || f1.ObjectType == ObjectTypes.Ray && f2.ObjectType == ObjectTypes.Point) return IntersectFigures.IntersectPointAndRay(f1, f2);
                }
                errors.AddError("RUNTIME ERROR: Can't intersect");
                return new ErrorType();
            }
            else
            {
                errors.AddError($"RUNTIME ERROR: Invalid expression in intersect(), Line: {Positions["intersect"].Item1}, Column: {Positions["intersect"].Item2}");
                return new ErrorType();
            }
        }

        public void HandleIntersectAsignationExpression(SymbolTable symbolTable, Error errors, AsignationStatement asignation)
        {
            var intersect = Evaluate(symbolTable, errors, toDraw: new List<Tuple<Type, Color>>());
            if (intersect is not ErrorType)
            {
                symbolTable.Define(asignation.Name.Text, (Sequence)intersect);
            }
        }

        public void HandleIntersectExpression(List<Tuple<Type, Color>> toDraw, Error errors, SymbolTable symbolTable, Color color, string name)
        {
            var intersect = Evaluate(symbolTable, errors, toDraw);
            if (intersect is not ErrorType)
            {
                ((Sequence)intersect).SetName(name);
                // Check if the intersect is not undefined
                if (((Sequence)intersect).GetElement(0).ObjectType != ObjectTypes.Undefined) toDraw.Add(new Tuple<Type, Color>(intersect, color));
                else errors.AddError($"RUNTIME ERROR: Expression in draw is Undefined, which is not drawable, Line: {Positions["intersect"].Item1}, Column: {Positions["intersect"].Item2}");
            }
        }
    }

}
