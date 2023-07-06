using BorsaBlogProjesi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BorsaBlogProjesi.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            TempData["ReturnUrl"] = ReturnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    if (await _userManager.IsLockedOutAsync(user)) //kitliyse
                    {
                        ModelState.AddModelError("", "Hesabınız bir süreliğine kitlenmiştir. Lütfen daha sonra tekrar deneyiniz.");
                        return View(model);
                    }
                    await _signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync
                          (user, model.Password,true, false); //başarısızlık durumunda kitlensin mi ? = false
                    if (result.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user); //başarısız giriş sayısı sıfırlanır.
                        if (TempData["ReturnUrl"] != null)
                        {
                            return Redirect(TempData["ReturnUrl"].ToString());
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        await _userManager.AccessFailedAsync(user); //başarısız giriş sayısını 1 arttır.
                        int fail = await _userManager.GetAccessFailedCountAsync(user);
                        if (fail >= 3)
                        {
                            await _userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(
                                DateTime.Now.AddMinutes(20))); //20 dakika kitle
                            ModelState.AddModelError("", "Hesabınız çok fazla hatalı girişten dolayı bir süreliğine kitlenmiştir.");
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Geçersiz email adresi veya şifresi.");
            }
            return View(model);
        }
       
        public IActionResult Index(int id=1)
        {
            List<Category> categories;
            List<BlogPost> posts;
            using (BorsaContext context = new BorsaContext())
            {
                 categories = context.Categories.ToList();
                posts = context.BlogPosts.ToList();
                if (id == 1)
                {
                    TempData["Posts"] = posts;
                }
                else
                {
                    posts = posts.Where(x => x.CategoryId == id).ToList();
                    TempData["Posts"] = posts;
                }

            }
            TempData["Categories"] = categories;
        
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Detail(int id)
        {
            BlogPost postDetail;
            using (BorsaContext context = new BorsaContext())
            {
                postDetail = context.BlogPosts.Include(x=> x.Category).Include(x=> x.AppUser).FirstOrDefault(x => x.Id == id);
            }
            return View(postDetail);
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View ();
        }
        [HttpPost]
        public IActionResult Contact(Message message)
        {
            using (var context = new BorsaContext())
            {

                Message msg = new Message();
                msg.FirstName = message.FirstName;
                message.MessageDate = DateTime.Now;
                context.Messages.Add(message);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Deneme()
        {
            return View();
        }
    }
}
