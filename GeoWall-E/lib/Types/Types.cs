
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
        X = SetX();
        Y = SetY(); 
        
    }
    public double SetX()
    {
        Random random = new Random();
        int drawingCanvasWidth = (int)MainWindow.DrawingCanvas.Width;
        double pointCenterX = drawingCanvasWidth / 2 + random.Next(0, 500);
        return pointCenterX;
    }

    public double SetY()
    {
        Random random = new Random();
        int drawingCanvasHeight = (int)MainWindow.DrawingCanvas.Height;
        double pointCenterY = drawingCanvasHeight / 2 - random.Next(0, 500);
        return pointCenterY;
    }
    public void Draw(Canvas drawingCanvas) 
    {
        CreatePointAndLabel(this, drawingCanvas);
        

    }
    void CreatePointAndLabel(Point P, Canvas drawingCanva)
    {
        string colorString = P.Color.GetString(); // Suponiendo que esto devuelve "blue"
        System.Windows.Media.Color mediaColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorString);
        Ellipse point = new Ellipse
        {
            Width = 10,
            Height = 10,
            Fill = new SolidColorBrush(mediaColor),
            ToolTip = P.Name // Asigna el nombre del punto a ToolTip
        };

        // Crear una etiqueta con el nombre del punto
        Label label = new Label
        {
            Content = P.Name,
            Foreground = Brushes.Black
        };

        drawingCanva.Children.Add(point);
        drawingCanva.Children.Add(label);

        double centerX = P.X - point.Width / 2;
        double centerY = P.Y - point.Height / 2;
        Canvas.SetLeft(point, centerX);
        Canvas.SetTop(point, centerY);

        double labelCenterX = centerX; // La misma X que el punto
        double labelCenterY = centerY - 20; // Un poco por encima del punto

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
    public Color Color { get; set; }

    public Line(Point p1, Point p2, Color color, string name = "")
    {
        P1 = p1;
        P2 = p2;
        Color = color;
        Name = name;
    }
    
    public void Draw(Canvas drawingCanva)
    {
        string colorString = P1.Color.GetString(); // Suponiendo que esto devuelve un string
        System.Windows.Media.Color mediaColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorString);
        // Crear una línea
        System.Windows.Shapes.Line line = new System.Windows.Shapes.Line();
        // Establecer propiedades de la línea
        line.Stroke = new SolidColorBrush(mediaColor);

        // Calcular la pendiente de la línea
        double m = (P2.Y - P1.Y) / (P2.X - P1.X);

        // Calcular el intercepto y
        double b = P1.Y - m * P1.X;

        // Coordenadas de inicio
        line.X1 = 0; // borde izquierdo del lienzo
        line.Y1 = m * line.X1 + b;

        // Coordenadas de fin
        line.X2 = drawingCanva.Width; // borde derecho del lienzo
        line.Y2 = m * line.X2 + b;

        // Crear los puntos y las etiquetas
       // CreatePointAndLabel(P1, drawingCanva);
        //CreatePointAndLabel(P2, drawingCanva);

        // Agregar la línea al canvas
        drawingCanva.Children.Add(line);
    }
    void CreatePointAndLabel(Point P, Canvas drawingCanva)
    {
        string colorString = P.Color.GetString(); // Suponiendo que esto devuelve "blue"
        System.Windows.Media.Color mediaColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorString);
        Ellipse point = new Ellipse
        {
            Width = 10,
            Height = 10,
            Fill = new SolidColorBrush(mediaColor), // Usa el color de point.Color
            ToolTip = P.Name // Asigna el nombre del punto a ToolTip
        };

        // Crear una etiqueta con el nombre del punto
       /* Label label = new Label
        {
            Content = P.Name,
            Foreground = Brushes.Black
        };
       */
        drawingCanva.Children.Add(point);
        //drawingCanva.Children.Add(label);

        Canvas.SetLeft(point, P.X - point.Width / 2);
        Canvas.SetTop(point, P.Y - point.Height / 2);

        //double labelCenterX = P.X; // La misma X que el punto
        //double labelCenterY = P.Y - 20; // Un poco por encima del punto

        //Canvas.SetLeft(label, labelCenterX - label.ActualWidth / 2);
        //Canvas.SetTop(label, labelCenterY - label.ActualHeight / 2);
    }

}

