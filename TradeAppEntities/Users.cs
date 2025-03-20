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

        public UserPasswords Password { get; private set; }

        public ICollection<UserRoles> UserRoles { get; private set; } = new List<UserRoles>();

        private Users() { }

        public Users(string name, string surname, string username, string email, string phoneNumber, string passwordHash, string salt)
        {
            Name = name;
            Surname = surname;
            Username = username;
            Email = email;
            PhoneNumber = phoneNumber;
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
            Password = new UserPasswords(Id, passwordHash, salt);
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
            Password.UpdatePassword(newPasswordHash, newSalt);
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
