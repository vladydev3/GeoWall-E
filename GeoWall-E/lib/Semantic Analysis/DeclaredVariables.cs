namespace GeoWall_E;

public class DeclaredVariable
{
    public string? Name { get; set; }
    public ObjectTypes? Type { get; set; }
    public Expression? Value { get; set; }
    public bool? IsSequence { get; set; }
    public bool? IsConstant { get; set; }
    public Color? Color { get; set; }
    public Token? Point1 { get; set; }
    public Token? Point2 { get; set; }

    public DeclaredVariable(string name, ObjectTypes type, Color color, bool sequence = false)
    {
        Name = name;
        Type = type;
        IsSequence = sequence;
        Color = color;
    }

    public DeclaredVariable(string name, Expression value, bool constant = true)
    {
        Name = name;
        Value = value;
        IsConstant = constant;
    }

    public DeclaredVariable(ObjectTypes type, Token point1, Token point2)
    {
        Type = type;
        Point1 = point1;
        Point2 = point2;
    }
}