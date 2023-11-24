namespace GeoWall_E
{
    public interface IDrawable
    {
        // Esta interface la puedes quitar, depende de como vayas a implementar tu parte
        void Draw(Canvas drawingCanva);

        static void CreatePointAndLabel(Point P, Canvas drawingCanva)
        {
            string colorString = P.Color.ToString(); // Suponiendo que esto devuelve "blue"
            System.Windows.Media.Color mediaColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorString);
            Ellipse point = new()
            {
                Width = 10,
                Height = 10,
                Fill = new SolidColorBrush(mediaColor),
                ToolTip = P.Name // Asigna el nombre del punto a ToolTip
            };

            // Crear una etiqueta con el nombre del punto
            Label label = new()
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
}