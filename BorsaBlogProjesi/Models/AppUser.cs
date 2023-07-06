using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BorsaBlogProjesi.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImagePath { get; set; }

        public List<BlogPost> BlogPosts { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
