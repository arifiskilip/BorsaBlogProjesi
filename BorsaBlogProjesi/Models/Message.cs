using System;

namespace BorsaBlogProjesi.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Time { get; set; }
        public string Description { get; set; }
        public DateTime? MessageDate { get; set; }
    }
}
