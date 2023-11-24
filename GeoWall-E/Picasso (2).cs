﻿global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;
global using System.Windows;
global using System.Windows.Controls;
global using System.Windows.Data;
global using System.Windows.Documents;
global using System.Windows.Input;
global using System.Windows.Media;
global using System.Windows.Media.Imaging;
global using System.Windows.Navigation;
global using System.Windows.Shapes;
global using System.Windows.Threading;
using System.Drawing.Drawing2D;
using System.Windows.Media.Media3D;

namespace GeoWall_E
{
    public class Picasso : IDrawable
    {
        public Type FigureToDraw;
        public Canvas drawingCanvas;
        public Picasso(Canvas drawingCanvas, Type FiguretoDraw)
        {
            this.FigureToDraw = FiguretoDraw;
            this.drawingCanvas = drawingCanvas;
        }
        public void Draw()
        {
            if (FigureToDraw is Point point)
            {

                DrawPoint(point,drawingCanvas);

            }
            else if (FigureToDraw is Line line)
            {
                DrawLines(line,drawingCanvas);

            }
            else if (FigureToDraw is Segment segment)
            {
                DrawSegment(segment,drawingCanvas);
            }
            else if (FigureToDraw is Ray ray)
            {
                DrawRay(ray,drawingCanvas);

            }
            else if (FigureToDraw is Circle circle)
            {
                DrawCircle(circle,drawingCanvas);
            }
            else if (FigureToDraw is Arc arc)
            {
                Point center = arc.Center;
                DrawPoint(center,drawingCanvas);
                Ray ray1 = new(arc.Center, arc.Start, arc.Color);
                DrawRay(ray1,drawingCanvas);
                Ray ray2 = new Ray(arc.Center, arc.End, arc.Color);
                DrawRay(ray2,drawingCanvas);
                DrawArc(arc,drawingCanvas);


            }
        }
        public static void DrawArc(Arc arc,Canvas drawingCanvas)
        {

            string colorString = arc.Color.ToString();
            System.Windows.Media.Color mediaColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorString);
            double measure = arc.Measure.GetMeasure();
            Point pointOnRay1 = GetPointOnRay(arc.Center, arc.Start, measure);
            DrawPoint(pointOnRay1, drawingCanvas);
            Point pointOnRay2 = GetPointOnRay(arc.Center, arc.End, measure);
            DrawPoint(pointOnRay2, drawingCanvas);

            // Crear un nuevo objeto Path
            Path arcPath = new()
            {
                Stroke = new SolidColorBrush(mediaColor),
                StrokeThickness = 5
            };

            // Crear un nuevo objeto PathGeometry
            PathGeometry pathGeometry = new();

            // Crear un nuevo objeto PathFigure
            PathFigure pathFigure = new();
            System.Windows.Point startPoint = new(pointOnRay1.X, pointOnRay1.Y); // Usar 'pointOnRay1' como el punto de inicio
            pathFigure.StartPoint = startPoint;

            
            SweepDirection sweepDirection = GetSweepDirection(pointOnRay1, arc.Center, pointOnRay2);

            // Crear un nuevo objeto ArcSegment
            ArcSegment arcSegment = new();
            System.Windows.Point endPoint = new(pointOnRay2.X, pointOnRay2.Y); // Usar 'pointOnRay2' como el punto final
            arcSegment.Point = endPoint;
            arcSegment.Size = new Size(measure, measure); // Usar 'measure' como el radio del arco
            arcSegment.SweepDirection = sweepDirection;

            // Añadir ArcSegment a PathFigure
            pathFigure.Segments.Add(arcSegment);

            // Añadir PathFigure a PathGeometry
            pathGeometry.Figures.Add(pathFigure);

            // Añadir PathGeometry a Path
            arcPath.Data = pathGeometry;

            // Añadir Path al Canvas
            drawingCanvas.Children.Add(arcPath);
            Circle circle = new Circle(arc.Center, arc.Measure, arc.Color);
            DrawCircle(circle, drawingCanvas);
        }
        public static SweepDirection GetSweepDirection(Point p1, Point p2, Point p3)
        {
            // Calcular el vector entre 'p1' y 'p2'
            double v1x = p1.X - p2.X;
            double v1y = p2.Y - p1.Y; // Invertir la dirección de Y

            // Calcular el vector entre 'p3' y 'p2'
            double v2x = p3.X - p2.X;
            double v2y = p2.Y - p3.Y; // Invertir la dirección de Y

            // Calcular el producto vectorial de los dos vectores
            double crossProduct = v1x * v2y - v1y * v2x;

            // La dirección del barrido depende del signo del producto vectorial
            SweepDirection sweepDirection = crossProduct >= 0 ? SweepDirection.Counterclockwise : SweepDirection.Clockwise;

            return sweepDirection;
        }

        public static Point GetPointOnRay(Point origin, Point direction, double distance)
        {
            // Calcular el ángulo del rayo usando la pendiente
            double angle = Math.Atan2(direction.Y - origin.Y, direction.X - origin.X);
            // Ajustar el ángulo en función de la dirección del rayo
            if (direction.X < origin.X && Math.Cos(angle) > 0)
            {
                angle += Math.PI;
            }
            else if (direction.X > origin.X && Math.Cos(angle) < 0)
            {
                angle -= Math.PI;
            }

            double x = origin.X + distance * Math.Cos(angle);
            double y = origin.Y + distance * Math.Sin(angle); // Usar 'origin.Y' en lugar de 'origin.X'
            Point newPoint = new(origin.Color);
            newPoint.AsignX(x);
            newPoint.AsignY(y);
            return newPoint;
        }

