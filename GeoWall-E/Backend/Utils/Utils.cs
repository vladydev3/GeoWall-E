namespace GeoWall_E
{
    public static class Utils
    {
        /// <summary>
        /// Ordena dos objetos Type en el orden deseado.
        /// </summary>
        /// <typeparam name="T1">Primer tipo.</typeparam>
        /// <typeparam name="T2">Segundo tipo.</typeparam>
        /// <param name="f1">El objecto del primer tipo.</param>
        /// <param name="f2">El objecto del segundo tipo.</param>
        /// <returns>Una tupla con los tipos ordenados.</returns>
        public static (T1, T2) OrderByType<T1, T2>(Type f1, Type f2) where T1 : Type where T2 : Type
        {
            if (f1 is T1 t && f2 is T2 t1) return (t, t1);
            else return ((T1)f2, (T2)f1);
        }

        // Calcular la pendiente de una recta
        public static double Slope(Point p1, Point p2)
        {
            if (p1.X == p2.X) return (p2.Y - p1.Y) / (p2.X - p1.X + 1);
            return (p2.Y - p1.Y) / (p2.X - p1.X);
        }

        // Calcular la pendiente de una recta y la ecuacion de la recta
        public static (double, double) SlopeAndEquation(Point p1, Point p2)
        {
            double m = Slope(p1, p2);
            double b = p1.Y - m * p1.X;
            return (m, b);
        }

        /// <summary>
        /// Reordena la lista de nodos colocando las importaciones y las declaraciones de funciones al inicio.
        /// </summary>
        /// <param name="nodes">La lista de nodos a ordenar.</param>
        /// <returns>La lista de nodos reordenada.</returns>
        public static List<Node> ReorderNodes(List<Node> nodes)
        {
            List<Node> newNodes = new();
            List<Node> importNodes = new();
            List<Node> functionNodes = new();
            List<Node> otherNodes = new();
            foreach (var node in nodes)
            {
                if (node is ImportStatement) importNodes.Add(node);
                else if (node is FunctionDeclaration) functionNodes.Insert(0, node);
                else otherNodes.Add(node);
            }
            newNodes.AddRange(importNodes);
            newNodes.AddRange(functionNodes);
            newNodes.AddRange(otherNodes);
            return newNodes;
        }
    }
}