using IdentityPractise.DAL;
using IdentityPractise.Models;
using IdentityPractise.ViewModels;
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

        public BlogController(AppDbContext context)
        {
            _context = context;
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

             return View(blogDetailVM);
        }
    }
}

