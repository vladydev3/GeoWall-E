namespace GeoWall_E
{
    public class Draw : Expression
    {
        public override TokenType Type => TokenType.Draw;

        private string Name_ { get; set; }
        private Expression? Expression_ { get; set; }
        private List<VariableExpression>? Sequence_ { get; set; }

        public Draw(Expression expression, string name = "")
        {
            Expression_ = expression;
            Name_ = name;
        }

        public Draw(List<VariableExpression> ids, string name = "")
        {
            Sequence_ = ids;
            Name_ = name;
        }

        public string Name => Name_;

        public Expression? Expression => Expression_;

        public List<VariableExpression>? Sequence => Sequence_;
    }
}