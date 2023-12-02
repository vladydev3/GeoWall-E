namespace GeoWall_E
{
    public class Measure : Type
    {
        public override ObjectTypes ObjectType => ObjectTypes.Measure;

        private Point P1_ { get; set; }
        private Point P2_ { get; set; }
        private string Name_ { get; set; }
        private double Measure_ { get; set; }

        public Measure(Point p1, Point p2, string name = "")
        {
            Name_ = name;
            P1_ = p1;
            P2_ = p2;
            Measure_ = Math.Sqrt(Math.Pow(P2.X - P1.X, 2) + Math.Pow(P2.Y - P1.Y, 2));
        }

        public Measure(double measure, string name = "")
        {
            Name_ = name;
            Measure_ = measure;
            P1_ = new Point(new Color(Colors.Black));
            P2_ = new Point(new Color(Colors.Black));
        }

        public Point P1 => P1_;

        public Point P2 => P2_;

        public string Name => Name_;

        public double Value => Measure_;

        public double GetMeasure()
        {
            double measure = Math.Sqrt(Math.Pow(P2.X - P1.X, 2) + Math.Pow(P2.Y - P1.Y, 2));
            return measure;
        }
    }
}
