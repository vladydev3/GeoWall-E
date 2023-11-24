namespace GeoWall_E
{
    public class AsignationStatement : Statement
    {
        public override TokenType Type => TokenType.AsignationStatement;

        private Token Name_ { get; }
        private Expression Value_ { get; }
        private Color Color_ { get; }

        public AsignationStatement(Token name, Expression value, Color color)
        {
            Name_ = name;
            Value_ = value;
            Color_ = color;
        }

        public Token Name => Name_;

        public Expression Value => Value_;

        public Color Color => Color_;
    }
}