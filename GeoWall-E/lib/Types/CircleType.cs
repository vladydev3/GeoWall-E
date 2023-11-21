namespace GeoWall_E;

public class Circle : Type
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
