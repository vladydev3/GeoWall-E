namespace GeoWall_E
{
    public class Circle : Type, IDrawable
    {
        public override ObjectTypes ObjectType => ObjectTypes.Circle;
        private Point Center_ { get; set; }
        private Measure Radius_ { get; set; }
        private Color Color_ { get; set; }
        private string Name_ { get; set; }

        public Circle(Point center, Measure radius, Color color, string name = "")
        {
            Center_ = center;
            Radius_ = radius;
            Color_ = color;
            Name_ = name;
        }

        public Point Center => Center_;

        public Measure Radius => Radius_;

        public Color Color => Color_;

        public string Name => Name_;

        public void Draw(Canvas drawingCanvas)
        {
            double radio = Radius.GetMeasure();// Establecer el radio
                                               // Crear una nueva instancia de Ellipse
            Ellipse miCirculo = new()
            {
                // Establecer las dimensiones del círculo
                Height = radio * 2, // Establecer la altura
                Width = radio * 2 // Establecer el ancho
            };
            string colorString = Color.ToString(); // Suponiendo que esto devuelve "blue"
            System.Windows.Media.Color mediaColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorString);

            // Establecer el color del círculo
            miCirculo.Stroke = new SolidColorBrush(mediaColor);
            miCirculo.StrokeThickness = 2;

            // Establecer el punto central
            double centroX = Center.X; // Establecer la coordenada X del centro
            double centroY = Center.Y; // Establecer la coordenada Y del centro
            IDrawable.CreatePointAndLabel(Center, drawingCanvas);
            // Comprobar si el círculo se pasa de los límites del Canvas

            // Añadir el círculo a un Canvas
            Canvas.SetTop(miCirculo, centroY - radio); // Establecer la posición superior
            Canvas.SetLeft(miCirculo, centroX - radio); // Establecer la posición izquierda
            drawingCanvas.Children.Add(miCirculo); // Añadir el círculo al Canvas
        }
    }
}