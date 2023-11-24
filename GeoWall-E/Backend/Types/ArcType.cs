namespace GeoWall_E
{
    public class Arc : Type, IDrawable
    {
        public override ObjectTypes ObjectType => ObjectTypes.Arc;
        private Point Center_ { get; set; }
        private Point Start_ { get; set; }
        private Point End_ { get; set; }
        private Measure Measure_ { get; set; }
        private string Name_ { get; set; }
        private Color Color_ { get; set; }

        public Arc(Point center, Point start, Point end, Measure measure, Color color, string name = "")
        {
            Center_ = center;
            Start_ = start;
            End_ = end;
            Measure_ = measure;
            Color_ = color;
            Name_ = name;
        }

        public Point Center => Center_;

        public Point Start => Start_;

        public Point End => End_;   

        public Measure Measure => Measure_;

        public string Name => Name_;

        public Color Color => Color_;
        
        public void Draw(Canvas drawingCanvas)
        {
            string colorString = this.Color.ToString(); // Suponiendo que esto devuelve "blue"
            System.Windows.Media.Color mediaColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorString);

            double measure = Measure.GetMeasure();
            Point pointOnRay1 = GetPointOnRay(Center, Start, measure);
            IDrawable.CreatePointAndLabel(pointOnRay1, drawingCanvas);
            Point pointOnRay2 = GetPointOnRay(Center, End, measure);
            IDrawable.CreatePointAndLabel(pointOnRay2, drawingCanvas);
            // Dibujar el arco
            DrawArc(drawingCanvas, pointOnRay1, pointOnRay2, Center, mediaColor);
        }
        public static void DrawArc(Canvas drawingCanvas, Point start, Point end, Point center, System.Windows.Media.Color color)
        {
            // Crear un nuevo objeto Path
            Path arcPath = new()
            {
                Stroke = new SolidColorBrush(color),
                StrokeThickness = 2
            };

            // Crear un nuevo objeto PathGeometry
            PathGeometry pathGeometry = new();

            // Crear un nuevo objeto PathFigure
            PathFigure pathFigure = new();
            System.Windows.Point startPoint = new(start.X, start.Y);
            pathFigure.StartPoint = startPoint;

            // Calcular la dirección del barrido
            SweepDirection sweepDirection = SweepDirection.Clockwise;
            if ((end.X < center.X && start.X >= center.X) || (end.Y < center.Y && start.Y >= center.Y))
            {
                sweepDirection = SweepDirection.Counterclockwise;
            }

            // Crear un nuevo objeto ArcSegment
            ArcSegment arcSegment = new();
            System.Windows.Point endPoint = new(end.X, end.Y);
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

            // Calcular las coordenadas del punto
            double x = origin.X + distance * Math.Cos(angle);
            double y = origin.X + distance * Math.Sin(angle);
            Point newPoint = new(origin.Color);
            newPoint.AsignX(x);
            newPoint.AsignY(y);
            return newPoint;
        }
    }
}