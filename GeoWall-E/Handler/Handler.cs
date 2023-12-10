using ICSharpCode.AvalonEdit.Rendering;
using System.Runtime.CompilerServices;

namespace GeoWall_E
{
    /// <summary>
    /// Establece la conexión entre la parte visual y el compilador.
    /// </summary>
    public class Handler
    {
        private readonly string code;
        private Error errors;
        private List<Tuple<Type, Color>> toDraw;
        private List<Token> tokens;
        private AST? ast;
        private Evaluator? evaluator;

        public Handler(string code)
        {
            this.code = code;
            errors = new Error();
            toDraw = new List<Tuple<Type, Color>>();
            tokens = new List<Token>();

            HandleLexer();
            HandleParse();
            HandleSemantic();
        }

        public void HandleEvaluate()
        {
            evaluator.Evaluate();
            toDraw = evaluator.ToDraw;
        }

        public void HandleLexer()
        {
            var lexer = new Lexer(code);
            tokens = lexer.Tokenize();
            errors = lexer.Errors;
        }

        public void HandleParse()
        {
            var parser = new Parser(tokens, errors);
            ast = parser.Parse_();
            errors = parser.Errors;
        }

        public void HandleSemantic()
        {
            if (ast == null) return;
            evaluator = new Evaluator(ast.Root, errors);
        }

        public bool CheckErrors()
        {
            return errors.AnyError();
        }

        public List<Tuple<Type, Color>> ToDraw => toDraw;

        public Error Errors => errors;
    }
}