
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
    }
    public void Draw(string name,Color color, Canvas drawingCanvas,Ellipse point1, Label label,double X,double Y) 
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

         // Coordenadas de inicio
         line.X1 = P1.X;
         line.Y1 = P1.Y;

         // Coordenadas de fin
         line.X2 = P2.X;
         line.Y2 = P2.Y;

         // Agregar la línea al canvas
         drawingCanva.Children.Add(line);
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
    public int Radius { get; set; }
    public string Name { get; set; }

    public Circle(Point center, int radius, string name = "")
    {
        Center = center;
        Radius = radius;
        Name = name;
    }
}

public class Measure : Types
{
    public override ObjectTypes Type => ObjectTypes.Measure;
    
    public Point P1 { get; set; }
    public Point P2 { get; set; }

    public Measure(Point p1, Point p2)
    {
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