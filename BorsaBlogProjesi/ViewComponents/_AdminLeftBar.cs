using BorsaBlogProjesi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BorsaBlogProjesi.ViewComponents
{
    public class _AdminLeftBar : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public _AdminLeftBar(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.userImageUrl = user.ImagePath;
            ViewBag.userFirstName = user.FirstName;
            ViewBag.userLastName = user.LastName;
            return View();
        }
    }
}
