using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoWall_E
{
    internal class MeasureStatement : Statement
    {
        public override TokenType Type => TokenType.Measure;
        Token Name_ {  get; set; }
        bool IsSequence_ { get; set; }

        public MeasureStatement(Token name, bool isSequence = false)
        {
            Name_ = name;
            IsSequence_ = isSequence;
        }

        public Token Name => Name_;
        public bool IsSequence => IsSequence_;


    }
}
