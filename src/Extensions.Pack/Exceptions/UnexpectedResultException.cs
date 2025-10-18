namespace Extensions.Pack
{
    public sealed class UnexpectedResultException : Exception
    {
        internal UnexpectedResultException(string message) : base(message)
        {
        }
    }
}
