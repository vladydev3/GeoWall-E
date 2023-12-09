namespace GeoWall_E
{
    public class VariableExpression : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.Variable;
        private Token Name_ { get; set; }
        public VariableExpression(Token name)
        {
            Name_ = name;
        }

        public Token Name => Name_;

        public Type Evaluate(SymbolTable table, Error error, List<Tuple<Type, Color>> toDraw)
        {
            return table.Resolve(Name.Text);
        }
    }
}