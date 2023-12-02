namespace GeoWall_E
{
    public class Arc : Type, IDraw
    {
        public override ObjectTypes ObjectType => ObjectTypes.Arc;
        private Point Center_ { get; set; }
        private Point Start_ { get; set; }
        private Point End_ { get; set; }
        private Measure Measure_ { get; set; }
        private string Name_ { get; set; }

        public Arc(Point center, Point start, Point end, Measure measure, string name = "")
        {
            Center_ = center;
            Start_ = start;
            End_ = end;
            Measure_ = measure;
            Name_ = name;
        }

        public Point Center => Center_;

        public Point Start => Start_;

        public Point End => End_;   

        public Measure Measure => Measure_;

        public string Name => Name_;

    }
}