namespace GeoWall_E;

public class SegmentStatement : Statement
{
    private readonly override TokenType Type => TokenType.Segment;
    private Token Name { get; set; }
    private bool IsSequence { get; set; }
    private Color Color { get; set; }

    public SegmentStatement(Token name, Color color, bool sequence = false)
    {
        Name = name;
        IsSequence = sequence;
        Color = color;
    }
}