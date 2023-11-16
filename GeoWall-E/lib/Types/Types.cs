namespace GeoWall_E;

public abstract class Types
{
    public abstract ObjectTypes Type { get; }
    
}

public class Point : Types
{
    public override ObjectTypes Type => ObjectTypes.Point;
    public double X { get; set; }
    public double Y { get; set; }
    public string Name { get; set; }

    public Point(string name = "")
    {
        Name = name;
    }
}


public class Line : Types
{
    public override ObjectTypes Type => ObjectTypes.Line;
    public Point P1 { get; set; }
    public Point P2 { get; set; }
    public string Name { get; set; }

    public Line(Point p1, Point p2, string name = "")
    {
        P1 = p1;
        P2 = p2;
        Name = name;
    }
}

public class Segment : Types
{
    public override ObjectTypes Type => ObjectTypes.Segment;
    public Point Start { get; set; }
    public Point End { get; set; }
    public string Name { get; set; }

    public Segment(Point start, Point end, string name = "")
    {
        Start = start;
        End = end;
        Name = name;
    }
}

public class Ray : Types
{
    public override ObjectTypes Type => ObjectTypes.Ray;
    public Point Start { get; set; }
    public Point End { get; set; }
    public string Name { get; set; }

    public Ray(Point start, Point end, string name = "")
    {
        Start = start;
        End = end;
        Name = name;
    }
}

public class Arc : Types
{
    public override ObjectTypes Type => ObjectTypes.Arc;
    public Point Center { get; set; }
    public Point Start { get; set; }
    public Point End { get; set; }
    public int Measure { get; set; }
    public string Name { get; set; }

    public Arc(Point center, Point start, Point end, int measure, string name = "")
    {
        Center = center;
        Start = start;
        End = end;
        Measure = measure;
        Name = name;
    }
}

public class Circle : Types
{
    public override ObjectTypes Type => ObjectTypes.Circle;
    public Point Center { get; set; }
    public int Radius { get; set; }
    public string Name { get; set; }

    public Circle(Point center, int radius, string name = "")
    {
        Center = center;
        Radius = radius;
        Name = name;
    }
}

public class Measure : Types
{
    public override ObjectTypes Type => ObjectTypes.Measure;
    
    public Point P1 { get; set; }
    public Point P2 { get; set; }

    public Measure(Point p1, Point p2)
    {
        P1 = p1;
        P2 = p2;
    }
}

public class Undefined : Types
{
    public override ObjectTypes Type => ObjectTypes.Undefined;
}

public class Sequence : Types
{
    public override ObjectTypes Type => ObjectTypes.Sequence;
    public List<Types> Elements { get; set; }

    public Sequence(List<Types> elements)
    {
        Elements = elements;
    }
}