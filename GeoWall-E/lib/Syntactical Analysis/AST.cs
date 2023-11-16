namespace GeoWallE;

public class AST
{
    public List<Node> Root { get; set; }
    public Errors Errors { get; set; }
    public AST(List<Node> root, Errors errors)
    {
        Root = root;
        Errors = errors;
    }
}