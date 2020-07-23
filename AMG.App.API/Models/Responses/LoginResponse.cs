namespace AMG.App.API.Models.Responses
{
    public class LoginResponse
    {
        public long UserId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}