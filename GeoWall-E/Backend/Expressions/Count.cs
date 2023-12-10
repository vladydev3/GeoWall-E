namespace GeoWall_E
{
    public class Count : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.Count;
        private Expression Sequence_ { get; set; }

        public Count(Expression sequence)
        {
            Sequence_ = sequence;
        }

        public Expression Sequence => Sequence_;

        public Type Evaluate(SymbolTable symbolTable, Error error, List<Tuple<Type, Color>> toDraw)
        {
            if (Sequence is not IEvaluable sequence)
            {
                error.AddError($"RUNTIME ERROR: Expression in count() isn't a sequence");
                return new ErrorType();
            }
            var sequenceEvaluated = sequence.Evaluate(symbolTable, error, toDraw);

            if (sequenceEvaluated is not Sequence seq)
            {
                error.AddError($"RUNTIME ERROR: Expression in count() isn't a sequence");
                return new ErrorType();
            }

            return new NumberLiteral(seq.Count());
        }
    }
}