using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoWall_E
{
    internal class SemanticChecker
    {
        Error Errors_ { get; set; }
        SymbolTable? SymbolTable_ { get; set; }
        HashSet<Expression> checkedExpressions = new();

        public SemanticChecker(Error error)
        {
            Errors_ = error;
        }

        public Error Errors => Errors_;
        public SymbolTable SymbolTable => SymbolTable_;

        internal void Check(List<Node>? root)
        {
            if (root == null) return;

            SymbolTable_ = new SymbolTable();
            SymbolTable.EnterScope();

            foreach (var node in root)
            {
                Check(node);
            }

            SymbolTable.ExitScope();
        }

        void CheckFigureStatement(IFigureStatement figure)
        {
            if (SymbolTable.Resolve(figure.Name.Text) is not ErrorType or Function) Errors.AddError($"SEMANTIC ERROR: '{figure.Name.Text}' already defined");
            else
            {
                if (figure.IsSequence) SymbolTable.Define(figure.Name.Text, figure.CreateSequence());
                else
                {
                    switch (figure)
                    {
                        case PointStatement:
                            SymbolTable.Define(figure.Name.Text, new Point(figure.Name.Text));
                            break;
                        case CircleStatement:
                            SymbolTable.Define(figure.Name.Text, new Circle(new Point(), new Measure(new Point(), new Point()), figure.Name.Text));
                            break;
                        case LineStatement:
                            SymbolTable.Define(figure.Name.Text, new Line(new Point(), new Point(), figure.Name.Text));
                            break;
                        case RayStatement:
                            SymbolTable.Define(figure.Name.Text, new Ray(new Point(), new Point(), figure.Name.Text));
                            break;
                        case SegmentStatement:
                            SymbolTable.Define(figure.Name.Text, new Segment(new Point(), new Point(), figure.Name.Text));
                            break;
                        case MeasureStatement:
                            SymbolTable.Define(figure.Name.Text, new Measure(new Point(), new Point(), figure.Name.Text));
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void Check(Node node)
        {
            switch (node)
            {
                case ImportStatement:
                    SymbolTable.Merge(((ImportStatement)node).Import(), Errors);
                    break;
                case PointStatement:
                case CircleStatement:
                case RayStatement:
                case SegmentStatement:
                case MeasureStatement:
                case LineStatement:
                    CheckFigureStatement(node as IFigureStatement);
                    break;
                case Draw draw:
                    CheckExpression(draw.Expression);
                    break;
                case FunctionDeclaration function:
                    if (SymbolTable.ResolveFunction(function.Name.Text) is Function) Errors.AddError($"SEMANTIC ERROR: Function '{function.Name.Text}' already defined.");
                    else SymbolTable.Define(function.Name.Text, new Function(function.Name, function.Arguments, function.Body));
                    break;
                case AsignationStatement asignation:
                    if (asignation.Name.Type == TokenType.Underline) break;
                    if (SymbolTable.Resolve(asignation.Name.Text) is not ErrorType) Errors.AddError($"SEMANTIC ERROR: Variable '{asignation.Name.Text}' already defined.");
                    else
                    {
                        CheckExpression(asignation.Value);
                        TypeInference inference = new(SymbolTable);
                        var type = inference.InferType(asignation.Value);
                        if (type == TypeInfered.ErrorType) Errors.AddError($"SEMANTIC ERROR: Variable '{asignation.Name.Text}' is not defined, Line: {asignation.Name.Line}, Column: {asignation.Name.Column}.");
                        SymbolTable.Define(asignation.Name.Text, InferedTypeToType(type));
                    }
                    break;
                case MultipleAsignationStatement multipleAsignation:
                    foreach (var variable in multipleAsignation.IDs)
                    {
                        if (variable.Type == TokenType.Underline) continue;
                        if (SymbolTable.Resolve(variable.Text) is not ErrorType) Errors.AddError($"SEMANTIC ERROR: Variable '{variable.Text}' already defined.");
                        else
                        {
                            CheckExpression(multipleAsignation.Value);
                            TypeInference inference = new(SymbolTable);
                            var type = inference.InferType(multipleAsignation.Value);

                            SymbolTable.Define(variable.Text, InferedTypeToType(type));
                        }
                    }
                    break;
                default:
                    if (node is Expression expression) CheckExpression(expression);
                    break;
            }
        }


        internal void CheckExpression(Expression expression)
        {
            TypeInference inference = new(SymbolTable);
            // Si ya hemos chequeado la expresion, no la volvemos a chequear
            if (checkedExpressions.Contains(expression)) return;
            checkedExpressions.Add(expression);
            switch (expression)
            {
                case ErrorExpression:
                case NumberExpression:
                case StringExpression:
                case UndefinedExpression:
                    break;
                case SequenceExpression sequence:
                    if (sequence.Elements == null) break;

                    if (sequence.Elements.Count > 0)
                    {
                        var firstElementType = inference.InferType(sequence.Elements[0]);
                        foreach (var element in sequence.Elements)
                        {
                            if (inference.InferType(element) != firstElementType && inference.InferType(element) != TypeInfered.Any && firstElementType != TypeInfered.Any) Errors.AddError($"SEMANTIC ERROR: Sequence elements must be of the same type. {firstElementType} != {inference.InferType(element)}");
                            CheckExpression(element);
                        }
                    }

                    break;
                case Count count:
                    CheckExpression(count.Sequence);
                    break;
                case BinaryExpression binary:
                    CheckExpression(binary.Left);
                    CheckExpression(binary.Right);
                    break;
                case UnaryExpression unary:
                    CheckExpression(unary.Operand);
                    break;
                case ArcExpression arc:
                    var center = inference.InferType(arc.Center);
                    if (center != TypeInfered.Point && center != TypeInfered.Any) Errors.AddError($"SEMANTIC ERROR: Expected Point type but got {center} Line: {arc.Positions["center"].Item1}, Column: {arc.Positions["center"].Item2}");
                    var start = inference.InferType(arc.Start);
                    if (start != TypeInfered.Point && start != TypeInfered.Any) Errors.AddError($"SEMANTIC ERROR: Expected Point type but got {start} Line: {arc.Positions["start"].Item1}, Column: {arc.Positions["start"].Item2}");
                    var end = inference.InferType(arc.End);
                    if (end != TypeInfered.Point && end != TypeInfered.Any) Errors.AddError($"SEMANTIC ERROR: Expected Point type but got {end} Line: {arc.Positions["end"].Item1}, Column: {arc.Positions["end"].Item2}");
                    var measure = inference.InferType(arc.Measure);
                    if (measure != TypeInfered.Measure && measure != TypeInfered.Any) Errors.AddError($"SEMANTIC ERROR: Expected Measure type but got {measure}, Line: {arc.Positions["measure"].Item1}, Column: {arc.Positions["measure"].Item2}");
                    CheckExpression(arc.Center);
                    CheckExpression(arc.Start);
                    CheckExpression(arc.End);
                    CheckExpression(arc.Measure);
                    break;
                case CircleExpression circle:
                    var center1 = inference.InferType(circle.Center);
                    if (center1 != TypeInfered.Point && center1 != TypeInfered.Any) Errors.AddError($"SEMANTIC ERROR: Expected Point type but got {center1} Line: {circle.Positions["c"].Item1}, Column: {circle.Positions["c"].Item2}");
                    var radius = inference.InferType(circle.Radius);
                    if (radius != TypeInfered.Measure) Errors.AddError($"SEMANTIC ERROR: Expected Measure type but got {radius} Line: {circle.Positions["m"].Item1}, Column: {circle.Positions["m"].Item2}");
                    CheckExpression(circle.Center);
                    CheckExpression(circle.Radius);
                    break;
                case IfExpression ifExpression:
                    var inferenceThen = inference.InferType(ifExpression.Then);
                    var inferenceElse = inference.InferType(ifExpression.Else);
                    if (inferenceThen != inferenceElse && inferenceThen != TypeInfered.Any && inferenceElse != TypeInfered.Any) Errors.AddError($"SEMANTIC ERROR: Then and Else expression must return the same type.");
                    CheckExpression(ifExpression.Condition);
                    CheckExpression(ifExpression.Then);
                    CheckExpression(ifExpression.Else);
                    break;
                case IntersectExpression intersect:
                    var inferType1 = InferedTypeToType(inference.InferType(intersect.F1));
                    var inferType2 = InferedTypeToType(inference.InferType(intersect.F2));
                    if ((inferType1 is not IDraw || inferType2 is not IDraw) && inference.InferType(intersect.F1) != TypeInfered.Any && inference.InferType(intersect.F2) != TypeInfered.Any) Errors.AddError($"SEMANTIC ERROR: Intersect expression must receive two figures");
                    CheckExpression(intersect.F1);
                    CheckExpression(intersect.F2);
                    break;
                case LetInExpression letIn:
                    CheckLetInStatements(letIn);
                    break;
                case LineExpression line:
                    var p1_ = inference.InferType(line.P1);
                    if (p1_ != TypeInfered.Point && p1_ != TypeInfered.Any) Errors.AddError($"SEMANTIC ERROR: Expected Point type but got {inference.InferType(line.P1)}, Line: {line.Positions["p1"].Item1}, Column: {line.Positions["p1"].Item2}");
                    var p2_ = inference.InferType(line.P2);
                    if (p2_ != TypeInfered.Point && p2_ != TypeInfered.Any) Errors.AddError($"SEMANTIC ERROR: Expected Point type but got {inference.InferType(line.P2)}, Line: {line.Positions["p2"].Item1}, Column: {line.Positions["p2"].Item2}");
                    CheckExpression(line.P1);
                    CheckExpression(line.P2);
                    break;
                case MeasureExpression measure1:
                    var p1 = inference.InferType(measure1.P1);
                    if (p1 != TypeInfered.Point && p1 != TypeInfered.Any) Errors.AddError($"SEMANTIC ERROR: Expected Point type but got {inference.InferType(measure1.P1)}, Line: {measure1.Positions["p1"].Item1}, Column: {measure1.Positions["p1"].Item2}");
                    var p2 = inference.InferType(measure1.P2);
                    if (p2 != TypeInfered.Point && p2 != TypeInfered.Any) Errors.AddError($"SEMANTIC ERROR: Expected Point type but got {inference.InferType(measure1.P2)}, Line: {measure1.Positions["p2"].Item1}, Column: {measure1.Positions["p2"].Item2}");
                    CheckExpression(measure1.P1);
                    CheckExpression(measure1.P2);
                    break;
                case ParenExpression paren:
                    CheckExpression(paren.Expression);
                    break;
                case RayExpression ray:
                    var start1 = inference.InferType(ray.Start);
                    if (start1 != TypeInfered.Point && start1 != TypeInfered.Any) Errors.AddError($"SEMANTIC ERROR: Expected Point type but got {inference.InferType(ray.Start)}, Line: {ray.Positions["start"].Item1}, Column: {ray.Positions["start"].Item2}");
                    var end1 = inference.InferType(ray.End);
                    if (end1 != TypeInfered.Point && end1 != TypeInfered.Any) Errors.AddError($"SEMANTIC ERROR: Expected Point type but got {inference.InferType(ray.End)}, Line: {ray.Positions["end"].Item1}, Column: {ray.Positions["end"].Item2}");
                    CheckExpression(ray.Start);
                    CheckExpression(ray.End);
                    break;
                case SegmentExpression segment:
                    var start2 = inference.InferType(segment.Start);
                    if (start2 != TypeInfered.Point && start2 != TypeInfered.Any) Errors.AddError($"SEMANTIC ERROR: Expected Point type but got {inference.InferType(segment.Start)}, Line: {segment.Positions["start"].Item1}, Column: {segment.Positions["start"].Item2}");
                    var end2 = inference.InferType(segment.End);
                    if (end2 != TypeInfered.Point && end2 != TypeInfered.Any) Errors.AddError($"SEMANTIC ERROR: Expected Point type but got {inference.InferType(segment.End)}, Line: {segment.Positions["end"].Item1}, Column: {segment.Positions["end"].Item2}");
                    CheckExpression(segment.Start);
                    CheckExpression(segment.End);
                    break;
                case FunctionCallExpression functionCall:
                    CheckFunctionCall(functionCall);
                    break;
                case VariableExpression variable:
                    var variableType = SymbolTable.Resolve(variable.Name.Text);
                    if (variableType.ObjectType == ObjectTypes.Error) Errors.AddError($"SEMANTIC ERROR: Variable '{variable.Name.Text}' not defined");
                    break;
                default:
                    break;
            }
        }

        private void CheckLetInStatements(LetInExpression letIn)
        {
            SymbolTable.EnterScope();

            foreach (var statement in letIn.Let)
            {
                Check(statement);
            }

            CheckExpression(letIn.In);

            SymbolTable.ExitScope();
        }
        private void CheckFunctionCall(FunctionCallExpression functionCall)
        {
            var function = SymbolTable.ResolveFunction(functionCall.FunctionName.Text);
            if (function.ObjectType == ObjectTypes.Error)
            {
                Errors.AddError($"SEMANTIC ERROR: Function '{functionCall.FunctionName.Text}' not defined");
                return;
            }

            SymbolTable.EnterScope();

            var functionDefined = (Function)function;

            if (functionCall.Arguments.Count != functionDefined.Arguments.Count)
            {
                Errors.AddError($"SEMANTIC ERROR: Function '{functionCall.FunctionName.Text}' expected {functionDefined.Arguments.Count} argument(s), but {functionCall.Arguments.Count} were given");
                return;
            }

            for (int i = 0; i < functionCall.Arguments.Count; i++)
            {
                TypeInference inference = new(SymbolTable);
                var argumentType = inference.InferType(functionCall.Arguments[i]);

                SymbolTable.Define(functionDefined.Arguments[i].Text, InferedTypeToType(argumentType));

                if (argumentType == TypeInfered.ErrorType) Errors.AddError($"SEMANTIC ERROR: Function '{functionCall.FunctionName.Text}' argument '{functionDefined.Arguments[i].Text}' is not defined");

                if (argumentType == TypeInfered.Any) continue;
            }

            // Chequear el cuerpo de la funcion con los argumentos actuales
            CheckExpression(functionDefined.Body);

            SymbolTable.ExitScope();
        }

        internal static Type InferedTypeToType(TypeInfered argumentType)
        {
            return argumentType switch
            {
                TypeInfered.Any => new Undefined(),
                TypeInfered.Number => new NumberLiteral(0),
                TypeInfered.String => new StringLiteral(""),
                TypeInfered.Point => new Point(),
                TypeInfered.Line => new Line(new Point(), new Point()),
                TypeInfered.Circle => new Circle(new Point(), new Measure(new Point(), new Point())),
                TypeInfered.Ray => new Ray(new Point(), new Point()),
                TypeInfered.Arc => new Arc(new Point(), new Point(), new Point(), new Measure(new Point(), new Point())),
                TypeInfered.Segment => new Segment(new Point(), new Point()),
                TypeInfered.Measure => new Measure(new Point(), new Point()),
                _ => new ErrorType(),
            };
        }
    }
}
