namespace GeoWall_E
{
    public class MultipleAsignationStatement : Statement
    {
        public override TokenType Type => TokenType.AsignationStatement;
        List<Token> IDs_ { get; set; }
        Expression Value_ { get; set; }

        public MultipleAsignationStatement(List<Token> ids, Expression value)
        {
            IDs_ = ids;
            Value_ = value;
        }

        public List<Token> IDs => IDs_;
        public Expression Value => Value_;
    }
}