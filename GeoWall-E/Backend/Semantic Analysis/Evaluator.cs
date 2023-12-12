using System.ComponentModel;
using System.Reflection.Metadata;

namespace GeoWall_E
{
    public class Evaluator
    {
        List<Node>? Root { get; set; }
        Error Errors_ { get; set; }
        SymbolTable SymbolTable { get; set; }
        List<Tuple<Type, Color>> ToDraw_ { get; set; }

        public Evaluator(List<Node> root, Error error) // If we want to evaluate the whole program
        {
            Root = root ?? new List<Node>();
            Errors_ = error;
            SymbolTable = new SymbolTable();
            var checker = new SemanticChecker(Errors);
            Root = Utils.ReorderNodes(Root);
            checker.Check(Root);
            ToDraw_ = new List<Tuple<Type, Color>>();
        }

        // Contructor for the evaluator of the let block
        public Evaluator(List<Statement> root, Error error, SymbolTable symbolTable)
        {
            // cast the list of statements to a list of nodes
            Root = root.ConvertAll(x => (Node)x);
            Errors_ = error;
            SymbolTable = symbolTable;
            ToDraw_ = new List<Tuple<Type, Color>>();
        }

        public Error Errors => Errors_;
        public SymbolTable SymbolTable_ => SymbolTable;
        public List<Tuple<Type, Color>> ToDraw => ToDraw_;

        public void Evaluate()
        {
            foreach (var node in Root)
            {
                switch (node)
                {
                    case ErrorExpression:
                    case ErrorStatement:
                    case EmptyNode:
                        break;
                    case ImportStatement import:
                        SymbolTable.Merge(import.Import(), Errors);
                        break;
                    case AsignationStatement asignation:
                        HandleAsignationNode(asignation);
                        break;
                    case MultipleAsignationStatement asignation:
                        HandleMultipleAsignationNode(asignation);
                        break;
                    case PointStatement point:
                        if (point.IsSequence) SymbolTable.Define(point.Name.Text, point.CreateSequence());
                        else SymbolTable.Define(point.Name.Text, new Point(point.Name.Text));
                        break;
                    case LineStatement line:
                        if (line.IsSequence) SymbolTable.Define(line.Name.Text, line.CreateSequence());
                        else SymbolTable.Define(line.Name.Text, new Line(new Point(), new Point(), line.Name.Text));
                        break;
                    case SegmentStatement segment:
                        if (segment.IsSequence) SymbolTable.Define(segment.Name.Text, segment.CreateSequence());
                        else SymbolTable.Define(segment.Name.Text, new Segment(new Point(), new Point(), segment.Name.Text));
                        break;
                    case RayStatement ray:
                        if (ray.IsSequence) SymbolTable.Define(ray.Name.Text, ray.CreateSequence());
                        else SymbolTable.Define(ray.Name.Text, new Ray(new Point(), new Point(), ray.Name.Text));
                        break;
                    case CircleStatement circle:
                        if (circle.IsSequence) SymbolTable.Define(circle.Name.Text, circle.CreateSequence());
                        else SymbolTable.Define(circle.Name.Text, new Circle(new Point(), new Measure(new Point(), new Point(), circle.Name.Text), circle.Name.Text));
                        break;
                    case MeasureStatement measure:
                        if (measure.IsSequence) SymbolTable.Define(measure.Name.Text, measure.CreateSequence());
                        else SymbolTable.Define(measure.Name.Text, new Measure(new Point(), new Point(), measure.Name.Text));
                        break;
                    case FunctionDeclaration function:
                        SymbolTable.Define(function.Name.Text, new Function(function.Name, function.Arguments, function.Body));
                        break;
                    case Draw draw_:
                        HandleDrawNode(draw_);
                        break;
                }
            }
        }

