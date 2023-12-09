namespace GeoWall_E{
    public class Constant : Type
    {
        public override ObjectTypes ObjectType { get; } = ObjectTypes.Constant;
        private Type Value_ { get; set; }
        private string Name_ { get; set; }

        public Constant(Type value, string name = "")
        {
            Value_ = value;
            Name_ = name;
        }

        public Type Value => Value_;

        public string Name => Name_;
    }
}