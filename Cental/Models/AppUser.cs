﻿using Microsoft.AspNetCore.Identity;

namespace Cental.Models
{
    public class AppUser : IdentityUser
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public List<Message> Messages { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
