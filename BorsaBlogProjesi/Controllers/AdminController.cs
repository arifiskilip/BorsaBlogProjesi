using BorsaBlogProjesi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BorsaBlogProjesi.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult BlogPost()
        {
            List<BlogPost> posts;
            using (var context = new BorsaContext())
            {
                posts = context.BlogPosts.Include(x => x.Category).Include(x => x.AppUser).ToList();
                TempData["Posts"] = posts;
            }
            return View();
        }
        [HttpGet]
        public IActionResult BlogPostAdd()
        {
            using (var context = new BorsaContext())
            {
                List<SelectListItem> valueCategory = (from c in context.Categories.ToList()
                                                  select new SelectListItem
                                                  {
                                                      Value = c.Id.ToString(),
                                                      Text = c.Name
                                                  }).ToList();
                ViewBag.valueCategory = valueCategory;
              
                
            }       
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> BlogPostAdd(BlogPostModel model)
        {
            using (var context = new BorsaContext())
            {
                var blogPost = new BlogPost();
                var userId =await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                blogPost.PostDate = DateTime.Now;
                blogPost.AppUserId = userId.Id;
                blogPost.CategoryId = model.CategoryId;
                blogPost.Title = model.Title;
                blogPost.Description = model.Description;
                //Image Add
                if (model.ImagePath != null)
                {
                    var directory = Directory.GetCurrentDirectory();
                    var extension = Path.GetExtension(model.ImagePath.FileName);
                    var imageName = Guid.NewGuid() + extension;
                    var saveLocation = directory + "/wwwroot/images/" + imageName;
                    var stream = new FileStream(saveLocation, FileMode.Create);
                    await model.ImagePath.CopyToAsync(stream);
                    blogPost.ImagePath = "/images/" + imageName;
                }      
                

                await context.BlogPosts.AddAsync(blogPost);
                await context.SaveChangesAsync();
            }
            return RedirectToAction("BlogPost");
        }

        public IActionResult BlogPostDelete(int id)
        {
            using (var context = new BorsaContext())
            {
                var deletedItem = context.BlogPosts.Where(x=> x.Id==id).FirstOrDefault();
                if (deletedItem != null)
                {
                    context.BlogPosts.Remove(deletedItem);
                    context.SaveChanges();
                    return RedirectToAction("BlogPost");
                }        

            }
            return RedirectToAction("BlogPost");
        }
        [HttpGet]
        public IActionResult BlogPostUpdate(int id)
        {
            using (var context = new BorsaContext())
            {
                var model = new BlogPostModel();
                var item = context.BlogPosts.Where(x => x.Id == id).FirstOrDefault();
                model.Title = item.Title;
                model.Description = item.Description;
                model.CategoryId = item.CategoryId;

                List<SelectListItem> valueCategory = (from c in context.Categories.ToList()
                                                      select new SelectListItem
                                                      {
                                                          Value = c.Id.ToString(),
                                                          Text = c.Name
                                                      }).ToList();
                ViewBag.valueCategory = valueCategory;
                TempData["id"] = id;
                return View(model);
            }
           
        }
        [HttpPost]
        public  async Task<IActionResult> BlogPostUpdate(BlogPostModel model)
        {
            using (var context = new BorsaContext())
            {
                var id = TempData["id"];
                var item = context.BlogPosts.Where(x => x.Id == Convert.ToInt16(id)).FirstOrDefault();

                item.Title = model.Title;
                item.Description = model.Description;
                item.CategoryId = model.CategoryId;
                if (model.ImagePath != null)
                {
                    var directory = Directory.GetCurrentDirectory();
                    var extension = Path.GetExtension(model.ImagePath.FileName);
                    var imageName = Guid.NewGuid() + extension;
                    var saveLocation = directory + "/wwwroot/images/" + imageName;
                    var stream = new FileStream(saveLocation, FileMode.Create);
                    await model.ImagePath.CopyToAsync(stream);
                    item.ImagePath = "/images/" + imageName;
                }

                context.BlogPosts.Update(item);
                context.SaveChanges();

                return RedirectToAction("BlogPost");
            }

        }
        public async Task<IActionResult> Profile()
        {
            var user =await _userManager.FindByNameAsync(User.Identity.Name);
            return View (user);
        }
        public async Task<IActionResult> ProfileEdit()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            AppUserModel model = new AppUserModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ProfileEdit(AppUserModel model)
        {
            using (var context = new BorsaContext())
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;
                if (model.ImagePath != null)
                {
                    var directory = Directory.GetCurrentDirectory();
                    var extension = Path.GetExtension(model.ImagePath.FileName);
                    var imageName = Guid.NewGuid() + extension;
                    var saveLocation = directory + "/wwwroot/images/" + imageName;
                    var stream = new FileStream(saveLocation, FileMode.Create);
                    await model.ImagePath.CopyToAsync(stream);
                    user.ImagePath = "/images/" + imageName;
                }
                await _userManager.UpdateAsync(user);
            }
            return RedirectToAction("Profile");
        }

        public async Task<IActionResult> LeftBar()
        {
            var userImage = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.firstName = userImage.FirstName;
            return View();
        }

        public IActionResult MailBox()
        {
            List<Message> messages;
            using (var context = new  BorsaContext())
            {
                messages = context.Messages.ToList();
                
            }
            return View(messages);
        }
        public IActionResult MailDetail(int id)
        {
            Message message;
            using (var context = new BorsaContext())
            {
                message = context.Messages.FirstOrDefault(x => x.Id == id);
                return View(message);
            }
        }
        public IActionResult MailDelete(int id)
        {
            Message message;
            using (var context = new BorsaContext())
            {
                message = context.Messages.FirstOrDefault(x => x.Id == id);
                context.Messages.Remove(message);
                context.SaveChanges();
                return RedirectToAction("Mailbox");
            }
        }
        [HttpGet]
        public IActionResult EditPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditPassword(UserPasswordModel editModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editModel);
            }
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var hashPassword = await _signInManager.PasswordSignInAsync(user, editModel.LastPassword, false, false);
            if (hashPassword.Succeeded)
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, editModel.NewPassword);
                await _userManager.UpdateAsync(user);
                return RedirectToAction("Home", "Login");
            }
            return View();
        }
        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(model);
        }
        public IActionResult GetUsers()
        {
            using (var context = new BorsaContext())
            {
                var result = context.Users.ToList();
                ViewBag.userName = result.FirstOrDefault(x => x.UserName == User.Identity.Name);
                return View(result);
            }
           
        }
        public async Task<IActionResult> DeleteUser(string id)
        {
            using (var context = new BorsaContext())
            {
                var result = await _userManager.FindByIdAsync(id);
                await _userManager.DeleteAsync(result);
                return RedirectToAction("GetUsers");
            }
        }

    }
}
