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
        Token FileName_ {  get; set; }

        public ImportStatement(Token fileName) {
            FileName_ = fileName;
        }

        public Token FileName => FileName_;

        public void Import(SymbolTable symbolTable)
        {
            throw new NotImplementedException(); // TODO
        }
    }
}
