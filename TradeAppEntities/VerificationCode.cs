namespace TradeAppEntity
{
    public class VerificationCode
    {
            public int Id { get; set; }
            public string Email { get; set; }
            public long Code { get; set; }
            public DateTime ExpireAt { get; set; }
            public bool IsUsed { get; set; } = false;

    }
}
