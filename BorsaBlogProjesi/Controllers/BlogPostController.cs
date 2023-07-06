using Microsoft.AspNetCore.Mvc;

namespace BorsaBlogProjesi.Controllers
{
    public class BlogPostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
