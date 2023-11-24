using System.ComponentModel;

namespace GeoWall_E
{
    public class Evaluator
    {
        private List<Node> Root { get; set; }
        private Error Errors_ { get; set; }
        private SymbolTable SymbolTable { get; set; }

        public Evaluator(List<Node> root, Error error)
        {
            Root = root;
            Errors_ = error;
            SymbolTable = new SymbolTable();
        }

        public Error Errors => Errors_;

        public List<Type> Evaluate()
        {
            List<Type> toDraw = new();
            foreach (var node in Root)
            {
                switch (node)
                {
                    case AsignationStatement asignation:
                        HandleAsignationNode(asignation, toDraw);
                        break;
                    case PointStatement point:
                        SymbolTable.Define(point.Name.Text, new Point(point.Color, point.Name.Text));
                        break;
                    case LineStatement line:
                        SymbolTable.Define(line.Name.Text, new Line(new Point(line.Color), new Point(line.Color), line.Color, line.Name.Text));
                        break;
                    case SegmentStatement segment:
                        SymbolTable.Define(segment.Name.Text, new Segment(new Point(segment.Color), new Point(segment.Color), segment.Color, segment.Name.Text));
                        break;
                    case RayStatement ray:
                        SymbolTable.Define(ray.Name.Text, new Ray(new Point(ray.Color), new Point(ray.Color), ray.Color, ray.Name.Text));
                        break;
                    case CircleStatement circle:
                        SymbolTable.Define(circle.Name.Text, new Circle(new Point(circle.Color), new Measure(new Point(circle.Color), new Point(circle.Color), circle.Name.Text), circle.Color, circle.Name.Text));
                        break;
                    case Draw draw_:
                        HandleDrawNode(draw_, toDraw);
                        break;
                }
            }
            return toDraw;
        }

        private void HandleAsignationNode(AsignationStatement asignation, List<Type> toDraw)
        {
            switch (asignation.Value)
            {
                case VariableExpression variable:
                    HandleVariableExpression(variable, asignation.Name.Text);
                    break;
                case MeasureExpression measure:
                    HandleMeasureExpression(measure, asignation.Name.Text);
                    break;
                case LineExpression lineexp:
                    HandleLineAsignationExpression(lineexp, asignation);
                    break;
                case SegmentExpression segmentexp:
                    HandleSegmentAsignationExpression(segmentexp, asignation);
                    break;
                case RayExpression rayexp:
                    HandleRayAsignationExpression(rayexp, asignation);
                    break;
                case CircleExpression circleexp:
                    HandleCircleAsignationExpression(circleexp, asignation);
                    break;
                case ArcExpression arcexp:
                    HandleArcAsignationExpression(arcexp, asignation);
                    break;
            }
        }

        private void HandleArcAsignationExpression(ArcExpression arcexp, AsignationStatement asignation)
        {
            var center = SymbolTable.Resolve(arcexp.Center.Text);
            var start = SymbolTable.Resolve(arcexp.Start.Text);
            var end = SymbolTable.Resolve(arcexp.End.Text);
            var measure = SymbolTable.Resolve(arcexp.Measure.Text);

            if (center is not ErrorType && start is not ErrorType && end is not ErrorType && measure is not ErrorType)
            {
                if (center.ObjectType == ObjectTypes.Point && start.ObjectType == ObjectTypes.Point && end.ObjectType == ObjectTypes.Point && measure.ObjectType == ObjectTypes.Measure)
                {
                    SymbolTable.Define(asignation.Name.Text, new Arc((Point)center, (Point)start, (Point)end, (Measure)measure, asignation.Color, asignation.Name.Text));
                }
                else
                {
                    Errors.AddError($"Invalid type for {arcexp.Center.Text} or {arcexp.Start.Text} or {arcexp.End.Text} or {arcexp.Measure.Text}, Line: {arcexp.Center.Line}, Column: {arcexp.Center.Column}");
                }
            }
            else
            {
                Errors.AddError($"Variable {arcexp.Center.Text} or {arcexp.Start.Text} or {arcexp.End.Text} or {arcexp.Measure.Text} not declared, Line: {arcexp.Center.Line}, Column: {arcexp.Center.Column}");
            }
        }

