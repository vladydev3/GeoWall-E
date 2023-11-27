namespace GeoWall_E;

public class FunctionCallExpression : Expression, IEvaluable
{
    public override TokenType Type => TokenType.FunctionCallExpression;
    private Token FunctionName_ { get; set; }
    private List<Expression> Arguments_ { get; set; }

    public FunctionCallExpression(Token functionName, List<Expression> arguments)
    {
        FunctionName_ = functionName;
        Arguments_ = arguments;
    }

    public Token FunctionName => FunctionName_;
    public List<Expression> Arguments => Arguments_;

    public Type Evaluate(SymbolTable symbolTable, Error error)
    {
        var function = symbolTable.Resolve(FunctionName.Text);
        if (function.ObjectType != ObjectTypes.Function || function.ObjectType == ObjectTypes.Error)
        {
            error.AddError($"SEMANTIC ERROR: Function {FunctionName.Text} not defined");
            return function;
        }

        var functionDefined = (Function)function;

        if (Arguments.Count != functionDefined.Arguments.Count)
        {
            error.AddError($"SEMANTIC ERROR: Function {FunctionName.Text} expected {functionDefined.Arguments.Count} arguments, but {Arguments.Count} were given");
            return new ErrorType();
        }

        Dictionary<string, Type> argumentsDefined = new();
        for (int i = 0; i < Arguments.Count; i++) // evaluo los argumentos y los agrego al dictionary
        {
            try
            {
                var arg = (IEvaluable)Arguments[i];
                var evaluatedArgument = arg.Evaluate(symbolTable, error);
                if (evaluatedArgument.ObjectType == ObjectTypes.Error) return evaluatedArgument;
                argumentsDefined.Add(functionDefined.Arguments[i].Text, evaluatedArgument);
            }
            catch (Exception)
            {
                error.AddError($"SEMANTIC ERROR: Can't evaluate argument {i + 1} of function {FunctionName.Text}");
                return new ErrorType();
            }
        }

        symbolTable.EnterScope();
        foreach (var argument in argumentsDefined)
        {
            symbolTable.Define(argument.Key, argument.Value);
        }
        var body = (IEvaluable)functionDefined.Body;
        var result = body.Evaluate(symbolTable, error);
        symbolTable.ExitScope();
        return result;
    }
}