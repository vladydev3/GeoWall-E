namespace GeoWall_E;

public class Evaluator
{
    private List<Node> Root { get; set; }
    private Error Errors { public get; private set; }
    public static List<(string, Type)> VariableScope { get; set; } = new();

    public Evaluator(List<Node> root)
    {
        Root = root;
    }

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
                    VariableScope.Add((point.Name.Text, new Point(point.Color, point.Name.Text)));
                    break;
                case LineStatement line:
                    VariableScope.Add((line.Name.Text, new Line(new Point(line.Color), new Point(line.Color), line.Color, line.Name.Text)));
                    break;
                case SegmentStatement segment:
                    VariableScope.Add((segment.Name.Text, new Segment(new Point(segment.Color), new Point(segment.Color), segment.Color, segment.Name.Text)));
                    break;
                case RayStatement ray:
                    VariableScope.Add((ray.Name.Text, new Ray(new Point(ray.Color), new Point(ray.Color), ray.Color, ray.Name.Text)));
                    break;
                case CircleStatement circle:
                    VariableScope.Add((circle.Name.Text, new Circle(new Point(circle.Color), new Measure(new Point(circle.Color), new Point(circle.Color)), circle.Color, circle.Name.Text)));
                    break;
                case Draw draw_:
                    HandleDrawNode(draw_, toDraw);
                    break;
            }
        }
        return toDraw;
    }

    private static Dictionary<Type, Action<Expression, List<Type>, AsignationStatement>> handlers = new Dictionary<Type, Action<Expression, List<Type>, AsignationStatement>>
    {
        { typeof(MeasureExpression), HandleMeasureExpression },
        { typeof(VariableExpression), HandleVariableExpression },
        { typeof(LineExpression), HandleLineAsignationExpression },
        { typeof(SegmentExpression), HandleSegmentAsignationExpression },
        { typeof(RayExpression), HandleRayAsignationExpression },
        { typeof(CircleExpression), HandleCircleAsignationExpression },
        { typeof(ArcExpression), HandleArcAsignationExpression }
    };

    private static void HandleAsignationNode(AsignationStatement asignation, List<Type> toDraw)
    {
        var handler = handlers[asignation.Value.GetType()];
        handler(asignation.Value, toDraw, asignation);
    }

    private static void HandleArcAsignationExpression(ArcExpression arcexp, List<Type> toDraw, AsignationStatement asignation)
    {
        var center = VariableScope.Find(x => x.Item1 == arcexp.Center.Text);
        var start = VariableScope.Find(x => x.Item1 == arcexp.Start.Text);
        var end = VariableScope.Find(x => x.Item1 == arcexp.End.Text);
        var measure = VariableScope.Find(x => x.Item1 == arcexp.Measure.Text);

        if (center.Item1 != null && start.Item1 != null && end.Item1 != null && measure.Item1 != null)
        {
            if (center.Item2.Type == ObjectType.Point && start.Item2.Type == ObjectType.Point && end.Item2.Type == ObjectType.Point && measure.Item2.Type == ObjectType.Measure)
            {
                VariableScope.Add((asignation.Name.Text, new Arc((Point)center.Item2, (Point)start.Item2, (Point)end.Item2, (Measure)measure.Item2, asignation.Color, asignation.Name.Text)));
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

    private static void HandleCircleAsignationExpression(CircleExpression circleexp, List<Type> toDraw, AsignationStatement asignation)
    {
        var center = VariableScope.Find(x => x.Item1 == circleexp.Center.Text);
        var radius = VariableScope.Find(x => x.Item1 == circleexp.Radius.Text);
        if (center.Item1 != null && radius.Item1 != null)
        {
            if (center.Item2.Type == ObjectType.Point && radius.Item2.Type == ObjectType.Measure)
            {
                VariableScope.Add((asignation.Name.Text, new Circle((Point)center.Item2, (Measure)radius.Item2, asignation.Color, asignation.Name.Text)));
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

    private static void HandleRayAsignationExpression(RayExpression rayexp, List<Type> toDraw, AsignationStatement asignation)
    {

    }

    private static void HandleSegmentAsignationExpression(SegmentExpression segmentexp, List<Type> toDraw, AsignationStatement asignation)
    {
        var start = VariableScope.Find(x => x.Item1 == segmentexp.Start.Text);
        var end = VariableScope.Find(x => x.Item1 == segmentexp.End.Text);
        if (start.Item1 != null && end.Item1 != null)
        {
            if (start.Item2.Type == ObjectType.Point && end.Item2.Type == ObjectType.Point)
            {
                VariableScope.Add((asignation.Name.Text, new Segment((Point)start.Item2, (Point)end.Item2, asignation.Color, asignation.Name.Text)));
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

    private static void HandleLineAsignationExpression(LineExpression lineexp, List<Type> toDraw, AsignationStatement asig)
    {
        var p1 = VariableScope.Find(x => x.Item1 == lineexp.P1.Text);
        var p2 = VariableScope.Find(x => x.Item1 == lineexp.P2.Text);
        if (p1.Item1 != null && p2.Item1 != null)
        {
            if (p1.Item2.Type == ObjectType.Point && p2.Item2.Type == ObjectType.Point)
            {
                VariableScope.Add((asig.Name.Text, new Line((Point)p1.Item2, (Point)p2.Item2, asig.Color, asig.Name.Text)));
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

    private static void HandleVariableExpression(VariableExpression variable, string text)
    {
        var variableFound = VariableScope.Find(x => x.Item1 == variable.Name.Text);
        if (variableFound.Item1 != null)
        {
            VariableScope.Add((text, variableFound.Item2));
        }
        else
        {
            Errors.AddError($"Variable {variable.Name.Text} not declared, Line: {variable.Name.Line}, Column: {variable.Name.Column}");
        }
    }

    private static void HandleMeasureExpression(MeasureExpression measure, string name)
    {
        var p1 = VariableScope.Find(x => x.Item1 == measure.P1.Text);
        var p2 = VariableScope.Find(x => x.Item1 == measure.P2.Text);
        if (p1.Item1 != null && p2.Item1 != null)
        {
            if (p1.Item2.Type == ObjectType.Point && p2.Item2.Type == ObjectType.Point)
            {
                VariableScope.Add((name, new Measure((Point)p1.Item2, (Point)p2.Item2, name)));
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

    private static void HandleDrawNode(Draw draw, List<Type> toDraw)
    {
        if (draw.Expression is VariableExpression variable)
        {
            AddTypeToDraw(variable, toDraw);
        }
        else if (draw.Expression is LineExpression lineexp)
        {
            HandleLineExpression(lineexp, toDraw);
        }
        else if (draw.Expression is SegmentExpression segmentexp)
        {
            HandleSegmentExpression(segmentexp, toDraw);
        }
        else if (draw.Expression is RayExpression rayexp)
        {
            HandleRayExpression(rayexp, toDraw);
        }
        else if (draw.Expression is CircleExpression circleexp)
        {
            HandleCircleExpression(circleexp, toDraw);
        }
        else if (draw.Expression is ArcExpression arcexp)
        {
            HandleArcExpression(arcexp, toDraw);
        }
    }

    private static void HandleArcExpression(ArcExpression arcexp, List<Type> toDraw)
    {
        var center = VariableScope.Find(x => x.Item1 == arcexp.Center.Text);
        var start = VariableScope.Find(x => x.Item1 == arcexp.Start.Text);
        var end = VariableScope.Find(x => x.Item1 == arcexp.End.Text);
        var measure = VariableScope.Find(x => x.Item1 == arcexp.Measure.Text);
        if (center.Item1 != null && start.Item1 != null && end.Item1 != null && measure.Item1 != null)
        {
            if (center.Item2.Type == ObjectType.Point && start.Item2.Type == ObjectType.Point && end.Item2.Type == ObjectType.Point && measure.Item2.Type == ObjectType.Measure)
            {
                toDraw.Add(new Arc((Point)center.Item2, (Point)start.Item2, (Point)end.Item2, (Measure)measure.Item2, arcexp.Color));
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

    private static void HandleCircleExpression(CircleExpression circleexp, List<Type> toDraw)
    {
        var center = VariableScope.Find(x => x.Item1 == circleexp.Center.Text);
        var radius = VariableScope.Find(x => x.Item1 == circleexp.Radius.Text);
        if (center.Item1 != null && radius.Item1 != null)
        {
            if (center.Item2.Type == ObjectType.Point && radius.Item2.Type == ObjectType.Measure)
            {
                toDraw.Add(new Circle((Point)center.Item2, (Measure)radius.Item2, circleexp.Color));
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

    private static void HandleRayExpression(RayExpression rayexp, List<Type> toDraw)
    {
        var start = VariableScope.Find(x => x.Item1 == rayexp.Start.Text);
        var end = VariableScope.Find(x => x.Item1 == rayexp.End.Text);
        if (start.Item1 != null && end.Item1 != null)
        {
            if (start.Item2.Type == ObjectType.Point && end.Item2.Type == ObjectType.Point)
            {
                toDraw.Add(new Ray((Point)start.Item2, (Point)end.Item2, rayexp.Color));
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

    private static void HandleSegmentExpression(SegmentExpression segmentexp, List<Type> toDraw)
    {
        var start = VariableScope.Find(x => x.Item1 == segmentexp.Start.Text);
        var end = VariableScope.Find(x => x.Item1 == segmentexp.End.Text);
        if (start.Item1 != null && end.Item1 != null)
        {
            if (start.Item2.Type == ObjectType.Point && end.Item2.Type == ObjectType.Point)
            {
                toDraw.Add(new Segment((Point)start.Item2, (Point)end.Item2, segmentexp.Color));
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

    private static void AddTypeToDraw(VariableExpression variable, List<Type> toDraw)
    {
        var variableFound = VariableScope.Find(x => x.Item1 == variable.Name.Text);
        if (variableFound.Item1 != null)
        {
            toDraw.Add(variableFound.Item2);
        }
        else
        {
            Errors.AddError($"Variable {variable.Name.Text} not declared, Line: {variable.Name.Line}, Column: {variable.Name.Column}");
        }
    }

    private static void HandleLineExpression(LineExpression lineexp, List<Type> toDraw)
    {
        var p1 = VariableScope.Find(x => x.Item1 == lineexp.P1.Text);
        var p2 = VariableScope.Find(x => x.Item1 == lineexp.P2.Text);
        if (p1.Item1 != null && p2.Item1 != null)
        {
            if (p1.Item2.Type == ObjectType.Point && p2.Item2.Type == ObjectType.Point)
            {
                toDraw.Add(new Line((Point)p1.Item2, (Point)p2.Item2, lineexp.Color));
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
}