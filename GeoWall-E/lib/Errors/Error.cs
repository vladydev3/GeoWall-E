namespace GeoWall_E;

public class Error
{
    private List<string> diagnostics = new();

    public bool AnyError()
    {
        return !(diagnostics.Count==0);
    }

    public void ShowError()
    {
        var color = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(diagnostics[0]);
        diagnostics = new();
        Console.ForegroundColor = color;
    }

    public void AddError(string error, bool begin = false)
    {
        if (begin)
        {
            diagnostics.Insert(0, error);
            return;
        }
        diagnostics.Add(error);
    }

    public int Count()
    {
        return diagnostics.Count;
    }

    public void RemoveError()
    {
        diagnostics.RemoveAt(diagnostics.Count-1);
    }
}