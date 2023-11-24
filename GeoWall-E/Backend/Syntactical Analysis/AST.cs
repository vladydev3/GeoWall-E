namespace GeoWall_E;

public class AST
{
    private List<Node> Root_ { get; set; }
    private Error Errors_ { get; set; }
    public AST(List<Node> root, Error errors)
    {
        Root_ = root;
        Errors_ = errors;
    }
    public List<Node> Root => Root_;
    public Error Errors => Errors_;
    public void SetErrors(string error)
    {
        Errors.AddError(error);
    }
}