namespace GeoWall_E
{
    public class Circle : Type, IDraw
    {
        public override ObjectTypes ObjectType => ObjectTypes.Circle;
        private Point Center_ { get; set; }
        private Measure Radius_ { get; set; }
        private Color Color_ { get; set; }
        private string Name_ { get; set; }

        public Circle(Point center, Measure radius, Color color, string name = "")
        {
            Center_ = center;
            Radius_ = radius;
            Color_ = color;
            Name_ = name;
        }

        public Point Center => Center_;

        public Measure Radius => Radius_;

        public Color Color => Color_;

        public string Name => Name_;       
    }
}