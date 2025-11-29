namespace UserService.BLL.Exceptions
{
    [Serializable]
    public class FailedToSendVerificationEmail : Exception
    {
        public FailedToSendVerificationEmail()
        {
        }

        public FailedToSendVerificationEmail(string? mes) : base(mes)
        {
                
        }
    }
}
