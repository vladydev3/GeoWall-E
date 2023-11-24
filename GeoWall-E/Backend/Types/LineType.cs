namespace GeoWall_E
{
    public class Line : Type, IDrawable
    {
        public override ObjectTypes ObjectType => ObjectTypes.Line;
        private Point P1_ { get; set; }
        private Point P2_ { get; set; }
        private string Name_ { get; set; }
        private Color Color_ { get; set; }

        public Line(Point p1, Point p2, Color color, string name = "")
        {
            P1_ = p1;
            P2_ = p2;
            Color_ = color;
            Name_ = name;
        }

        public Point P1 => P1_;

        public Point P2 => P2_;

        public string Name => Name_;

        public Color Color => Color_;

        public void Draw(Canvas drawingCanva)
        {
            string colorString = P1.Color.ToString(); // Suponiendo que esto devuelve un string
            System.Windows.Media.Color mediaColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorString);
            // Crear una línea
            System.Windows.Shapes.Line line = new()
            {
                // Establecer propiedades de la línea
                Stroke = new SolidColorBrush(mediaColor)
            };

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
            IDrawable.CreatePointAndLabel(P1, drawingCanva);
            IDrawable.CreatePointAndLabel(P2, drawingCanva);

            // Agregar la línea al canvas
            drawingCanva.Children.Add(line);
        }
    }
}
