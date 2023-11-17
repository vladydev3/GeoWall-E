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
                    }
                    else
                    {
                        Errors.AddError($"Variable {variable.Name.Text} not declared, Line: {variable.Name.Line}, Column: {variable.Name.Column}");
                    }
                }
            }
        }
        return toDraw;
    }

}