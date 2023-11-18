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
            if (node is PointStatement point)
            {
                VariableScope.Add(new DeclaredVariable(point.Name.Text, ObjectTypes.Point, point.Color, point.IsSequence));
                continue;
            }
            if (node is LineStatement line)
            {
                VariableScope.Add(new DeclaredVariable(line.Name.Text, ObjectTypes.Line, line.Color, line.IsSequence));
                continue;
            }

            if (node is SegmentStatement segment)
            {
                VariableScope.Add(new DeclaredVariable(segment.Name.Text, ObjectTypes.Segment, segment.Color, segment.IsSequence));
                continue;
            }
            if (node is RayStatement ray)
            {
                VariableScope.Add(new DeclaredVariable(ray.Name.Text, ObjectTypes.Ray, ray.Color, ray.IsSequence));
                continue;
            }

            if (node is Draw draw)
            {
                if (draw.Expression is VariableExpression variable)
                {
                    var variableToDraw = VariableScope.Find(x => x.Name == variable.Name.Text);
                    if (variableToDraw != null)
                    {
                        if (variableToDraw.Type == ObjectTypes.Point)
                        {
                            toDraw.Add(new Point(variableToDraw.Color ?? new Color(Colors.Black), variableToDraw.Name));
                        }
                        else if (variableToDraw.Type == ObjectTypes.Line)
                        {
                            toDraw.Add(new Line(new Point(variableToDraw.Color), new Point(variableToDraw.Color), variableToDraw.Name));
                        }
                        else if (variableToDraw.Type == ObjectTypes.Segment)
                        {
                            toDraw.Add(new Segment(new Point(variableToDraw.Color), new Point(variableToDraw.Color), variableToDraw.Name));
                        }
                        else if (variableToDraw.Type == ObjectTypes.Ray)
                        {
                            toDraw.Add(new Ray(new Point(variableToDraw.Color), new Point(variableToDraw.Color), variableToDraw.Name));
                        }
                    }
                    else
                    {
                        Errors.AddError($"Variable {variable.Name.Text} not declared, Line: {variable.Name.Line}, Column: {variable.Name.Column}");
                    }
                    continue;
                }
                if (draw.Expression is LineExpression lineexp)
                {
                    // looking for P1 and P2 in the scope
                    var p1 = VariableScope.Find(x => x.Name == lineexp.P1.Text);
                    var p2 = VariableScope.Find(x => x.Name == lineexp.P2.Text);
                    if (p1 != null && p2 != null)
                    {
                        if (p1.Type == ObjectTypes.Point && p2.Type == ObjectTypes.Point)
                        {
                            toDraw.Add(new Line(new Point(p1.Color), new Point(p2.Color)));
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
        }
        return toDraw;
    }

}