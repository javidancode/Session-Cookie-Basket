using Entity_Frame_Work_Project.Data;
using Entity_Frame_Work_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entity_Frame_Work_Project.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.count = await _context.Products.Where(m => !m.IsDeleted).CountAsync();
            IEnumerable<Product> products = await _context.Products.Where(m => !m.IsDeleted).Take(4).Include(m => m.ProductImages).ToListAsync();
            return View(products);
        }

        //product/loadmore
        public async Task<IActionResult> LoadMore(int skip)
        {
            IEnumerable<Product> products = await _context.Products.Where(m => !m.IsDeleted).Include(m=> m.Category).Include(m=> m.ProductImages).Skip(skip).Take(4).ToListAsync();
            return PartialView("_ProductsPartial",products);
        }
    }
}
