namespace GeoWall_E
{
    public class Ray : Type, IDraw
    {
        public override ObjectTypes ObjectType => ObjectTypes.Ray;
        private Point Start_ { get; set; }
        private Point End_ { get; set; }
        private string Name_ { get; set; }
        private Color Color_ { get; set; }

        public Ray(Point start, Point end, Color color, string name = "")
        {
            Start_ = start;
            End_ = end;
            Color_ = color;
            Name_ = name;
        }
        public Point Start => Start_;

        public Point End => End_;

        public string Name => Name_;

        public Color Color => Color_;

        
    }
}
