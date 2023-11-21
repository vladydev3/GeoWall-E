namespace GeoWall_E;

public class RayStatement : Statement
{
    private readonly override TokenType Type => TokenType.Ray;
    private Token Name { get; set; }
    private bool IsSequence { get; set; }
    private Color Color { get; set; }

    public RayStatement(Token name, Color color, bool sequence = false)
    {
        Name = name;
        IsSequence = sequence;
        Color = color;
    }
}