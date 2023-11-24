namespace GeoWall_E
{
    public class Parser
    {
        private readonly List<Token> tokens;
        private readonly Error errors;
        private int position;
        private Color PreviousColor = new(Colors.Black);
        private Color color = new(Colors.Black);

        public Parser(List<Token> tokens, Error errors)
        {
            this.tokens = tokens;
            this.errors = errors;
        }

        public Error Errors => errors;

        private Token Peek(int offset) => position + offset >= tokens.Count ? tokens[^1] : tokens[position + offset]; // Accede al token sin consumirlo

        private Token NextToken() => tokens[position++];

        private Token Current => Peek(0);

        private Token Match(TokenType Type) // comprueba si el token actual es el correcto
        {
            if (Current.Type == Type) return NextToken();
            errors.AddError($"Expected {Type} but got {Current.Type}, Line: {Current.Line}, Column: {Current.Column}");
            return new Token(TokenType.Error, "", Current.Line, Current.Column);
        }

        public AST Parse_()
        {
            List<Node> nodes = ParseMembers();
            Match(TokenType.EOF);
            return new AST(nodes, errors);
        }

        private List<Node> ParseMembers()
        {
            List<Node> members = new();

            while (Current.Type != TokenType.EOF)
            {
                var startToken = Current;

                members.Add(ParseMember());

                if (Current == startToken) NextToken();
            }
            return members;
        }

        private Node ParseMember()
        {
            if (Current.Type == TokenType.Identifier && Peek(1).Type == TokenType.LParen)
            {
                var function = ParseFunction();
                if (function is FunctionDeclaration) return function;
            }
            return ParseStatement();
        }

        private Node ParseFunction(bool canBeDeclaration = true)
        {
            var name = NextToken();
            NextToken();
            List<Expression> arguments = new();
            if (Current.Type != TokenType.RParen)
            {
                arguments.Add(ParseExpression());
                int overflow = 0;
                while (Current.Type == TokenType.Comma)
                {
                    if (overflow > 100)
                    {
                        errors.AddError($"Can't parse, Line: {Current.Line}, Column: {Current.Column}");
                        return new ErrorExpression();
                    }
                    NextToken();
                    arguments.Add(ParseExpression());
                    overflow++;
                }
            }
            Match(TokenType.RParen);
            if (Current.Type == TokenType.Asignation && canBeDeclaration)
            {
                NextToken();
                var value = ParseExpression();
                Match(TokenType.EOL);
                return new FunctionDeclaration(name, arguments, value);
            }
            return new FunctionCallExpression(name, arguments);
        }

        private Node ParseStatement() => Current.Type switch
        {
            TokenType.Point => ParsePoint(),
            TokenType.Line => ParseLine(),
            TokenType.Segment => ParseSegment(),
            TokenType.Ray => ParseRay(),
            TokenType.Circle => ParseCircle(),
            TokenType.Arc => ParseArc(),
            TokenType.Measure => ParseMeasure(),
            TokenType.Intersect => ParseIntersect(),
            TokenType.Count => ParseCount(),
            TokenType.Randoms => ParseRandom(),
            TokenType.Points => ParsePoints(),
            TokenType.Samples => ParseSamples(),
            TokenType.Draw => ParseDraw(),
            TokenType.Color => ParseColor(),
            TokenType.Restore => ParseRestore(),
            TokenType.Identifier => ParseIdentifier(),
            _ => ParseExpression(),
        };

        private Node ParseRestore()
        {
            color = PreviousColor;
            NextToken();
            Match(TokenType.EOL);
            return new EmptyNode();
        }

        private Node ParseColor()
        {
            NextToken();
            switch (Current.Text)
            {
                case "black":
                    PreviousColor = new Color(color.GetColor());
                    color = new Color(Colors.Black);
                    break;
                case "white":
                    PreviousColor = new Color(color.GetColor());
                    color = new Color(Colors.White);
                    break;
                case "red":
                    PreviousColor = new Color(color.GetColor());
                    color = new Color(Colors.Red);
                    break;
                case "green":
                    PreviousColor = new Color(color.GetColor());
                    color = new Color(Colors.Green);
                    break;
                case "blue":
                    PreviousColor = new Color(color.GetColor());
                    color = new Color(Colors.Blue);
                    break;
                case "yellow":
                    PreviousColor = new Color(color.GetColor());
                    color = new Color(Colors.Yellow);
                    break;
                case "magenta":
                    PreviousColor = new Color(color.GetColor());
                    color = new Color(Colors.Magenta);
                    break;
                case "cyan":
                    PreviousColor = new Color(color.GetColor());
                    color = new Color(Colors.Cyan);
                    break;
                case "gray":
                    PreviousColor = new Color(color.GetColor());
                    color = new Color(Colors.Gray);
                    break;
                default:
                    errors.AddError($"Unexpected color {Current.Text}, Line: {Current.Line}, Column: {Current.Column}");
                    return new ErrorExpression();
            }
            NextToken();
            Match(TokenType.EOL);
            return new EmptyNode();
        }

        private Node ParseIdentifier()
        {
            var name = NextToken();
            if (Current.Type == TokenType.Asignation) return ParseAsignation(name);
            return new VariableExpression(name);
        }

        private Expression ParseExpression(int parentPrecedence = 0)
        {
            Expression left;
            var unaryOperatorPrecedence = Current.Type.GetUnaryOperatorPrecedence();
            if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
            {
                var operatorToken = NextToken();
                var operand = ParseExpression(unaryOperatorPrecedence);
                left = new UnaryExpression(operatorToken, operand);
            }
            else left = ParsePrimaryExpression();

            while (true)
            {
                var precedence = Current.Type.GetBinaryOperatorPrecedence();
                if (precedence == 0 || precedence <= parentPrecedence) break;
                var operatorToken = NextToken();
                var right = ParseExpression(precedence);
                left = new BinaryExpression(left, operatorToken, right);
            }
            return left;
        }

        private Expression ParsePrimaryExpression()
        {
            switch (Current.Type)
            {
                case TokenType.Line:
                    return ParseLineExpression();
                case TokenType.Segment:
                    return ParseSegmentExpression();
                case TokenType.Ray:
                    return ParseRayExpression();
                case TokenType.Circle:
                    return ParseCircleExpression();
                case TokenType.Arc:
                    return ParseArc();
                case TokenType.Measure:
                    return ParseMeasure();
                case TokenType.Intersect:
                    return ParseIntersect();
                case TokenType.Points:
                    return ParsePoints();
                case TokenType.Samples:
                    NextToken();
                    Match(TokenType.LParen);
                    Match(TokenType.RParen);
                    return new Samples();
                case TokenType.Identifier:
                    if (Peek(1).Type == TokenType.LParen) return (Expression)ParseFunction(false);
                    var name = NextToken();
                    return new VariableExpression(name);
                case TokenType.Number:
                    return new NumberExpression(NextToken());
                case TokenType.String:
                    return new StringExpression(NextToken());
                case TokenType.LParen:
                    NextToken();
                    var expression = ParseExpression();
                    Match(TokenType.RParen);
                    return new ParenExpression(expression);
                case TokenType.Let:
                    return ParseLet();
                case TokenType.If:
                    return ParseIf();
                default:
                    errors.AddError($"Unexpected token {Current.Type}, Line: {Current.Line}, Column: {Current.Column}");
                    return new ErrorExpression();
            }
        }

        private Node ParseAsignation(Token name)
        {
            NextToken();
            var value = ParseExpression();
            Match(TokenType.EOL);
            return new AsignationStatement(name, value, color);
        }

        private Expression ParseLet()
        {
            // NO PINCHA
            NextToken();
            var instructions = ParseMembers();
            Match(TokenType.In);
            var inExpression = ParseExpression();
            return new LetInExpression(instructions, inExpression);
        }

        private Expression ParseIf()
        {
            NextToken();
            var condition = ParseExpression();
            Match(TokenType.Then);
            var then = ParseExpression();
            Match(TokenType.Else);
            var @else = ParseExpression();
            return new IfExpression(condition, then, @else);
        }

        private Node ParseRandom()
        {
            NextToken();
            Match(TokenType.LParen);
            Match(TokenType.RParen);
            return new Randoms();
        }

        private Node ParseSamples()
        {
            NextToken();
            Match(TokenType.LParen);
            Match(TokenType.RParen);
            return new Samples();
        }

        private Node ParseDraw()
        {
            NextToken();
            if (Current.Type == TokenType.LBracket) return ParseDrawSequence();
            Expression exp = ParseExpression();
            if (Current.Type == TokenType.String)
            {
                var name = Current.Text;
                NextToken();
                Match(TokenType.EOL);
                return new Draw(exp, name);
            }
            Match(TokenType.EOL);
            return new Draw(exp);
        }

        private Node ParseDrawSequence()
        {
            NextToken();
            List<VariableExpression> ids = new();
            ids.Add(new VariableExpression(Match(TokenType.Identifier)));
            while (Current.Type != TokenType.RBracket)
            {
                Match(TokenType.Comma);
                ids.Add(new VariableExpression(Match(TokenType.Identifier)));
            }
            Match(TokenType.RBracket);
            Match(TokenType.EOL);
            return new Draw(ids);
        }

        private RandomPointsInFigure ParsePoints()
        {
            NextToken();
            Match(TokenType.LParen);
            var fig = Match(TokenType.Identifier);
            Match(TokenType.RParen);
            return new RandomPointsInFigure(fig);
        }

        private Count ParseCount()
        {
            NextToken();
            Match(TokenType.LParen);
            var sequence = Match(TokenType.Identifier);
            Match(TokenType.RParen);

            return new Count(sequence);
        }

        private IntersectExpression ParseIntersect()
        {
            NextToken();
            Match(TokenType.LParen);
            var f1 = Match(TokenType.Identifier);
            Match(TokenType.Comma);
            var f2 = Match(TokenType.Identifier);
            Match(TokenType.RParen);

            return new IntersectExpression(f1, f2, color);
        }

        private MeasureExpression ParseMeasure()
        {
            NextToken();
            Match(TokenType.LParen);
            var p1 = Match(TokenType.Identifier);
            Match(TokenType.Comma);
            var p2 = Match(TokenType.Identifier);
            Match(TokenType.RParen);

            return new MeasureExpression(p1, p2);
        }

        private ArcExpression ParseArc()
        {
            NextToken();
            Match(TokenType.LParen);
            var c = Match(TokenType.Identifier);
            Match(TokenType.Comma);
            var p1 = Match(TokenType.Identifier);
            Match(TokenType.Comma);
            var p2 = Match(TokenType.Identifier);
            Match(TokenType.Comma);
            var m = Match(TokenType.Identifier);
            Match(TokenType.RParen);

            return new ArcExpression(c, p1, p2, m, color);
        }

        private Node ParseCircle()
        {
            NextToken();
            if (Current.Type == TokenType.LParen) return ParseCircleExpression();
            if (Current.Type == TokenType.Sequence)
            {
                NextToken();
                var id_ = Match(TokenType.Identifier);
                Match(TokenType.EOL);
                return new CircleStatement(id_, color, true);
            }
            var id = Match(TokenType.Identifier);
            Match(TokenType.EOL);
            return new CircleStatement(id, color);
        }

        private CircleExpression ParseCircleExpression()
        {
            Match(TokenType.Circle);
            Match(TokenType.LParen);
            var c = Match(TokenType.Identifier);
            Match(TokenType.Comma);
            var m = Match(TokenType.Identifier);
            Match(TokenType.RParen);
            return new CircleExpression(c, m, color);
        }

        private Node ParseRay()
        {
            NextToken();
            if (Current.Type == TokenType.LParen) return ParseRayExpression();
            if (Current.Type == TokenType.Sequence)
            {
                NextToken();
                var id_ = Match(TokenType.Identifier);
                Match(TokenType.EOL);
                return new RayStatement(id_, color, true);
            }
            var id = Match(TokenType.Identifier);
            Match(TokenType.EOL);
            return new RayStatement(id, color);
        }

        private RayExpression ParseRayExpression()
        {
            Match(TokenType.Ray);
            Match(TokenType.LParen);
            var p1 = Match(TokenType.Identifier);
            Match(TokenType.Comma);
            var p2 = Match(TokenType.Identifier);
            Match(TokenType.RParen);
            return new RayExpression(p1, p2, color);
        }

        private Node ParseSegment()
        {
            NextToken();
            if (Current.Type == TokenType.LParen) return ParseSegmentExpression();

            if (Current.Type == TokenType.Sequence)
            {
                NextToken();
                var id_ = Match(TokenType.Identifier);
                Match(TokenType.EOL);
                return new SegmentStatement(id_, color, true);
            }

            var id = Match(TokenType.Identifier);
            Match(TokenType.EOL);
            return new SegmentStatement(id, color);
        }

        private SegmentExpression ParseSegmentExpression()
        {
            Match(TokenType.Segment);
            Match(TokenType.LParen);
            var p1 = Match(TokenType.Identifier);
            Match(TokenType.Comma);
            var p2 = Match(TokenType.Identifier);
            Match(TokenType.RParen);
            return new SegmentExpression(p1, p2, color);
        }

        private Node ParseLine()
        {
            Match(TokenType.Line);
            if (Current.Type == TokenType.LParen) return ParseLineExpression();

            if (Current.Type == TokenType.Sequence)
            {
                NextToken();
                var id_ = Match(TokenType.Identifier);
                Match(TokenType.EOL);
                return new LineStatement(id_, color, true);
            }
            var id = Match(TokenType.Identifier);
            Match(TokenType.EOL);
            return new LineStatement(id, color);
        }

        private LineExpression ParseLineExpression()
        {
            Match(TokenType.Line);
            Match(TokenType.LParen);
            var p1 = Match(TokenType.Identifier);
            Match(TokenType.Comma);
            var p2 = Match(TokenType.Identifier);
            Match(TokenType.RParen);
            return new LineExpression(p1, p2, color);
        }

        private PointStatement ParsePoint()
        {
            Match(TokenType.Point);
            if (Current.Type == TokenType.Sequence)
            {
                NextToken();
                var id_ = Match(TokenType.Identifier);
                Match(TokenType.EOL);
                return new PointStatement(id_, color, true);
            }
            var id = Match(TokenType.Identifier);
            Match(TokenType.EOL);
            return new PointStatement(id, color);
        }
    }
}