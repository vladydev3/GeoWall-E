namespace GeoWall_E;

public class Sequence : Type
{
    public override ObjectTypes Type => ObjectTypes.Sequence;
    public List<Type> Elements { get; set; }

    public Sequence(List<Types> elements)
    {
        Elements = elements;
    }
}