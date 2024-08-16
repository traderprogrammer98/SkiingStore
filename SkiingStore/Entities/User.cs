using Microsoft.AspNetCore.Identity;

namespace SkiingStore.Entities
{
    public class User : IdentityUser<int>
    {
        public UserAddress Address { get; set; }
    }
}
