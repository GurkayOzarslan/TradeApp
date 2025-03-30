namespace TradeAppAPI.Controllers.Commands.Password
{
    public class ChangePasswordCommandPayload
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
