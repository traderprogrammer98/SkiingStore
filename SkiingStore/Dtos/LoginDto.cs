using System.ComponentModel.DataAnnotations;

namespace SkiingStore.Dtos
{
    public class LoginDto
    {
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
