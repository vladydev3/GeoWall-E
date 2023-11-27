namespace GeoWall_E
{
    public class Line : Type, IDraw
    {
        public override ObjectTypes ObjectType => ObjectTypes.Line;
        private Point P1_ { get; set; }
        private Point P2_ { get; set; }
        private string Name_ { get; set; }
        private Color Color_ { get; set; }

        public Line(Point p1, Point p2, Color color, string name = "")
        {
            P1_ = p1;
            P2_ = p2;
            Color_ = color;
            Name_ = name;
        }

        public Point P1 => P1_;

        public Point P2 => P2_;

        public string Name => Name_;

        public Color Color => Color_;

    }
}
