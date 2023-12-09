namespace GeoWall_E
{
    public abstract class Node
    {
        public abstract TokenType Type { get; }
        private Error? Errors_ { get; set; }

        public Error? Errors => Errors_;
        public void AddError(string error)
        {
            if (Errors_ == null)
            {
                Errors_ = new Error();
                Errors_.AddError(error);
            }
            else Errors_.AddError(error);
        }
    }
}