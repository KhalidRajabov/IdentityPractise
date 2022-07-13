using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace IdentityPractise.Models
{
    public class AppUser:IdentityUser
    {
        public string Fullname { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
