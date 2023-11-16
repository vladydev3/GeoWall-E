namespace GeoWallE;

public class LineExpression : Expression
{
    public override TokenType Type => TokenType.Line;
    public Token P1 { get; set; }
    public Token P2 { get; set; }
    public Color Color { get; set; }
    
    public LineExpression(Token p1, Token p2, Color color)
    {
        P1 = p1;
        P2 = p2;
        Color = color;
    }
}

public class SegmentExpression : Expression
{
    public override TokenType Type => TokenType.Segment;
    public Token Start { get; set; }
    public Token End { get; set; }
    public Color Color { get; set; }

    public SegmentExpression(Token start, Token end, Color color)
    {
        Start = start;
        End = end;
        Color = color;
    }
}

public class RayExpression : Expression
{
    public override TokenType Type => TokenType.Ray;
    public Token Start { get; set; }
    public Token End { get; set; }
    public Color Color { get; set; }

    public RayExpression(Token start, Token end, Color color)
    {
        Start = start;
        End = end;
        Color = color;
    }
}

public class ArcExpression : Expression
{
    public override TokenType Type => TokenType.Arc;
    public Token Center { get; set; }
    public Token Start { get; set; }
    public Token End { get; set; }
    public Token Measure { get; set; }
    public Color Color { get; set; }    

    public ArcExpression(Token center, Token start, Token end, Token measure, Color color)
    {
        Center = center;
        Start = start;
        End = end;
        Measure = measure;
        Color = color;
    }
}

public class CircleExpression : Expression
{
    public override TokenType Type => TokenType.Circle;
    public Token Center { get; set; }
    public Token Radius { get; set; }
    public Color Color { get; set; }

    public CircleExpression(Token center, Token radius, Color color)
    {
        Center = center;
        Radius = radius;
        Color = color;
    }
}

public class MeasureExpression : Expression
{
    public override TokenType Type => TokenType.Measure;
    public Token P1 { get; set; }
    public Token P2 { get; set; }
    public Color Color { get; set; }

    public MeasureExpression(Token p1, Token p2, Color color)
    {
        P1 = p1;
        P2 = p2;
        Color = color;
    }
}

public class IntersectExpression : Expression
{
    public override TokenType Type => TokenType.Intersect;
    public Token F1 { get; set; }
    public Token F2 { get; set; }
    public Color Color { get; set; }
    
    public IntersectExpression(Token f1, Token f2, Color color)
    {
        F1 = f1;
        F2 = f2;
        Color = color;
    }
}

