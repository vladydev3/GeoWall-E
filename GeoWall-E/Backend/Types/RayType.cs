namespace GeoWall_E
{
    public class Ray : Type, IDrawable
    {
        public override ObjectTypes ObjectType => ObjectTypes.Ray;
        private Point Start_ { get; set; }
        private Point End_ { get; set; }
        private string Name_ { get; set; }
        private Color Color_ { get; set; }

        public Ray(Point start, Point end, Color color, string name = "")
        {
            Start_ = start;
            End_ = end;
            Color_ = color;
            Name_ = name;
        }
        public Point Start => Start_;

        public Point End => End_;

        public string Name => Name_;

        public Color Color => Color_;

        public void Draw(Canvas drawingCanva)
        {
            string colorString = Start.Color.ToString(); // Suponiendo que esto devuelve "blue"
            System.Windows.Media.Color mediaColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorString);
            IDrawable.CreatePointAndLabel(Start, drawingCanva);
            IDrawable.CreatePointAndLabel(End, drawingCanva);
            System.Windows.Shapes.Line line = new()
            {
                // Establecer propiedades de la línea
                Stroke = new SolidColorBrush(mediaColor)
            };
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
    }
}
