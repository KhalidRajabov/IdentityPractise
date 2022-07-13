using IdentityPractise.DAL;
using IdentityPractise.Models;
using IdentityPractise.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityPractise.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public BlogController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if(id == null) return NotFound();
            Blog blog = await _context.Blogs
                .Include(b => b.Comments).FirstOrDefaultAsync(b => b.Id == id);
            if(blog == null) return NotFound();
            BlogDetailVM blogDetailVM = new BlogDetailVM
            {
                Blog = blog,
                Blogs = _context.Blogs.OrderByDescending(b => b.Id).Take(5).ToList()
            };

            ViewBag.AppuserId = "";
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.AppuserId = user.Id;
            }

            return View(blogDetailVM);
        }
        public async Task<IActionResult> AddComment(Comment comment, int BlogId)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = new AppUser();
            if (User.Identity.IsAuthenticated)
            {
               user = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            Comment newcomment = new Comment
            {
                Message = comment.Message,
                BlogId=BlogId,
                AppUserId=user.Id,
                CreatedTime=System.DateTime.Now
            };
            await _context.AddAsync(newcomment);
            _context.SaveChanges();
            return RedirectToAction("detail", new {id=BlogId});
        }
        public async Task<IActionResult> DeleteComment(int? id, int BlogId)
        {
            Comment comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            _context.Remove(comment);
            _context.SaveChanges();
            return RedirectToAction("Detail", new {id= BlogId});
        }
    }
}

