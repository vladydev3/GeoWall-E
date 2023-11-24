namespace GeoWall_E
{
    /// <summary>
    /// Establece la conexión entre la parte visual y el compilador.
    /// </summary>
    public class Handler
    {
        private readonly Error errors;
        private readonly List<Type> toDraw;
        public Handler(string code)
        {
            var lexer = new Lexer(code);
            var parser = new Parser(lexer.Tokenize(), lexer.Errors);
            var ast = parser.Parse_();
            var evaluator = new Evaluator(ast.Root, ast.Errors);
            toDraw = evaluator.Evaluate();
            errors = Evaluator.Errors;
        }

        public bool CheckErrors()
        {
            return errors.AnyError();
        }

        public List<Type> ToDraw => toDraw;

        public Error Errors => errors;
    }
}