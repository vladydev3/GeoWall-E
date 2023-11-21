namespace GeoWall_E;

public class FunctionDeclaration : Statement
{
    private readonly override TokenType Type => TokenType.FunctionDeclaration;
    private Token Name { get; set; }
    private List<Expression> Arguments { get; set; }
    private Expression Body { get; set; }

    public FunctionDeclaration(Token name, List<Expression> arguments, Expression body)
    {
        Name = name;
        Arguments = arguments;
        Body = body;
    }
}
