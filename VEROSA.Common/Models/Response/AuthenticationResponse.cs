namespace VEROSA.Common.Models.Response
{
    public class AuthenticationResponse
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public AuthResponse Account { get; set; }
    }
}
