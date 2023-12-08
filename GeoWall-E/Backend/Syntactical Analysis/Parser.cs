

namespace GeoWall_E
{
    public class Parser
    {
        readonly List<Token> tokens;
        readonly Error errors;
        int position;
        Color PreviousColor = new(Colors.Black);
        Color color = new(Colors.Black);

        public Parser(List<Token> tokens, Error errors)
        {
            this.tokens = tokens;
            this.errors = errors;
        }

        public Error Errors => errors;

        Token Peek(int offset) => position + offset >= tokens.Count ? tokens[^1] : tokens[position + offset];

        Token NextToken() => tokens[position++];

        Token Current => Peek(0);

        Token Match(TokenType Type)
        {
            if (Current.Type == Type) return NextToken();
            errors.AddError($"SYNTAX ERROR: Expected {Type} but got {Current.Type}, Line: {Current.Line}, Column: {Current.Column}");
            return new Token(TokenType.Error, "", Current.Line, Current.Column);
        }

        public AST Parse_()
        {
            List<Node> nodes = ParseMembers();
            Match(TokenType.EOF);
            return new AST(nodes, errors);
        }

        List<Node> ParseMembers()
        {
            List<Node> members = new();

            while (Current.Type != TokenType.EOF)
            {
                var startToken = Current;
                var member = ParseMember();
                if (member is ErrorExpression || member is ErrorStatement) break;
                members.Add(member);
                if (member as Expression != null) Match(TokenType.EOL);

                if (Current == startToken) NextToken();
            }
            return members;
        }

        Node ParseMember()
        {
            if (Current.Type == TokenType.Identifier && Peek(1).Type == TokenType.LParen)
            {
                var function = ParseFunctionDeclaration();
                if (function is FunctionDeclaration) return function;
            }
            return ParseStatement();
        }

        Node ParseFunctionDeclaration()
        {
            var name = NextToken();
            Match(TokenType.LParen);
            List<Token> arguments = new();
            if (Current.Type != TokenType.RParen)
            {
                var id = Match(TokenType.Identifier);
                if (id.Type != TokenType.Error) arguments.Add(id);
                int overflow = 0;
                while (Current.Type == TokenType.Comma)
                {
                    if (overflow > 100)
                    {
                        errors.AddError($"Can't parse, Line: {Current.Line}, Column: {Current.Column}");
                        return new ErrorStatement();
                    }
                    NextToken();
                    arguments.Add(Match(TokenType.Identifier));
                    overflow++;
                }
            }
            Match(TokenType.RParen);
            Match(TokenType.Asignation);
            var value = ParseExpression();
            Match(TokenType.EOL);
            return new FunctionDeclaration(name, arguments, value);
        }

        Node ParseStatement() => Current.Type switch
        {
            TokenType.Point => ParsePoint(),
            TokenType.Line => ParseLine(),
            TokenType.Segment => ParseSegment(),
            TokenType.Ray => ParseRay(),
            TokenType.Circle => ParseCircle(),
            TokenType.Arc => ParseArc(),
            TokenType.Measure => ParseMeasure(),
            TokenType.Intersect => ParseIntersect(),
            TokenType.Randoms => ParseRandom(),
            TokenType.Points => ParsePoints(),
            TokenType.Samples => ParseSamples(),
            TokenType.Draw => ParseDraw(),
            TokenType.Color => ParseColor(),
            TokenType.Restore => ParseRestore(),
            TokenType.Identifier => ParseIdentifier(),
            TokenType.Underline => ParseIdentifier(),
            TokenType.Import => ParseImport(),
            _ => ParseExpression(),
        };

        Node ParseImport()
        {
            NextToken();
            var filename = Match(TokenType.String);
            Match(TokenType.EOL);
            return new ImportStatement(filename);
        }

        Node ParseRestore()
        {
            color = PreviousColor;
            NextToken();
            Match(TokenType.EOL);
            return new EmptyNode();
        }

        Node ParseColor()
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

        Node ParseIdentifier()
        {
            if (Peek(1).Type == TokenType.Asignation)
            {
                if (Current.Type == TokenType.Underline)
                {
                    NextToken(); // _
                    NextToken(); // =
                    ParseExpression();
                    Match(TokenType.EOL);
                    return new EmptyNode();
                }
                else
                {
                    var name = Match(TokenType.Identifier);
                    return ParseAsignation(name);
                }
            }
            if (Peek(1).Type == TokenType.Comma)
            {
                List<Token> ids = new() { NextToken() };
                while (Current.Type == TokenType.Comma)
                {
                    NextToken();
                    if (Current.Type == TokenType.Underline)
                    {
                        var id = NextToken();
                        ids.Add(id);
                    }
                    else
                    {
                        var id = Match(TokenType.Identifier);
                        if (id.Type == TokenType.Error) return new EmptyNode();
                        ids.Add(id);
                    }
                }
                Match(TokenType.Asignation);
                var value = ParseExpression();
                Match(TokenType.EOL);
                return new MultipleAsignationStatement(ids, value);
            }
            return ParseExpression();
        }

