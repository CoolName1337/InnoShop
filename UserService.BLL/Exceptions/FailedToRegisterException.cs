namespace UserService.BLL.Exceptions
{
    [Serializable]
    public class FailedToRegisterException : Exception
    {
        public FailedToRegisterException()
        {
        }

        public FailedToRegisterException(string? mes) : base(mes)
        {
                
        }
    }
}
