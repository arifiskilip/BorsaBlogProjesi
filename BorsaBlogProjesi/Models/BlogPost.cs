using System;
using System.Collections.Generic;

namespace BorsaBlogProjesi.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? PostDate { get; set; }
        public int CategoryId { get; set; }
        public string ImagePath { get; set; }

        public AppUser AppUser { get; set; }
        public Category Category { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
