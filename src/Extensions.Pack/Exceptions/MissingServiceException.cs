namespace Extensions.Pack.Exceptions
{
    public class MissingServiceException<TSettings> : Exception where TSettings : class
    {
        public MissingServiceException() : base($"The service: '{typeof(TSettings).Name}' is missing. Please check your service registration, that you have already registered the: {typeof(TSettings).Name} you like to resolve.")
        {
        }
    }
}