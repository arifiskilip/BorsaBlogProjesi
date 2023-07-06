using Microsoft.AspNetCore.Http;

namespace BorsaBlogProjesi.Models
{
    public class AppUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile ImagePath { get; set; }
    }
}
