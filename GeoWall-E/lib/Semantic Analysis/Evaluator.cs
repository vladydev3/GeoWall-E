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
            if (node is LineExpression linexp)
            {
                VariableScope.Add(new DeclaredVariable(ObjectTypes.Line, linexp.P1, linexp.P2));
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
            }
        }
        return toDraw;
    }

}