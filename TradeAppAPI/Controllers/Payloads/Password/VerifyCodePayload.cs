namespace TradeAppAPI.Controllers.Payloads.Password
{
    public class VerifyCodePayload
    {
        public string Email { get; set; }
        public long Code { get; set; }
    }
}
