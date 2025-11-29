namespace UserService.BLL.Exceptions
{
    [Serializable]
    public class FailedToSendEmail : Exception
    {
        public FailedToSendEmail()
        {
        }

        public FailedToSendEmail(string? mes) : base(mes)
        {
                
        }
    }
}
