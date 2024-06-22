using Microsoft.AspNetCore.Identity;

namespace postgresTest.Model
{
  public class User : IdentityUser
  {
    public ICollection<Study> Studies { get; set; } = new List<Study>();
  }
}