        private void HandleCircleAsignationExpression(CircleExpression circleexp, AsignationStatement asignation)
        {
            var center = SymbolTable.Resolve(circleexp.Center.Text);
            var radius = SymbolTable.Resolve(circleexp.Radius.Text);
            if (center is not ErrorType && radius is not ErrorType)
            {
                if (center.ObjectType == ObjectTypes.Point && radius.ObjectType == ObjectTypes.Measure)
                {
                    SymbolTable.Define(asignation.Name.Text, new Circle((Point)center, (Measure)radius, asignation.Color, asignation.Name.Text));
                }
                else
                {
                    Errors.AddError($"Invalid type for {circleexp.Center.Text} or {circleexp.Radius.Text}, Line: {circleexp.Center.Line}, Column: {circleexp.Center.Column}");
                }
            }
            else
            {
                Errors.AddError($"Variable {circleexp.Center.Text} or {circleexp.Radius.Text} not declared, Line: {circleexp.Center.Line}, Column: {circleexp.Center.Column}");
            }
        }

        private void HandleRayAsignationExpression(RayExpression rayexp, AsignationStatement asignation)
        {
            var start = SymbolTable.Resolve(rayexp.Start.Text);
            var end = SymbolTable.Resolve(rayexp.End.Text);
            if (start is not ErrorType && end is not ErrorType)
            {
                if (start.ObjectType == ObjectTypes.Point && end.ObjectType == ObjectTypes.Point)
                {
                    SymbolTable.Define(asignation.Name.Text, new Ray((Point)start, (Point)end, asignation.Color, asignation.Name.Text));
                }
                else
                {
                    Errors.AddError($"Invalid type for {rayexp.Start.Text} or {rayexp.End.Text}, Line: {rayexp.Start.Line}, Column: {rayexp.Start.Column}");
                }
            }
            else
            {
                Errors.AddError($"Variable {rayexp.Start.Text} or {rayexp.End.Text} not declared, Line: {rayexp.Start.Line}, Column: {rayexp.Start.Column}");
            }
        }

        private void HandleSegmentAsignationExpression(SegmentExpression segmentexp, AsignationStatement asignation)
        {
            var start = SymbolTable.Resolve(segmentexp.Start.Text);
            var end = SymbolTable.Resolve(segmentexp.End.Text);
            if (start is not ErrorType && end is not ErrorType)
            {
                if (start.ObjectType == ObjectTypes.Point && end.ObjectType == ObjectTypes.Point)
                {
                    SymbolTable.Define(asignation.Name.Text, new Segment((Point)start, (Point)end, asignation.Color, asignation.Name.Text));
                }
                else
                {
                    Errors.AddError($"Invalid type for {segmentexp.Start.Text} or {segmentexp.End.Text}, Line: {segmentexp.Start.Line}, Column: {segmentexp.Start.Column}");
                }
            }
            else
            {
                Errors.AddError($"Variable {segmentexp.Start.Text} or {segmentexp.End.Text} not declared, Line: {segmentexp.Start.Line}, Column: {segmentexp.Start.Column}");
            }
        }

        private void HandleLineAsignationExpression(LineExpression lineexp, AsignationStatement asig)
        {
            var p1 = SymbolTable.Resolve(lineexp.P1.Text);
            var p2 = SymbolTable.Resolve(lineexp.P2.Text);
            if (p1 is not ErrorType && p2 is not ErrorType)
            {
                if (p1.ObjectType == ObjectTypes.Point && p2.ObjectType == ObjectTypes.Point)
                {
                    SymbolTable.Define(asig.Name.Text, new Line((Point)p1, (Point)p2, asig.Color, asig.Name.Text));
                }
                else
                {
                    Errors.AddError($"Invalid type for {lineexp.P1.Text} or {lineexp.P2.Text}, Line: {lineexp.P1.Line}, Column: {lineexp.P1.Column}");
                }
            }
            else
            {
                Errors.AddError($"Variable {lineexp.P1.Text} or {lineexp.P2.Text} not declared, Line: {lineexp.P1.Line}, Column: {lineexp.P1.Column}");
            }
        }

