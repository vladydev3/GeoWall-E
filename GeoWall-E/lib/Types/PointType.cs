namespace GeoWall_E;

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