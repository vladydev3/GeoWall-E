namespace GeoWall_E
{
    public class Point : Type
    {
        public override ObjectTypes ObjectType => ObjectTypes.Point;
        private double X_ { get; set; }
        private double Y_ { get; set; }
        private string Name_ { get; set; }
        private Color Color_ { get; set; }

        public Point(Color color, string name = "")
        {
            Name_ = name;
            Color_ = color;
            X_ = SetX();
            Y_ = SetY();
        }
        public string Name => Name_;

        public Color Color => Color_;
        
        public double X => X_;

        public double Y => Y_;

        public void AsignX(double x)
        {
            X_ = x;
        }

        public void AsignY(double y)
        {
            Y_ = y;
        }

        public static double SetX()
        {
            Random random = new();
            int drawingCanvasWidth = 10000;
            double pointCenterX = drawingCanvasWidth / 2 + random.Next(0, 500);
            return pointCenterX;
        }

        public static double SetY()
        {
            Random random = new();
            int drawingCanvasHeight =10000;
            double pointCenterY = drawingCanvasHeight / 2 - random.Next(0, 500);
            return pointCenterY;
        }
        
    }
}