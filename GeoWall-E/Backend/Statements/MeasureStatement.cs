using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoWall_E
{
    internal class MeasureStatement : Statement, IFigureStatement
    {
        public override TokenType Type => TokenType.Measure;
        Token Name_ { get; set; }
        readonly bool IsSequence_ = false;

        public MeasureStatement(Token name, bool sequence = false)
        {
            Name_ = name;
            IsSequence_ = sequence;
        }

        public Token Name => Name_;
        public bool IsSequence => IsSequence_;

        public Sequence CreateSequence()
        {
            // crear una secuencia de medidas aleatorias, una cantidad aleatoria entre 10 y 10000
            var random = new Random();
            var measures = new List<Measure>();
            var count = random.Next(10, 10000);
            for (var i = 0; i < count; i++)
            {
                measures.Add(new Measure(random.Next(0, 9000)));
            }
            return new Sequence(measures);
        }
    }
}
