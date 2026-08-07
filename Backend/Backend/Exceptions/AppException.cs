namespace Backend.Exceptions
{
    public class AppException : Exception
    {
        public virtual int StatusCode {  get;}
        protected AppException(string message) : base(message) { }
    }
}
