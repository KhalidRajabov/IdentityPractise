using System;

namespace IdentityPractise.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedTime { get; set; }
        public int BlogId { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
