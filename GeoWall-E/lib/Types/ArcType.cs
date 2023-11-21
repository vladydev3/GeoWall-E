namespace GeoWall_E;

public class Arc : Type
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
    public void Draw(Canvas drawingCanvas) 
    {
        string colorString = this.Color.GetString(); // Suponiendo que esto devuelve "blue"
        System.Windows.Media.Color mediaColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorString);


        double measure = Measure.GetMeasure();
        Point pointOnRay1=GetPointOnRay(Center, Start,measure);
        CreatePointAndLabel(pointOnRay1, drawingCanvas);
        Point pointOnRay2= GetPointOnRay(Center, End, measure);
        CreatePointAndLabel(pointOnRay2 , drawingCanvas);
        // Dibujar el arco
        DrawArc(drawingCanvas, pointOnRay1, pointOnRay2, Center, mediaColor);





    }
    public void DrawArc(Canvas drawingCanvas, Point start, Point end, Point center, System.Windows.Media.Color color)
    {
        // Crear un nuevo objeto Path
        Path arcPath = new Path();
        
        arcPath.Stroke = new SolidColorBrush(color);
        arcPath.StrokeThickness = 2;

        // Crear un nuevo objeto PathGeometry
        PathGeometry pathGeometry = new PathGeometry();

        // Crear un nuevo objeto PathFigure
        PathFigure pathFigure = new PathFigure();
        System.Windows.Point startPoint = new System.Windows.Point(start.X, start.Y);
        pathFigure.StartPoint = startPoint;

        // Calcular la dirección del barrido
        SweepDirection sweepDirection = SweepDirection.Clockwise;
        if ((end.X < center.X && start.X >= center.X) || (end.Y < center.Y && start.Y >= center.Y))
        {
            sweepDirection = SweepDirection.Counterclockwise;
        }

        // Crear un nuevo objeto ArcSegment
        ArcSegment arcSegment = new ArcSegment();
        System.Windows.Point endPoint = new System.Windows.Point(end.X, end.Y);
        arcSegment.Point = endPoint;
        arcSegment.Size = new Size(Math.Abs(end.X - start.X), Math.Abs(end.Y - start.Y));
        arcSegment.SweepDirection = sweepDirection;

        // Añadir ArcSegment a PathFigure
        pathFigure.Segments.Add(arcSegment);

        // Añadir PathFigure a PathGeometry
        pathGeometry.Figures.Add(pathFigure);

        // Añadir PathGeometry a Path
        arcPath.Data = pathGeometry;

        // Añadir Path al Canvas
        drawingCanvas.Children.Add(arcPath);
    }
    public Point GetPointOnRay(Point origin,Point direction, double distance)
    {
        double pentienteRay = (direction.Y - origin.Y) / (direction.X - origin.X);
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


        // Calcular las coordenadas del punto
        double x = origin.X + distance * Math.Cos(angle);
        double y = origin.Y + distance * Math.Sin(angle);
        Point newPoint = new Point(origin.Color);
        newPoint.X = x;
        newPoint.Y = y; 
        return newPoint;
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