public class Segment : Types
{
    public override ObjectTypes Type => ObjectTypes.Segment;
    public Point Start { get; set; }
    public Point End { get; set; }
    public string Name { get; set; }
    public Color Color { get; set; }

    public Segment(Point start, Point end, Color color, string name = "")
    {
        Start = start;
        End = end;
        Color = color;
        Name = name;
    }
    public void Draw(Canvas drawingCanva) 
    {
        string colorString = Start.Color.GetString(); // Suponiendo que esto devuelve "blue"
        System.Windows.Media.Color mediaColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorString);
        System.Windows.Shapes.Line line = new System.Windows.Shapes.Line();
        // Establecer propiedades de la línea
        line.Stroke = new SolidColorBrush(mediaColor);
        line.X1 = Start.X;
        line.Y1=Start.Y;
        line.X2 = End.X;
        line.Y2 = End.Y;
        CreatePointAndLabel(Start, drawingCanva);
        CreatePointAndLabel(End, drawingCanva);
        // Agregar la línea al canvas
        drawingCanva.Children.Add(line);
    }
    private void CreatePointAndLabel(Point P, Canvas drawingCanva)
    {
        string colorString = P.Color.GetString(); // Suponiendo que esto devuelve "blue"
        System.Windows.Media.Color mediaColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorString);
        Ellipse point = new Ellipse
        {
            Width = 10,
            Height = 10,
            Fill = new SolidColorBrush(mediaColor),
            ToolTip = P.Name // Asigna el nombre del punto a ToolTip
        };

        // Crear una etiqueta con el nombre del punto
        Label label = new Label
        {
            Content = P.Name,
            Foreground = Brushes.Black
        };

        drawingCanva.Children.Add(point);
        drawingCanva.Children.Add(label);

        Canvas.SetLeft(point, P.X - point.Width / 2);
        Canvas.SetTop(point, P.Y - point.Height / 2);

        double labelCenterX = P.X; // La misma X que el punto
        double labelCenterY = P.Y - 20; // Un poco por encima del punto

        Canvas.SetLeft(label, labelCenterX - label.ActualWidth / 2);
        Canvas.SetTop(label, labelCenterY - label.ActualHeight / 2);
    }

}

public class Ray : Types
{
    public override ObjectTypes Type => ObjectTypes.Ray;
    public Point Start { get; set; }
    public Point End { get; set; }
    public string Name { get; set; }
    public Color Color { get; set; }

    public Ray(Point start, Point end, Color color, string name = "")
    {
        Start = start;
        End = end;
        Color = color;
        Name = name;
    }
    public void Draw(Canvas drawingCanva) 
    {
        string colorString = Start.Color.GetString(); // Suponiendo que esto devuelve "blue"
        System.Windows.Media.Color mediaColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorString);
        CreatePointAndLabel(Start, drawingCanva);
        CreatePointAndLabel(End, drawingCanva);
        System.Windows.Shapes.Line line = new System.Windows.Shapes.Line();
        // Establecer propiedades de la línea
        line.Stroke = new SolidColorBrush(mediaColor);
        // Calcular la pendiente de la línea
        double m = (End.Y - Start.Y) / (End.X - Start.X);

        // Calcular el intercepto y
        double b = Start.Y - m * Start.X;

        // Coordenadas de inicio
        line.X1 = Start.X;
        line.Y1 = Start.Y;

