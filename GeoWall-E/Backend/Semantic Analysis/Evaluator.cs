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
        }

        public Evaluator(List<Statement> letblock, Error error) // If we want to evaluate the let block
        {
            LetBlock = letblock;
            Errors_ = error;
            SymbolTable = new SymbolTable();
        }

        public Error Errors => Errors_;

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
                    case AsignationStatement asignation:
                        HandleAsignationNode(asignation);
                        break;
                    case PointStatement point:
                        SymbolTable.Define(point.Name.Text, new Point(point.Name.Text));
                        break;
                    case LineStatement line:
                        SymbolTable.Define(line.Name.Text, new Line(new Point(), new Point(), line.Name.Text));
                        break;
                    case SegmentStatement segment:
                        SymbolTable.Define(segment.Name.Text, new Segment(new Point(), new Point(), segment.Name.Text));
                        break;
                    case RayStatement ray:
                        SymbolTable.Define(ray.Name.Text, new Ray(new Point(), new Point(), ray.Name.Text));
                        break;
                    case CircleStatement circle:
                        SymbolTable.Define(circle.Name.Text, new Circle(new Point(), new Measure(new Point(), new Point(), circle.Name.Text), circle.Name.Text));
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

        void HandleAsignationNode(AsignationStatement asignation)
        {
            switch (asignation.Value)
            {
                case SequenceExpression sequence:
                    SymbolTable.Define(asignation.Name.Text, sequence.Evaluate(SymbolTable, Errors));
                    break;
                case NumberExpression number:
                    SymbolTable.Define(asignation.Name.Text, number.Evaluate(SymbolTable, Errors));
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
            if (variableFound is not ErrorType) SymbolTable.Define(text, variableFound);
            else Errors_.AddError($"Variable {variable.Name.Text} not declared, Line: {variable.Name.Line}, Column: {variable.Name.Column}");
        }

        void HandleDrawNode(Draw draw, List<Tuple<Type, Color>> toDraw)
        {
            switch (draw.Expression)
            {
                case VariableExpression variable:
                    AddTypeToDraw(variable, toDraw, draw.Color);
                    break;
                case LineExpression lineexp:
                    lineexp.HandleLineExpression(toDraw, Errors, SymbolTable, draw.Color);
                    break;
                case SegmentExpression segmentexp:
                    segmentexp.HandleSegmentExpression(toDraw, Errors, SymbolTable, draw.Color);
                    break;
                case RayExpression rayexp:
                    rayexp.HandleRayExpression(toDraw, Errors, SymbolTable, draw.Color);
                    break;
                case CircleExpression circleexp:
                    circleexp.HandleCircleExpression(toDraw, Errors, SymbolTable, draw.Color);
                    break;
                case ArcExpression arcexp:
                    arcexp.HandleArcExpression(toDraw, Errors, SymbolTable, draw.Color);
                    break;
                case FunctionCallExpression function:
                    HandleFunctionCallExpression(function, toDraw, draw.Color);
                    break;
                case LetInExpression letin:
                    HandleLetInExpression(letin, toDraw, draw.Color);
                    break;
            }
            if (draw.Sequence is not null)
            {
                foreach (var id in draw.Sequence)
                {
                    AddTypeToDraw(id, toDraw, draw.Color);
                }
            }
        }

        void HandleLetInExpression(LetInExpression letin, List<Tuple<Type, Color>> toDraw, Color color) => toDraw.Add(new Tuple<Type, Color>(letin.Evaluate(SymbolTable, Errors), color));

        void HandleFunctionCallExpression(FunctionCallExpression function, List<Tuple<Type, Color>> toDraw, Color color) => toDraw.Add(new Tuple<Type, Color>(function.Evaluate(SymbolTable, Errors), color));

        void AddTypeToDraw(VariableExpression variable, List<Tuple<Type, Color>> toDraw, Color color)
        {
            var variableFound = SymbolTable.Resolve(variable.Name.Text);
            if (variableFound is not ErrorType && variableFound is IDraw) toDraw.Add(new Tuple<Type, Color>(variableFound, color));

            else if (variableFound is ErrorType) Errors.AddError($"Variable {variable.Name.Text} not declared, Line: {variable.Name.Line}, Column: {variable.Name.Column}");

            else Errors.AddError($"Variable {variable.Name.Text} is not drawable, Line: {variable.Name.Line}, Column: {variable.Name.Column}");
        }
    }
}