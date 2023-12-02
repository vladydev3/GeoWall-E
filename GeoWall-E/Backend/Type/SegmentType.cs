namespace GeoWall_E
{
    public class Segment : Type, IDraw
    {
        public override ObjectTypes ObjectType => ObjectTypes.Segment;
        private Point Start_ { get; set; }
        private Point End_ { get; set; }
        private string Name_ { get; set; }

        public Segment(Point start, Point end, string name = "")
        {
            Start_ = start;
            End_ = end;
            Name_ = name;
        }

        public Point Start => Start_;

        public Point End => End_;

        public string Name => Name_;

        public void SetName(string name)
        {
            Name_ = name;
        }
    }
}