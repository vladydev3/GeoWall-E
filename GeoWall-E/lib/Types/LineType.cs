namespace GeoWall_E;

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
       CreatePointAndLabel(P1, drawingCanva);
        CreatePointAndLabel(P2, drawingCanva);

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
