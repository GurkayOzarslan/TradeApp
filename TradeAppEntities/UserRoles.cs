using System.Data;

namespace TradeAppEntity
{
    public class UserRoles
    {
        public int UserId { get; private set; }  
        public int RoleId { get; private set; }  

        public Users User { get; private set; }
        public Roles Role { get; private set; }

        // Entity Framework Core için gerekli boş constructor
        private UserRoles() { }

        // Kullanıcıya rol atama metodu
        public UserRoles(int userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}
