namespace GeoWall_E
{
    public class NumberLiteral : Type
    {
        public override ObjectTypes ObjectType => ObjectTypes.Number;
        private double Value_ { get; set; }

        public NumberLiteral(double value)
        {
            Value_ = value;
        }

        public double Value => Value_;
    }
}