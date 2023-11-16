namespace GeoWall_E;

public class PointStatement : Statement
{
    public override TokenType Type => TokenType.Point;
    public Token Name { get; set; }
    public bool IsSequence { get; set; }
    public Color Color { get; set; }

    public PointStatement(Token name, Color color, bool sequence = false)
    {
        Name = name;
        IsSequence = sequence;
        Color = color;
    }
}

public class LineStatement : Statement
{
    public override TokenType Type => TokenType.Line;
    public Token Name { get; set; }
    public bool IsSequence { get; set; }
    public Color Color { get; set; }

    public LineStatement(Token name, Color color, bool sequence = false)
    {
        Name = name;
        IsSequence = sequence;
        Color = color;
    }
}

public class SegmentStatement : Statement
{
    public override TokenType Type => TokenType.Segment;
    public Token Name { get; set; }
    public bool IsSequence { get; set; }
    public Color Color { get; set; }

    public SegmentStatement(Token name, Color color, bool sequence = false)
    {
        Name = name;
        IsSequence = sequence;
        Color = color;
    }
}

public class RayStatement : Statement
{
    public override TokenType Type => TokenType.Ray;
    public Token Name { get; set; }
    public bool IsSequence { get; set; }
    public Color Color { get; set; }

    public RayStatement(Token name, Color color, bool sequence = false)
    {
        Name = name;
        IsSequence = sequence;
        Color = color;
    }
}

public class CircleStatement : Statement
{
    public override TokenType Type => TokenType.Circle;
    public Token Name { get; set; }
    public bool IsSequence { get; set; }
    public Color Color { get; set; }

    public CircleStatement(Token name, Color color, bool sequence = false)
    {
        Name = name;
        IsSequence = sequence;
        Color = color;
    }
}
