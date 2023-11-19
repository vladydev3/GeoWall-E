




namespace GeoWall_E;

public class Evaluator
{
    public List<Node> Root { get; set; }
    public static Errors Errors { get; set; } = new Errors();
    public static List<DeclaredVariable> VariableScope { get; set; } = new List<DeclaredVariable>();

    public Evaluator(List<Node> root)
    {
        Root = root;
    }

    public List<Types> Evaluate()
    {
        List<Types> toDraw = new List<Types>();
        foreach (var node in Root)
        {
            switch (node)
            {
                case PointStatement point:
                    VariableScope.Add(new DeclaredVariable(point.Name.Text, ObjectTypes.Point, point.Color, point.IsSequence));
                    break;
                case LineStatement line:
                    VariableScope.Add(new DeclaredVariable(line.Name.Text, ObjectTypes.Line, line.Color, line.IsSequence));
                    break;
                case SegmentStatement segment:
                    VariableScope.Add(new DeclaredVariable(segment.Name.Text, ObjectTypes.Segment, segment.Color, segment.IsSequence));
                    break;
                case RayStatement ray:
                    VariableScope.Add(new DeclaredVariable(ray.Name.Text, ObjectTypes.Ray, ray.Color, ray.IsSequence));
                    break;
                case Draw draw_:
                    HandleDrawNode(draw_, toDraw);
                    break;
            }
        }
        return toDraw;
    }
    private static void HandleDrawNode(Draw draw, List<Types> toDraw)
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
       /* else if (draw.Expression is CircleExpression circleexp)
        {
            HandleCircleExpression(circleexp, toDraw);
        }*/
        else if (draw.Expression is ArcExpression arcexp)
        {
            HandleArcExpression(arcexp, toDraw);
        }
    }

    private static void HandleArcExpression(ArcExpression arcexp, List<Types> toDraw)
    {
        throw new NotImplementedException();
    }

   /* private static void HandleCircleExpression(CircleExpression circleexp, List<Types> toDraw)
    {
        var p1 = VariableScope.Find(x => x.Name == circleexp.Center.Text);
        var p2 = VariableScope.Find(x => x.Name == circleexp.Radius.Text);
        if (p1 != null && p2 != null)
        {
            if (p1.Type == ObjectTypes.Point && p2.Type == ObjectTypes.Point)
            {
                toDraw.Add(new Circle(new Point(p1.Color), new Point(p2.Color)));
            }
            else
            {
                Errors.AddError($"Invalid type for {circleexp.Center.Text} or {circleexp.Radius.Text}, Line: {circleexp.Center.Line}, Column: {circleexp.Center.Column}");
            }
        }
        else
        {
            Errors.AddError($"Variable {circleexp.Center.Text} or {circleexp.Point.Text} not declared, Line: {circleexp.Center.Line}, Column: {circleexp.Center.Column}");
        }
    
    }
   */

    private static void HandleRayExpression(RayExpression rayexp, List<Types> toDraw)
    {
        var p1 = VariableScope.Find(x => x.Name == rayexp.Start.Text);
        var p2 = VariableScope.Find(x => x.Name == rayexp.End.Text);
        if (p1 != null && p2 != null)
        {
            if (p1.Type == ObjectTypes.Point && p2.Type == ObjectTypes.Point)
            {
                toDraw.Add(new Ray(new Point(p1.Color,p1.Name), new Point(p2.Color,p2.Name)));
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

    private static void HandleSegmentExpression(SegmentExpression segmentexp, List<Types> toDraw)
    {
        var p1 = VariableScope.Find(x => x.Name == segmentexp.Start.Text);
        var p2 = VariableScope.Find(x => x.Name == segmentexp.End.Text);
        if (p1 != null && p2 != null)
        {
            if (p1.Type == ObjectTypes.Point && p2.Type == ObjectTypes.Point)
            {
                toDraw.Add(new Segment(new Point(p1.Color,p1.Name), new Point(p2.Color,p2.Name)));
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

    private static void AddTypeToDraw(VariableExpression variable, List<Types> toDraw)
    {
        var variableToDraw = VariableScope.Find(x => x.Name == variable.Name.Text);
        if (variableToDraw != null)
        {
            switch (variableToDraw.Type)
            {
                case ObjectTypes.Point:
                    toDraw.Add(new Point(variableToDraw.Color ?? new Color(Colors.Black), variableToDraw.Name));
                    break;
                case ObjectTypes.Line:
                    toDraw.Add(new Line(new Point(variableToDraw.Color), new Point(variableToDraw.Color), variableToDraw.Name));
                    break;
                case ObjectTypes.Segment:
                    toDraw.Add(new Segment(new Point(variableToDraw.Color), new Point(variableToDraw.Color), variableToDraw.Name));
                    break;
                case ObjectTypes.Ray:
                    toDraw.Add(new Ray(new Point(variableToDraw.Color), new Point(variableToDraw.Color), variableToDraw.Name));
                    break;
            }
        }
        else
        {
            Errors.AddError($"Variable {variable.Name.Text} not declared, Line: {variable.Name.Line}, Column: {variable.Name.Column}");
        }
    }

    private static void HandleLineExpression(LineExpression lineexp, List<Types> toDraw)
    {
        var p1 = VariableScope.Find(x => x.Name == lineexp.P1.Text);
        var p2 = VariableScope.Find(x => x.Name == lineexp.P2.Text);
        if (p1 != null && p2 != null)
        {
            if (p1.Type == ObjectTypes.Point && p2.Type == ObjectTypes.Point)
            {
                toDraw.Add(new Line(new Point(p1.Color,p1.Name), new Point(p2.Color,p2.Name)));
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


