using WebElectroShop.Server.Core.Contracts;

namespace WebElectroShop.Server.Core.Entities
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public bool IsActive { get; set; }
    }
}
