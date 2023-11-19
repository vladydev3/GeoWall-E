namespace GeoWall_E;

public class AsignationStatement : Statement
{
    public override TokenType Type => TokenType.AsignationStatement;
    public Token Name { get; set; }
    public Expression Value { get; set; }
    public Color Color { get; set; }

    public AsignationStatement(Token name, Expression value, Color color)
    {
        Name = name;
        Value = value;
        Color = color;
    }
}