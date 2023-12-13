namespace GeoWall_E
{
    internal enum TypeInfered
    {
        Number,
        String,
        Point,
        Line,
        Circle,
        Ray,
        Arc,
        Segment,
        Measure,
        Any,
        ErrorType
    }
    internal class TypeInference
    {
        readonly SymbolTable SymbolTable_;

        public TypeInference(SymbolTable symbolTable)
        {
            SymbolTable_ = symbolTable;
        }

        public SymbolTable SymbolTable => SymbolTable_;

        internal TypeInfered InferType(Node node)
        {
            switch (node)
            {

                case IntersectExpression:
                    return TypeInfered.Point;
                case ErrorExpression:
                case ErrorStatement:
                    return TypeInfered.ErrorType;
                case NumberExpression:
                    return TypeInfered.Number;
                case StringExpression:
                    return TypeInfered.String;
                case PointStatement:
                    return TypeInfered.Point;
                case LineExpression:
                case LineStatement:
                    return TypeInfered.Line;
                case CircleExpression:
                case CircleStatement:
                    return TypeInfered.Circle;
                case RayExpression:
                case RayStatement:
                    return TypeInfered.Ray;
                case ArcExpression:
                    return TypeInfered.Arc;
                case SegmentExpression:
                case SegmentStatement:
                    return TypeInfered.Segment;
                case MeasureExpression:
                case MeasureStatement:
                    return TypeInfered.Measure;
                case FunctionCallExpression:
                    return TypeInfered.Any;
                case VariableExpression identifier:
                    var symbol = SymbolTable.Resolve(identifier.Name.Text);
                    return InferType(symbol);
                case BinaryExpression binary:
                    var left = InferType(binary.Left);
                    var right = InferType(binary.Right);
                    if (left == TypeInfered.ErrorType || right == TypeInfered.ErrorType) return TypeInfered.ErrorType;
                    if (left == TypeInfered.Any) return right;
                    if (right == TypeInfered.Any) return left;
                    if (left == TypeInfered.Measure && right == TypeInfered.Measure && binary.Operator.Text == "/") return TypeInfered.Number;
                    if (left == right) return left;
                    if (left == TypeInfered.Measure || right == TypeInfered.Measure) return TypeInfered.Measure;
                    return TypeInfered.Any;
                case UnaryExpression unary:
                    return InferType(unary.Operand);
                case SequenceExpression sequence:
                    if (sequence.Elements != null && sequence.Elements.Count == 0) return TypeInfered.Any;
                    return InferType(sequence.Elements[0]);
                case FunctionDeclaration:
                    return TypeInfered.Any;
                case LetInExpression letIn:
                    return InferType(letIn.In);
                case IfExpression ifExpression:
                    var then = InferType(ifExpression.Then);
                    var else_ = InferType(ifExpression.Else);
                    if (then == TypeInfered.ErrorType || else_ == TypeInfered.ErrorType) return TypeInfered.ErrorType;
                    if (then == TypeInfered.Any) return else_;
                    if (else_ == TypeInfered.Any) return then;
                    if (then == else_) return then;
                    return TypeInfered.Any;
                default:
                    return TypeInfered.Any;
            }
        }

        internal TypeInfered InferType(Type type)
        {
            return type switch
            {
                NumberLiteral => TypeInfered.Number,
                StringLiteral => TypeInfered.String,
                Point => TypeInfered.Point,
                Line => TypeInfered.Line,
                Circle => TypeInfered.Circle,
                Ray => TypeInfered.Ray,
                Arc => TypeInfered.Arc,
                Segment => TypeInfered.Segment,
                Measure => TypeInfered.Measure,
                Intersect => TypeInfered.Point,
                Sequence sequence => CheckSequenceType(sequence),
                ErrorType => TypeInfered.ErrorType,
                _ => TypeInfered.Any,
            };
        }

        private TypeInfered CheckSequenceType(Sequence sequence)
        {
            if (sequence.Count() == 0) return TypeInfered.Any;
            return InferType(sequence.GetElement(0));
        }
    }
}
