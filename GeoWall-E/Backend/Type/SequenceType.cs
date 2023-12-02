using System.Windows.Markup;

namespace GeoWall_E
{

    public class Sequence : Type, IDraw
    {
        public override ObjectTypes ObjectType => ObjectTypes.Sequence;
        IEnumerable<Type> Elements_ { get; set; }
        string Name_ { get; set; }

        public Sequence(IEnumerable<Type> elements, string name = "")
        {
            Elements_ = elements;
            Name_ = name;
        }

        public int Count()
        {
            return Elements.Count();    // Devolver undefined si la secuencia es infinita
        }

        public IEnumerable<Type> Elements => Elements_;
        public string Name => Name_;

        public void SetName(string name)
        {
            Name_ = name;
        }
    }
}