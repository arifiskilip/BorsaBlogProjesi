using System;

namespace BorsaBlogProjesi.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int BlogPostId { get; set; }
        public int AppUserId { get; set; }
        public string Description { get; set; }
        public DateTime? CommentDate { get; set; }

        public BlogPost BlogPost { get; set; }
        public AppUser AppUser { get; set; }
    }
}
