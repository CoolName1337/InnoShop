namespace UserService.BLL.Exceptions
{
    [Serializable]
    public class FailedToValidate : Exception
    {
        public FailedToValidate()
        {
        }

        public FailedToValidate(string? mes) : base(mes)
        {
                
        }
    }
}
