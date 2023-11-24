namespace GeoWall_E
{
    public class StringLiteral : Type
    {
        public override ObjectTypes ObjectType => ObjectTypes.String;
        private string Value_ { get; set; }

        public StringLiteral(string value)
        {
            Value_ = value;
        }

        public string Value => Value_;
    }
}