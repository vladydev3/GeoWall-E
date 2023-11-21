namespace GeoWall_E;

public abstract class Expression : Node
{
    public abstract override TokenType Type { get; }
    public Errors? Errors { get; set; }
}