        private void HandleVariableExpression(VariableExpression variable, string text)
        {
            var variableFound = SymbolTable.Resolve(variable.Name.Text);
            if (variableFound is not ErrorType)
            {
                if (variableFound.ObjectType == ObjectTypes.Point)
                {
                    SymbolTable.Define(text, variableFound);
                }
                else if (variableFound.ObjectType == ObjectTypes.Line)
                {
                    SymbolTable.Define(text, variableFound);
                }
                else if (variableFound.ObjectType == ObjectTypes.Segment)
                {
                    SymbolTable.Define(text, variableFound);
                }
                else if (variableFound.ObjectType == ObjectTypes.Ray)
                {
                    SymbolTable.Define(text, variableFound);
                }
                else if (variableFound.ObjectType == ObjectTypes.Circle)
                {
                    SymbolTable.Define(text, variableFound);
                }
                else if (variableFound.ObjectType == ObjectTypes.Arc)
                {
                    SymbolTable.Define(text, variableFound);
                }
            }
            else
            {
                Errors_.AddError($"Variable {variable.Name.Text} not declared, Line: {variable.Name.Line}, Column: {variable.Name.Column}");
            }
        }

        private void HandleMeasureExpression(MeasureExpression measure, string name)
        {
            var p1 = SymbolTable.Resolve(measure.P1.Text);
            var p2 = SymbolTable.Resolve(measure.P2.Text);
            if (p1 is not ErrorType && p2 is not ErrorType)
            {
                if (p1.ObjectType == ObjectTypes.Point && p2.ObjectType == ObjectTypes.Point)
                {
                    SymbolTable.Define(name, new Measure((Point)p1, (Point)p2, name));
                }
                else
                {
                    Errors.AddError($"Invalid type for {measure.P1.Text} or {measure.P2.Text}, Line: {measure.P1.Line}, Column: {measure.P1.Column}");
                }
            }
            else
            {
                Errors.AddError($"Variable {measure.P1.Text} or {measure.P2.Text} not declared, Line: {measure.P1.Line}, Column: {measure.P1.Column}");
            }
        }

        private void HandleDrawNode(Draw draw, List<Type> toDraw)
        {
            switch (draw.Expression)
            {
                case VariableExpression variable:
                    AddTypeToDraw(variable, toDraw);
                    break;
                case LineExpression lineexp:
                    HandleLineExpression(lineexp, toDraw);
                    break;
                case SegmentExpression segmentexp:
                    HandleSegmentExpression(segmentexp, toDraw);
                    break;
                case RayExpression rayexp:
                    HandleRayExpression(rayexp, toDraw);
                    break;
                case CircleExpression circleexp:
                    HandleCircleExpression(circleexp, toDraw);
                    break;
                case ArcExpression arcexp:
                    HandleArcExpression(arcexp, toDraw);
                    break;
            }
        }

        private void HandleArcExpression(ArcExpression arcexp, List<Type> toDraw)
        {
            var center = SymbolTable.Resolve(arcexp.Center.Text);
            var start = SymbolTable.Resolve(arcexp.Start.Text);
            var end = SymbolTable.Resolve(arcexp.End.Text);
            var measure = SymbolTable.Resolve(arcexp.Measure.Text);
            if (center is not ErrorType && start is not ErrorType && end is not ErrorType && measure is not ErrorType)
            {
                if (center.ObjectType == ObjectTypes.Point && start.ObjectType == ObjectTypes.Point && end.ObjectType == ObjectTypes.Point && measure.ObjectType == ObjectTypes.Measure)
                {
                    toDraw.Add(new Arc((Point)center, (Point)start, (Point)end, (Measure)measure, arcexp.Color));
                }
                else
                {
                    Errors.AddError($"Invalid type for {arcexp.Center.Text} or {arcexp.Start.Text} or {arcexp.End.Text} or {arcexp.Measure.Text}, Line: {arcexp.Center.Line}, Column: {arcexp.Center.Column}");
                }
            }
            else
            {
                Errors.AddError($"Variable {arcexp.Center.Text} or {arcexp.Start.Text} or {arcexp.End.Text} or {arcexp.Measure.Text} not declared, Line: {arcexp.Center.Line}, Column: {arcexp.Center.Column}");
            }
        }

        private void HandleCircleExpression(CircleExpression circleexp, List<Type> toDraw)
        {
            var center = SymbolTable.Resolve(circleexp.Center.Text);
            var radius = SymbolTable.Resolve(circleexp.Radius.Text);
            if (center is not ErrorType && radius is not ErrorType)
            {
                if (center.ObjectType == ObjectTypes.Point && radius.ObjectType == ObjectTypes.Measure)
                {
                    toDraw.Add(new Circle((Point)center, (Measure)radius, circleexp.Color));
                }
                else
                {
                    Errors.AddError($"Invalid type for {circleexp.Center.Text} or {circleexp.Radius.Text}, Line: {circleexp.Center.Line}, Column: {circleexp.Center.Column}");
                }
            }
            else
            {
                Errors.AddError($"Variable {circleexp.Center.Text} or {circleexp.Radius.Text} not declared, Line: {circleexp.Center.Line}, Column: {circleexp.Center.Column}");
            }

        }

