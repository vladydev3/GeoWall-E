namespace GeoWall_E;

public class NumberExpression : Expression
{
    public override TokenType Type => TokenType.Number;
    public Token Number { get; set; }
    public NumberExpression(Token number)
    {
        Number = number;
    }
}

public class StringExpression : Expression
{
    public override TokenType Type => TokenType.String;
    public Token String { get; set; }
    public StringExpression(Token String)
    {
        this.String = String;
    }
}

public class SequenceExpression : Expression
{
    public override TokenType Type => TokenType.Sequence;
    public List<Token> Elements { get; set; }
    public SequenceExpression(List<Token> elements)
    {
        Elements = elements;
    }
}

public class VariableExpression : Expression
{
    public override TokenType Type => TokenType.Variable;
    public Token Name { get; set; }
    public int Scope { get; set; }
    public VariableExpression(Token name, int scope=0)
    {
        Name = name;
        Scope = scope;
    }
}