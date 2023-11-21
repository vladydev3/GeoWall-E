namespace GeoWall_E;

public abstract class Statement : Node
{
    public abstract override TokenType Type { get; }
    public Errors? Errors { get; set; }
}