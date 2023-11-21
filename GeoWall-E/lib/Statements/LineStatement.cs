namespace GeoWall_E;

public class LineStatement : Statement
{
    private readonly override TokenType Type => TokenType.Line;
    private Token Name { get; set; }
    private bool IsSequence { get; set; }
    private Color Color { get; set; }

    public LineStatement(Token name, Color color, bool sequence = false)
    {
        Name = name;
        IsSequence = sequence;
        Color = color;
    }
}