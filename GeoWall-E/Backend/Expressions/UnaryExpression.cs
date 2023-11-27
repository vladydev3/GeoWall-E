namespace GeoWall_E;

public class UnaryExpression : Expression, IEvaluable
{
    public override TokenType Type => TokenType.UnaryExpression;
    private Expression Operand_ { get; }
    private Token Operator_ { get; }

    public UnaryExpression(Token @operator, Expression operand)
    {
        Operator_ = @operator;
        Operand_ = operand;
    }

    public Token Operator => Operator_;

    public Expression Operand => Operand_;

    public Type Evaluate(SymbolTable symbolTable, Error error)
    {
        var evaluatedOperand = (IEvaluable)Operand;
        var operandResult = evaluatedOperand.Evaluate(symbolTable, error);
        if (operandResult.ObjectType == ObjectTypes.Error) return operandResult;
        if (operandResult.ObjectType == ObjectTypes.Number)
        {
            var number = (NumberLiteral)operandResult;
            if (Operator.Type == TokenType.Minus)
            {
                return new NumberLiteral(-number.Value);
            }
            else if (Operator.Type == TokenType.Plus)
            {
                return new NumberLiteral(number.Value);
            }
            else
            {
                error.AddError($"SEMANTIC ERROR: Unknown unary operator {Operator.Text}");
                return new ErrorType();
            }
        }
        else
        {
            error.AddError($"SEMANTIC ERROR: Unknown unary operator {Operator.Text}");
            return new Undefined();
        }
    }
}