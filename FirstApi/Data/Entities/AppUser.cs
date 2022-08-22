using Microsoft.AspNetCore.Identity;

namespace FirstApi.Data.Entities
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
    public enum Roles
    {
        Admin = 1,
        Member
    }
}
