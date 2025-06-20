namespace TradeAppEntity
{
    public class Users
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string? MiddleName { get; private set; }
        public string Surname { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public DateTime? BirthDate { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; private set; }
        public bool IsActive { get; private set; } = true;
        public bool IsBanned { get; private set; } = false;

        public ICollection<UserPasswords> Passwords { get; private set; } = new List<UserPasswords>();
        public ICollection<UserPasswords2> Passwords2 { get; private set; } = new List<UserPasswords2>();

        public ICollection<UserRoles> UserRoles { get; private set; } = new List<UserRoles>();
        public ICollection<UserSymbols> UserSymbols { get; private set; }
        public UserSignals UserSignals { get; private set; }


        private Users() { }

        public Users(string name, string middleName, string surname, string username, string email, string phoneNumber)
        {
            Name = name;
            MiddleName = middleName;
            Surname = surname;
            Username = username;
            Email = email;
            PhoneNumber = phoneNumber;
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
            Passwords = new List<UserPasswords>();
            IsBanned = false;
        }

        public void UpdateUser(string name, string surname, string phoneNumber)
        {
            Name = name;
            Surname = surname;
            PhoneNumber = phoneNumber;
            UpdatedAt = DateTime.UtcNow;
        }

        public void DeactivateUser()
        {
            IsActive = false;
        }

        public void UpdatePassword(string newPasswordHash, string newSalt)
        {
            var lastPassword = Passwords.OrderByDescending(p => p.CreatedAt).FirstOrDefault();
            if (lastPassword != null)
            {
                lastPassword.UpdatePassword(newPasswordHash, newSalt);
            }
        }


        public void AddRole(Roles role)
        {
            if (!UserRoles.Any(ur => ur.RoleId == role.Id))
            {
                UserRoles.Add(new UserRoles(Id, role.Id));
            }
        }

        public bool HasRole(string roleName)
        {
            return UserRoles.Any(ur => ur.Role.RoleName == roleName);
        }
    }
}
