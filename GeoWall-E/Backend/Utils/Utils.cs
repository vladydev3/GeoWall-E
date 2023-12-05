namespace GeoWall_E
{
    public static class Utils
    {
        public static (Line, Circle) LineAndCircleOrdered(Type f1, Type f2)
        {
            if (f1.ObjectType == ObjectTypes.Line && f2.ObjectType == ObjectTypes.Circle) return ((Line)f1, (Circle)f2);

            else return ((Line)f2, (Circle)f1);
        }

        public static (Line, Segment) LineAndSegmentOrdered(Type f1, Type f2)
        {
            if (f1.ObjectType == ObjectTypes.Line && f2.ObjectType == ObjectTypes.Segment) return ((Line)f1, (Segment)f2);

            else return ((Line)f2, (Segment)f1);
        }

        public static (Line, Point) LineAndPointOrdered(Type f1, Type f2)
        {
            if (f1.ObjectType == ObjectTypes.Line && f2.ObjectType == ObjectTypes.Point) return ((Line)f1, (Point)f2);

            else return ((Line)f2, (Point)f1);
        }

        public static (Line, Arc) LineAndArcOrdered(Type f1, Type f2)
        {
            if (f1.ObjectType == ObjectTypes.Line && f2.ObjectType == ObjectTypes.Arc) return ((Line)f1, (Arc)f2);

            else return ((Line)f2, (Arc)f1);
        }

        public static (Line, Ray) LineAndRayOrdered(Type f1, Type f2)
        {
            if (f1.ObjectType == ObjectTypes.Line && f2.ObjectType == ObjectTypes.Ray) return ((Line)f1, (Ray)f2);

            else return ((Line)f2, (Ray)f1);
        }

        public static (Segment, Circle) SegmentAndCircleOrdered(Type f1, Type f2)
        {
            if (f1.ObjectType == ObjectTypes.Segment && f2.ObjectType == ObjectTypes.Circle) return ((Segment)f1, (Circle)f2);

            else return ((Segment)f2, (Circle)f1);
        }

        public static (Segment, Point) SegmentAndPointOrdered(Type f1, Type f2)
        {
            if (f1.ObjectType == ObjectTypes.Segment && f2.ObjectType == ObjectTypes.Point) return ((Segment)f1, (Point)f2);

            else return ((Segment)f2, (Point)f1);
        }

        public static (Segment, Arc) SegmentAndArcOrdered(Type f1, Type f2)
        {
            if (f1.ObjectType == ObjectTypes.Segment && f2.ObjectType == ObjectTypes.Arc) return ((Segment)f1, (Arc)f2);

            else return ((Segment)f2, (Arc)f1);
        }

        public static (Segment, Ray) SegmentAndRayOrdered(Type f1, Type f2)
        {
            if (f1.ObjectType == ObjectTypes.Segment && f2.ObjectType == ObjectTypes.Ray) return ((Segment)f1, (Ray)f2);

            else return ((Segment)f2, (Ray)f1);
        }

        public static (Circle, Point) CircleAndPointOrdered(Type f1, Type f2)
        {
            if (f1.ObjectType == ObjectTypes.Circle && f2.ObjectType == ObjectTypes.Point) return ((Circle)f1, (Point)f2);

            else return ((Circle)f2, (Point)f1);
        }

        public static (Circle, Arc) CircleAndArcOrdered(Type f1, Type f2)
        {
            if (f1.ObjectType == ObjectTypes.Circle && f2.ObjectType == ObjectTypes.Arc) return ((Circle)f1, (Arc)f2);

            else return ((Circle)f2, (Arc)f1);
        }

        public static (Circle, Ray) CircleAndRayOrdered(Type f1, Type f2)
        {
            if (f1.ObjectType == ObjectTypes.Circle && f2.ObjectType == ObjectTypes.Ray) return ((Circle)f1, (Ray)f2);

            else return ((Circle)f2, (Ray)f1);
        }

        public static (Point, Arc) PointAndArcOrdered(Type f1, Type f2)
        {
            if (f1.ObjectType == ObjectTypes.Point && f2.ObjectType == ObjectTypes.Arc) return ((Point)f1, (Arc)f2);

            else return ((Point)f2, (Arc)f1);
        }

        public static (Point, Ray) PointAndRayOrdered(Type f1, Type f2)
        {
            if (f1.ObjectType == ObjectTypes.Point && f2.ObjectType == ObjectTypes.Ray) return ((Point)f1, (Ray)f2);

            else return ((Point)f2, (Ray)f1);
        }

        public static (Arc, Ray) ArcAndRayOrdered(Type f1, Type f2)
        {
            if (f1.ObjectType == ObjectTypes.Arc && f2.ObjectType == ObjectTypes.Ray) return ((Arc)f1, (Ray)f2);

            else return ((Arc)f2, (Ray)f1);
        }
    }
}