        public static void DrawSegment(Segment segment, Canvas drawingCanvas)
        {
            Point Start = segment.Start;
            Point End = segment.End;
            string colorString = Start.Color.ToString(); // Suponiendo que esto devuelve "blue"
            System.Windows.Media.Color mediaColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorString);
            System.Windows.Shapes.Line line2 = new()
            {
                // Establecer propiedades de la línea
                Stroke = new SolidColorBrush(mediaColor),
                X1 = Start.X,
                Y1 = Start.Y,
                X2 = End.X,
                Y2 = End.Y
            };
            DrawPoint(Start, drawingCanvas);
            DrawPoint(End, drawingCanvas);
            // Agregar la línea al canvas
            drawingCanvas.Children.Add(line2);

        }

        public static void DrawCircle(Circle circle,Canvas drawingCanvas)
        {
            Point Center = circle.Center;
            Measure radius = circle.Radius;
            double radio = radius.GetMeasure();// Establecer el radio
                                               // Crear una nueva instancia de Ellipse
            Ellipse miCirculo = new()
            {
                // Establecer las dimensiones del círculo
                Height = radio * 2, // Establecer la altura
                Width = radio * 2 // Establecer el ancho
            };
            string colorString = circle.Color.ToString(); // Suponiendo que esto devuelve "blue"
            System.Windows.Media.Color mediaColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorString);

            // Establecer el color del círculo
            miCirculo.Stroke = new SolidColorBrush(mediaColor);
            miCirculo.StrokeThickness = 2;

            // Establecer el punto central
            double centroX = Center.X; // Establecer la coordenada X del centro
            double centroY = Center.Y; // Establecer la coordenada Y del centro
            DrawPoint(Center, drawingCanvas);
            // Comprobar si el círculo se pasa de los límites del Canvas

            // Añadir el círculo a un Canvas
            Canvas.SetTop(miCirculo, centroY - radio); // Establecer la posición superior
            Canvas.SetLeft(miCirculo, centroX - radio); // Establecer la posición izquierda
            drawingCanvas.Children.Add(miCirculo); // Añadir el círculo al Canvas     

        }

        public static void DrawRay(Ray ray, Canvas drawingCanvas)
        {
            Point Start = ray.Start;
            Point End = ray.End;
            string colorString = Start.Color.ToString(); // Suponiendo que esto devuelve "blue"
            System.Windows.Media.Color mediaColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorString);
            DrawPoint(Start, drawingCanvas);
            DrawPoint(End, drawingCanvas);
            System.Windows.Shapes.Line line3 = new()
            {
                // Establecer propiedades de la línea
                Stroke = new SolidColorBrush(mediaColor)
            };
            // Calcular la pendiente de la línea
            double m = (End.Y - Start.Y) / (End.X - Start.X);

            // Calcular el intercepto y
            double b = Start.Y - m * Start.X;

            // Coordenadas de inicio
            line3.X1 = Start.X;
            line3.Y1 = Start.Y;

            // Coordenadas de fin
            if (Start.X < End.X)
            {
                line3.X2 = drawingCanvas.Width; // borde derecho del lienzo
            }
            else
            {
                line3.X2 = 0; // borde izquierdo del lienzo
            }
            line3.Y2 = m * line3.X2 + b;
            drawingCanvas.Children.Add(line3);

        }

        public static void DrawPoint(Point P,Canvas drawingCanvas)
        {
            string colorString = P.Color.ToString(); // Suponiendo que esto devuelve "blue"
            System.Windows.Media.Color mediaColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorString);
            Ellipse point = new()
            {
                Width = 10,
                Height = 10,
                Fill = new SolidColorBrush(mediaColor),
                ToolTip = P.Name // Asigna el nombre del punto a ToolTip
            };

            // Crear una etiqueta con el nombre del punto
            Label label = new()
            {
                Content = P.Name,
                Foreground = Brushes.Black
            };

            drawingCanvas.Children.Add(point);
            drawingCanvas.Children.Add(label);

            Canvas.SetLeft(point, P.X - point.Width / 2);
            Canvas.SetTop(point, P.Y - point.Height / 2);

            double labelCenterX = P.X; // La misma X que el punto
            double labelCenterY = P.Y - 20; // Un poco por encima del punto

            Canvas.SetLeft(label, labelCenterX - label.ActualWidth / 2);
            Canvas.SetTop(label, labelCenterY - label.ActualHeight / 2);
        }

        public void DrawLines(Line line,Canvas drawingCanvas)
        {
            Point P1 = line.P1;
            Point P2 = line.P2;
            string colorString = line.Color.ToString();
            System.Windows.Media.Color mediaColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorString);
            // Crear una línea
            System.Windows.Shapes.Line line1 = new()
            {
                // Establecer propiedades de la línea
                Stroke = new SolidColorBrush(mediaColor)
            };

            // Calcular la pendiente de la línea
            double m = (P2.Y - P1.Y) / (P2.X - P1.X);

            // Calcular el intercepto y
            double b = P1.Y - m * P1.X;

            // Coordenadas de inicio
            line1.X1 = 0; // borde izquierdo del lienzo
            line1.Y1 = m * line1.X1 + b;

            // Coordenadas de fin
            line1.X2 = drawingCanvas.Width; // borde derecho del lienzo
            line1.Y2 = m * line1.X2 + b;

            // Crear los puntos y las etiquetas
            DrawPoint(P1, drawingCanvas);
            DrawPoint(P2, drawingCanvas);

            // Agregar la línea al canvas
            drawingCanvas.Children.Add(line1);

        }


    }
}
