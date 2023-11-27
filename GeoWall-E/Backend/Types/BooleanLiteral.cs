namespace GeoWall_E
{
    public class BooleanLiteral : Type
    {
        public override ObjectTypes ObjectType { get; } = ObjectTypes.Boolean;
        
        public int Value { get; }

        public BooleanLiteral(bool value)
        {
            if (value) Value = 1;
            else Value = 0;
        }
    }
}