        void HandleMultipleAsignationNode(MultipleAsignationStatement asignation)
        {
            switch (asignation.Value)
            {
                case SequenceExpression sequenceExpression:
                    var sequence = sequenceExpression.Evaluate(SymbolTable, Errors, ToDraw);
                    if (sequence is ErrorType) return;

                    for (int i = 0; i < asignation.IDs.Count; i++)
                    {
                        if (asignation.IDs[i].Type == TokenType.Underline) continue;

                        if (i == asignation.IDs.Count - 1)
                        {
                            SymbolTable.Define(asignation.IDs[i].Text, new Sequence(((Sequence)sequence).RestOfElements(i)));
                        }

                        else SymbolTable.Define(asignation.IDs[i].Text, ((Sequence)sequence).GetElement(i));
                    }
                    break;
                case IntersectExpression intersectExpression:
                    var intersect = intersectExpression.Evaluate(SymbolTable, Errors, ToDraw);
                    if (intersect is ErrorType) return;

                    for (int i = 0; i < asignation.IDs.Count; i++)
                    {
                        if (asignation.IDs[i].Type == TokenType.Underline) continue;

                        if (i == asignation.IDs.Count - 1)
                        {
                            SymbolTable.Define(asignation.IDs[i].Text, new Sequence(((Sequence)intersect).RestOfElements(i)));
                        }

                        else SymbolTable.Define(asignation.IDs[i].Text, ((Sequence)intersect).GetElement(i));
                    }
                    break;
                case UndefinedExpression:
                    for (int i = 0; i < asignation.IDs.Count; i++)
                    {
                        SymbolTable.Define(asignation.IDs[i].Text, new Undefined());
                    }
                    break;
                case Samples samples:
                    var points = samples.Evaluate(SymbolTable, Errors, ToDraw);
                    if (points is ErrorType) return;

                    for (int i = 0; i < asignation.IDs.Count; i++)
                    {
                        if (asignation.IDs[i].Type == TokenType.Underline) continue;

                        if (i == asignation.IDs.Count - 1)
                        {
                            SymbolTable.Define(asignation.IDs[i].Text, new Sequence(((Sequence)points).RestOfElements(i)));
                        }

                        else SymbolTable.Define(asignation.IDs[i].Text, ((Sequence)points).GetElement(i));
                    }
                    break;
                case RandomPointsInFigure random:
                    var randomPoints = random.Evaluate(SymbolTable, Errors, ToDraw);
                    if (randomPoints is ErrorType) return;

                    for (int i = 0; i < asignation.IDs.Count; i++)
                    {
                        if (asignation.IDs[i].Type == TokenType.Underline) continue;

                        if (i == asignation.IDs.Count - 1)
                        {
                            SymbolTable.Define(asignation.IDs[i].Text, new Sequence(((Sequence)randomPoints).RestOfElements(i)));
                        }

                        else SymbolTable.Define(asignation.IDs[i].Text, ((Sequence)randomPoints).GetElement(i));
                    }
                    break;
                case FunctionCallExpression function:
                    var result = function.Evaluate(SymbolTable, Errors, ToDraw);
                    if (result is ErrorType) return;

                    if (result is not Sequence)
                    {
                        SymbolTable.Define(asignation.IDs[0].Text, result);
                    }

                    for (int i = 0; i < asignation.IDs.Count; i++)
                    {
                        if (asignation.IDs[i].Type == TokenType.Underline) continue;

                        if (i == asignation.IDs.Count - 1)
                        {
                            SymbolTable.Define(asignation.IDs[i].Text, new Sequence(((Sequence)result).RestOfElements(i)));
                        }

                        else SymbolTable.Define(asignation.IDs[i].Text, ((Sequence)result).GetElement(i));
                    }
                    break;
                default:
                    HandleAsignationNode(new AsignationStatement(asignation.IDs[0], asignation.Value));
                    for (int i = 1; i < asignation.IDs.Count; i++)
                    {
                        SymbolTable.Define(asignation.IDs[i].Text, new Undefined());
                    }
                    break;
            }
        }

