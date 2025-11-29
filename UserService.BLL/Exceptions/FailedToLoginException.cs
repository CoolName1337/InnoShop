namespace UserService.BLL.Exceptions
{
    [Serializable]
    public class FailedToLoginException : Exception
    {
        public FailedToLoginException()
        {
        }

        public FailedToLoginException(string? mes) : base(mes)
        {
                
        }
    }
}