        private void HandleRayExpression(RayExpression rayexp, List<Type> toDraw)
        {
            var start = SymbolTable.Resolve(rayexp.Start.Text);
            var end = SymbolTable.Resolve(rayexp.End.Text);
            if (start is not ErrorType && end is not ErrorType)
            {
                if (start.ObjectType == ObjectTypes.Point && end.ObjectType == ObjectTypes.Point)
                {
                    toDraw.Add(new Ray((Point)start, (Point)end, rayexp.Color));
                }
                else
                {
                    Errors.AddError($"Invalid type for {rayexp.Start.Text} or {rayexp.End.Text}, Line: {rayexp.Start.Line}, Column: {rayexp.Start.Column}");
                }
            }
            else
            {
                Errors.AddError($"Variable {rayexp.Start.Text} or {rayexp.End.Text} not declared, Line: {rayexp.Start.Line}, Column: {rayexp.Start.Column}");
            }
        }

        private void HandleSegmentExpression(SegmentExpression segmentexp, List<Type> toDraw)
        {
            var start = SymbolTable.Resolve(segmentexp.Start.Text);
            var end = SymbolTable.Resolve(segmentexp.End.Text);
            if (start is not ErrorType && end is not ErrorType)
            {
                if (start.ObjectType == ObjectTypes.Point && end.ObjectType == ObjectTypes.Point)
                {
                    toDraw.Add(new Segment((Point)start, (Point)end, segmentexp.Color));
                }
                else
                {
                    Errors.AddError($"Invalid type for {segmentexp.Start.Text} or {segmentexp.End.Text}, Line: {segmentexp.Start.Line}, Column: {segmentexp.Start.Column}");
                }
            }
            else
            {
                Errors.AddError($"Variable {segmentexp.Start.Text} or {segmentexp.End.Text} not declared, Line: {segmentexp.Start.Line}, Column: {segmentexp.Start.Column}");
            }
        }

        private void AddTypeToDraw(VariableExpression variable, List<Type> toDraw)
        {
            var variableFound = SymbolTable.Resolve(variable.Name.Text);
            if (variableFound is not ErrorType)
            {
                if (variableFound.ObjectType == ObjectTypes.Point)
                {
                    toDraw.Add((Point)variableFound);
                }
                else if (variableFound.ObjectType == ObjectTypes.Line)
                {
                    toDraw.Add((Line)variableFound);
                }
                else if (variableFound.ObjectType == ObjectTypes.Segment)
                {
                    toDraw.Add((Segment)variableFound);
                }
                else if (variableFound.ObjectType == ObjectTypes.Ray)
                {
                    toDraw.Add((Ray)variableFound);
                }
                else if (variableFound.ObjectType == ObjectTypes.Circle)
                {
                    toDraw.Add((Circle)variableFound);
                }
                else if (variableFound.ObjectType == ObjectTypes.Arc)
                {
                    toDraw.Add((Arc)variableFound);
                }
            }
            else
            {
                Errors.AddError($"Variable {variable.Name.Text} not declared, Line: {variable.Name.Line}, Column: {variable.Name.Column}");
            }
        }

        private void HandleLineExpression(LineExpression lineexp, List<Type> toDraw)
        {
            var p1 = SymbolTable.Resolve(lineexp.P1.Text);
            var p2 = SymbolTable.Resolve(lineexp.P2.Text);
            if (p1 is not ErrorType && p2 is not ErrorType)
            {
                if (p1.ObjectType == ObjectTypes.Point && p2.ObjectType == ObjectTypes.Point)
                {
                    toDraw.Add(new Line((Point)p1, (Point)p2, lineexp.Color));
                }
                else
                {
                    Errors_.AddError($"Invalid type for {lineexp.P1.Text} or {lineexp.P2.Text}, Line: {lineexp.P1.Line}, Column: {lineexp.P1.Column}");
                }
            }
            else
            {
                Errors_.AddError($"Variable {lineexp.P1.Text} or {lineexp.P2.Text} not declared, Line: {lineexp.P1.Line}, Column: {lineexp.P1.Column}");
            }
        }
    }
}