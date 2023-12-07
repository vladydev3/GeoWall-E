namespace GeoWall_E;

public class Count : Expression, IEvaluable
{
    public override TokenType Type => TokenType.Count;
    private Expression Sequence_ { get; set; }

    public Count(Expression sequence)
    {
        Sequence_ = sequence;
    }

    public Expression Sequence => Sequence_;

    public Type Evaluate(SymbolTable symbolTable, Error error)
    {
        if (Sequence is not IEvaluable sequence)
        {
            error.AddError($"SEMANTIC ERROR: Expression in count() isn't a sequence");
            return new ErrorType();
        }
        var sequenceEvaluated = sequence.Evaluate(symbolTable, error);

        if (sequenceEvaluated is not Sequence seq)
        {
            error.AddError($"SEMANTIC ERROR: Expression in count() isn't a sequence");
            return new ErrorType();
        }

        return new NumberLiteral(seq.Count());
    }
}

public class Randoms : Expression, IEvaluable
{
    public override TokenType Type => TokenType.Randoms;

    public Type Evaluate(SymbolTable symbolTable, Error error)
    {
        var randoms = CreateRandoms();  //IEnumerable<NumberLiteral>
        return new Sequence(randoms);
    }

    IEnumerable<Type> CreateRandoms() // Create random numbers between 0 and 1
    {
        throw new NotImplementedException();
    }
}



public class Samples : Expression, IEvaluable // random points
{
    public override TokenType Type => TokenType.Samples;

    public Type Evaluate(SymbolTable symbolTable, Error error)
    {
        var points = CreatePoints();
        return new Sequence(points);
    }

    IEnumerable<Type> CreatePoints()
    {
        throw new NotImplementedException();
    }
}
