namespace Bejewled.Model.Exceptions
{
    using System;

    public class InvalidTileCoordinateException : Exception
    {
        public InvalidTileCoordinateException(string message)
            : base(message)
        {
        }
    }
}