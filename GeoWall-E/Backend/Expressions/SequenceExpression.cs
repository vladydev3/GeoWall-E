namespace GeoWall_E
{
    public class SequenceExpression : Expression, IEvaluable
    {
        public override TokenType Type => TokenType.Sequence;
        private List<Expression>? Elements_ { get; set; }
        private Token? LowerBound_ { get; set; }
        private Token? UpperBound_ { get; set; }
        public SequenceExpression(List<Expression> elements)
        {
            Elements_ = elements;
        }

        public SequenceExpression(Token lowerBound, Token upperBound)
        {
            LowerBound_ = lowerBound;
            UpperBound_ = upperBound;
        }

        public SequenceExpression(Token lowerLimit)
        {
            LowerBound_ = lowerLimit;
        }

        public Token? LowerBound => LowerBound_;
        public Token? UpperBound => UpperBound_;

        public List<Expression>? Elements => Elements_;

        public Type Evaluate(SymbolTable symbolTable, Error error)
        {
            var sequenceElementsEvaluated = new List<Type>();
            if (Elements != null)
            {
                // Evaluate all elements of the sequence
                foreach (var element in Elements)
                {
                    var elementEvaluable = (IEvaluable)element;
                    var evaluatedElement = elementEvaluable.Evaluate(symbolTable, error);
                    if (evaluatedElement != null) sequenceElementsEvaluated.Add(evaluatedElement);
                }

                return new Sequence(sequenceElementsEvaluated);
            }
            if (LowerBound != null && UpperBound == null)
            {
                // If the upper bound is not specified, the sequence is infinite

                int lowerBound = int.Parse(LowerBound.Text);

                return new Sequence(Enumerable.Range(lowerBound, int.MaxValue - lowerBound).Select(x => new NumberLiteral(x)));
            }
            if (LowerBound != null && UpperBound != null)
            {
                // If the upper bound is specified, the sequence is finite

                int lowerBound = int.Parse(LowerBound.Text);
                int upperBound = int.Parse(UpperBound.Text);

                return new Sequence(Enumerable.Range(lowerBound, upperBound - lowerBound).Select(x => new NumberLiteral(x)));
            }
            return new ErrorType();
        }

        internal void HandleSequenceExpression(List<Tuple<Type, Color>> toDraw, Error errors, SymbolTable symbolTable, Color color, string name)
        {
            var sequenceElementsEvaluated = new List<Type>();
            if (Elements != null)
            {
                // Evaluate all elements of the sequence
                foreach (var element in Elements)
                {
                    var elementEvaluable = (IEvaluable)element;
                    var evaluatedElement = elementEvaluable.Evaluate(symbolTable, errors);
                    if (evaluatedElement != null) sequenceElementsEvaluated.Add(evaluatedElement);
                }

                Sequence sequence = new(sequenceElementsEvaluated);
                TypeInference typeInference = new(symbolTable);
                foreach (var element in sequence.Elements)
                {
                    if (element is not IDraw)
                    {
                        var type = typeInference.InferType(element);
                        errors.AddError($"RUNTIME ERROR: Variable in draw function is {SemanticChecker.InferedTypeToType(type).ObjectType}, which is not drawable.");
                    }
                    ((IDraw)element).SetName(name);
                    toDraw.Add(new Tuple<Type, Color>(element, color));
                }
                return;
            }
        }
    }
}