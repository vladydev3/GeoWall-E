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
                case EmptyNode:
                case ErrorExpression:
                case ErrorStatement:
                    break;
                case ImportStatement:
                    SymbolTable.Merge(((ImportStatement)node).Import(), Errors);
                    break;
                case PointStatement point:
                    CheckFigureStatement(point);
                    break;
                case Draw draw:
                    CheckExpression(draw.Expression);
                    break;
                case LineStatement line:
                    CheckFigureStatement(line);
                    break;
                case CircleStatement circle:
                    CheckFigureStatement(circle);
                    break;
                case RayStatement ray:
                    CheckFigureStatement(ray);
                    break;
                case SegmentStatement segment:
                    CheckFigureStatement(segment);
                    break;
                case MeasureStatement measure:
                    CheckFigureStatement(measure);
                    break;
                case FunctionDeclaration function:
                    if (SymbolTable.Resolve(function.Name.Text) is Function) Errors.AddError($"SEMANTIC ERROR: Function '{function.Name.Text}' already defined");
                    else SymbolTable.Define(function.Name.Text, new Function(function.Name, function.Arguments, function.Body));
                    break;
                case AsignationStatement asignation:
                    if (asignation.Name.Type == TokenType.Underline) break;
                    if (SymbolTable.Resolve(asignation.Name.Text) is not ErrorType) Errors.AddError($"SEMANTIC ERROR: Variable '{asignation.Name.Text}' already defined");
                    else
                    {
                        CheckExpression(asignation.Value);
                        TypeInference inference = new(SymbolTable);
                        var type = inference.InferType(asignation.Value);

                        SymbolTable.Define(asignation.Name.Text, InferedTypeToType(type));
                    }
                    break;
                case MultipleAsignationStatement multipleAsignation:
                    foreach (var variable in multipleAsignation.IDs)
                    {
                        if (variable.Type == TokenType.Underline) continue;
                        if (SymbolTable.Resolve(variable.Text) is not ErrorType) Errors.AddError($"SEMANTIC ERROR: Variable '{variable.Text}' already defined");
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

        private void CheckFunctionCall(FunctionCallExpression functionCall)
        {
            var function = SymbolTable.Resolve(functionCall.FunctionName.Text);
            if (function.ObjectType != ObjectTypes.Function || function.ObjectType == ObjectTypes.Error)
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
                            if (inference.InferType(element) != firstElementType) Errors.AddError("SEMANTIC ERROR: Sequence elements must be of the same type");
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
                    if (inference.InferType(arc.Center) != TypeInfered.Point) Errors.AddError($"SEMANTIC ERROR: Expected Point type but got {inference.InferType(arc.Center)} Line: {arc.Positions["center"].Item1}, Column: {arc.Positions["center"].Item2}");
                    if (inference.InferType(arc.Start) != TypeInfered.Point) Errors.AddError($"SEMANTIC ERROR: Expected Point type but got {inference.InferType(arc.Start)} Line: {arc.Positions["start"].Item1}, Column: {arc.Positions["start"].Item2}");
                    if (inference.InferType(arc.End) != TypeInfered.Point) Errors.AddError($"SEMANTIC ERROR: Expected Point type but got {inference.InferType(arc.End)} Line: {arc.Positions["end"].Item1}, Column: {arc.Positions["end"].Item2}");
                    if (inference.InferType(arc.Measure) != TypeInfered.Measure) Errors.AddError($"SEMANTIC ERROR: Expected Measure type but got {inference.InferType(arc.Measure)}, Line: {arc.Positions["measure"].Item1}, Column: {arc.Positions["measure"].Item2}");
                    CheckExpression(arc.Center);
                    CheckExpression(arc.Start);
                    CheckExpression(arc.End);
                    CheckExpression(arc.Measure);
                    break;
                case CircleExpression circle:
                    if (inference.InferType(circle.Center) != TypeInfered.Point) Errors.AddError($"SEMANTIC ERROR: Expected Point type but got {inference.InferType(circle.Center)} Line: {circle.Positions["center"].Item1}, Column: {circle.Positions["center"].Item2}");
                    if (inference.InferType(circle.Radius) != TypeInfered.Measure) Errors.AddError($"SEMANTIC ERROR: Expected Measure type but got {inference.InferType(circle.Radius)} Line: {circle.Positions["radius"].Item1}, Column: {circle.Positions["radius"].Item2}");
                    CheckExpression(circle.Center);
                    CheckExpression(circle.Radius);
                    break;
                case IfExpression ifExpression:
                    if (inference.InferType(ifExpression.Then) != inference.InferType(ifExpression.Else)) Errors.AddError($"SEMANTIC ERROR: Then and Else expression must return the same type.");
                    CheckExpression(ifExpression.Condition);
                    CheckExpression(ifExpression.Then);
                    CheckExpression(ifExpression.Else);
                    break;
                case IntersectExpression intersect:
                    var inferType1 = InferedTypeToType(inference.InferType(intersect.F1));
                    var inferType2 = InferedTypeToType(inference.InferType(intersect.F2));
                    if (inferType1 is not IDraw || inferType2 is not IDraw) Errors.AddError($"SEMANTIC ERROR: Intersect expression must receive two figures");
                    CheckExpression(intersect.F1);
                    CheckExpression(intersect.F2);
                    break;
                case LetInExpression letIn:
                    CheckLetInStatements(letIn);
                    break;
                case LineExpression line:
                    CheckExpression(line.P1);
                    CheckExpression(line.P2);
                    break;
                case MeasureExpression measure:
                    CheckExpression(measure.P1);
                    CheckExpression(measure.P2);
                    break;
                case ParenExpression paren:
                    CheckExpression(paren.Expression);
                    break;
                case RayExpression ray:
                    CheckExpression(ray.Start);
                    CheckExpression(ray.End);
                    break;
                case SegmentExpression segment:
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
    }
}
