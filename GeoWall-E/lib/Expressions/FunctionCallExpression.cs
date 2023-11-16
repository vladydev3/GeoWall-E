namespace GeoWallE;

public class FunctionCallExpression : Expression
{
    public override TokenType Type => TokenType.FunctionCallExpression;
    public Token FunctionName { get; set; }
    public List<Expression> Arguments { get; set; }

    public FunctionCallExpression(Token functionName, List<Expression> arguments)
    {
        FunctionName = functionName;
        Arguments = arguments;
    }
}