namespace GeoWallE;

public abstract class Node
{
    public abstract TokenType Type { get; }

    public override string ToString()
    {
        return Type.ToString();
    }
}

public abstract class Statement : Node
{
    public abstract override TokenType Type { get; }
    public Errors? Errors { get; set; }
}

public abstract class Expression : Node
{
    public abstract override TokenType Type { get; }
    public Errors? Errors { get; set; }
}

public class EmptyNode : Node
{
    public override TokenType Type => TokenType.Empty;
}