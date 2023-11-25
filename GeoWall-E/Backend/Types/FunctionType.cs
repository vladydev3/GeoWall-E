using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoWall_E
{
    public class Function: Type
    {
        public override ObjectTypes ObjectType => ObjectTypes.Function;
        private Token Name_ { get; }
        private List<Expression> Arguments_ { get; }
        private Expression Body_ { get; }

        public Function(Token name, List<Expression> arguments, Expression body)
        {
            Name_ = name;
            Arguments_ = arguments;
            Body_ = body;
        }

        public Token Name => Name_;

        public List<Expression> Arguments => Arguments_;

        public Expression Body => Body_;
    }
}
