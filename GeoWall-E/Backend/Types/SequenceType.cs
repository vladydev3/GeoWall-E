namespace GeoWall_E
{

    public class Sequence : Type
    {
        public override ObjectTypes ObjectType => ObjectTypes.Sequence;
        public List<Type> Elements { get; set; }

        public Sequence(List<Type> elements)
        {
            Elements = elements;
        }
    }
}