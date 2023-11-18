
using System.Windows.Controls;

namespace GeoWall_E;

public abstract class Types
{
    public abstract ObjectTypes Type { get; }

}

public class Point : Types
{
    public override ObjectTypes Type => ObjectTypes.Point;
    public double X { get; set; }
    public double Y { get; set; }
    public string Name { get; set; }
    public Color Color { get; set; }

    public Point(Color color, string name = "")
    {
        Name = name;
        Color = color;
        X = GetX();
        Y = GetY();

    }
    public double GetX()
    {
        Random random = new Random();
        int drawingCanvasWidth = (int)MainWindow.DrawingCanvas.Width;
        double pointCenterX = random.Next(0, drawingCanvasWidth);
        return pointCenterX;
    }

    public double GetY()
    {
        Random random = new Random();
        int drawingCanvasHeight = (int)MainWindow.DrawingCanvas.Height;
        double pointCenterY = random.Next(0, drawingCanvasHeight);
        return pointCenterY;
    }
    public void Draw(string name, Color color, Canvas drawingCanvas, Ellipse point1, Label label, double X, double Y)
    {

        // Añadir el punto y la etiqueta al Canvas
        drawingCanvas.Children.Add(point1);
        drawingCanvas.Children.Add(label);

        Canvas.SetLeft(point1, X - point1.Width / 2);
        Canvas.SetTop(point1, Y - point1.Height / 2);

        double labelCenterX = X; // La misma X que el punto
        double labelCenterY = Y - 20; // Un poco por encima del punto

        Canvas.SetLeft(label, labelCenterX - label.ActualWidth / 2);
        Canvas.SetTop(label, labelCenterY - label.ActualHeight / 2);
    }
}


public class Line : Types
{
    public override ObjectTypes Type => ObjectTypes.Line;
    public Point P1 { get; set; }
    public Point P2 { get; set; }
    public string Name { get; set; }

    public Line(Point p1, Point p2, string name = "")
    {
        P1 = p1;
        P2 = p2;
        Name = name;
    }
    public void Draw(Canvas drawingCanva)
    {
        // Crear una línea
        System.Windows.Shapes.Line line = new System.Windows.Shapes.Line();

        // Establecer propiedades de la línea
        line.Stroke = Brushes.LightSteelBlue;
        if (P1.X == 0 && P1.Y == 0)
        {
            Random rand = new Random();

            // Generar una inclinación aleatoria
            double angle = rand.NextDouble() * Math.PI * 2;  // Ángulo en radianes

            // Generar una posición aleatoria para el centro de la línea
            double centerX = rand.NextDouble() * drawingCanva.Width;
            double centerY = rand.NextDouble() * drawingCanva.Height;

            // Calcular los puntos de inicio y final de la línea
            double radius = Math.Sqrt(drawingCanva.Width * drawingCanva.Width * drawingCanva.Height * drawingCanva.Height);  // Radio suficientemente grande para cubrir todo el lienzo
            line.X1 = centerX + radius * Math.Cos(angle + Math.PI);  // Punto de inicio
            line.Y1 = centerY + radius * Math.Sin(angle + Math.PI);
            line.X2 = centerX + radius * Math.Cos(angle);  // Punto final
            line.Y2 = centerY + radius * Math.Sin(angle);

            // Agregar la línea al lienzo
            drawingCanva.Children.Add(line);
        }
        else
        {
            // Coordenadas de inicio
            line.X1 = P1.X;
            line.Y1 = P1.Y;
            Ellipse point1 = new Ellipse
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.Blue,
                ToolTip = P1.Name// Asigna el nombre del punto a ToolTip
            };
            // Crear una etiqueta con el nombre del punto
            Label label1 = new Label
            {
                Content = P1.Name,
                Foreground = Brushes.Black
            };
            drawingCanva.Children.Add(point1);
            drawingCanva.Children.Add(label1);

            Canvas.SetLeft(point1, P1.X - point1.Width / 2);
            Canvas.SetTop(point1, P1.Y - point1.Height / 2);

            double labelCenterX1 = P1.X; // La misma X que el punto
            double labelCenterY1 = P1.Y - 20; // Un poco por encima del punto

            Canvas.SetLeft(label1, labelCenterX1 - label1.ActualWidth / 2);
            Canvas.SetTop(label1, labelCenterY1 - label1.ActualHeight / 2);
            // Coordenadas de fin
            line.X2 = P2.X;
            line.Y2 = P2.Y;
            Ellipse point2 = new Ellipse
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.Blue,
                ToolTip = P2.Name// Asigna el nombre del punto a ToolTip
            };
            // Crear una etiqueta con el nombre del punto
            Label label2 = new Label
            {
                Content = P2.Name,
                Foreground = Brushes.Black
            };
            drawingCanva.Children.Add(point2);
            drawingCanva.Children.Add(label2);

            Canvas.SetLeft(point1, P2.X - point1.Width / 2);
            Canvas.SetTop(point1, P2.Y - point1.Height / 2);

            double labelCenterX2 = P2.X; // La misma X que el punto
            double labelCenterY2 = P2.Y - 20; // Un poco por encima del punto

            Canvas.SetLeft(label2, labelCenterX2 - label2.ActualWidth / 2);
            Canvas.SetTop(label2, labelCenterY2 - label2.ActualHeight / 2);
            // Coordenadas de fin

            // Agregar la línea al canvas
            drawingCanva.Children.Add(line);
        }

    }
}

public class Segment : Types
{
    public override ObjectTypes Type => ObjectTypes.Segment;
    public Point Start { get; set; }
    public Point End { get; set; }
    public string Name { get; set; }

    public Segment(Point start, Point end, string name = "")
    {
        Start = start;
        End = end;
        Name = name;
    }
}

public class Ray : Types
{
    public override ObjectTypes Type => ObjectTypes.Ray;
    public Point Start { get; set; }
    public Point End { get; set; }
    public string Name { get; set; }

    public Ray(Point start, Point end, string name = "")
    {
        Start = start;
        End = end;
        Name = name;
    }
}

public class Arc : Types
{
    public override ObjectTypes Type => ObjectTypes.Arc;
    public Point Center { get; set; }
    public Point Start { get; set; }
    public Point End { get; set; }
    public int Measure { get; set; }
    public string Name { get; set; }

    public Arc(Point center, Point start, Point end, int measure, string name = "")
    {
        Center = center;
        Start = start;
        End = end;
        Measure = measure;
        Name = name;
    }
}

public class Circle : Types
{
    public override ObjectTypes Type => ObjectTypes.Circle;
    public Point Center { get; set; }
    public Measure Radius { get; set; }
    public Color Color { get; set; }
    public string Name { get; set; }

    public Circle(Point center, Measure radius, Color color, string name = "")
    {
        Center = center;
        Radius = radius;
        Color = color;
        Name = name;
    }
}

public class Measure : Types
{
    public override ObjectTypes Type => ObjectTypes.Measure;

    public Point P1 { get; set; }
    public Point P2 { get; set; }
    public string Name { get; set; }

    public Measure(Point p1, Point p2, string name = "")
    {
        Name = name;
        P1 = p1;
        P2 = p2;
    }
}

public class Undefined : Types
{
    public override ObjectTypes Type => ObjectTypes.Undefined;
}

public class Sequence : Types
{
    public override ObjectTypes Type => ObjectTypes.Sequence;
    public List<Types> Elements { get; set; }

    public Sequence(List<Types> elements)
    {
        Elements = elements;
    }
}