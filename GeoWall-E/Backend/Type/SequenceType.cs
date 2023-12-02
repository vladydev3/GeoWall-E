namespace GeoWall_E
{

    public class Sequence : Type
    {
        public override ObjectTypes ObjectType => ObjectTypes.Sequence;
        public IEnumerable<Type> Elements { get; set; }

        public Sequence(IEnumerable<Type> elements)
        {
            Elements = elements;
        }

        public int Count()
        {
            return Elements.Count();    // Devolver undefined si la secuencia es infinita
        }
    }
}