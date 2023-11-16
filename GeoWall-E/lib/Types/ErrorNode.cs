namespace GeoWall_E;

public class ErrorStatement : Statement
{
    public override TokenType Type => TokenType.Error;
}

public class ErrorExpression : Expression
{
    public override TokenType Type => TokenType.Error;
}