namespace GeoWall_E
{
    public class AsignationStatement : Statement
    {
        public override TokenType Type => TokenType.AsignationStatement;
        Token Name_ { get; }
        Expression Value_ { get; }

        public AsignationStatement(Token name, Expression value)
        {
            Name_ = name;
            Value_ = value;
        }

        public Token Name => Name_;

        public Expression Value => Value_;
    }
}