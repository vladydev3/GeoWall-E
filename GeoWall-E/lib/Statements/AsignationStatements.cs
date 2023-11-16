namespace GeoWall_E;

public class AsignationStatement : Statement
{
    public override TokenType Type => TokenType.AsignationStatement;
    public Token Name { get; set; }
    public Expression Value { get; set; }

    public AsignationStatement(Token name, Expression value)
    {
        Name = name;
        Value = value;
    }
}