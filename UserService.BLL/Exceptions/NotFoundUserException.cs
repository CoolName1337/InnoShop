namespace UserService.BLL.Exceptions
{
    [Serializable]
    public class NotFoundUserException : Exception
    {
        public NotFoundUserException()
        {
        }

        public NotFoundUserException(string? message) : base(message)
        {
        }
    }
}