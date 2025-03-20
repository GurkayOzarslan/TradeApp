namespace TradeAppEntity
{
    public class Roles
    {
        public int Id { get; private set; }
        public string RoleName { get; private set; }
        public ICollection<UserRoles> UserRoles { get; private set; } 

        private Roles() { }

        // Yeni rol oluşturma metodu
        public Roles(string roleName)
        {
            RoleName = roleName;
        }
    }
}
