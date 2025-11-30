using Infrastructure.Interfaces;

namespace Infrastructure
{
    public class TokenProvider : ITokenProvider
    {
        private string? _token;
        public string Token {
            get => _token ?? throw new InvalidOperationException("Token not set");
            set => _token = value;
        }
    }
}
