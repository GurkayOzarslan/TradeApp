namespace TradeAppSharedKernel.Infrastructure.Helpers.TokenInfo
{
    public class TokenInfoResponse
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string Surname { get; set; }
        public long? NotBefore { get; set; } 
        public long? Expires { get; set; }  
        public long? IssuedAt { get; set; } 
        public string Issuer { get; set; }   
        public string Audience { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