        // Coordenadas de fin
        if (Start.X < End.X)
        {
            line.X2 = drawingCanva.Width; // borde derecho del lienzo
        }
        else
        {
            line.X2 = 0; // borde izquierdo del lienzo
        }
        line.Y2 = m * line.X2 + b;
        drawingCanva.Children.Add(line);


    }
    private void CreatePointAndLabel(Point P, Canvas drawingCanva)
    {
        string colorString = P.Color.GetString(); // Suponiendo que esto devuelve "blue"
        System.Windows.Media.Color mediaColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorString);
        Ellipse point = new Ellipse
        {
            Width = 10,
            Height = 10,
            Fill = new SolidColorBrush(mediaColor),
            ToolTip = P.Name // Asigna el nombre del punto a ToolTip
        };

        // Crear una etiqueta con el nombre del punto
        Label label = new Label
        {
            Content = P.Name,
            Foreground = Brushes.Black
        };

        drawingCanva.Children.Add(point);
        drawingCanva.Children.Add(label);

        Canvas.SetLeft(point, P.X - point.Width / 2);
        Canvas.SetTop(point, P.Y - point.Height / 2);

        double labelCenterX = P.X; // La misma X que el punto
        double labelCenterY = P.Y - 20; // Un poco por encima del punto

        Canvas.SetLeft(label, labelCenterX - label.ActualWidth / 2);
        Canvas.SetTop(label, labelCenterY - label.ActualHeight / 2);
    }
}

public class Arc : Types
{
    public override ObjectTypes Type => ObjectTypes.Arc;
    public Point Center { get; set; }
    public Point Start { get; set; }
    public Point End { get; set; }
    public Measure Measure { get; set; }
    public string Name { get; set; }
    public Color Color { get; set; }

    public Arc(Point center, Point start, Point end, Measure measure, Color color, string name = "")
    {
        Center = center;
        Start = start;
        End = end;
        Measure = measure;
        Color = color;
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
    public void Draw(Canvas drawingCanvas) 
    {
        double radio = Radius.GetMeasure();// Establecer el radio
        // Crear una nueva instancia de Ellipse
        Ellipse miCirculo = new Ellipse();

        // Establecer las dimensiones del círculo
         
        miCirculo.Height = radio * 2; // Establecer la altura
        miCirculo.Width = radio * 2; // Establecer el ancho
        string colorString =Color.GetString(); // Suponiendo que esto devuelve "blue"
        System.Windows.Media.Color mediaColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorString);

        // Establecer el color del círculo
        miCirculo.Stroke = new SolidColorBrush(mediaColor);
        miCirculo.StrokeThickness = 2;

        // Establecer el punto central
        double centroX =  Center.X; // Establecer la coordenada X del centro
        double centroY = Center.Y; // Establecer la coordenada Y del centro
        CreatePointAndLabel(Center, drawingCanvas);
        // Comprobar si el círculo se pasa de los límites del Canvas

        // Añadir el círculo a un Canvas
        Canvas.SetTop(miCirculo, centroY - radio); // Establecer la posición superior
        Canvas.SetLeft(miCirculo, centroX - radio); // Establecer la posición izquierda
        drawingCanvas.Children.Add(miCirculo); // Añadir el círculo al Canvas
    }
    private void CreatePointAndLabel(Point P, Canvas drawingCanva)
    {
        string colorString = P.Color.GetString(); // Suponiendo que esto devuelve "blue"
        System.Windows.Media.Color mediaColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorString);
        Ellipse point = new Ellipse
        {
            Width = 10,
            Height = 10,
            Fill = new SolidColorBrush(mediaColor),
            ToolTip = P.Name // Asigna el nombre del punto a ToolTip
        };

        // Crear una etiqueta con el nombre del punto
        Label label = new Label
        {
            Content = P.Name,
            Foreground = Brushes.Black
        };

        drawingCanva.Children.Add(point);
        drawingCanva.Children.Add(label);

        Canvas.SetLeft(point, P.X - point.Width / 2);
        Canvas.SetTop(point, P.Y - point.Height / 2);

        double labelCenterX = P.X; // La misma X que el punto
        double labelCenterY = P.Y - 20; // Un poco por encima del punto

        Canvas.SetLeft(label, labelCenterX - label.ActualWidth / 2);
        Canvas.SetTop(label, labelCenterY - label.ActualHeight / 2);
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
    public double GetMeasure() 
    {
        double measure = Math.Sqrt(Math.Pow(P2.X - P1.X, 2) + Math.Pow(P2.Y - P1.Y, 2));
        return measure;
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