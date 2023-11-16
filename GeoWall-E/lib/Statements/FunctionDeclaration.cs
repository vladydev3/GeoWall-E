namespace GeoWallE;

public class FunctionDeclaration : Statement
{
    public override TokenType Type => TokenType.FunctionDeclaration;
    public Token Name { get; set; }
    public List<Expression> Arguments { get; set; }
    public Expression Body { get; set; }

    public FunctionDeclaration(Token name, List<Expression> arguments, Expression body)
    {
        Name = name;
        Arguments = arguments;
        Body = body;
    }
}
