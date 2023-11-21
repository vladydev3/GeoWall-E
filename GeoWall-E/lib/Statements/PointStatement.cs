namespace GeoWall_E;

public class PointStatement : Statement
{
    private readonly override TokenType Type => TokenType.Point;
    private Token Name { get; set; }
    private bool IsSequence { get; set; }
    private Color Color { get; set; }

    public PointStatement(Token name, Color color, bool sequence = false)
    {
        Name = name;
        IsSequence = sequence;
        Color = color;
    }
}