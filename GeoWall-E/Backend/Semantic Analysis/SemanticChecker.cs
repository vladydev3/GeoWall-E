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
        SymbolTable SymbolTable_ { get; set; }

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

        private void Check(Node node)
        {
            switch (node)
            {
                case EmptyNode:
                case ErrorExpression:
                case ErrorStatement:
                case ImportStatement:
                    break;
                case PointStatement point:
                    if (SymbolTable.Resolve(point.Name.Text) is not ErrorType or Function) Errors.AddError($"SEMANTIC ERROR: Point '{point.Name.Text}' already defined");
                    else SymbolTable.Define(point.Name.Text, new Point(point.Name.Text));
                    break;
                case Draw draw:
                    CheckExpression(draw.Expression);
                    break;
                case LineStatement line:
                    if (SymbolTable.Resolve(line.Name.Text) is not ErrorType or Function) Errors.AddError($"SEMANTIC ERROR: Line '{line.Name.Text}' already defined");
                    else SymbolTable.Define(line.Name.Text, new Line(new Point(), new Point(), line.Name.Text));
                    break;
                case CircleStatement circle:
                    if (SymbolTable.Resolve(circle.Name.Text) is not ErrorType or Function) Errors.AddError($"SEMANTIC ERROR: Circle '{circle.Name.Text}' already defined");
                    else SymbolTable.Define(circle.Name.Text, new Circle(new Point(), new Measure(new Point(), new Point()), circle.Name.Text));
                    break;
                case RayStatement ray:
                    if (SymbolTable.Resolve(ray.Name.Text) is not ErrorType or Function) Errors.AddError($"SEMANTIC ERROR: Ray '{ray.Name.Text}' already defined");
                    else SymbolTable.Define(ray.Name.Text, new Ray(new Point(), new Point(), ray.Name.Text));
                    break;
                case SegmentStatement segment:
                    if (SymbolTable.Resolve(segment.Name.Text) is not ErrorType or Function) Errors.AddError($"SEMANTIC ERROR: Segment '{segment.Name.Text}' already defined");
                    else SymbolTable.Define(segment.Name.Text, new Segment(new Point(), new Point(), segment.Name.Text));
                    break;
                case MeasureStatement measure:
                    if (SymbolTable.Resolve(measure.Name.Text) is not ErrorType or Function) Errors.AddError($"SEMANTIC ERROR: Measure '{measure.Name.Text}' already defined");
                    else SymbolTable.Define(measure.Name.Text, new Measure(new Point(), new Point(), measure.Name.Text));
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

        static Type InferedTypeToType(TypeInfered argumentType)
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
                TypeInfered.ErrorType => new ErrorType(),
                _ => throw new NotImplementedException(),
            };
        }

        internal void CheckExpression(Expression expression)
        {
            switch (expression)
            {
                case ErrorExpression:
                case NumberExpression:
                case StringExpression:
                case UndefinedExpression:
                    break;
                case SequenceExpression sequence:
                    if (sequence.Elements == null) break;
                    TypeInference inference = new(SymbolTable);
                    var firstElementType = inference.InferType(sequence.Elements[0]);
                    foreach (var element in sequence.Elements)
                    {
                        if (inference.InferType(element) != firstElementType) Errors.AddError("SEMANTIC ERROR: Sequence elements must be of the same type");
                        CheckExpression(element);
                    }
                    break;
                case BinaryExpression binary:
                    CheckExpression(binary.Left);
                    CheckExpression(binary.Right);
                    break;
                case UnaryExpression unary:
                    CheckExpression(unary.Operand);
                    break;
                case ArcExpression arc:
                    CheckExpression(arc.Center);
                    CheckExpression(arc.Start);
                    CheckExpression(arc.End);
                    CheckExpression(arc.Measure);
                    break;
                case CircleExpression circle:
                    CheckExpression(circle.Center);
                    CheckExpression(circle.Radius);
                    break;
                case IfExpression ifExpression:
                    CheckExpression(ifExpression.Condition);
                    CheckExpression(ifExpression.Then);
                    CheckExpression(ifExpression.Else);
                    break;
                case IntersectExpression intersect:
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
