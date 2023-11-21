namespace GeoWall_E;

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
