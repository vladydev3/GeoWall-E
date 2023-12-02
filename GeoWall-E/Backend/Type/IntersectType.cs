namespace GeoWall_E
{
    public class Intersect : Type
    {
        public override ObjectTypes ObjectType { get; }
        Sequence Intersection_ { get; set; }

        public Intersect(Sequence intersection)
        {
            Intersection_ = intersection;
        }

        public Sequence Intersection => Intersection_;
    }
}