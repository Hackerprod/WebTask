using System.ComponentModel.DataAnnotations;

namespace WebTask.Entities
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}