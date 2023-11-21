namespace GeoWall_E;

public class AST
{
    private readonly List<Node> Root { get; set; }
    private Error Errors { get; set; }
    public AST(List<Node> root, Errors errors)
    {
        Root = root;
        Errors = errors;
    }
}