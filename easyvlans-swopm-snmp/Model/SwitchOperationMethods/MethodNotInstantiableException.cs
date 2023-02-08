namespace easyvlans.Model.SwitchOperationMethods
{
    public class MethodNotInstantiableException : Exception
    {
        public MethodNotInstantiableException() { }
        public MethodNotInstantiableException(string message) : base(message) { }
        public MethodNotInstantiableException(string message, Exception innerException) : base(message, innerException) { }
    }
}
