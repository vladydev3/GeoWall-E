
namespace GeoWall_E
{
    public enum ObjectTypes
    {
        String,
        Number,
        Constant,
        Point,
        Line,
        Segment,
        Ray,
        Circle,
        Arc,
        Measure,
        Sequence,
        Undefined,
        Error,
    }

    public abstract class Type
    {
        public abstract ObjectTypes ObjectType { get; }

    }
}