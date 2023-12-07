using System.ComponentModel;
using System.Reflection.Metadata;

namespace GeoWall_E
{
    public class Evaluator
    {
        List<Node>? Root { get; set; }
        List<Statement>? LetBlock { get; set; }
        Error Errors_ { get; set; }
        SymbolTable SymbolTable { get; set; }
        // Esta variable es temporal para comprobar que se evaluen bien las expresiones que no se pintan
        public Type? Result;

        public Evaluator(List<Node> root, Error error) // If we want to evaluate the whole program
        {
            Root = root;
            Errors_ = error;
            SymbolTable = new SymbolTable();
            var checker = new SemanticChecker(Errors);
            checker.Check(Root);
        }

        public Evaluator(List<Statement> letblock, Error error) // If we want to evaluate the let block
        {
            LetBlock = letblock;
            Errors_ = error;
            SymbolTable = new SymbolTable();
        }

        public Error Errors => Errors_;
        public SymbolTable SymbolTable_ => SymbolTable;

        public Dictionary<string, Type> EvaluateLetBlock()
        {
            Dictionary<string, Type> addToScope = new();
            foreach (var statement in LetBlock)
            {
                switch (statement)
                {
                    case AsignationStatement asignation:
                        addToScope.Add(asignation.Name.Text, HandleAsignationLet(asignation));
                        break;
                    case MultipleAsignationStatement asignation:
                        switch (asignation.Value)
                        {
                            case SequenceExpression sequenceExpression:
                                var sequence = sequenceExpression.Evaluate(SymbolTable, Errors);
                                if (sequence is ErrorType) break;

                                for (int i = 0; i < asignation.IDs.Count; i++)
                                {
                                    if (asignation.IDs[i].Type == TokenType.Underline) continue;

                                    if (i == asignation.IDs.Count - 1)
                                    {
                                        addToScope.Add(asignation.IDs[i].Text, new Sequence(((Sequence)sequence).RestOfElements(i)));
                                    }

                                    else addToScope.Add(asignation.IDs[i].Text, ((Sequence)sequence).GetElement(i));
                                }
                                break;
                            case IntersectExpression intersectExpression:
                                var intersect = intersectExpression.Evaluate(SymbolTable, Errors);
                                if (intersect is ErrorType) break;

                                for (int i = 0; i < asignation.IDs.Count; i++)
                                {
                                    if (asignation.IDs[i].Type == TokenType.Underline) continue;

                                    if (i == asignation.IDs.Count - 1)
                                    {
                                        addToScope.Add(asignation.IDs[i].Text, new Sequence(((Sequence)intersect).RestOfElements(i)));
                                    }

                                    else addToScope.Add(asignation.IDs[i].Text, ((Sequence)intersect).GetElement(i));
                                }
                                break;
                            case UndefinedExpression:
                                for (int i = 0; i < asignation.IDs.Count; i++)
                                {
                                    addToScope.Add(asignation.IDs[i].Text, new Undefined());
                                }
                                break;
                            default:
                                HandleAsignationNode(new AsignationStatement(asignation.IDs[0], asignation.Value));
                                for (int i = 1; i < asignation.IDs.Count; i++)
                                {
                                    addToScope.Add(asignation.IDs[i].Text, new Undefined());
                                }
                                break;
                        }
                        break;
                    case PointStatement point:
                        addToScope.Add(point.Name.Text, new Point(point.Name.Text));
                        break;
                    case LineStatement line:
                        addToScope.Add(line.Name.Text, new Line(new Point(), new Point(), line.Name.Text));
                        break;
                    case SegmentStatement segment:
                        addToScope.Add(segment.Name.Text, new Segment(new Point(), new Point(), segment.Name.Text));
                        break;
                    case RayStatement ray:
                        addToScope.Add(ray.Name.Text, new Ray(new Point(), new Point(), ray.Name.Text));
                        break;
                    case CircleStatement circle:
                        addToScope.Add(circle.Name.Text, new Circle(new Point(), new Measure(new Point(), new Point(), circle.Name.Text), circle.Name.Text));
                        break;
                    case FunctionDeclaration function:
                        addToScope.Add(function.Name.Text, new Function(function.Name, function.Arguments, function.Body));
                        break;
                }
            }
            return addToScope;
        }

