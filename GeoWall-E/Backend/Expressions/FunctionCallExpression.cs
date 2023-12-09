namespace GeoWall_E;

public class FunctionCallExpression : Expression, IEvaluable
{
    public override TokenType Type => TokenType.FunctionCallExpression;
    private Token FunctionName_ { get; set; }
    private List<Expression> Arguments_ { get; set; }
    private int depth = 0;

    public FunctionCallExpression(Token functionName, List<Expression> arguments)
    {
        FunctionName_ = functionName;
        Arguments_ = arguments;
    }

    public Token FunctionName => FunctionName_;
    public List<Expression> Arguments => Arguments_;

    public Type Evaluate(SymbolTable symbolTable, Error error, List<Tuple<Type, Color>> toDraw)
    {
        depth++;

        if (depth > 500)
        {
            error.AddError($"RUNTIME ERROR: StackOverflow");
            return new ErrorType();
        }
        var function = symbolTable.Resolve(FunctionName.Text);

        var functionDefined = (Function)function;

        Dictionary<string, Type> argumentsDefined = new();
        for (int i = 0; i < Arguments.Count; i++) // evaluo los argumentos y los agrego al dictionary
        {
            try
            {
                var arg = (IEvaluable)Arguments[i];
                var evaluatedArgument = arg.Evaluate(symbolTable, error, toDraw);
                if (evaluatedArgument.ObjectType == ObjectTypes.Error) return evaluatedArgument;
                argumentsDefined.Add(functionDefined.Arguments[i].Text, evaluatedArgument);
            }
            catch (Exception)
            {
                error.AddError($"RUNTIME ERROR: Can't evaluate argument {i + 1} of function {FunctionName.Text}");
                return new ErrorType();
            }
        }

        symbolTable.EnterScope();
        foreach (var argument in argumentsDefined)
        {
            symbolTable.Define(argument.Key, argument.Value);
        }
        var body = (IEvaluable)functionDefined.Body;
        var result = body.Evaluate(symbolTable, error, toDraw);
        symbolTable.ExitScope();
        return result;
    }
}
