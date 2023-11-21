namespace GeoWall_E;


public class Measure : Type
{
    public override ObjectTypes Type => ObjectTypes.Measure;

    public Point P1 { get; set; }
    public Point P2 { get; set; }
    public string Name { get; set; }

    public Measure(Point p1, Point p2, string name = "")
    {
        Name = name;
        P1 = p1;
        P2 = p2;
    }
    public double GetMeasure() 
    {
        double measure = Math.Sqrt(Math.Pow(P2.X - P1.X, 2) + Math.Pow(P2.Y - P1.Y, 2));
        return measure;
    }
}
