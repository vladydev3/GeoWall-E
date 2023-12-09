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

        public Type Evaluate(SymbolTable table, Error error)
        {
            var variable = table.Resolve(Name.Text);
            if (variable is ErrorType)
            {
                error.AddError($"SEMANTIC ERROR: Variable {Name.Text} is not defined");
            }
            return variable;
        }
    }
}