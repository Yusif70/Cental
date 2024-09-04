namespace Cental.Exceptions
{
    public class EntityNotFoundException:Exception
    {
        private static readonly string _message = "Service is not found";
        public EntityNotFoundException():base(_message) { }
        public EntityNotFoundException(string message) : base(message) { }
    }
}
