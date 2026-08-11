namespace Backend.Exceptions
{
    public abstract class AppException : Exception
    {
        public abstract int StatusCode {  get;}
         public AppException(string message) : base(message) { }
    }
}
