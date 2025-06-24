namespace WebElectroShop.Server.Core.DTO
{
    public class RegisterModel
    {
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Role { get; set; } = "Customer";
    }
}
