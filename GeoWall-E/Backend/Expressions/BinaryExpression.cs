using Accessibility;

namespace GeoWall_E;

public class BinaryExpression : Expression, IEvaluable
{
    public override TokenType Type => TokenType.BinaryExpression;
    private Expression Left_ { get; }
    private Token Operator_ { get; }
    private Expression Right_ { get; }

    public BinaryExpression(Expression left, Token @operator, Expression right)
    {
        Left_ = left;
        Operator_ = @operator;
        Right_ = right;
    }

    public Expression Left => Left_;
    public Token Operator => Operator_;
    public Expression Right => Right_;

    public Type Evaluate(SymbolTable symbolTable, Error error)
    {
        if (Left as IEvaluable != null && Right as IEvaluable != null)
        {
            var left = (IEvaluable)Left;
            var right = (IEvaluable)Right;
            Type leftResult = left.Evaluate(symbolTable, error);
            Type rightResult = right.Evaluate(symbolTable, error);

            if (leftResult is NumberLiteral literal && rightResult is NumberLiteral literal1)
            {
                var left_ = literal;
                var right_ = literal1;
                switch (Operator.Text)
                {
                    case "+":
                        return new NumberLiteral(left_.Value + right_.Value);
                    case "-":
                        return new NumberLiteral(left_.Value - right_.Value);
                    case "*":
                        return new NumberLiteral(left_.Value * right_.Value);
                    case "/":
                        if (right_.Value == 0)
                        {
                            error.AddError("SEMANTIC ERROR: Division by zero");
                            return new ErrorType();
                        }
                        return new NumberLiteral(left_.Value / right_.Value);
                    case "^":
                        return new NumberLiteral(Math.Pow(left_.Value, right_.Value));
                    case "%":
                        return new NumberLiteral(left_.Value % right_.Value);
                    case "<":
                        return new BooleanLiteral(left_.Value < right_.Value);
                    case ">":
                        return new BooleanLiteral(left_.Value > right_.Value);
                    case "<=":
                        return new BooleanLiteral(left_.Value <= right_.Value);
                    case ">=":
                        return new BooleanLiteral(left_.Value >= right_.Value);
                    case "==":
                        return new BooleanLiteral(left_.Value == right_.Value);
                    case "!=":
                        return new BooleanLiteral(left_.Value != right_.Value);
                    default:
                        error.AddError($"SEMANTIC ERROR: Operator {Operator.Text} not supported for Number type");
                        return new ErrorType();
                };
            }
            else if (leftResult is Measure measure1 && rightResult is Measure measure2)
            {
                switch (Operator.Text)
                {
                    case "+":
                        return new Measure(measure1.Value + measure2.Value);
                    case "-":
                        return new Measure(Math.Abs(measure1.Value - measure2.Value));
                    case "/":
                        return new NumberLiteral((int)(measure1.Value / measure2.Value));
                    default:
                        error.AddError($"SEMANTIC ERROR: Operator {Operator.Text} not supported for Measure type");
                        return new ErrorType();
                }
            }
            else if (leftResult is Measure measure && rightResult is NumberLiteral number)
            {
                switch (Operator.Text)
                {
                    case "*":
                        return new Measure(measure.Value * (int)number.Value);
                    default:
                        error.AddError($"SEMANTIC ERROR: Operator {Operator.Text} not supported for Measure and Number type");
                        return new ErrorType();
                }
            }
            else if (leftResult is NumberLiteral number1 && rightResult is Measure measure3)
            {
                switch (Operator.Text)
                {
                    case "*":
                        return new Measure(measure3.Value * (int)number1.Value);
                    default:
                        error.AddError($"SEMANTIC ERROR: Operator {Operator.Text} not supported for Measure and Number type");
                        return new ErrorType();
                }
            }
            else if (leftResult is BooleanLiteral boolean1 && rightResult is BooleanLiteral boolean2)
            {
                var left_ = boolean1;
                var right_ = boolean2;
                switch (Operator.Text)
                {
                    case "&&":
                        if (left_.Value == 1 && right_.Value == 1)
                            return new BooleanLiteral(true);
                        else
                            return new BooleanLiteral(false);
                    case "||":
                        if (left_.Value == 1 || right_.Value == 1)
                            return new BooleanLiteral(true);
                        else
                            return new BooleanLiteral(false);
                    default:
                        error.AddError($"SEMANTIC ERROR: Operator {Operator.Text} not supported for Boolean type");
                        return new ErrorType();
                }
            }
            else
            {
                error.AddError($"SEMANTIC ERROR: Invalid operands");
                return new ErrorType();
            }
        }
        error.AddError($"SEMANTIC ERROR: Invalid operands");
        return new ErrorType();
    }
}