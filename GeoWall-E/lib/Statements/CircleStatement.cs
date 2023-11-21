namespace GeoWall_E;

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