        void HandleAsignationNode(AsignationStatement asignation)
        {
            switch (asignation.Value)
            {
                case ParenExpression parenExpression:
                    SymbolTable.Define(asignation.Name.Text, parenExpression.Evaluate(SymbolTable, Errors, ToDraw));
                    break;
                case BinaryExpression binaryExpression:
                    SymbolTable.Define(asignation.Name.Text, binaryExpression.Evaluate(SymbolTable, Errors, ToDraw));
                    break;
                case UnaryExpression unaryExpression:
                    SymbolTable.Define(asignation.Name.Text, unaryExpression.Evaluate(SymbolTable, Errors, ToDraw));
                    break;
                case UndefinedExpression:
                    SymbolTable.Define(asignation.Name.Text, new Undefined());
                    break;
                case SequenceExpression sequence:
                    SymbolTable.Define(asignation.Name.Text, sequence.Evaluate(SymbolTable, Errors, ToDraw));
                    break;
                case Count count:
                    SymbolTable.Define(asignation.Name.Text, count.Evaluate(SymbolTable, Errors, ToDraw));
                    break;
                case NumberExpression number:
                    SymbolTable.Define(asignation.Name.Text, number.Evaluate(SymbolTable, Errors, ToDraw));
                    break;
                case StringExpression stringexp:
                    SymbolTable.Define(asignation.Name.Text, stringexp.Evaluate(SymbolTable, Errors, ToDraw));
                    break;
                case VariableExpression variable:
                    HandleVariableExpression(variable, asignation.Name.Text);
                    break;
                case MeasureExpression measure:
                    measure.HandleMeasureExpression(SymbolTable, Errors, asignation.Name.Text);
                    break;
                case LineExpression lineexp:
                    lineexp.HandleLineAsignationExpression(SymbolTable, Errors, asignation);
                    break;
                case SegmentExpression segmentexp:
                    segmentexp.HandleSegmentAsignationExpression(SymbolTable, Errors, asignation);
                    break;
                case RayExpression rayexp:
                    rayexp.HandleRayAsignationExpression(SymbolTable, Errors, asignation);
                    break;
                case CircleExpression circleexp:
                    circleexp.HandleCircleAsignationExpression(SymbolTable, Errors, asignation);
                    break;
                case ArcExpression arcexp:
                    arcexp.HandleArcAsignationExpression(SymbolTable, Errors, asignation);
                    break;
                case LetInExpression letin:
                    HandleLetInAsignationExpression(letin, asignation);
                    break;
                case IntersectExpression intersect:
                    intersect.HandleIntersectAsignationExpression(SymbolTable, Errors, asignation);
                    break;
                case FunctionCallExpression function:
                    var result = function.Evaluate(SymbolTable, Errors, ToDraw);
                    if (result is not ErrorType) SymbolTable.Define(asignation.Name.Text, result);
                    break;
                case IfExpression ifexp:
                    var resultIf = ifexp.Evaluate(SymbolTable, Errors, ToDraw);
                    if (resultIf is not ErrorType) SymbolTable.Define(asignation.Name.Text, resultIf);
                    break;
                case RandomPointsInFigure random:
                    var randomPoints = random.Evaluate(SymbolTable, Errors, ToDraw);
                    if (randomPoints is not ErrorType) SymbolTable.Define(asignation.Name.Text, randomPoints);
                    break;
                case Samples samples:
                    var points = samples.Evaluate(SymbolTable, Errors, ToDraw);
                    if (points is not ErrorType) SymbolTable.Define(asignation.Name.Text, points);
                    break;
                default:
                    Errors.AddError($"RUNTIME ERROR: Expression in asignation is {asignation.Value.Type}, which is not assignable");
                    break;
            }
        }

        void HandleLetInAsignationExpression(LetInExpression letin, AsignationStatement asignation)
        {
            SymbolTable.EnterScope();
            var evaluator = new Evaluator(letin.Let, Errors, SymbolTable);
            evaluator.Evaluate();
            var evaluatedIn = (IEvaluable)letin.In;
            var result = evaluatedIn.Evaluate(SymbolTable, Errors, ToDraw);
            SymbolTable.ExitScope();
            SymbolTable.Define(asignation.Name.Text, result);
        }

        void HandleVariableExpression(VariableExpression variable, string text)
        {
            var variableFound = SymbolTable.Resolve(variable.Name.Text);
            SymbolTable.Define(text, variableFound);
        }

