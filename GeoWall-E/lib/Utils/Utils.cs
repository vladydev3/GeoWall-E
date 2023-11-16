namespace GeoWall_E;

public enum Colors
{
    Black,
    White,
    Red,
    Green,
    Blue,
    Yellow,
    Magenta,
    Cyan,
    Gray
}

public class Color
{
    public Colors Color_ { get; set; }
    public Color(Colors color)
    {
        Color_ = color;
    }

    public void SetColor(Colors color)
    {
        Color_ = color;
    }
}

public enum ObjectTypes
{
    Point,
    Line,
    Segment,
    Ray,
    Circle,
    Arc,
    Measure,
    Sequence,
    Undefined,
}
