using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoWall_E
{
    class ImportStatement : Statement
    {
        public override TokenType Type => TokenType.Import;
        Token FileName_ { get; set; }

        public ImportStatement(Token fileName)
        {
            FileName_ = fileName;
        }

        public Token FileName => FileName_;

        public SymbolTable Import()
        {
            // Importar archivo .gs
            // Ruta actual
            var currentPath = System.IO.Directory.GetCurrentDirectory();
            // Route to the file ubicated in the folder SavedFiles
            var fileName = $"{currentPath}/../../../SavedFiles/{FileName.Text}";
            var file = System.IO.File.ReadAllText(fileName);
            var lexer = new Lexer(file);
            var parser = new Parser(lexer.Tokenize(), lexer.Errors);
            var statements = parser.Parse_();
            var errors = parser.Errors;
            if (statements == null) return new SymbolTable();
            var evaluator = new Evaluator(statements.Root, errors);
            evaluator.Evaluate();
            return evaluator.SymbolTable_;
        }
    }
}
