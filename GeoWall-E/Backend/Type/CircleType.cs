namespace GeoWall_E
{
    public class Circle : Type, IDraw
    {
        public override ObjectTypes ObjectType => ObjectTypes.Circle;
        private Point Center_ { get; set; }
        private Measure Radius_ { get; set; }
        private string Name_ { get; set; }

        public Circle(Point center, Measure radius, string name = "")
        {
            Center_ = center;
            Radius_ = radius;
            Name_ = name;
        }

        public Point Center => Center_;

        public Measure Radius => Radius_;

        public string Name => Name_;       
    }
}