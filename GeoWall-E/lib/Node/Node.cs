namespace GeoWall_E;

public abstract class Node
{
    public abstract TokenType Type { get; }

    public override string ToString()
    {
        return Type.ToString();
    }
}
