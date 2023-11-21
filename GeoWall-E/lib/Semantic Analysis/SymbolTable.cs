namespace GeoWall_E;

public class SymbolTable
{
    private Stack<Dictionary<string, Type>> scopes = new Stack<Dictionary<string, Type>>();

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
        return ObjectTypes.Error;
    }
}