        Expression ParseExpression(int parentPrecedence = 0)
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

        Expression ParsePrimaryExpression()
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
                    return ParseMeasureExpression();
                case TokenType.Intersect:
                    return ParseIntersect();
                case TokenType.Points:
                    return ParsePoints();
                case TokenType.Count:
                    return ParseCount();
                case TokenType.Samples:
                    NextToken();
                    Match(TokenType.LParen);
                    Match(TokenType.RParen);
                    return new Samples();
                case TokenType.Identifier:
                    if (Peek(1).Type == TokenType.LParen) return ParseFunctionCall();
                    var name = NextToken();
                    return new VariableExpression(name);
                case TokenType.Number:
                    return new NumberExpression(NextToken());
                case TokenType.String:
                    return new StringExpression(NextToken());
                case TokenType.Undefined:
                    NextToken();
                    return new UndefinedExpression();
                case TokenType.LParen:
                    NextToken();
                    var expression = ParseExpression();
                    Match(TokenType.RParen);
                    return new ParenExpression(expression);
                case TokenType.Let:
                    return ParseLet();
                case TokenType.If:
                    return ParseIf();
                case TokenType.LBracket:
                    return ParseSequence();
                default:
                    errors.AddError($"Unexpected token {Current.Type}, Line: {Current.Line}, Column: {Current.Column}");
                    return new ErrorExpression();
            }
        }

        Expression ParseSequence()
        {
            NextToken();
            if (Current.Type == TokenType.RBracket)
            {
                NextToken();
                return new SequenceExpression(new List<Expression>());
            }
            if (Peek(1).Type == TokenType.InfiniteSequence)
            {
                var lowerBound = Match(TokenType.Number);
                NextToken();
                if (Current.Type == TokenType.Number)
                {
                    var upperBound = Match(TokenType.Number);
                    Match(TokenType.RBracket);
                    return new SequenceExpression(lowerBound, upperBound);
                }
                Match(TokenType.RBracket);
                return new SequenceExpression(lowerBound);
            }
            List<Expression> sequenceElements = new();
            var element = ParseExpression();
            if (element is not ErrorExpression) sequenceElements.Add(element);
            while (Current.Type == TokenType.Comma)
            {
                NextToken();
                sequenceElements.Add(ParseExpression());
            }
            Match(TokenType.RBracket);
            return new SequenceExpression(sequenceElements);
        }

        Expression ParseFunctionCall()
        {
            var name = NextToken();
            Match(TokenType.LParen);
            List<Expression> arguments = new();
            if (Current.Type != TokenType.RParen)
            {
                arguments.Add(ParseExpression());
                while (Current.Type == TokenType.Comma)
                {
                    NextToken();
                    arguments.Add(ParseExpression());
                }
            }
            Match(TokenType.RParen);
            return new FunctionCallExpression(name, arguments);
        }

        Node ParseAsignation(Token name)
        {
            NextToken();
            var value = ParseExpression();
            Match(TokenType.EOL);
            return new AsignationStatement(name, value);
        }

        Expression ParseLet()
        {
            NextToken();
            List<Statement> instructions = new();
            while (Current.Type != TokenType.In)
            {
                var statement = ParseStatement();
                if (statement as Statement != null) instructions.Add((Statement)statement);
            }
            Match(TokenType.In);
            var inExpression = ParseExpression();
            return new LetInExpression(instructions, inExpression);
        }

        Expression ParseIf()
        {
            NextToken();
            var condition = ParseExpression();
            Match(TokenType.Then);
            var then = ParseExpression();
            Match(TokenType.Else);
            var @else = ParseExpression();
            return new IfExpression(condition, then, @else);
        }

        Node ParseRandom()
        {
            NextToken();
            Match(TokenType.LParen);
            Match(TokenType.RParen);
            return new Randoms();
        }

        Node ParseSamples()
        {
            NextToken();
            Match(TokenType.LParen);
            Match(TokenType.RParen);
            return new Samples();
        }

        Node ParseDraw()
        {
            NextToken();
            Expression exp = ParseExpression();
            if (Current.Type == TokenType.String)
            {
                var name = Current.Text;
                NextToken();
                Match(TokenType.EOL);
                return new Draw(exp, color, name);
            }
            Match(TokenType.EOL);
            return new Draw(exp, color);
        }

        RandomPointsInFigure ParsePoints()
        {
            NextToken();
            Match(TokenType.LParen);
            var fig = ParseExpression();
            Match(TokenType.RParen);
            return new RandomPointsInFigure(fig);
        }

        Count ParseCount()
        {
            NextToken();
            Match(TokenType.LParen);
            var sequence = ParseExpression();
            Match(TokenType.RParen);

            return new Count(sequence);
        }

        IntersectExpression ParseIntersect()
        {
            Dictionary<string, Tuple<int, int>> positions = new();
            var intersectToken = NextToken();
            positions.Add(intersectToken.Text, new Tuple<int, int>(intersectToken.Line, intersectToken.Column));
            var lParenToken = Match(TokenType.LParen);
            positions.Add("f1", new Tuple<int, int>(lParenToken.Line, lParenToken.Column + 1));
            var f1 = ParseExpression();
            var commaToken = Match(TokenType.Comma);
            positions.Add("f2", new Tuple<int, int>(commaToken.Line, commaToken.Column + 1));
            var f2 = ParseExpression();
            Match(TokenType.RParen);

            return new IntersectExpression(f1, f2, positions);
        }

        Node ParseMeasure()
        {
            if (Peek(1).Type == TokenType.LParen) return ParseMeasureExpression();

            NextToken();
            if (Current.Type == TokenType.Sequence)
            {
                NextToken();
                var id_ = Match(TokenType.Identifier);
                Match(TokenType.EOL);
                return new MeasureStatement(id_, true);
            }
            var name = Match(TokenType.Identifier);
            Match(TokenType.EOL);

            return new MeasureStatement(name);
        }

        MeasureExpression ParseMeasureExpression()
        {
            Dictionary<string, Tuple<int, int>> positions = new();
            var tokenMeasure = NextToken();
            positions.Add(tokenMeasure.Text, new Tuple<int, int>(tokenMeasure.Line, tokenMeasure.Column));
            var lParenToken = Match(TokenType.LParen);
            positions.Add("p1", new Tuple<int, int>(lParenToken.Line, lParenToken.Column + 1));
            var p1 = ParseExpression();
            var commaToken = Match(TokenType.Comma);
            positions.Add("p2", new Tuple<int, int>(commaToken.Line, commaToken.Column + 1));
            var p2 = ParseExpression();
            Match(TokenType.RParen);

            return new MeasureExpression(p1, p2, positions);
        }

        ArcExpression ParseArc()
        {
            Dictionary<string, Tuple<int, int>> positions = new();
            var arcToken = Match(TokenType.Arc);
            positions.Add(arcToken.Text, new Tuple<int, int>(arcToken.Line, arcToken.Column));
            var lParenToken = Match(TokenType.LParen);
            positions.Add("center", new Tuple<int, int>(lParenToken.Line, lParenToken.Column + 1));
            var c = ParseExpression();
            var commaToken = Match(TokenType.Comma);
            positions.Add("start", new Tuple<int, int>(commaToken.Line, commaToken.Column + 1));
            var s = ParseExpression();
            var commaToken2 = Match(TokenType.Comma);
            positions.Add("end", new Tuple<int, int>(commaToken2.Line, commaToken2.Column + 1));
            var e = ParseExpression();
            var commaToken3 = Match(TokenType.Comma);
            positions.Add("measure", new Tuple<int, int>(commaToken3.Line, commaToken3.Column + 1));
            var m = ParseExpression();
            Match(TokenType.RParen);
            return new ArcExpression(c, s, e, m, positions);
        }

        Node ParseCircle()
        {
            NextToken();
            if (Current.Type == TokenType.LParen) return ParseCircleExpression();
            if (Current.Type == TokenType.Sequence)
            {
                NextToken();
                var id_ = Match(TokenType.Identifier);
                Match(TokenType.EOL);
                return new CircleStatement(id_, true);
            }
            var id = Match(TokenType.Identifier);
            Match(TokenType.EOL);
            return new CircleStatement(id);
        }

        CircleExpression ParseCircleExpression()
        {
            Dictionary<string, Tuple<int, int>> positions = new();
            var circleToken = Match(TokenType.Circle);
            positions.Add(circleToken.Text, new Tuple<int, int>(circleToken.Line, circleToken.Column));
            var lParenToken = Match(TokenType.LParen);
            positions.Add("c", new Tuple<int, int>(lParenToken.Line, lParenToken.Column + 1));
            var c = ParseExpression();
            var commaToken = Match(TokenType.Comma);
            positions.Add("m", new Tuple<int, int>(commaToken.Line, commaToken.Column + 1));
            var m = ParseExpression();
            Match(TokenType.RParen);
            return new CircleExpression(c, m, positions);
        }

        Node ParseRay()
        {
            NextToken();
            if (Current.Type == TokenType.LParen) return ParseRayExpression();
            if (Current.Type == TokenType.Sequence)
            {
                NextToken();
                var id_ = Match(TokenType.Identifier);
                Match(TokenType.EOL);
                return new RayStatement(id_, true);
            }
            var id = Match(TokenType.Identifier);
            Match(TokenType.EOL);
            return new RayStatement(id);
        }

        RayExpression ParseRayExpression()
        {
            // This dict is used to store the position of the tokens in the expression
            Dictionary<string, Tuple<int, int>> positions = new();
            var rayToken = Match(TokenType.Ray);
            positions.Add(rayToken.Text, new Tuple<int, int>(rayToken.Line, rayToken.Column));
            var lParenToken = Match(TokenType.LParen);
            positions.Add("p1", new Tuple<int, int>(lParenToken.Line, lParenToken.Column + 1));
            var p1 = ParseExpression();
            var commaToken = Match(TokenType.Comma);
            positions.Add("p2", new Tuple<int, int>(commaToken.Line, commaToken.Column + 1));
            var p2 = ParseExpression();
            Match(TokenType.RParen);
            return new RayExpression(p1, p2, positions);
        }

        Node ParseSegment()
        {
            NextToken();
            if (Current.Type == TokenType.LParen) return ParseSegmentExpression();

            if (Current.Type == TokenType.Sequence)
            {
                NextToken();
                var id_ = Match(TokenType.Identifier);
                Match(TokenType.EOL);
                return new SegmentStatement(id_, true);
            }

            var id = Match(TokenType.Identifier);
            Match(TokenType.EOL);
            return new SegmentStatement(id);
        }

        SegmentExpression ParseSegmentExpression()
        {
            Dictionary<string, Tuple<int, int>> positions = new();
            var segmentToken = Match(TokenType.Segment);
            positions.Add(segmentToken.Text, new Tuple<int, int>(segmentToken.Line, segmentToken.Column));
            var lParenToken = Match(TokenType.LParen);
            positions.Add("start", new Tuple<int, int>(lParenToken.Line, lParenToken.Column + 1));
            var p1 = ParseExpression();
            var commaToken = Match(TokenType.Comma);
            positions.Add("end", new Tuple<int, int>(commaToken.Line, commaToken.Column + 1));
            var p2 = ParseExpression();
            Match(TokenType.RParen);
            return new SegmentExpression(p1, p2, positions);
        }

        Node ParseLine()
        {
            Match(TokenType.Line);
            if (Current.Type == TokenType.LParen) return ParseLineExpression();

            if (Current.Type == TokenType.Sequence)
            {
                NextToken();
                var id_ = Match(TokenType.Identifier);
                Match(TokenType.EOL);
                return new LineStatement(id_, true);
            }
            var id = Match(TokenType.Identifier);
            Match(TokenType.EOL);
            return new LineStatement(id);
        }

        LineExpression ParseLineExpression()
        {
            Dictionary<string, Tuple<int, int>> positions = new();
            var lineToken = Match(TokenType.Line);
            positions.Add(lineToken.Text, new Tuple<int, int>(lineToken.Line, lineToken.Column));
            var lParenToken = Match(TokenType.LParen);
            positions.Add("p1", new Tuple<int, int>(lParenToken.Line, lParenToken.Column + 1));
            var p1 = ParseExpression();
            var commaToken = Match(TokenType.Comma);
            positions.Add("p2", new Tuple<int, int>(commaToken.Line, commaToken.Column + 1));
            var p2 = ParseExpression();
            Match(TokenType.RParen);
            return new LineExpression(p1, p2, positions);
        }

        PointStatement ParsePoint()
        {
            Match(TokenType.Point);
            if (Current.Type == TokenType.Sequence)
            {
                NextToken();
                var id_ = Match(TokenType.Identifier);
                Match(TokenType.EOL);
                return new PointStatement(id_, true);
            }
            var id = Match(TokenType.Identifier);
            Match(TokenType.EOL);
            return new PointStatement(id);
        }
    }
}