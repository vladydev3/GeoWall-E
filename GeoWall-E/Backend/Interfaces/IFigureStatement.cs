namespace GeoWall_E
{
    public interface IFigureStatement
    {
        Token Name { get; }
        bool IsSequence { get; }
        public Sequence CreateSequence();
    }
}