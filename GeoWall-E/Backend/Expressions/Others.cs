namespace GeoWall_E;

public class Count : Expression
{
    public override TokenType Type => TokenType.Count;
    public Token Sequence { get; set; }

    public Count(Token sequence)
    {
        Sequence = sequence;
    }
}

public class Randoms : Expression
{
    public override TokenType Type => TokenType.Randoms;
}

public class RandomPointsInFigure : Expression
{
    public override TokenType Type => TokenType.Points;
    public Token Figure { get; set; }

    public RandomPointsInFigure(Token figure)
    {
        Figure = figure;
    }
}

public class Samples : Expression // random points
{
    public override TokenType Type => TokenType.Samples;
}
