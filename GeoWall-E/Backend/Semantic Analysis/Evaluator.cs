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
                // TODO: Seguir simplificando el codigo con los demas tipos de asignacion
                SegmentExpression segmentexp => HandleSegmentAsignationExpressionInLet(segmentexp, asignation),
                RayExpression rayexp => HandleAsignationRayExpression(rayexp, asignation),
                CircleExpression circleexp => HandleAsignationCircleExpression(circleexp, asignation),
                ArcExpression arcexp => HandleAsignationArcExpression(arcexp, asignation),
                LetInExpression letin => HandleAsignationLetExpression(letin),
                NumberExpression exp => exp.Evaluate(SymbolTable, Errors),
                ParenExpression exp => exp.Evaluate(SymbolTable, Errors),
                StringExpression exp => exp.Evaluate(SymbolTable, Errors),
                _ => new ErrorType(),
            };
        }

        Type HandleSegmentAsignationExpressionInLet(SegmentExpression segmentexp, AsignationStatement asignation)
        {
            var start = SymbolTable.Resolve(segmentexp.Start.Text);
            var end = SymbolTable.Resolve(segmentexp.End.Text);
            if (start is not ErrorType && end is not ErrorType)
            {
                if (start.ObjectType == ObjectTypes.Point && end.ObjectType == ObjectTypes.Point)
                {
                    return new Segment((Point)start, (Point)end, asignation.Name.Text);
                }
                else
                {
                    Errors.AddError($"Invalid type for {segmentexp.Start.Text} or {segmentexp.End.Text}, Line: {segmentexp.Start.Line}, Column: {segmentexp.Start.Column}");
                    return new ErrorType();
                }
            }
            else
            {
                Errors.AddError($"Variable {segmentexp.Start.Text} or {segmentexp.End.Text} not declared, Line: {segmentexp.Start.Line}, Column: {segmentexp.Start.Column}");
                return new ErrorType();
            }
        }

        Type HandleAsignationRayExpression(RayExpression rayexp, AsignationStatement asignation)
        {
            var start = SymbolTable.Resolve(rayexp.Start.Text);
            var end = SymbolTable.Resolve(rayexp.End.Text);
            if (start is not ErrorType && end is not ErrorType)
            {
                if (start.ObjectType == ObjectTypes.Point && end.ObjectType == ObjectTypes.Point)
                {
                    return new Ray((Point)start, (Point)end, asignation.Name.Text);
                }
                else
                {
                    Errors.AddError($"Invalid type for {rayexp.Start.Text} or {rayexp.End.Text}, Line: {rayexp.Start.Line}, Column: {rayexp.Start.Column}");
                    return new ErrorType();
                }
            }
            else
            {
                Errors.AddError($"Variable {rayexp.Start.Text} or {rayexp.End.Text} not declared, Line: {rayexp.Start.Line}, Column: {rayexp.Start.Column}");
                return new ErrorType();
            }
        }

        Type HandleAsignationCircleExpression(CircleExpression circleexp, AsignationStatement asignation)
        {
            var center = SymbolTable.Resolve(circleexp.Center.Text);
            var radius = SymbolTable.Resolve(circleexp.Radius.Text);
            if (center is not ErrorType && radius is not ErrorType)
            {
                if (center.ObjectType == ObjectTypes.Point && radius.ObjectType == ObjectTypes.Measure)
                {
                    return new Circle((Point)center, (Measure)radius, asignation.Name.Text);
                }
                else
                {
                    Errors.AddError($"Invalid type for {circleexp.Center.Text} or {circleexp.Radius.Text}, Line: {circleexp.Center.Line}, Column: {circleexp.Center.Column}");
                    return new ErrorType();
                }
            }
            else
            {
                Errors.AddError($"Variable {circleexp.Center.Text} or {circleexp.Radius.Text} not declared, Line: {circleexp.Center.Line}, Column: {circleexp.Center.Column}");
                return new ErrorType();
            }
        }

        Type HandleAsignationArcExpression(ArcExpression arcexp, AsignationStatement asignation)
        {
            var center = SymbolTable.Resolve(arcexp.Center.Text);
            var start = SymbolTable.Resolve(arcexp.Start.Text);
            var end = SymbolTable.Resolve(arcexp.End.Text);
            var measure = SymbolTable.Resolve(arcexp.Measure.Text);

            if (center is not ErrorType && start is not ErrorType && end is not ErrorType && measure is not ErrorType)
            {
                if (center.ObjectType == ObjectTypes.Point && start.ObjectType == ObjectTypes.Point && end.ObjectType == ObjectTypes.Point && measure.ObjectType == ObjectTypes.Measure)
                {
                    return new Arc((Point)center, (Point)start, (Point)end, (Measure)measure, asignation.Name.Text);
                }
                else
                {
                    Errors.AddError($"Invalid type for {arcexp.Center.Text} or {arcexp.Start.Text} or {arcexp.End.Text} or {arcexp.Measure.Text}, Line: {arcexp.Center.Line}, Column: {arcexp.Center.Column}");

                    return new ErrorType();
                }
            }
            else
            {
                Errors.AddError($"Variable {arcexp.Center.Text} or {arcexp.Start.Text} or {arcexp.End.Text} or {arcexp.Measure.Text} not declared, Line: {arcexp.Center.Line}, Column: {arcexp.Center.Column}");
                return new ErrorType();
            }
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
                    HandleMeasureExpression(measure, asignation.Name.Text);
                    break;
                case LineExpression lineexp:
                    HandleLineAsignationExpression(lineexp, asignation);
                    break;
                case SegmentExpression segmentexp:
                    HandleSegmentAsignationExpression(segmentexp, asignation);
                    break;
                case RayExpression rayexp:
                    HandleRayAsignationExpression(rayexp, asignation);
                    break;
                case CircleExpression circleexp:
                    HandleCircleAsignationExpression(circleexp, asignation);
                    break;
                case ArcExpression arcexp:
                    HandleArcAsignationExpression(arcexp, asignation);
                    break;
                case LetInExpression letin:
                    HandleLetInAsignationExpression(letin, asignation);
                    break;
            }
        }

        Type HandleAsignationLetExpression(LetInExpression letin)
        {
            SymbolTable.EnterScope();
            var evaluator = new Evaluator(letin.Let, Errors);
            var toAddtoScope = evaluator.EvaluateLetBlock();
            foreach (var item in toAddtoScope) SymbolTable.Define(item.Key, item.Value);
            var evaluatedIn = (IEvaluable)letin.In;
            var result = evaluatedIn.Evaluate(SymbolTable, Errors);
            SymbolTable.ExitScope();
            return result;
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

        void HandleArcAsignationExpression(ArcExpression arcexp, AsignationStatement asignation)
        {
            var center = SymbolTable.Resolve(arcexp.Center.Text);
            var start = SymbolTable.Resolve(arcexp.Start.Text);
            var end = SymbolTable.Resolve(arcexp.End.Text);
            var measure = SymbolTable.Resolve(arcexp.Measure.Text);

            if (center is not ErrorType && start is not ErrorType && end is not ErrorType && measure is not ErrorType)
            {
                if (center.ObjectType == ObjectTypes.Point && start.ObjectType == ObjectTypes.Point && end.ObjectType == ObjectTypes.Point && measure.ObjectType == ObjectTypes.Measure)
                {
                    SymbolTable.Define(asignation.Name.Text, new Arc((Point)center, (Point)start, (Point)end, (Measure)measure, asignation.Name.Text));
                }
                else
                {
                    Errors.AddError($"Invalid type for {arcexp.Center.Text} or {arcexp.Start.Text} or {arcexp.End.Text} or {arcexp.Measure.Text}, Line: {arcexp.Center.Line}, Column: {arcexp.Center.Column}");
                }
            }
            else
            {
                Errors.AddError($"Variable {arcexp.Center.Text} or {arcexp.Start.Text} or {arcexp.End.Text} or {arcexp.Measure.Text} not declared, Line: {arcexp.Center.Line}, Column: {arcexp.Center.Column}");
            }
        }

        void HandleCircleAsignationExpression(CircleExpression circleexp, AsignationStatement asignation)
        {
            var center = SymbolTable.Resolve(circleexp.Center.Text);
            var radius = SymbolTable.Resolve(circleexp.Radius.Text);
            if (center is not ErrorType && radius is not ErrorType)
            {
                if (center.ObjectType == ObjectTypes.Point && radius.ObjectType == ObjectTypes.Measure)
                {
                    SymbolTable.Define(asignation.Name.Text, new Circle((Point)center, (Measure)radius, asignation.Name.Text));
                }
                else
                {
                    Errors.AddError($"Invalid type for {circleexp.Center.Text} or {circleexp.Radius.Text}, Line: {circleexp.Center.Line}, Column: {circleexp.Center.Column}");
                }
            }
            else
            {
                Errors.AddError($"Variable {circleexp.Center.Text} or {circleexp.Radius.Text} not declared, Line: {circleexp.Center.Line}, Column: {circleexp.Center.Column}");
            }
        }

        void HandleRayAsignationExpression(RayExpression rayexp, AsignationStatement asignation)
        {
            var start = SymbolTable.Resolve(rayexp.Start.Text);
            var end = SymbolTable.Resolve(rayexp.End.Text);
            if (start is not ErrorType && end is not ErrorType)
            {
                if (start.ObjectType == ObjectTypes.Point && end.ObjectType == ObjectTypes.Point)
                {
                    SymbolTable.Define(asignation.Name.Text, new Ray((Point)start, (Point)end, asignation.Name.Text));
                }
                else
                {
                    Errors.AddError($"Invalid type for {rayexp.Start.Text} or {rayexp.End.Text}, Line: {rayexp.Start.Line}, Column: {rayexp.Start.Column}");
                }
            }
            else
            {
                Errors.AddError($"Variable {rayexp.Start.Text} or {rayexp.End.Text} not declared, Line: {rayexp.Start.Line}, Column: {rayexp.Start.Column}");
            }
        }

        void HandleSegmentAsignationExpression(SegmentExpression segmentexp, AsignationStatement asignation)
        {
            var start = SymbolTable.Resolve(segmentexp.Start.Text);
            var end = SymbolTable.Resolve(segmentexp.End.Text);
            if (start is not ErrorType && end is not ErrorType)
            {
                if (start.ObjectType == ObjectTypes.Point && end.ObjectType == ObjectTypes.Point)
                {
                    SymbolTable.Define(asignation.Name.Text, new Segment((Point)start, (Point)end, asignation.Name.Text));
                }
                else
                {
                    Errors.AddError($"Invalid type for {segmentexp.Start.Text} or {segmentexp.End.Text}, Line: {segmentexp.Start.Line}, Column: {segmentexp.Start.Column}");
                }
            }
            else
            {
                Errors.AddError($"Variable {segmentexp.Start.Text} or {segmentexp.End.Text} not declared, Line: {segmentexp.Start.Line}, Column: {segmentexp.Start.Column}");
            }
        }

        void HandleLineAsignationExpression(LineExpression lineexp, AsignationStatement asig)
        {
            var p1 = SymbolTable.Resolve(lineexp.P1.Text);
            var p2 = SymbolTable.Resolve(lineexp.P2.Text);
            if (p1 is not ErrorType && p2 is not ErrorType)
            {
                if (p1.ObjectType == ObjectTypes.Point && p2.ObjectType == ObjectTypes.Point)
                {
                    SymbolTable.Define(asig.Name.Text, new Line((Point)p1, (Point)p2, asig.Name.Text));
                }
                else
                {
                    Errors.AddError($"Invalid type for {lineexp.P1.Text} or {lineexp.P2.Text}, Line: {lineexp.P1.Line}, Column: {lineexp.P1.Column}");
                }
            }
            else
            {
                Errors.AddError($"Variable {lineexp.P1.Text} or {lineexp.P2.Text} not declared, Line: {lineexp.P1.Line}, Column: {lineexp.P1.Column}");
            }
        }

        void HandleVariableExpression(VariableExpression variable, string text)
        {
            var variableFound = SymbolTable.Resolve(variable.Name.Text);
            if (variableFound is not ErrorType) SymbolTable.Define(text, variableFound);
            else Errors_.AddError($"Variable {variable.Name.Text} not declared, Line: {variable.Name.Line}, Column: {variable.Name.Column}");
        }

        void HandleMeasureExpression(MeasureExpression measure, string name)
        {
            var p1 = SymbolTable.Resolve(measure.P1.Text);
            var p2 = SymbolTable.Resolve(measure.P2.Text);
            if (p1 is not ErrorType && p2 is not ErrorType)
            {
                if (p1.ObjectType == ObjectTypes.Point && p2.ObjectType == ObjectTypes.Point)
                {
                    SymbolTable.Define(name, new Measure((Point)p1, (Point)p2, name));
                }
                else
                {
                    Errors.AddError($"Invalid type for {measure.P1.Text} or {measure.P2.Text}, Line: {measure.P1.Line}, Column: {measure.P1.Column}");
                }
            }
            else
            {
                Errors.AddError($"Variable {measure.P1.Text} or {measure.P2.Text} not declared, Line: {measure.P1.Line}, Column: {measure.P1.Column}");
            }
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