namespace GeoWall_E
{
    public class Segment : Type, IDrawable
    {
        public override ObjectTypes ObjectType => ObjectTypes.Segment;
        private Point Start_ { get; set; }
        private Point End_ { get; set; }
        private string Name_ { get; set; }
        private Color Color_ { get; set; }

        public Segment(Point start, Point end, Color color, string name = "")
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
            System.Windows.Shapes.Line line = new()
            {
                // Establecer propiedades de la línea
                Stroke = new SolidColorBrush(mediaColor),
                X1 = Start.X,
                Y1 = Start.Y,
                X2 = End.X,
                Y2 = End.Y
            };
            IDrawable.CreatePointAndLabel(Start, drawingCanva);
            IDrawable.CreatePointAndLabel(End, drawingCanva);
            // Agregar la línea al canvas
            drawingCanva.Children.Add(line);
        }
    }
}