        void HandleDrawNode(Draw draw)
        {
            switch (draw.Expression)
            {
                case VariableExpression variable:
                    AddTypeToDraw(variable, draw.Color, draw.Name);
                    break;
                case LineExpression lineexp:
                    lineexp.HandleLineExpression(ToDraw, Errors, SymbolTable, draw.Color, draw.Name);
                    break;
                case SegmentExpression segmentexp:
                    segmentexp.HandleSegmentExpression(ToDraw, Errors, SymbolTable, draw.Color, draw.Name);
                    break;
                case RayExpression rayexp:
                    rayexp.HandleRayExpression(ToDraw, Errors, SymbolTable, draw.Color, draw.Name);
                    break;
                case CircleExpression circleexp:
                    circleexp.HandleCircleExpression(ToDraw, Errors, SymbolTable, draw.Color, draw.Name);
                    break;
                case ArcExpression arcexp:
                    arcexp.HandleArcExpression(ToDraw, Errors, SymbolTable, draw.Color, draw.Name);
                    break;
                case SequenceExpression seqexp:
                    seqexp.HandleSequenceExpression(ToDraw, Errors, SymbolTable, draw.Color, draw.Name);
                    break;
                case FunctionCallExpression function:
                    HandleFunctionCallExpression(function, draw.Color, draw.Name);
                    break;
                case LetInExpression letin:
                    HandleLetInExpression(letin, draw.Color);
                    break;
                case IfExpression ifexp:
                    HandleIfExpression(ifexp, draw.Color);
                    break;
                case RandomPointsInFigure random:
                    var randomPoints = random.Evaluate(SymbolTable, Errors, ToDraw);
                    ((IDraw)randomPoints).SetName(draw.Name);
                    ToDraw.Add(new Tuple<Type, Color>(randomPoints, draw.Color));
                    break;
                case Samples samples:
                    var sample = samples.Evaluate(SymbolTable, Errors, ToDraw);
                    ToDraw.Add(new Tuple<Type, Color>(sample, draw.Color));
                    break;
                case IntersectExpression intersect:
                    intersect.HandleIntersectExpression(ToDraw, Errors, SymbolTable, draw.Color, draw.Name);
                    break;
                default:
                    Errors.AddError($"RUNTIME ERROR: Expression in draw is {draw.Expression.Type}, which is not drawable");
                    break;
            }
        }

        private void HandleIfExpression(IfExpression ifexp, Color color)
        {
            var result = ifexp.Evaluate(SymbolTable, Errors, ToDraw);
            if (result is not ErrorType && result as IDraw != null)
            {
                ToDraw.Add(new Tuple<Type, Color>(result, color));
            }
            else Errors.AddError($"RUNTIME ERROR: If expression in draw is {result.ObjectType}, which is not drawable.");
        }

        void HandleLetInExpression(LetInExpression letin, Color color) => ToDraw.Add(new Tuple<Type, Color>(letin.Evaluate(SymbolTable, Errors, ToDraw), color));

        void HandleFunctionCallExpression(FunctionCallExpression function, Color color, string name)
        {
            var result = function.Evaluate(SymbolTable, Errors, ToDraw);
            if (result is not ErrorType && result as IDraw != null)
            {
                ((IDraw)result).SetName(name);
                ToDraw.Add(new Tuple<Type, Color>(result, color));
            }
            else Errors.AddError($"RUNTIME ERROR: Function '{function.FunctionName.Text}' in draw is {result.ObjectType}, which is not drawable result:{((NumberLiteral)result).Value}, Line: {function.FunctionName.Line}, Column: {function.FunctionName.Column}");
        }
        void AddTypeToDraw(VariableExpression variable, Color color, string name)
        {
            var variableFound = SymbolTable.Resolve(variable.Name.Text);
            if (variableFound is not ErrorType && variableFound as IDraw != null)
            {
                if (variableFound is Sequence sequence)
                {
                    if (sequence.GetElement(0) is Undefined && sequence.Count() > 1) return;
                    if (sequence.GetElement(0) as IDraw == null)
                    {
                        Errors.AddError($"RUNTIME ERROR: Variable {variable.Name.Text} is a sequence of {variableFound.ObjectType}, which is not drawable, Line: {variable.Name.Line}, Column: {variable.Name.Column}");
                        return;
                    }
                }

                ((IDraw)variableFound).SetName(name);
                ToDraw.Add(new Tuple<Type, Color>(variableFound, color));
            }

            else Errors.AddError($"RUNTIME ERROR: Variable {variable.Name.Text} is {variableFound.ObjectType}, which is not drawable, Line: {variable.Name.Line}, Column: {variable.Name.Column}");
        }
    }
}