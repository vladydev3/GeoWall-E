namespace GeoWall_E
{
    public class Arc : Type, IDraw
    {
        public override ObjectTypes ObjectType => ObjectTypes.Arc;
        private Point Center_ { get; set; }
        private Point Start_ { get; set; }
        private Point End_ { get; set; }
        private Measure Measure_ { get; set; }
        private string Name_ { get; set; }

        public Arc(Point center, Point start, Point end, Measure measure, string name = "")
        {
            Center_ = center;
            Start_ = start;
            End_ = end;
            Measure_ = measure;
            Name_ = name;
            Extremo1 = GetPointOnRay(Center, Start, Measure.Value);
            Extremo2 = GetPointOnRay(Center, End, Measure.Value);
        }

        public Point Center => Center_;

        public Point Start => Start_;

        public Point End => End_;
        public Point Extremo1 { get; set; }
        public Point Extremo2 { get; set; }

        public Measure Measure => Measure_;

        public string Name => Name_;
        public Point SignificativePoint => Center_;


        public void SetName(string name)
        {
            Name_ = name;
        }
        public Point GetPointOnRay(Point origin, Point direction, double distance)
        {

            // Calcular el �ngulo del rayo usando la pendiente
            double angle = Math.Atan2(direction.Y - origin.Y, direction.X - origin.X);
            // Ajustar el �ngulo en funci�n de la direcci�n del rayo
            if (direction.X < origin.X && Math.Cos(angle) > 0)
            {
                angle += Math.PI;
            }
            else if (direction.X > origin.X && Math.Cos(angle) < 0)
            {
                angle -= Math.PI;
            }

            double x = origin.X + distance * Math.Cos(angle);
            double y = origin.Y + distance * Math.Sin(angle); // Usar 'origin.Y' en lugar de 'origin.X'
            Point newPoint = new();
            newPoint.AsignX(x);
            newPoint.AsignY(y);
            return newPoint;
        }


    }
}