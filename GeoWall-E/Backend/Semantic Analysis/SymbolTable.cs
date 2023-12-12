namespace GeoWall_E;

public class SymbolTable
{
    private readonly Stack<Dictionary<string, Type>> scopes = new();

    public SymbolTable()
    {
        scopes.Push(new Dictionary<string, Type>()); // Global scope
    }

    public void EnterScope()
    {
        scopes.Push(new Dictionary<string, Type>());
    }

    public void ExitScope()
    {
        scopes.Pop();
    }

    public void Define(string name, Type value)
    {
        scopes.Peek()[name] = value;
    }

    public Type Resolve(string name)
    {
        foreach (var scope in scopes)
        {
            if (scope.TryGetValue(name, out Type value))
            {
                return value;
            }
        }
        return new ErrorType();
    }

    public Type ResolveFunction(string name)
    {
        foreach (var scope in scopes)
        {
            if (scope.TryGetValue(name, out Type value))
            {
                if (value.ObjectType == ObjectTypes.Function)
                {
                    return value;
                }
            }
        }
        return new ErrorType();
    }

    public void Merge(SymbolTable symbolTable, Error error)
    {
        foreach (var scope in symbolTable.scopes)
        {
            foreach (var (key, value) in scope)
            {
                Define(key, value);
            }
        }
    }
}