using Microsoft.AspNetCore.Identity;

namespace MiniETicaret.Domain.Entities.Identity;

public class AppUser : IdentityUser<string>
{
    public string? NameSurname { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenEndDateTime { get; set; }
    public ICollection<Cart> Carts { get; set; }
}