        Type HandleAsignationLet(AsignationStatement asignation)
        {
            return asignation.Value switch
            {
                VariableExpression variable => variable.Evaluate(SymbolTable, Errors),
                MeasureExpression measure => measure.Evaluate(SymbolTable, Errors),
                LineExpression lineexp => lineexp.Evaluate(SymbolTable, Errors),
                SequenceExpression sequence => sequence.Evaluate(SymbolTable, Errors),
                SegmentExpression segmentexp => segmentexp.Evaluate(SymbolTable, Errors),
                RayExpression rayexp => rayexp.Evaluate(SymbolTable, Errors),
                CircleExpression circleexp => circleexp.Evaluate(SymbolTable, Errors),
                ArcExpression arcexp => arcexp.Evaluate(SymbolTable, Errors),
                LetInExpression letin => letin.Evaluate(SymbolTable, Errors),
                NumberExpression exp => exp.Evaluate(SymbolTable, Errors),
                ParenExpression exp => exp.Evaluate(SymbolTable, Errors),
                StringExpression exp => exp.Evaluate(SymbolTable, Errors),
                _ => new ErrorType(),
            };
        }

        public List<Tuple<Type, Color>> Evaluate()
        {
            List<Tuple<Type, Color>> toDraw = new();
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
                        if (point.IsSequence) SymbolTable.Define(point.Name.Text, PointStatement.CreateSequence());
                        else SymbolTable.Define(point.Name.Text, new Point(point.Name.Text));
                        break;
                    case LineStatement line:
                        if (line.IsSequence) SymbolTable.Define(line.Name.Text, LineStatement.CreateSequence());
                        else SymbolTable.Define(line.Name.Text, new Line(new Point(), new Point(), line.Name.Text));
                        break;
                    case SegmentStatement segment:
                        if (segment.IsSequence) SymbolTable.Define(segment.Name.Text, SegmentStatement.CreateSequence());
                        else SymbolTable.Define(segment.Name.Text, new Segment(new Point(), new Point(), segment.Name.Text));
                        break;
                    case RayStatement ray:
                        if (ray.IsSequence) SymbolTable.Define(ray.Name.Text, RayStatement.CreateSequence());
                        else SymbolTable.Define(ray.Name.Text, new Ray(new Point(), new Point(), ray.Name.Text));
                        break;
                    case CircleStatement circle:
                        if (circle.IsSequence) SymbolTable.Define(circle.Name.Text, CircleStatement.CreateSequence());
                        else SymbolTable.Define(circle.Name.Text, new Circle(new Point(), new Measure(new Point(), new Point(), circle.Name.Text), circle.Name.Text));
                        break;
                    case MeasureStatement measure:
                        SymbolTable.Define(measure.Name.Text, new Measure(new Point(), new Point(), measure.Name.Text));
                        break;
                    case FunctionDeclaration function:
                        SymbolTable.Define(function.Name.Text, new Function(function.Name, function.Arguments, function.Body));
                        break;
                    case Draw draw_:
                        HandleDrawNode(draw_, toDraw);
                        break;
                    default:
                        var exp = (IEvaluable)node;
                        Result = exp.Evaluate(SymbolTable, Errors_);
                        break;
                }
            }
            return toDraw;
        }

        void HandleMultipleAsignationNode(MultipleAsignationStatement asignation)
        {
            switch (asignation.Value)
            {
                case SequenceExpression sequenceExpression:
                    var sequence = sequenceExpression.Evaluate(SymbolTable, Errors);
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
                    var intersect = intersectExpression.Evaluate(SymbolTable, Errors);
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
                    var points = samples.Evaluate(SymbolTable, Errors);
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
                    var randomPoints = random.Evaluate(SymbolTable, Errors);
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
                case BinaryExpression binaryExpression:
                    SymbolTable.Define(asignation.Name.Text, binaryExpression.Evaluate(SymbolTable, Errors));
                    break;
                case UndefinedExpression:
                    SymbolTable.Define(asignation.Name.Text, new Undefined());
                    break;
                case SequenceExpression sequence:
                    SymbolTable.Define(asignation.Name.Text, sequence.Evaluate(SymbolTable, Errors));
                    break;
                case Count count:
                    SymbolTable.Define(asignation.Name.Text, count.Evaluate(SymbolTable, Errors));
                    break;
                case NumberExpression number:
                    SymbolTable.Define(asignation.Name.Text, number.Evaluate(SymbolTable, Errors));
                    break;
                case StringExpression stringexp:
                    SymbolTable.Define(asignation.Name.Text, stringexp.Evaluate(SymbolTable, Errors));
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
            }
        }

        void HandleLetInAsignationExpression(LetInExpression letin, AsignationStatement asignation)
        {
            // SymbolTable.EnterScope();
            var evaluator = new Evaluator(letin.Let, Errors);
            var toAddtoScope = evaluator.EvaluateLetBlock();
            foreach (var item in toAddtoScope)
            {
                SymbolTable.Define(item.Key, item.Value);
            }
            var evaluatedIn = (IEvaluable)letin.In;
            var result = evaluatedIn.Evaluate(SymbolTable, Errors);
            // SymbolTable.ExitScope();
            SymbolTable.Define(asignation.Name.Text, result);
        }

        void HandleVariableExpression(VariableExpression variable, string text)
        {
            var variableFound = SymbolTable.Resolve(variable.Name.Text);
            SymbolTable.Define(text, variableFound);
        }

        void HandleDrawNode(Draw draw, List<Tuple<Type, Color>> toDraw)
        {
            switch (draw.Expression)
            {
                case VariableExpression variable:
                    AddTypeToDraw(variable, toDraw, draw.Color, draw.Name);
                    break;
                case LineExpression lineexp:
                    lineexp.HandleLineExpression(toDraw, Errors, SymbolTable, draw.Color, draw.Name);
                    break;
                case SegmentExpression segmentexp:
                    segmentexp.HandleSegmentExpression(toDraw, Errors, SymbolTable, draw.Color, draw.Name);
                    break;
                case RayExpression rayexp:
                    rayexp.HandleRayExpression(toDraw, Errors, SymbolTable, draw.Color, draw.Name);
                    break;
                case CircleExpression circleexp:
                    circleexp.HandleCircleExpression(toDraw, Errors, SymbolTable, draw.Color, draw.Name);
                    break;
                case ArcExpression arcexp:
                    arcexp.HandleArcExpression(toDraw, Errors, SymbolTable, draw.Color, draw.Name);
                    break;
                case SequenceExpression seqexp:
                    seqexp.HandleSequenceExpression(toDraw, Errors, SymbolTable, draw.Color, draw.Name);
                    break;
                case FunctionCallExpression function:
                    HandleFunctionCallExpression(function, toDraw, draw.Color, draw.Name);
                    break;
                case LetInExpression letin:
                    HandleLetInExpression(letin, toDraw, draw.Color);
                    break;
                case IfExpression ifexp:
                    HandleIfExpression(ifexp, toDraw, draw.Color);
                    break;
                case RandomPointsInFigure random:
                    var randomPoints = random.Evaluate(SymbolTable, Errors);
                    ((IDraw)randomPoints).SetName(draw.Name);
                    toDraw.Add(new Tuple<Type, Color>(randomPoints, draw.Color));
                    break;
                case IntersectExpression intersect:
                    intersect.HandleIntersectExpression(toDraw, Errors, SymbolTable, draw.Color, draw.Name);
                    break;
                default:
                    Errors.AddError($"RUNTIME ERROR: Expression in draw is {draw.Expression.Type}, which is not drawable");
                    break;
            }
        }

        private void HandleIfExpression(IfExpression ifexp, List<Tuple<Type, Color>> toDraw, Color color)
        {
            var result = ifexp.Evaluate(SymbolTable, Errors);
            if (result is not ErrorType && result as IDraw != null)
            {
                toDraw.Add(new Tuple<Type, Color>(result, color));
            }
            else Errors.AddError($"RUNTIME ERROR: If expression in draw is {result.ObjectType}, which is not drawable.");
        }

        void HandleLetInExpression(LetInExpression letin, List<Tuple<Type, Color>> toDraw, Color color) => toDraw.Add(new Tuple<Type, Color>(letin.Evaluate(SymbolTable, Errors), color));

        void HandleFunctionCallExpression(FunctionCallExpression function, List<Tuple<Type, Color>> toDraw, Color color, string name)
        {
            var result = function.Evaluate(SymbolTable, Errors);
            if (result is not ErrorType && result as IDraw != null)
            {
                ((IDraw)result).SetName(name);
                toDraw.Add(new Tuple<Type, Color>(result, color));
            }
            else Errors.AddError($"RUNTIME ERROR: Function '{function.FunctionName.Text}' in draw is {result.ObjectType}, which is not drawable.");
        }
        void AddTypeToDraw(VariableExpression variable, List<Tuple<Type, Color>> toDraw, Color color, string name)
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
                toDraw.Add(new Tuple<Type, Color>(variableFound, color));
            }

            else if (variableFound is ErrorType) Errors.AddError($"Variable {variable.Name.Text} not declared, Line: {variable.Name.Line}, Column: {variable.Name.Column}");

            else Errors.AddError($"RUNTIME ERROR: Variable {variable.Name.Text} is {variableFound.ObjectType}, which is not drawable, Line: {variable.Name.Line}, Column: {variable.Name.Column}");
        }
    }
}