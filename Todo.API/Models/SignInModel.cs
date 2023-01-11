using System.ComponentModel.DataAnnotations;

namespace Todo.API.Models
{
    public class SignInModel
    {
        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(16)]
        public string Password { get; set; }
    }
}
